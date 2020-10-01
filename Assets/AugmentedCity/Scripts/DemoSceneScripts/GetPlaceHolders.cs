using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetPlaceHolders : MonoBehaviour
{
    public GameObject dot;
    public GameObject linePrefab;
    public GameObject cantLocalizeImage;
    public GameObject localizedImage;

    ACityAPI acapi;
    GameObject placeHolderParent;

    void Start()
    {
        acapi = GetComponent<ACityAPI>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }


    public void startLocalization() {
        acapi.ARLocation(showPlaceHolders);
    }

    void showPlaceHolders(ACityAPI.StickerInfo[] stickers) {
        if (placeHolderParent != null) { Destroy(placeHolderParent); }
        placeHolderParent = new GameObject("PlaceHolderParent");
        if (stickers != null)
        {
            for (int j = 0; j < stickers.Length; j++)
            {
                //Placeholders
                for (int i = 0; i < 4; i++) {
                    Debug.Log(stickers[j].positions[i].x + "   " + stickers[j].positions[i].y + "  " + stickers[j].positions[i].z);
                    GameObject go = Instantiate(dot, placeHolderParent.transform);
                    go.transform.position = stickers[j].positions[i];
                }
                //Lines
                GameObject lineHolder = Instantiate(linePrefab,placeHolderParent.transform);
                LineRenderer lr = lineHolder.GetComponent<LineRenderer>();
                lr.positionCount = 4;
                lr.SetPositions(stickers[j].positions);
            }
            localizedImage.SetActive(true);
        }
        else {
            Debug.Log("Can't localize");
            cantLocalizeImage.SetActive(true);
            ACityAPI.LocalizationStatus ls = acapi.getLocalizationStatus();
        }
    }
}
