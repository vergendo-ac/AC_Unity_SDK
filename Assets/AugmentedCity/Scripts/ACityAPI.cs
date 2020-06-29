using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.Networking;
using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using SimpleJSON;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Android;



public class ACityAPI : MonoBehaviour
{

    public class StickerInfo {
        public Vector3[] positions;
        public string sPath;
        public string sText;
    }

    public enum LocalizationStatus
    {
        NotStarted,
        GetGPSData,
        NoGPSData,
        WaitForAPIAnswer,
        ServerError,
        CantLocalize,
        Ready
    }

    bool editorTestMode;
    public string ServerAPI = "http://developer.augmented.city/api/localizer/localize";

    Vector3 cameraRotationInLocalization;
    Vector3 cameraPositionInLocalization;
    GameObject ARCamera;
    ARCameraManager m_CameraManager;
    bool startedLocalization;
    bool configurationSetted;
    bool GPSlocation;
    string apiURL;
    Action<StickerInfo[]> getStickersAction;

    
    LocalizationStatus localizationStatus = LocalizationStatus.NotStarted;

    void Start()
    {
        ARCamera = Camera.main.gameObject;
        m_CameraManager = Camera.main.GetComponent<ARCameraManager>();
        setApiURL(ServerAPI);
        #if PLATFORM_ANDROID || UNITY_IOS
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }
        #endif
    }

    void SetCameraConfiguration()
    {
        using (var configurations = m_CameraManager.GetConfigurations(Allocator.Temp))
        {
            Debug.Log("configurations.Length =   " + configurations.Length);

            // Get that configuration by index
            var configuration = configurations[configurations.Length - 1];
            for (int i = 0; i < configurations.Length; i++)
            {
                Debug.Log("Conf.height = " + configurations[i].height + ";  Conf.width = " + configurations[i].width + ";  conf.framerate = " + configurations[i].framerate);
            }
            // Make it the active one
            m_CameraManager.currentConfiguration = configuration;
        }
        configurationSetted = true;
    }

    public unsafe string CamGetFrame()
    {
        XRCameraImage image;
        if (m_CameraManager.TryGetLatestImage(out image))
        {
            var conversionParams = new XRCameraImageConversionParams
            {
                // Get the entire image
                inputRect = new RectInt(0, 0, image.width, image.height),

                // Downsample by 2
                outputDimensions = new Vector2Int(image.width, image.height),

                // Choose RGBA format
                outputFormat = TextureFormat.RGBA32,

                // Flip across the vertical axis (mirror image)
                transformation = CameraImageTransformation.MirrorY
            };

            // See how many bytes we need to store the final image.
            int size = image.GetConvertedDataSize(conversionParams);

            // Allocate a buffer to store the image
            var buffer = new NativeArray<byte>(size, Allocator.Temp);

            // Extract the image data

            image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);
            Debug.Log("buffer.Length" + buffer.Length);
            // The image was converted to RGBA32 format and written into the provided buffer
            // so we can dispose of the CameraImage. We must do this or it will leak resources.
            image.Dispose();

            // At this point, we could process the image, pass it to a computer vision algorithm, etc.
            // In this example, we'll just apply it to a texture to visualize it.

            // We've got the data; let's put it into a texture so we can visualize it.
            Texture2D m_Texture = new Texture2D(
                conversionParams.outputDimensions.x,
                conversionParams.outputDimensions.y,
                conversionParams.outputFormat,
                false);

            m_Texture.LoadRawTextureData(buffer);
            m_Texture.Apply();

            Texture2D normTex = rotateTexture(m_Texture, false);
            byte[] bb = normTex.EncodeToJPG();
            string pathToScreen = Application.persistentDataPath + "/augframe.jpg";
            if (File.Exists(pathToScreen)) File.Delete(pathToScreen);
            File.WriteAllBytes(pathToScreen, bb);
            Debug.Log(pathToScreen);
            return pathToScreen;
        }
        return null;
    }

    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    public void camLocalize(string jsonanswer)
    {
        var jsonParse = JSON.Parse(jsonanswer);
        float px, py, pz, ox, oy, oz, ow;
        int nodeAmount = -1; string js;
        if (jsonParse["camera"] != null)
        {
            Debug.Log("PATH  " + jsonParse["objects_info"][0]["sticker"]["path"]);
            Debug.Log("sticker_id  " + jsonParse["objects_info"][0]["sticker"]["sticker_id"]);
            Debug.Log("Object  " + jsonParse["scene"][0]["node"]["id"]);
            Debug.Log("Camera  " + jsonParse["camera"]["position"][0].AsFloat);

            do
            {
                nodeAmount++;
                js = jsonParse["scene"][nodeAmount]["node"]["points"][0][0];
                Debug.Log("js node [" + nodeAmount + "]  - " + js);
            } while (js != null);

            Debug.Log("nodeAmount =   " + nodeAmount);
            px = jsonParse["camera"]["position"][0].AsFloat;
            py = jsonParse["camera"]["position"][1].AsFloat;
            pz = jsonParse["camera"]["position"][2].AsFloat;
            ox = jsonParse["camera"]["orientation"][0].AsFloat;
            oy = jsonParse["camera"]["orientation"][1].AsFloat;
            oz = jsonParse["camera"]["orientation"][2].AsFloat;
            ow = jsonParse["camera"]["orientation"][3].AsFloat;
            GameObject newCam = new GameObject("tempCam"); 
            newCam.transform.localPosition = new Vector3(px, py, pz);
            newCam.transform.localRotation = new Quaternion(ox, oy, oz, ow);
            newCam.transform.localPosition = new Vector3(px, -newCam.transform.localPosition.y, pz);
            Debug.Log("Camera new: " + newCam.transform.localPosition.x + ", " + newCam.transform.localPosition.y + ", " + newCam.transform.localPosition.z);
            newCam.transform.localRotation = Quaternion.Euler(-newCam.transform.localRotation.eulerAngles.x, newCam.transform.localRotation.eulerAngles.y, -newCam.transform.localRotation.eulerAngles.z);


            GameObject[,] placeHolders = new GameObject[nodeAmount, 4];
            StickerInfo[] stickers = new StickerInfo[nodeAmount];

            for (int j = 0; j < nodeAmount; j++)
            {
                stickers[j] = new StickerInfo();
                Debug.Log("Error?? "+ j + jsonParse["objects_info"][j]["sticker"]["path"]);
                Debug.Log("Error?? " + j + jsonParse["objects_info"][j]["sticker"]["sticker_text"]);

                for (int i = 0; i < 4; i++)
                {
                    px = jsonParse["scene"][j]["node"]["points"][i][0].AsFloat;
                    py = jsonParse["scene"][j]["node"]["points"][i][1].AsFloat;
                    pz = jsonParse["scene"][j]["node"]["points"][i][2].AsFloat;
                    placeHolders[j, i] = new GameObject("Placeholder" + j +" " + i);
                    placeHolders[j, i].transform.SetParent(newCam.transform);
                    py = -py;
                    placeHolders[j, i].transform.position = new Vector3(px, py, pz);
                }
                string idnode = "" + jsonParse["scene"][j]["node"]["id"];
                for (int x = 0; x < nodeAmount; x++)
                {
                    string idobj = "" + jsonParse["objects_info"][x]["sticker"]["sticker_id"];
                    if (idobj.Contains(idnode))
                    {
                        stickers[j].sPath = "" + jsonParse["objects_info"][x]["sticker"]["path"];
                        stickers[j].sText = "" + jsonParse["objects_info"][x]["sticker"]["sticker_text"];
                    }
                }
            }
            newCam.transform.position = cameraPositionInLocalization;
            newCam.transform.eulerAngles = cameraRotationInLocalization;

            for (int j = 0; j < nodeAmount; j++)
            {
                stickers[j].positions = new Vector3[4];

                for (int i = 0; i < 4; i++)
                {
                    stickers[j].positions[i] = placeHolders[j, i].transform.position;
                }
            }
            localizationStatus = LocalizationStatus.Ready;
            Destroy(newCam);
            getStickersAction(stickers);
        }
        else {
            Debug.Log("Cant localize");
            localizationStatus = LocalizationStatus.CantLocalize;
            getStickersAction(null);
        }
    }

    public void ARLocation(Action<StickerInfo[]> getStickers)
    {
        if (!configurationSetted) SetCameraConfiguration();
        getStickersAction = getStickers;
        StartCoroutine(Locate(firstLocalization));
    }

    public void firstLocalization(float langitude, float latitude, string path, Action<StickerInfo[]> getStickers) {
        UnityWebRequest.ClearCookieCache();
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        if (getStickers !=null) getStickersAction = getStickers;
        string framePath;
        if (!editorTestMode)
        {
            framePath = CamGetFrame();
        }
        else { framePath = path; }
        cameraRotationInLocalization = ARCamera.transform.rotation.eulerAngles;
        cameraPositionInLocalization = ARCamera.transform.position;
        if (framePath != null) {
            StartCoroutine(UploadJPGwithGPS(framePath, apiURL, langitude, latitude));
        }
    }

    void setApiURL(string url) {
        apiURL = url;
    }

    IEnumerator UploadJPGwithGPS(string filePath, string apiURL, float langitude, float latitude)
    {
        localizationStatus = LocalizationStatus.WaitForAPIAnswer;
        byte[] bytes = File.ReadAllBytes(filePath);
        Debug.Log("bytes = " + bytes.Length);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        string jsona = "{\"gps\":{\"latitude\":" + langitude + ",\"longitude\":" + latitude + "},\"rotation\": 0,\"mirrored\": false}";
        Debug.Log("" + jsona);
        form.Add(new MultipartFormFileSection("image", bytes, "test.jpg", "image/jpeg"));
        form.Add(new MultipartFormDataSection("description", jsona));
        byte[] boundary = UnityWebRequest.GenerateBoundary();
        var w = UnityWebRequest.Post(apiURL, form, boundary);
        w.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
        w.SetRequestHeader("Accept", "application/vnd.myplace.v2+json");
        yield return w.SendWebRequest();
        if (w.isNetworkError || w.isHttpError) { print(w.error); localizationStatus = LocalizationStatus.ServerError;}
        else { print("Finished Uploading Screenshot"); }
        Debug.Log(w.downloadHandler.text);
        camLocalize(w.downloadHandler.text);
    }


    IEnumerator Locate(Action<float, float, string, Action<StickerInfo[]>> getLocData)
    {
        Debug.Log("Started Locate GPS");
        localizationStatus = LocalizationStatus.GetGPSData;
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("geo not enabled"); PlayerPrefs.SetInt("LocLoaded", -1);
            yield break;
        }
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            localizationStatus = LocalizationStatus.NoGPSData;
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            PlayerPrefs.SetInt("LocLoaded", -2);
            localizationStatus = LocalizationStatus.NoGPSData;
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            getLocData(Input.location.lastData.latitude, Input.location.lastData.longitude, null, null);


        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    public LocalizationStatus getLocalizationStatus() { return localizationStatus; }
}
