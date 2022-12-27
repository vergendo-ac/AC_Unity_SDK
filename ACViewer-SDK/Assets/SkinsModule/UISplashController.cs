using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISplashController : MonoBehaviour
{
    [System.Serializable]
    public class SkinSplash
    {
        public string name;
        public GameObject mainPanel;
        public GameObject panelInstruction;
        public GameObject panelNotInPlace;
        public GameObject panelNoGeoPermisson;
    }

    [SerializeField] GameObject mainSkin;
    [SerializeField] SkinSplash[] skins;

    public void SetZoneControllerUI()
    {
        foreach (SkinSplash skin in skins)
        {
            if (PlayerPrefs.GetString("skin").ToLower().Contains(skin.name.ToLower())) 
            {
                mainSkin.SetActive(false);
                skin.mainPanel.SetActive(true);
            }

            if (PlayerPrefs.GetInt("Instruction") == 1)
            {
                skin.panelInstruction.SetActive(true);
            }
            else
            {
                skin.panelInstruction.SetActive(false);
            }

            if (PlayerPrefs.GetInt("NotInPlace") == 1)
            {
                skin.panelNotInPlace.SetActive(true);
            }
            else
            {
                skin.panelNotInPlace.SetActive(false);
            }

        }
    /*    if (PlayerPrefs.GetInt("Instruction") == 1)
            {   
            foreach (Skin skin in skins)
            {
                skin.panelInstruction.SetActive(true);
            }
        }
        else
        {
            foreach (Skin skin in skins)
            {
                panelInstruction.SetActive(false);
            }
        }

        if (PlayerPrefs.GetInt("NotInPlace") == 1)
        {
            panelNotInPlace.SetActive(true);
        }
        else
        {
            panelNotInPlace.SetActive(false);
        }*/
    }

    public void NoGeoPermisson()
    {
        foreach (SkinSplash skin in skins)
        {
            skin.panelNoGeoPermisson.SetActive(true);
        }
    }
}
