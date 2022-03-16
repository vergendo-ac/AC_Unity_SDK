using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    public ARPlaneManager arplaner;
    public float yGround;
    int planetimer = 0;
    List<GameObject> planes;
    GameObject aRcamera;

    void Start()
    {
        aRcamera = Camera.main.gameObject;
        yGround = -1000;
    }

    private void FixedUpdate()
    {
        planetimer++;
        if (planetimer > 100) { updatePlaneY(); }
    }

    public void updatePlaneY()
    {
        planes = new List<GameObject>();
        var go = arplaner.trackables;
        foreach (var g in go) {
            planes.Add(g.gameObject);
        }
        //Debug.Log("planes length = " + planes.Count);

        float minY = 10;
        foreach (GameObject pl in planes) {
            // search for the plane on distance >0.8m from the camera
            if (aRcamera.transform.position.y - pl.transform.position.y > 0.8f) {
                //FixMe: How to proceed planes under the ground?
                if (pl.transform.position.y < minY)
                    minY = pl.transform.position.y;         // search for the minimal Y coord - closest plane to camera
            }
            planetimer = -50;                               // remeasure in 3sec if AR has found some planes
        }

        if (minY < 10)                                      // if found
        {
            yGround = minY;                                 // store closest plane to the camera on dist >0.8m
            if (aRcamera.transform.position.y - yGround > 1.7f) {
                yGround = aRcamera.transform.position.y - 1.7f;  // assume that a ground couldn't more far than 1.7m
            }
            GetComponent<UIManager>().planeDebug(yGround);  // show the found plane Y-coord
        }
        else
        {
            yGround = aRcamera.transform.position.y - 1.5f; // there're no found planes, assume the ground is in 1.5m
            planetimer = 0;                                 // force to recalculate the plane in 2secs
        }
    }

    public float getPlaneY()
    {
        if (yGround != -1000) {  //FixMe: use bool "found"?
            return yGround;
        }
        Debug.Log("NOT READY yGround!!!");

        return aRcamera.transform.position.y - 1.5f;        // assume the ground is in 1.5m
    }
}
