using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupServersBtn : MonoBehaviour
{
    const string pass = "acbari";

    [SerializeField]
    SetupServersPanel panel;

    [SerializeField]
    GameObject passwordPanel;

    [SerializeField]
    Button btn;

    private void Start()
    {
        btn.onClick.AddListener(OnButtonClick);
        if (panel == null) {
            panel = FindObjectOfType<SetupServersPanel>(true);
        }
    }

    void OnButtonClick()
    {
        if (PlayerPrefs.HasKey("serpass"))
        {
            OpenChangeServerPanel();
        }
        else
        {
            passwordPanel.SetActive(true);
        }
    }

    public void CheckPassword(InputField ifield) {
        if (ifield.text.Contains(pass)) 
        {
            passwordPanel.SetActive(false);
            PlayerPrefs.SetInt("serpass", 1);
            OpenChangeServerPanel();
        }
    }

    void OpenChangeServerPanel() {
        panel.gameObject.SetActive(true);
    }
}
