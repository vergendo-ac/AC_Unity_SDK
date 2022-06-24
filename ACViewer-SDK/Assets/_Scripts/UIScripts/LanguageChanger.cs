using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private string[] languages; 
    private List <List<GameObject>> textsObjects = new List<List<GameObject>>();


    private RectTransform[] allRect;
    void Start()
    {
        allRect = GameObject.FindObjectsOfType<RectTransform>(true);

        for (int i = 0; i < languages.Length; i++)
        {
            List<GameObject> textObject = new List<GameObject>();
            foreach (RectTransform rt in allRect)
            {
                if (rt.gameObject.name.ToLower().Contains(languages[i]))
                {
                    textObject.Add(rt.gameObject);
                }
            }
            textsObjects.Add(textObject);
        }

        if (Application.systemLanguage == SystemLanguage.Italian)
        {
            Debug.Log("This system is in Italian. ");
            changeLanguage(1);
        }
    }

    void changeLanguage(int languageNumber) {
        if (languageNumber < languages.Length)
        {
            turnOffAllLanguages();
            List<GameObject>[] textsObjectsArray = textsObjects.ToArray();
            for (int i = 0; i < textsObjectsArray.Length; i++)
            {
                foreach (GameObject goText in textsObjectsArray[languageNumber])
                {
                    goText.SetActive(true);
                }
            }
        }
    }

    void turnOffAllLanguages() {
        foreach (List<GameObject> textObject in textsObjects)
        {
            foreach (GameObject goText in textObject)
            {
                goText.SetActive(false);
            }
        }
    }
}
