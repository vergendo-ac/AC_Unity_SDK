using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using SimpleJSON;
using System.IO;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Video;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using ServerUrlsNs;

public class augApi : MonoBehaviour
{
    // Start is called before the first frame update


    string apiURL; // POST http://developer.augmented.city/api/get_objects?type=placeholder&scene_format=json_3d
    public string[] paths;
    public GameObject camMesh;
    public GameObject dot;
    public Material mmm;
    public GameObject ScaledParent;
    public GameObject LR;
    public GameObject videoPref;
    public GameObject tpBTN;
    public GameObject reloadBTN;
    public GameObject localizedText;
    public GameObject noGPSText;


    public Slider scaleSlider;
    public Text inputSlider;

    public Text scaleText;
    public GameObject ErrorBTN;


    GameObject apiCam;
    GameObject ARCamera;


    public bool VideoLookAtUser;
    public bool invert;
    Texture2D[] tex;
    Transform[] trans;
    int icam = 0;
    float scaleMashtab = 1;
    float nkoef = 1;
    Vector3 camRotBeforeLocalization;
    Vector3 camPosBeforeLocalization;


    GameObject[,] placeHolders;
    GameObject[] lineHolders;
    GameObject[] videoDemos;
    List<GameObject> videoURLs = new List<GameObject>();


    int placeHolderAmount = 0;
    string[] objects;


    ARCameraManager m_CameraManager;
    Texture2D m_Texture;
    int FileCounter = 1;
    bool configurationSetted;

    void Start()
    {
        //   PlayerPrefs.SetString("ApiUrl", "http://developer.vergendo.com:5000");//"developer.augmented.city");
        //PlayerPrefs.SetString("ApiUrl", "developer.augmented.city");
        // direct using of ApiUrl outdated,  see URLsData.Instance.URls
        // PlayerPrefs.SetString("ApiUrl", "http://developer.vergendo.com:15000");

        ARCamera = Camera.main.gameObject;
            m_CameraManager = Camera.main.GetComponent<ARCameraManager>();
            UnityWebRequest.ClearCookieCache();
      //  Camera.main.gameObject.GetComponent<AudioListener>().pause = true;
    }


