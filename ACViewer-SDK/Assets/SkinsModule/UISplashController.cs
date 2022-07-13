using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplashController : MonoBehaviour
{
    [SerializeField] GameObject panelInstruction;
    [SerializeField] GameObject panelNotInPlace;
    [SerializeField] GameObject panelNoGeoPermisson;

    public void SetZoneControllerUI()
    {
        if (PlayerPrefs.GetInt("Instruction") == 1)
        {
            panelInstruction.SetActive(true);
        }
        else
        {
            panelInstruction.SetActive(false);
        }

        if (PlayerPrefs.GetInt("NotInPlace") == 1)
        {
            panelNotInPlace.SetActive(true);
        }
        else
        {
            panelNotInPlace.SetActive(false);
        }
    }

    public void NoGeoPermisson() 
    {
        panelNoGeoPermisson.SetActive(true);
    }
}
