using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonImageLoader : MonoBehaviour
{
    [HideInInspector] public string url;
    [HideInInspector] public string stickerId;
    [HideInInspector] public string source;
    [HideInInspector] public string height;
    [HideInInspector] public string width;

    UIManager uim;

    [SerializeField]
    RawImage ButtonRawImage;
    
    [SerializeField]
    GameObject buttonCanvas;

    void Start()
    {
    }

    public void SetRawImage(string rawImageURL)
    {
        url = GetLatestImage() ?? rawImageURL;
        StartCoroutine(DownloadImage());
    }

    IEnumerator DownloadImage()
    {
        ButtonRawImage.gameObject.SetActive(true);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("DownloadImage: err=" + request.error);
        }
        else
        {
            SetScale();
            ButtonRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    void SetScale()
    {
        float heightF = 1;
        if (!string.IsNullOrEmpty(height) && float.TryParse(height, out float heightParse))
        {
             heightF = heightParse;
        }

        float widthF = 1;
        if (!string.IsNullOrEmpty(width) && float.TryParse(width, out float widthParse))
        {
             widthF = widthParse;
        }

        buttonCanvas.transform.localScale =
            new Vector3(buttonCanvas.transform.localScale.x * widthF, 
                        buttonCanvas.transform.localScale.y * heightF, 
                        1);
    }


    public void ActivateImage()
    {
        if (source == "nft-api" &&
            int.TryParse(stickerId, out int acvId))
        {
            Debug.Log("Image-sticker button pressed, acvId=" + acvId);
            return;
        }
    }

    public void UpdateImageAfterSceneExit()
    {
        SetRawImage(GetLatestImage() ?? url);
    }

    public string GetLatestImage()
    {
        if (source == "nft-api" &&
            int.TryParse(stickerId, out int acvId))
        {
            // We may ask for the data in react to this signal
            Debug.Log($"Latest image for ACVID {acvId} is: null");
            //return null;
        }
        return null;
    }

}
