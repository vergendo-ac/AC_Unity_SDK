using ServerUrlsNs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupServersPanel : MonoBehaviour
{
    [SerializeField]
    Button CloseBtn;

    [SerializeField]
    Button Apply;

    [SerializeField]
    GameObject TogglesRoot;

    [SerializeField]
    List<URLTogle> Toggles;

    [SerializeField]
    GameObject reloadingPanel;
    
    ServerURLs URLs => URLSData.Instance?.URLs;
    void Start()
    {
        gameObject.GetComponentsInChildren<URLTogle>(Toggles);
        CloseBtn.onClick.AddListener(OnClose);
        GetComponentInChildren<URLTogle>(true);
        RedrawToggles();
        Apply.onClick.AddListener(OnApply);
    }

    void RedrawToggles()
    {
        if (Toggles.Count != URLs.Count)
            Debug.LogError($"Check the ServerURLs.Count {URLs.Count}  and count of toggles {Toggles.Count}, they are different");
        var URLsArr = URLs.URLsArr;
        for (int idx = 0; idx < URLsArr.Length; idx++)
        {
            int urlId = URLs.IndexToUrlId(idx);
            var toggle = Toggles.Find(el=>el != null && el.UrlId == urlId);
            if (toggle == null)
                Debug.LogError($"Check toggles - not found element with id={urlId} for {URLsArr[idx]}");
            else
            {
                toggle.SetCaption(URLsArr[idx]);
                toggle.IsOn = (URLs.CurUrlId == toggle.UrlId);
            }
            
        }

    }

    void OnClose()
    {
        gameObject.SetActive(false);
    }

    void OnApply()
    {
        var activeToggle = Toggles.Find(
                el => el != null
                && el.IsOn);
        if (activeToggle == null)
        {
            activeToggle = Toggles[0];
            activeToggle.IsOn = true;
        }
        URLs.SetCurUrlId(activeToggle.UrlId);
        if (activeToggle.UrlId == URLs.CustomUrlId)
        {
            URLCustomTogle customToggle = activeToggle as URLCustomTogle;
            URLs.SetCustomUrl(customToggle.GetEnteredUrl());
        }

        /*
        if (URLs.CurUrlId == ServerURLs.CustomUrlId)
        {
            var toggle = Toggles.Find(
                el => el != null 
                && el.UrlId == ServerURLs.CustomUrlId);
            URLCustomTogle customToggle = toggle as URLCustomTogle;
            URLs.SetCustomUrl(customToggle.GetEnteredUrl());
        }
        */
        URLs.Save();
      
        StartCoroutine(PauseThenRestart());
    }

    IEnumerator PauseThenRestart()
    {
        yield return null;
        reloadingPanel.SetActive(true);
        yield return new WaitForSeconds(4);
        UIManager.Instance.Restart();
    }

}
