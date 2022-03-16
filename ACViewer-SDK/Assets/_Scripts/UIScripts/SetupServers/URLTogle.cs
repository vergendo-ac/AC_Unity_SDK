using ServerUrlsNs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class URLTogle : MonoBehaviour
{
    [SerializeField]
    Toggle toggle;
    [SerializeField]
    Text Caption;
    ServerURLs URLs => URLSData.Instance?.URLs;

    public bool IsOn 
    { 
        get { return toggle.isOn; }
        set 
        {
            if (toggle == null)
                Debug.LogError($"Toggle is null at {gameObject.name} for {UrlId}");
            else
                toggle.isOn = value;
        }
    }
    //[SerializeField]
    public int UrlId;

    public virtual void SetCaption(string newCaption)
    {
        if (Caption != null)
            Caption.text = newCaption;
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();

        if (toggle == null)
            Debug.LogError($" Can't found component Toggle at {gameObject.name} for {UrlId}");

        if (Caption == null)
            Caption = GetComponentInChildren<Text>();
     //   toggle.onValueChanged.AddListener(OnToggleSwitched);
    }

    void OnToggleSwitched(bool newVal)
    {
        //Debug.Log($"URLTogle: isOn={toggle.isOn}, newVal = {newVal}");
        if (newVal)
        {
            URLs.SetCurUrlId(UrlId);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
