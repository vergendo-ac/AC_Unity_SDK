using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupServersBtn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    SetupServersPanel panel;

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
        panel.gameObject.SetActive(true);
    }

    void Update()
    {
    }
}
