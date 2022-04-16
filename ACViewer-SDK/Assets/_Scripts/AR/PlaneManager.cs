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
    
    const float yGroundMin = 0.8f;   // min AR-camera level above the ground, where ground is detected
    const float yGroundDef = 1.5f;   // assume ground level which is under AR camera by 1.5m in case of not detected
    const float yGroundMax = 1.6f;   // max AR-camera level above the ground, where ground is detected

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
        foreach (GameObject pl in planes)
        {
            // search for the plane on distance >MIN from the camera
            if (aRcamera.transform.position.y - pl.transform.position.y > yGroundMin)
            {
                //FixMe: How to proceed planes under the ground?
                if (pl.transform.position.y < minY) {
                    minY = pl.transform.position.y;         // search for the minimal Y coord - closest plane to camera
                }
            }
            planetimer = -50;                               // remeasure in 3sec if AR has found some planes
        }

        if (minY < 10)                                      // if found
        {
            yGround = minY;                                 // store closest plane to the camera on dist >MIN
            if (aRcamera.transform.position.y - yGround > yGroundMax) {
                yGround = aRcamera.transform.position.y - yGroundMax;  // assume that a ground couldn't more far than MAX
            }
            GetComponent<UIManager>().planeDebug(yGround);  // show the found plane Y-coord
        }
        else
        {
            yGround = aRcamera.transform.position.y - yGroundDef;   // there're no found planes, assume the ground is in DEF
            planetimer = 0;                                         // force to recalculate the plane in 2secs
        }
    }

    public float getPlaneY()
    {
        if (yGround != -1000) {  //FixMe: use bool "found"?
            return yGround;
        }
        Debug.Log("NOT READY yGround!!!");

        return aRcamera.transform.position.y - yGroundDef;     // assume the ground is in DEF
    }
}
