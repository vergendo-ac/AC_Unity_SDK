using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class SkipIntro : MonoBehaviour
{
    public void LoadMainScene() {
        SceneManager.LoadScene(1);
    }
}