    void SetCameraConfiguration() {
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


    public unsafe void GetAugFrame(){//(ARCameraFrameEventArgs eventArgs) {



        XRCpuImage image;
        // if (m_CameraManager.TryGetLatestImage(out image))
        if (m_CameraManager.TryAcquireLatestCpuImage(out image))
        {
            var conversionParams = new XRCpuImage.ConversionParams
            {
                // Get the entire image.
                inputRect = new RectInt(0, 0, image.width, image.height),

                // Downsample by 2.
                outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

                // Choose RGBA format.
                outputFormat = TextureFormat.RGBA32,

                // Flip across the vertical axis (mirror image).
                transformation = XRCpuImage.Transformation.MirrorY
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
            m_Texture = new Texture2D(
                conversionParams.outputDimensions.x,
                conversionParams.outputDimensions.y,
                conversionParams.outputFormat,
                false);

            m_Texture.LoadRawTextureData(buffer);
            m_Texture.Apply();

            Texture2D normTex = rotateTexture(m_Texture, false);
            mmm.mainTexture = normTex;
            byte[] bb = normTex.EncodeToJPG();
            string pathToScreen = Application.persistentDataPath + "/" + FileCounter + ".jpg";
            File.WriteAllBytes(pathToScreen, bb);
            Debug.Log(pathToScreen);
            FileCounter++;
            paths[0] = pathToScreen;// Application.persistentDataPath + "/brl.jpg";
            buildScene();

        }

    }




    public void buildScene() {
        UnityWebRequest.ClearCookieCache();

        camRotBeforeLocalization = Camera.main.transform.localRotation.eulerAngles;
        camPosBeforeLocalization = Camera.main.transform.localPosition;
        tex = new Texture2D[paths.Length];
        trans = new Transform[paths.Length];
       // if (FileCounter == paths.Length) FileCounter = 0;
        checkCamera(paths[0], 1);
        //for (int i = 0; i < paths.Length; i++) { checkCamera(paths[i], i + 1); }
    }



public void checkCamera(string urlFile, int a) {
        scaleMashtab = 1;
        nkoef = 1;
        scaleSlider.value = 1;
        StartCoroutine(UploadJPGwithGPS(urlFile, a));

        //        StartCoroutine(UploadJPG(urlFile,a));
    }



    IEnumerator UploadJPGwithGPS(string filePath, int a)
    {
        apiURL = URLSData.Instance.URLs.CurrentUrl +"/api/localizer/localize";// "http://developer.augmented.city:15000/api/localizer/localize";
        byte[] bytes = File.ReadAllBytes(filePath);
        Debug.Log("bytes = " + bytes.Length);

        tex[a - 1] = new Texture2D(2, 2);
        tex[a - 1].LoadImage(bytes);
        mmm.mainTexture = tex[a - 1];


        //   File.WriteAllBytes(Application.persistentDataPath + "/ttt.jpg", bytes);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
      //  string jsona= "{\"gps\":{\"latitude\":59.877427,\"longitude\":30.318510},\"rotation\": 0,\"mirrored\": false}"; // мои
      //  string jsona = "{\"gps\":{\"latitude\":"+PlayerPrefs.GetFloat("Latitude")+",\"longitude\":" + PlayerPrefs.GetFloat("Longitude") + "},\"rotation\": 0,\"mirrored\": false}"; // норм, для билда


        string jsona = "{\"gps\":{\"latitude\":" + PlayerPrefs.GetString("Latitud") + ",\"longitude\":" + PlayerPrefs.GetString("Longitud") + "},\"rotation\": 0,\"mirrored\": false}"; // Альфирина

        //Альфирина -   59.832010, 30.332837
        Debug.Log("" + jsona);
        //59.877427, 30.318510
        form.Add(new MultipartFormFileSection("image", bytes, "test.jpg", "image/jpeg"));
        form.Add(new MultipartFormDataSection("description", jsona));

        byte[] boundary = UnityWebRequest.GenerateBoundary();
        var w = UnityWebRequest.Post(apiURL, form, boundary);
        w.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
        w.SetRequestHeader("Accept", "application/vnd.myplace.v2+json");
        yield return w.SendWebRequest();
        if (w.isNetworkError || w.isHttpError) { print(w.error); }
        else { print("Finished Uploading Screenshot"); }
        Debug.Log(w.downloadHandler.text);
        /*  #if !UNITY_EDITOR && PLATFORM_ANDROID
            File.Delete(filePath);
           #endif*/

       GetCameraTransform(w.downloadHandler.text, a);
    }







    IEnumerator UploadJPG(string filePath, int a)
    {
        apiURL = URLSData.Instance.URLs.CurrentUrl
             + "/api/get_objects?objects_type=placeholder&scene_format=json_3d"; //  objects_
        Debug.Log(apiURL);
            byte[] bytes = File.ReadAllBytes(filePath);
        Debug.Log("bytes = " + bytes.Length);
        tex[a - 1] = new Texture2D(2, 2);
        tex[a - 1].LoadImage(bytes);
       mmm.mainTexture = tex[0];
     //   File.WriteAllBytes(Application.persistentDataPath + "/ttt.jpg", bytes);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
            form.Add(new MultipartFormFileSection("file", bytes, "test.jpg", "image/jpeg"));
            byte[] boundary = UnityWebRequest.GenerateBoundary();
            var w = UnityWebRequest.Post(apiURL, form, boundary);
            w.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
            w.SetRequestHeader("Accept", "application/vnd.myplace.v1+json");
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError){ print(w.error); }
            else{print("Finished Uploading Screenshot");}
            Debug.Log(w.downloadHandler.text);
         /*  #if !UNITY_EDITOR && PLATFORM_ANDROID
             File.Delete(filePath);
            #endif*/

            GetCameraTransform(w.downloadHandler.text, a);
    }





    public void GetCameraTransform(string jsonanswer, int number) {

        var jsonParse = JSON.Parse(jsonanswer);
        float px, py, pz, ox, oy, oz, ow;



        int n = -1; string js;
        if (jsonParse["camera"] != null)
        {
            Debug.Log("PATH  " + jsonParse["objects_info"][0]["sticker"]["path"]);
            Debug.Log("sticker_id  " + jsonParse["objects_info"][0]["sticker"]["sticker_id"]);
            Debug.Log("Object  " + jsonParse["scene"][0]["node"]["id"]);
            Debug.Log("Camera  " + jsonParse["camera"]["position"][0].AsFloat);

            do
            {
                n++;
                js = jsonParse["scene"][n]["node"]["points"][0][0];
                Debug.Log("js node [" + n + "]  - " + js);
            } while (js != null);

            Debug.Log("n =   " + n);

            px = jsonParse["camera"]["position"][0].AsFloat;//px = jsonParse["scene"][n]["camera"]["position"][0].AsFloat;
            py = jsonParse["camera"]["position"][1].AsFloat;
            pz = jsonParse["camera"]["position"][2].AsFloat;
            ox = jsonParse["camera"]["orientation"][0].AsFloat;
            oy = jsonParse["camera"]["orientation"][1].AsFloat;
            oz = jsonParse["camera"]["orientation"][2].AsFloat;
            ow = jsonParse["camera"]["orientation"][3].AsFloat;
            /*
                        Vector3[,] pointsCoord = new Vector3[n, 4];
                        float pxp = 0, pyp = 0, pzp = 0;

                        for (int j = 0; j < n; j++)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                pxp = jsonParse["scene"][j]["node"]["points"][i][0].AsFloat;
                                pyp = jsonParse["scene"][j]["node"]["points"][i][1].AsFloat;
                                pzp = jsonParse["scene"][j]["node"]["points"][i][2].AsFloat;
                                pyp = -pyp;
                                pointsCoord[j, i] = new Vector3(pxp, pyp, pzp);
                                Debug.Log("pxp = " + pxp + ", " + pyp + ", " + pzp);
                            }
                        }


                        /*  Debug.Log(px+", " + py + ", " + pz);
                          Debug.Log(ox + ", " + oy + ", " + oz + ", " + ow);*/


            scaleSlider.value = 1;
            scaleMashtab = 1;
            GameObject newCam = Instantiate(camMesh);                                                       
            newCam.transform.localPosition = new Vector3(px, py, pz);
            newCam.transform.localRotation = new Quaternion(ox, oy, oz, ow);
            newCam.transform.localPosition = new Vector3(px, -newCam.transform.localPosition.y, pz);
            Debug.Log("Camera new: " + newCam.transform.localPosition.x + ", " + newCam.transform.localPosition.y + ", " + newCam.transform.localPosition.z);
            newCam.transform.localRotation = Quaternion.Euler(-newCam.transform.localRotation.eulerAngles.x, newCam.transform.localRotation.eulerAngles.y, -newCam.transform.localRotation.eulerAngles.z);
            newCam.name = "cam" + number;

          //  nkoef = GetNormalizeKoef(pointsCoord, n);
            newCam.transform.localPosition = newCam.transform.localPosition;// * nkoef;

            // trans[number - 1] = newCam.transform;
            if (apiCam != null) Destroy(apiCam);
            apiCam = newCam;

            placeHolderAmount = n;
            placeHolders = new GameObject[n, 4];
            lineHolders = new GameObject[n];
            videoDemos = new GameObject[n];

            for (int j = 0; j < n; j++)
            {
                Vector3[] points = new Vector3[5];
                for (int i = 0; i < 4; i++)
                {
                    px = jsonParse["scene"][j]["node"]["points"][i][0].AsFloat;
                    py = jsonParse["scene"][j]["node"]["points"][i][1].AsFloat;
                    pz = jsonParse["scene"][j]["node"]["points"][i][2].AsFloat;
                    placeHolders[j,i] = Instantiate(dot, apiCam.transform); //objectsDots.transform
                    py = -py;
                    placeHolders[j, i].transform.position = new Vector3(px, py, pz);// * nkoef; ; // was local
                    points[i] = placeHolders[j, i].transform.position;  //new Vector3(px, py, pz);
                    if (i == 0) points[4] = placeHolders[j, i].transform.position; //new Vector3(px, py, pz);

                                                                       // Destroy(newDot);
                }

                //LINES
                  lineHolders[j] = Instantiate(LR);
                  LineRenderer lr = lineHolders[j].GetComponent<LineRenderer>();
                  lr.positionCount = points.Length;
                  lr.SetPositions(points);
                  lineHolders[j].transform.SetParent(apiCam.transform);
                  

                //---VideoPlayer


                GameObject temp1 = Instantiate(dot, apiCam.transform);
                    temp1.transform.position = points[0];
                    GameObject temp2 = Instantiate(dot, apiCam.transform);
                    temp2.transform.position = new Vector3(points[1].x, points[0].y, points[1].z);
                    GameObject temp3 = Instantiate(dot, apiCam.transform);
                    temp3.transform.position = points[2];
                    Vector3 raznp = (points[0] - points[2]) / 2;
                    Debug.Log("magnit1 = " + Vector3.Magnitude(points[0] - points[1]));
                GameObject vp = Instantiate(videoPref, apiCam.transform);
                    vp.transform.position = temp1.transform.position;
                    vp.transform.SetParent(temp1.transform);
                    temp1.transform.LookAt(temp2.transform);
                    vp.transform.position = points[0] - raznp;
                    vp.transform.localEulerAngles = new Vector3 (vp.transform.localEulerAngles.x, vp.transform.localEulerAngles.y + 90, vp.transform.localEulerAngles.z);
                vp.transform.SetParent(apiCam.transform);

                vp.transform.localEulerAngles = new Vector3(0, vp.transform.localEulerAngles.y, 0);
                    vp.transform.localScale = (vp.transform.localScale * Vector3.Magnitude(points[0] - points[1]));
                videoDemos[j] = vp;

                string idnode = "" + jsonParse["scene"][j]["node"]["id"];
                for (int x = 0; x < n; x++) {
                    string idobj = "" + jsonParse["objects_info"][x]["sticker"]["sticker_id"];

                    if (idobj.Contains(idnode)) {
                        string path = "" + jsonParse["objects_info"][j]["sticker"]["path"];
                        if (path.Contains(".mp4"))
                        {
                            GameObject urlVid = Instantiate(vp, apiCam.transform);
                            VideoPlayer vidos = urlVid.GetComponentInChildren<VideoPlayer>();
                            vidos.source = VideoSource.Url;
                            vidos.url = path;
                            videoURLs.Add(urlVid);
                        }
                        else { Debug.Log("path is not video mp4");
                        }
                    }
                }
                                                     
                Debug.Log("sticker_id  " + jsonParse["objects_info"][0]["sticker"]["sticker_id"]);
                Debug.Log("Object  " + jsonParse["scene"][0]["node"]["id"]);


                Destroy(temp1); Destroy(temp2); 
                Destroy(temp3);
                //-----
            }
            turnOffPlaceholders(false);
            SetApiCameraToNewTransform(0);
            turnOffVideoDemos(false);
            reloadBTN.SetActive(true);
            localizedText.SetActive(true);
        }
        else { Debug.Log("ERROR localize");
            ErrorBTN.SetActive(true);
            tpBTN.SetActive(true);
        }

    }


    public void SetApiCameraToNewTransform(int objNumber) {

        apiCam.transform.localPosition = Camera.main.transform.localPosition;     
        apiCam.transform.eulerAngles = camRotBeforeLocalization;
        ScaledParent.transform.localPosition = Camera.main.transform.localPosition;
        apiCam.transform.SetParent(ScaledParent.transform);
        scaleMashtab = 1;

        /*    Camera.main.transform.localPosition = trans[objNumber].position;
        Camera.main.transform.rotation = trans[objNumber].localRotation;
        mmm.mainTexture = tex[objNumber];*/
    }


    public void turnOffVideoDemos(bool setup) {
        if (videoDemos != null)
        {
            foreach (GameObject go in videoDemos)
            {
                go.SetActive(setup);
            }

            foreach (GameObject go in videoURLs)
            {
                go.SetActive(!setup);
            }
        }
    }

    public void turnOffPlaceholders(bool pb)
    {
        if (placeHolderAmount > 0)
        {
            for (int j = 0; j < placeHolderAmount; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    placeHolders[j, i].SetActive(pb);
                    lineHolders[j].SetActive(pb);
                }

            }
        }
    }

