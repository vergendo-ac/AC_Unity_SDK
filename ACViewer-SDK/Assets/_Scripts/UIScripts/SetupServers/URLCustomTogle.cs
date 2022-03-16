using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class URLCustomTogle : URLTogle
{
   [SerializeField]
   InputField inputField;
    //  public string ggg;
    public override void SetCaption(string newCaption)
    {
        inputField.text = newCaption;
    }

    public string GetEnteredUrl() => inputField.text;



}
