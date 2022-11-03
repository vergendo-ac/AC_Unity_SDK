using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecoFilter
{
    // DistanceToLastCam is a distance in meters that AR-camera(smartphone)
    // have to go between relocalizations in real world.
    // To add relocalization to Start or Work filter.
    const int DistanceToLastCam = 1;

    // StartFilterCount is how many times the start filter will get the result, 
    // after which it will trigger
    const int StartFilterC = 3;
    public const int StartFilterRelTimer = 2;
    const int OffsetDistance = 100;
    const int OffsetAngle = 25;

    public static RecoFilter instance { get; private set; }
    Vector3 lastCamPosition = Vector3.zero;
    UIManager uim;
    GetPlaceHoldersDev gph;

    public static RecoFilter Instance()
    {
        if (instance == null)
            instance = new RecoFilter();
        return instance;
    }

    List<GetPlaceHoldersDev.RecoPose> firstRecos = new List<GetPlaceHoldersDev.RecoPose>();
    List<GetPlaceHoldersDev.RecoPose> inWorkRecos = new List<GetPlaceHoldersDev.RecoPose>();

    private bool isStartFilterOn = true;

    public bool IsStartFilterOn()
    {
        return isStartFilterOn;
    }

    public void FilterStart(GameObject placeHolderParent,
                   Transform zeroP,
                   Vector3 arCamCoordinates,
                   float animationTime,
                   Action<GameObject, Transform, float> Translocation)
    {
        PlayerPrefs.SetInt("StartFilterCount", StartFilterC);
        PlayerPrefs.SetFloat("StartFilterRelTimer", StartFilterRelTimer);
        PlayerPrefs.SetFloat("OffsetDistance", OffsetDistance);
        PlayerPrefs.SetFloat("OffsetAngle", OffsetAngle);

        if (gph == null)
        {
            gph = GameObject.FindGameObjectWithTag("Manager").GetComponent<GetPlaceHoldersDev>();
            uim = GameObject.FindGameObjectWithTag("Manager").GetComponent<UIManager>();
        }

        GameObject tempBiasVector = new GameObject("TempBiasVector");

        float recOffset = (placeHolderParent.transform.position - zeroP.position).magnitude;
        float angleOffset = Quaternion.Angle(placeHolderParent.transform.rotation, zeroP.rotation);
        uim.debugPose[18].text = "RecOffset: "   + recOffset;
        uim.debugPose[19].text = "AngleOffset: " + angleOffset;

        int StartFilterCount = 0;
        if (PlayerPrefs.HasKey("StartFilterCount"))
        {
            StartFilterCount = PlayerPrefs.GetInt("StartFilterCount");
        }

        float distanceToLastCamNow = (lastCamPosition - arCamCoordinates).magnitude;
        bool filterDataIsGood = (lastCamPosition == Vector3.zero ||
                                 distanceToLastCamNow > DistanceToLastCam);

        GetPlaceHoldersDev.RecoPose rp = new GetPlaceHoldersDev.RecoPose();
        rp.pos = zeroP.transform.position;
        rp.ori = zeroP.transform.rotation;

        if (StartFilterCount > 0 && firstRecos.Count < StartFilterCount)
        {
            if (filterDataIsGood)
            {
                // Accumulate loca answers
                firstRecos.Add(rp);
                Debug.Log("StartF: +rec, n=" + firstRecos.Count);
                // All answers accumulated - start
                if (firstRecos.Count == StartFilterCount)
                {
                    GetPlaceHoldersDev.RecoPose defParent = SetDefaultObjectParent(firstRecos);
                    tempBiasVector.transform.position = defParent.pos;
                    tempBiasVector.transform.rotation = defParent.ori;
                    Translocation(placeHolderParent, tempBiasVector.transform, animationTime);
                    GameObject.Destroy(tempBiasVector);
                    gph.setTimeForRelocation(PlayerPrefs.GetFloat("TimeForRelocation"));    // set reloc timer to proper value
                    uim.debugPose[17].text = "FIRST MOVE";
                }
                else
                {
                    gph.setTimeForRelocation(PlayerPrefs.GetFloat("StartFilterRelTimer"));  // set reloc timer to proper value
                    uim.debugPose[17].text = "S." + firstRecos.Count + " getframe";
                    gph.SetRelocationState(true);
                }
            }
            else
            {
                uim.debugPose[17].text = "S." + firstRecos.Count
                    + " d=" + (Mathf.Round(distanceToLastCamNow * 100.0f) * 0.01f)
                    + "<" + DistanceToLastCam + " m.";
                gph.setTimeForRelocation(PlayerPrefs.GetFloat("StartFilterRelTimer"));  // set reloc timer to proper value
                gph.SetRelocationState(true);
            }
        }
        else
        {
            if (recOffset < PlayerPrefs.GetFloat("OffsetDistance") &&
                angleOffset < PlayerPrefs.GetFloat("OffsetAngle"))
            {
                tempBiasVector.transform.position = zeroP.position;
                tempBiasVector.transform.eulerAngles = zeroP.eulerAngles;
                Translocation(placeHolderParent, tempBiasVector.transform, animationTime);
                GameObject.Destroy(tempBiasVector);
                inWorkRecos.Clear();
                uim.debugPose[17].text = "W." + inWorkRecos.Count + " Move";
            }
            else
            {
                if (StartFilterCount > 0 && inWorkRecos.Count < StartFilterCount)
                {
                    if (filterDataIsGood)
                    {
                        inWorkRecos.Add(rp);
                    }
                    if (inWorkRecos.Count == StartFilterCount)
                    {
                        GetPlaceHoldersDev.RecoPose defParent = SetDefaultObjectParent(inWorkRecos);
                        tempBiasVector.transform.position = defParent.pos;
                        tempBiasVector.transform.rotation = defParent.ori;
                        Translocation(placeHolderParent, tempBiasVector.transform, animationTime);
                        GameObject.Destroy(tempBiasVector);
                        uim.debugPose[17].text = "W." + inWorkRecos.Count + " Move";
                        inWorkRecos.Clear();
                    }
                    else
                    {
                        uim.debugPose[17].text = "W." + inWorkRecos.Count + " Jump";
                        gph.SetRelocationState(true);
                    }
                }
            }
        }
        lastCamPosition = arCamCoordinates;
    }

    public GetPlaceHoldersDev.RecoPose SetDefaultObjectParent(List<GetPlaceHoldersDev.RecoPose> recosList)
    {
        float step = 10;
        if (PlayerPrefs.HasKey("OffsetAngle"))
        {
            step = PlayerPrefs.GetFloat("OffsetAngle");
        }
        int angleDist = Mathf.RoundToInt(180f / step);
        List<List<GetPlaceHoldersDev.RecoPose>> allTransforms = new List<List<GetPlaceHoldersDev.RecoPose>>();
        for (int i = 0; i < angleDist; i++)
        {
            allTransforms.Add(new List<GetPlaceHoldersDev.RecoPose>());
        }
        List<GetPlaceHoldersDev.RecoPose>[] transArray = allTransforms.ToArray();
        GetPlaceHoldersDev.RecoPose[] recos = recosList.ToArray();
        transArray[0].Add(recos[0]);
        for (int j = 1; j < recos.Length; j++)
        {
            float angleOffset = Mathf.Abs(Quaternion.Angle(recos[0].ori, recos[j].ori));
            Debug.Log(j + "    angleOffset = " + angleOffset);
            for (int i = 0; i < angleDist; i++)
            {
                Debug.Log("(i * step) = " + (i * step) + "; ((i + 1) * step) = " + ((i + 1) * step));
                if (angleOffset >= (i * step) && angleOffset < ((i + 1) * step))
                {
                    transArray[i].Add(recos[j]);
                    Debug.Log("Number #" + j + "    Angle offset=" + angleOffset + " -  Goes to " + i * step + " anglebox.");
                }
            }
        }
        int max = 1;
        int maxNum = 0;
        for (int i = 0; i < transArray.Length; i++)
        {
            Debug.Log("transArray #" + i + "  contains " + transArray[i].Count + " recos");
            if (transArray[i].Count > max)
            {
                max = transArray[i].Count;
                maxNum = i;
            }
        }
        Debug.Log("maxNum =  " + maxNum);
        return transArray[maxNum].ToArray()[transArray[maxNum].ToArray().Length - 1];
    }

    public void OnReloadScene()
    {
        firstRecos.Clear();
        inWorkRecos.Clear();
    }
}