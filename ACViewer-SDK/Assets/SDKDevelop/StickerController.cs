﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StickerController : MonoBehaviour
{
    public GameObject pins;
    public GameObject passivePin;
    public GameObject activePin;

    public RectTransform markerRt;
    public Text sText;
    public Text subType;
    Transform target;
    string url;
    UIManager uim;
    Vector3 startLocalScale, startPinScale, startPinPosition;
    float koefSticker = 0.5f;
    float koefPin = 0.017f;
    float koefUpPin = 100f;
    float distanceToPinScalingMin = 4f;
    float distanceToPinScalingMax = 25f;
    RectTransform rtt;
    GameObject marker;
    Visiability pinVis;
    ACityAPIDev.StickerInfo stickerInfo;
    int timer;
    bool activated;
    [HideInInspector]
    public float distanceToCamera;

    const float maxDistanceCamToVisibleMarker = 15f;   // marker is visible if the distance to the camera less than this value in meters
    const float maxDistanceCamToVisiblePin    = 30f;   // pin    is visible if the distance to the camera less than this value in meters

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Canvas cv = GetComponent<Canvas>();
        cv.worldCamera = target.GetComponent<Camera>();
        uim = GameObject.FindGameObjectWithTag("Manager").GetComponent<UIManager>();

        startLocalScale  = this.transform.localScale * 2;
        startPinScale    = pins.transform.localScale;
        startPinPosition = pins.transform.localPosition;
        distanceToCamera = Vector3.Magnitude(this.transform.position - target.transform.position);
        if (distanceToCamera > distanceToPinScalingMax) {
            SetScaling(distanceToPinScalingMax);
        }
        pinVis = GetComponentInChildren<Visiability>();
    }

    void Update()
    {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0 , transform.eulerAngles.y, 0);
        distanceToCamera = Vector3.Magnitude(this.transform.position - target.transform.position);
        this.transform.localScale = startLocalScale;    // * koefSticker * distanceToCamera;
        if (distanceToCamera > maxDistanceCamToVisibleMarker) {
            SetMarker(false);
        }
        else {
            SetMarker(true);
        }
        if ((distanceToCamera > distanceToPinScalingMin) &&
            (distanceToCamera < distanceToPinScalingMax)) {
            SetScaling(distanceToCamera);
        }
        activePin.SetActive(activated);
        passivePin.SetActive(!activated);

        if (distanceToCamera > maxDistanceCamToVisiblePin) {
            pins.SetActive(false);
        }
        else {
            pins.SetActive(true);
        }

        timer++;
        if (timer % 10 == 0)
        {
            if (!PinVisiability()) SetMarker(false);
        }
    }

    void SetScaling(float distanceToCamera)
    {
        pins.transform.localScale = startPinScale;
        /*
        float difScaling = koefPin * (distanceToCamera - distanceToPinScalingMin);
        pins.transform.localScale = startPinScale - new Vector3(difScaling, difScaling, difScaling);
        pins.transform.localPosition = startPinPosition + new Vector3(0, (distanceToCamera - distanceToPinScalingMin) * koefUpPin * koefPin, 0);
        */
    }

    public void setStickerInfo(ACityAPIDev.StickerInfo sInfo)
    {
        if (sInfo.sText.Length > 40) sText.text = sInfo.sText.Substring(0, 30); else sText.text = sInfo.sText;
        if (sInfo.sSubType.Length > 40) subType.text = sInfo.sSubType.Substring(0, 30); else subType.text = sInfo.sSubType;
        rtt = sText.gameObject.GetComponent<RectTransform>();
        marker = markerRt.gameObject;
        StartCoroutine(setScale());
        stickerInfo = sInfo;
    }

    public void buttonPressed()
    {
        if (!string.IsNullOrEmpty(stickerInfo.sOnTap) && stickerInfo.sOnTap.Contains("open_url"))
        {
            if (!string.IsNullOrEmpty(stickerInfo.sPath)) {
                uim.GoToURL(stickerInfo.sPath);
            }
        }
        else
        {
            uim.SetStickerPanel(stickerInfo, activate);
            activate(true);
            //Debug.Log(Vector3.Magnitude(this.transform.position - target.transform.position));
        }
    }

    IEnumerator setScale()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("WIDTH ssText     " + sText.text + "     " + rtt.sizeDelta.x);

        if (rtt.sizeDelta.x > 444) {  //FixMe: ???
            markerRt.sizeDelta = new Vector2(rtt.sizeDelta.x * 0.18f, markerRt.sizeDelta.y);  //FixMe: ???
        }
    }

    public bool PinVisiability() {
        return pinVis.VisibleA();
    }

    public void SetMarker(bool markerActive)
    {
        if (marker != null)
        {
            marker.SetActive(markerActive);
        }
    }

    public float GetDistance() {
        return distanceToCamera;
    }

    void activate(bool act) {
        activated = act;
    }

}