    /* public void NextCam() {
         if (icam < tex.Length)
         {
             SetApiCameraToNewTransform(icam); 
         }
         else {
             icam = 0;
             SetApiCameraToNewTransform(icam);
         }
         icam++;
     }*/

    public float GetNormalizeKoef(Vector3[,] points, int places) {
        float maxr = 0;
        for (int i = 0; i < places; i++) {
            for (int j = 0; j < 4; j++)
            {
                maxr = Mathf.Max(maxr, Mathf.Abs(points[i, j].x));
                maxr = Mathf.Max(maxr, Mathf.Abs(points[i, j].y));
                maxr = Mathf.Max(maxr, Mathf.Abs(points[i, j].z));
            }
        }
        Debug.Log("maxR = " + maxr);

        return 1/maxr;
    }


    void Update()
    {
       /* if (scaleMashtab != 1)
        {*/
            ScaledParent.transform.localScale = new Vector3(scaleSlider.value * scaleMashtab, scaleSlider.value * scaleMashtab, scaleSlider.value * scaleMashtab);
            scaleText.text = "" + scaleSlider.value;
        // }

        if (PlayerPrefs.GetInt("LocLoaded") == 1 && ARCamera.transform.localPosition.x != 0)
        {
            tpBTN.SetActive(true);
            PlayerPrefs.SetInt("LocLoaded", 0);

        }
        else if (PlayerPrefs.GetInt("LocLoaded") < 0) {
            noGPSText.SetActive(true);
            reloadBTN.SetActive(true);

        }
    }

    public void SetSliderMax() {
        scaleSlider.maxValue = float.Parse(inputSlider.text);
    }

    public void SetLookVideo(bool look) {
        VideoLookAtUser = look;
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


}

// file:///C:/Users/timew/OneDrive/Desktop/ChryslerBuilding.jpg


//  w.SetRequestHeader("Content-Type", "multipart/form-data");//; boundary=" + System.Text.Encoding.UTF8.GetString(boundary));


// w.uploadHandler.contentType = "multipart/form-data; boundary=" + System.Text.Encoding.UTF8.GetString(boundary);
// w.useHttpContinue = false;