using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;

public class GetPlaceHoldersDev : MonoBehaviour
{
    public GameObject dot;
    public GameObject linePrefab;
    public GameObject cantLocalizeImage;
    public GameObject localizedImage;
    public GameObject videoPref;
    public GameObject stickerPref;
    public GameObject stickerFood;
    public GameObject stickerPlace;
    public GameObject stickerShop;

    //public Material devCamMat;
    public string devImagePath;

    List<GameObject> recos = new List<GameObject>();        // cache of created scene of gameobjects by entry 'id'
    List<GameObject> videoDemos = new List<GameObject>();
    List<GameObject> stickerObjects = new List<GameObject>();
    List<GameObject> videoURLs = new List<GameObject>();
    List<GameObject> placeHoldersDotsLines = new List<GameObject>();
    List<GameObject> plyObjects = new List<GameObject>();
    List<GameObject> models = new List<GameObject>();
    List<GameObject> navigateMesh = new List<GameObject>();

    ACityAPIDev acapi;
    Vector3 deltaTranslateVector, deltaRotateVector;
    public bool needScaling;
    bool translateAction;
    Transform movingTransform;
    float moveFrames;
    float frameCounter;
    Vector3 arCamCoordinates, pastArCamCoordinates;
    Quaternion targetRotation, startRotation;
    GameObject aRcamera;
    string lastLocalizedRecoId;
    float timerRelocation;
    float animationTime = 2f;
    UIManager uim;
    GameObject activeReco, modelToServer;

    public TextAsset cloudAsset;
    GameObject pcloud;

    bool ARStarted, relocationCompleted, toShowPlaceHolders, videoDemosTurn, toShowStickers;
    public float timeForRelocation = 20f;           // set reloc time to 20 secs by default

    bool  firstStart      = true;                   // flag if it's first loca is active
    float cantLocTimerDef = 30f;                    // set loca timeout default value to 30 secs
    float cantLocTimer;                             

    void Start()
    {
        cantLocTimer = cantLocTimerDef;             // set loca timeout to 30 secs
        acapi = GetComponent<ACityAPIDev>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        aRcamera = Camera.main.gameObject;
        relocationCompleted = true;
        toShowStickers = true;
        uim = this.GetComponent<UIManager>();

        //acapi.prepareSession(preparationCheck);  //FixMe: ACV has it on, why?
    }

    public void setTimeForRelocation(float tfr) {
        timeForRelocation = tfr;
        timerRelocation = tfr;
    }

    public void setTimeForAnimation(float tfa)
    {
        animationTime = tfa;
    }

    void preparationCheck(bool b, string ans) {
        Debug.Log(ans+"CLIENT");
    }

    public void startDevLocation()   // Test localization from Unity Editor
    {
        if (acapi.editorTestMode) {
            timeForRelocation = 200f;
            PlayerPrefs.SetFloat("TimeForRelocation", timeForRelocation);
        }
        pastArCamCoordinates = arCamCoordinates;
        arCamCoordinates = new Vector3(aRcamera.transform.position.x, aRcamera.transform.position.y, aRcamera.transform.position.z);

        byte[] bytes = File.ReadAllBytes(devImagePath);
        Debug.Log("bytes = " + bytes.Length);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        //devCamMat.mainTexture = tex;  //FixMe: ??????

        //acapi.firstLocalization(59.877427f,  30.318510f, 30, devImagePath, showPlaceHolders); // home 59.877427f, 30.318510f
        //acapi.firstLocalization(59.907458f,  30.298400f, 30, devImagePath, showPlaceHolders); // Balthof
        //acapi.firstLocalization(59.832010f,  30.332837f, 30, devImagePath, showPlaceHolders); // Alf yard 59.832010f, 30.332837f
        //acapi.firstLocalization(59.832050f,  30.334570f, 30, devImagePath, showPlaceHolders); // Alf home 59.83205f, 30.33457f       // 59.832046, 30.334141 // евразия, велосипеды 59.836716, 30.334282
        //acapi.firstLocalization(59.934320f,  30.272610f, 30, devImagePath, showPlaceHolders); // Spb VO-yard vooff1.jpg
        //acapi.firstLocalization(59.933880f,  30.272511f, 30, devImagePath, showPlaceHolders); // Spb VO-street shlacbaum vostreet1.jpg 
        //acapi.firstLocalization(41.122400f,  16.868400f, 30, devImagePath, showPlaceHolders); // Bari cafe (lat=41.1224f, lon=16.8684f)
        //acapi.firstLocalization(43.405290f,  39.955740f, 30, devImagePath, showPlaceHolders); // Sochi 43.404521f,39.954741f 43.404080,39.954735 43.404769,39.954042 43.40529,39.95574
        //acapi.firstLocalization(22.662310f, 114.064710f, 30, devImagePath, showPlaceHolders); // China 22.66231f, 114.06471f.... 47,61163, -122,33717
        //acapi.firstLocalization(22.660012f, 114.054631f,  6, devImagePath, showPlaceHolders); // China new 22.660012f, 114.054631f
        //acapi.firstLocalization(47.611630f,-122.337170f, 30, devImagePath, showPlaceHolders); // Seattle ZARA  // 47.61163f, -122.33717f
        //acapi.firstLocalization(47.610907f,-122.337000f, 30, devImagePath, showPlaceHolders); // Seattle westpark 47.610907, -122.33700
        //acapi.firstLocalization(47.612983f,-122.337657f, 30, devImagePath, showPlaceHolders); // Statue westpark  47.612983, -122.337657 // 955564154, 53827994
        //acapi.firstLocalization(47.612786f,-122.335430f, 30, devImagePath, showPlaceHolders); // Statue westpark  47.612786, -122.335430
        //acapi.firstLocalization(59.9145560f, 30.304109f, 30, devImagePath, showPlaceHolders); // 59.9145560f, 30.304109f - Дом, кресло rec #15142 (календарь)
          acapi.firstLocalization(59.9145560f, 30.304109f, 30, devImagePath, showPlaceHolders); // 59.9145560f, 30.304109f - Дом, кресло rec #15142 (календарь)
        //acapi.firstLocalization(55.756477f,  37.619737f, 30, devImagePath, showPlaceHolders); // Msk metro west 55.756477f, 37.619737f
        //acapi.firstLocalization(55.756680f,  37.619830f, 30, devImagePath, showPlaceHolders); // Msk metro subw 55.756680f, 37.619830f
        //acapi.firstLocalization(59.987274f,  30.197186f, 30, devImagePath, showPlaceHolders); // Sergey Fed home 59.987274f, 30.197186f sergk.jpg
        //acapi.firstLocalization(55.755384f,  37.619539f, 30, devImagePath, showPlaceHolders); // {Nikolskaya-kremlin 55.755384, 37.619539 nikolkp.jpg, nikolkp2.jpg},  Center 55.756512, 37.621499, {Lubyanka 55.759247, 37.625317 - nikol1.jpg}
        //acapi.firstLocalization(59.300038597411586f, 30.021741184295532f, 30, devImagePath, showPlaceHolders);    //{Constantine  59.300038597411586, 30.021741184295532, constantineoutdoor.jpg, constantinedoor.jpg}
        //Milan - rec 15113, 15112
        //acapi.firstLocalization(devLatitude, devLongitude, 30, devImagePath, showPlaceHolders);

        timerRelocation = timeForRelocation;
        ARStarted = true;
        relocationCompleted = false;
    }

    public void startLocalization()
    {
        pastArCamCoordinates = arCamCoordinates;
        arCamCoordinates = new Vector3(aRcamera.transform.position.x, aRcamera.transform.position.y, aRcamera.transform.position.z);
        Debug.Log("ARcam x = " + aRcamera.transform.position.x);
        acapi.ARLocation(showPlaceHolders);
        timerRelocation = timeForRelocation;
        ARStarted = true;
        relocationCompleted = false;
    }

    // Get the normal to a triangle from the three corner points, a, b and c.
    Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        // Find vectors corresponding to two of the sides of the triangle.
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;
        // Cross the vectors to get a perpendicular vector, then normalize it.
        return Vector3.Cross(side1, side2).normalized;
    }

    bool checkVideoOrientation(Vector3 point1, Vector3 point2, Vector3 point3)
    {
        bool isReversedVideo = false;
        //1. Calculate vector product (1-2) and (2-3) that's the plane normal
        //2. Check the sign of Z normal component => if positive (parallel to Z axis), it's forward - it's reversed; otherwise - it's ok.
        Vector3 normal = GetNormal(point1, point2, point3);
        Debug.Log("normal: x: " + normal.x + " y: " + normal.y + " z: " + normal.z);
        if (normal.z > 0) {
            isReversedVideo = true;
        }
        return isReversedVideo;
    }

    void showPlaceHolders(string id, Transform zeroP, ACityAPIDev.StickerInfo[] stickers)
    {
        if (id != null)
        {
            /*
            Debug.Log("zeroPpos = " + zeroP.position.x + "    " + zeroP.position.y + "    " + zeroP.position.z);
            Debug.Log("zeroPori = " + zeroP.eulerAngles.x + "    " + zeroP.eulerAngles.y + "    " + zeroP.eulerAngles.z);
            */
            firstStart = false;
            GameObject placeHolderParent;
            placeHolderParent = checkSavedID(id);

            if (pcloud == null)
            {
                if (pcloud != null)
                {
                    pcloud.transform.position = zeroP.position;
                    pcloud.transform.rotation = zeroP.rotation;
                }
            }

            setTimeForRelocation(PlayerPrefs.GetFloat("TimeForRelocation"));

            if (placeHolderParent == null)          // if it's first time we need to generate a new scene
            {
                /*if (stickers != null)*/           // prepare the main object to be able to add new model into the empty scene
                //{
                    GameObject scaleParent = new GameObject("CamParent-" + id);  // add 'id' into the name
                    scaleParent.transform.position = arCamCoordinates;
                    placeHolderParent = new GameObject(id);
                    placeHolderParent.transform.position = zeroP.position;
                    placeHolderParent.transform.rotation = zeroP.rotation;
                    activeReco = placeHolderParent;
                //}

                if (pcloud != null) {
                    pcloud.transform.SetParent(placeHolderParent.transform);
                }

                if (stickers != null)               // if there're any objects?
                {
                    for (int j = 0; j < stickers.Length; j++)
                    {
                        // Placeholders
                        for (int i = 0; i < 4; i++)
                        {
                            GameObject go = Instantiate(dot, placeHolderParent.transform);
                            go.transform.position = stickers[j].positions[i];
                            placeHoldersDotsLines.Add(go);
                        }
                        // Lines
                        GameObject lineHolder = Instantiate(linePrefab);
                        LineRenderer lr = lineHolder.GetComponent<LineRenderer>();
                        lr.positionCount = 4;
                        lr.SetPositions(stickers[j].positions);
                        lineHolder.transform.SetParent(placeHolderParent.transform);
                        lr.useWorldSpace = false;
                        placeHoldersDotsLines.Add(lineHolder);

                        // VideoPlayer
                        GameObject temp1 = Instantiate(dot, placeHolderParent.transform);
                        temp1.transform.position = stickers[j].positions[0];
                        //Debug.Log("temp1 -> x: " + temp1.transform.position.x + "y: " + temp1.transform.position.y + " z: " + temp1.transform.position.z);
                        GameObject temp2 = Instantiate(dot, placeHolderParent.transform);
                        temp2.transform.position = new Vector3(stickers[j].positions[1].x, stickers[j].positions[1].y, stickers[j].positions[1].z); 
                        //Debug.Log("temp2 -> x: " + temp2.transform.position.x + "y: " + temp2.transform.position.y + " z: " + temp2.transform.position.z);
                        GameObject temp3 = Instantiate(dot, placeHolderParent.transform);
                        temp3.transform.position = stickers[j].positions[2];
                        //Debug.Log("temp3 -> x: " + temp3.transform.position.x + "y: " + temp3.transform.position.y + " z: " + temp3.transform.position.z);
                        Vector3 raznp = (stickers[j].positions[0] - stickers[j].positions[2]) / 2;
                        GameObject vp = Instantiate(videoPref, placeHolderParent.transform);
                        vp.transform.position = temp1.transform.position;
                        vp.transform.SetParent(temp1.transform);
                        temp1.transform.LookAt(temp2.transform);
                        vp.transform.position = stickers[j].positions[0] - raznp;
                        vp.transform.localEulerAngles = new Vector3(vp.transform.localEulerAngles.x, vp.transform.localEulerAngles.y + 90, vp.transform.localEulerAngles.z);
                        vp.transform.SetParent(placeHolderParent.transform);
                        vp.transform.localEulerAngles = new Vector3(0, vp.transform.localEulerAngles.y, 0);
                        vp.transform.localScale = (vp.transform.localScale * Vector3.Magnitude(stickers[j].positions[0] - stickers[j].positions[1]));
                        //fix the reversed video checking the plane orientation
                        bool isReversedVideoSticker = checkVideoOrientation(temp1.transform.position, temp2.transform.position, temp3.transform.position);
                        Debug.Log("isReversedVideoSticker: " + isReversedVideoSticker);
                        if (isReversedVideoSticker)
                        {
                            vp.transform.localEulerAngles = new Vector3(0, vp.transform.localEulerAngles.y + 180, 0);
                        }
                        videoDemos.Add(vp);

                        if (stickers[j] != null)                        // if the sticker object is not failed
                        {
                            bool isVideoSticker =
                                stickers[j].sPath != null &&
                                stickers[j].sPath.Contains(".mp4");

                            bool is3dModel = !isVideoSticker &&
                                (stickers[j].type.ToLower().Contains("3d") ||   // new 3d object format
                                 stickers[j].sSubType.Contains("3dobject") ||   // old 3d object format
                                 (stickers[j].sPath != null &&
                                  stickers[j].sPath.Contains("3dobject"))       // oldest 3d object format
                                );

                            bool is3dModelTransfer =
                                stickers[j].sDescription.ToLower().Contains("transfer") ||
                                stickers[j].subType.ToLower().Contains("transfer");

                            if (isVideoSticker)                         // if it's a video-sticker
                            {
                                GameObject urlVid = Instantiate(vp, placeHolderParent.transform);
                                VideoPlayer vidos = urlVid.GetComponentInChildren<VideoPlayer>();
                                vidos.source = VideoSource.Url;
                                vidos.url = stickers[j].sPath;
                                videoURLs.Add(urlVid);
                            }
                            else if (is3dModel || is3dModelTransfer)    // 3d object or special navi object
                            {
                                GameObject model = Instantiate(GetComponent<ModelManager>().ABloader, placeHolderParent.transform);
                                string bundleName = stickers[j].sText.ToLower();
                                if (stickers[j].type.ToLower().Contains("3d"))      // is it new format
                                {
                                    bundleName = stickers[j].bundleName.ToLower();
                                    if (string.IsNullOrEmpty(bundleName)) {
                                        bundleName = stickers[j].sText.ToLower();  // return back to default bundle name as the 'name'
                                    }
                                }
                                model.GetComponent<AssetLoader>().ABName = bundleName;
                                model.transform.localPosition = stickers[j].mainPositions; // * acapi.tempScale3d;
                                model.transform.localRotation = new Quaternion(
                                    stickers[j].orientations.x,
                                    stickers[j].orientations.y,
                                    stickers[j].orientations.z,
                                    stickers[j].orientations.w);

                                if (stickers[j].sTrajectoryPath.Length > 1)
                                {
                                    Trajectory tr = model.GetComponent<Trajectory>();
                                    tr.go = true;
                                    tr.acapi = acapi;
                                    tr.sTrajectory = stickers[j].sTrajectoryPath;
                                    tr.sTimePeriod = stickers[j].sTrajectoryPeriod;
                                    tr.sOffset = stickers[j].sTrajectoryOffset;
                                }

                                Mover mover = model.GetComponent<Mover>();
                                mover.setLocked(true);
                                mover.objectId = stickers[j].objectId;

                                if (!stickers[j].vertical ||
                                    bundleName.Contains("nograv")) {
                                    mover.noGravity = true;
                                }

                                bool is3dModelNaviData =
                                    bundleName.Contains("navgard") ||
                                    stickers[j].subType.ToLower().Contains("navmesh");

                                if (stickers[j].grounded ||
                                    bundleName.Contains("quar") ||
                                    bundleName.Contains("santa") ||
                                    bundleName.Contains("pavel") ||
                                    bundleName.Contains("gard"))
                                {
                                    mover.landed = true;
                                    if (is3dModelNaviData) {
                                        navigateMesh.Add(model);
                                    }
                                }

                                if (is3dModelTransfer)
                                {
                                    string desc = stickers[j].sDescription;
                                    desc = desc.Replace(" ", "");                       // kill spaces
                                    if (stickers[j].sDescription.ToLower().Contains("transfer")) {
                                        desc = desc.Substring(9, desc.Length - 9);  // exclude starting 'Transfer:'
                                    }
                                    Debug.Log("!!! Descs = " + desc);
                                    string[] descs = desc.Split(',');

                                }

                                /*Debug.Log(j + ". 3dmodel " + stickers[j].sText
                                    + " = " + model.transform.localPosition
                                    + " model.rot = " + model.transform.localRotation
                                    + " stick.ori = " + stickers[j].orientations);*/

                                if (stickers[j].SModel_scale.Length > 0)
                                {
                                    float scale = float.Parse(stickers[j].SModel_scale);
                                    model.transform.localScale = new Vector3(scale, scale, scale);
                                }

                                models.Add(model);                      // store the new just created model
                            }
                            else                                        // other types of objects - info-stickers
                            {
                                GameObject newSticker = null;
                                string checkType = stickers[j].sType.ToLower();
                                if (checkType.Contains("food") || checkType.Contains("restaurant")) {
                                    newSticker = Instantiate(stickerFood, placeHolderParent.transform);
                                } else if (checkType.Contains("place")) {
                                    newSticker = Instantiate(stickerPlace, placeHolderParent.transform);
                                } else if (checkType.Contains("shop")) {
                                    newSticker = Instantiate(stickerShop, placeHolderParent.transform);
                                } else {
                                    newSticker = Instantiate(stickerPref, placeHolderParent.transform);
                                }
                                if (newSticker != null)
                                {
                                    newSticker.transform.position = stickers[j].positions[0] - raznp;
                                    StickerController sc = newSticker.GetComponent<StickerController>();
                                    sc.setStickerInfo(stickers[j]);

                                    stickerObjects.Add(newSticker);     // store the new just created info-sticker
                                }

                            }

                        } // if (stickers[j] != null...)

                        Destroy(temp1);
                        Destroy(temp2);
                        Destroy(temp3);

                    } // for (j < stickers.Length)

                } // if (stickers != null)

                GameObject id3d = get3dFromLocal(id);       //FixMe: ?
                if (id3d != null)
                {
                    GameObject model = Instantiate(GetComponent<ModelManager>().ABloader, placeHolderParent.transform);
                    model.GetComponent<AssetLoader>().ABName = id3d.name;
                    model.transform.localPosition = id3d.transform.position;
                    model.transform.localRotation = id3d.transform.rotation;
                    model.GetComponent<Mover>().setLocked(true);
                    Debug.Log("Loaded pos = " + id3d.transform.position + ", ori = " + id3d.transform.rotation);
                    models.Add(model);
                }

                turnOffVideoDemos(videoDemosTurn);
                turnOffPlaceHolders(toShowPlaceHolders);
                turnOffStickers(toShowStickers);

                localizedImage.SetActive(true);
                placeHolderParent.transform.SetParent(scaleParent.transform);
                recos.Add(placeHolderParent);  // store processed scene into the cache
                uim.Located();

                relocationCompleted = true;
            }
            else // if (placeHolderParent == null)
            {
                Transform scaleParentTransform = placeHolderParent.transform.root;
                //if (needScaling && lastLocalizedRecoId.Contains(id)) {}
                placeHolderParent.SetActive(true);
                GameObject tempScaler = new GameObject("TempScaler");
                tempScaler.transform.position = arCamCoordinates;
                GameObject tempBiasVector = new GameObject("TempBiasVector");
                tempBiasVector.transform.position    = zeroP.position;
                tempBiasVector.transform.eulerAngles = zeroP.eulerAngles;

                tempBiasVector.transform.SetParent(tempScaler.transform);
                tempScaler.transform.localScale = scaleParentTransform.localScale;

                Translocation(placeHolderParent, tempBiasVector.transform, animationTime);
                Destroy(tempScaler);
                Destroy(tempBiasVector);
            }

            lastLocalizedRecoId = id;
        }
        else // if (id != null)
        {
            CantLocalize();
        }
    }

    void cantFindLocations()
    {
        firstStart = true;
        ARStarted  = false;
        uim.localizeProgress.SetActive(false);
        uim.notLocalizedForSomeTime.SetActive(true);
        cantLocTimer = cantLocTimerDef;
    }

    void Translocation(GameObject transObject, Transform targetTransform, float time)
    {
        movingTransform      = transObject.transform;
        moveFrames           = time / Time.fixedDeltaTime;
        frameCounter         = moveFrames;
        deltaTranslateVector = movingTransform.position;
        deltaRotateVector    = targetTransform.position;
        translateAction      = true;
        targetRotation       = targetTransform.rotation;
        startRotation        = movingTransform.rotation;
    }

    void TranslationMover()
    {
        movingTransform.position = Vector3.Lerp(deltaTranslateVector, deltaRotateVector, (moveFrames - frameCounter) / moveFrames);
      //movingTransform.Translate(-deltaTranslateVector);
        movingTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, (moveFrames - frameCounter) / moveFrames);
        frameCounter--;
        if (frameCounter <= 0)
        {
            translateAction = false;
            relocationCompleted = true;
        }
    }

    void FixedUpdate()
    {
        if (translateAction) {
            TranslationMover();
        }
        if (ARStarted)
        {
            timerRelocation = timerRelocation - Time.fixedDeltaTime;
            if ((timerRelocation < 0) && relocationCompleted)
            {
                if (acapi.editorTestMode) {
                    startDevLocation();
                }
                else {
                    startLocalization();
                }
                timerRelocation = timeForRelocation;
            }

            if (firstStart)
            {
                cantLocTimer = cantLocTimer - Time.fixedDeltaTime;
                if (cantLocTimer < 0) {
                    cantFindLocations();
                }
            }
        }
    }


    public List<GameObject> GetAllStickers() {
        return stickerObjects;
    }

    void CantLocalize()
    {
        Debug.Log("Can't localize client or no stickers");
        ACityAPIDev.LocalizationStatus ls = acapi.getLocalizationStatus();
        /*if (recos.Count > 0) timerRelocation = timeForRelocation;
        else */
        timerRelocation = 1.1f;
        relocationCompleted = true;
    }

    public void SetRecoScale(float scale)
    {
        Transform llrt = checkSavedID(lastLocalizedRecoId).transform.root;
        if (llrt != null) {
            llrt.localScale = new Vector3(scale, scale, scale);
        }
    }

    GameObject checkSavedID(string id)  // extract from the cache the gameobject by 'id'
    {
        GameObject reco = null;
        foreach (GameObject go in recos)
        {
            if (go.name.Contains(id)) {
                reco = go;
            }
            else {
                go.SetActive(false);
            }
        }
        return reco;
    }

    public bool GetRelocationState() {
        return relocationCompleted;
    }

    public void turnOffVideoDemos(bool setup)
    {
        videoDemosTurn = setup;
        if (videoDemos != null)
        {
            foreach (GameObject go in videoDemos) {
                go.SetActive(setup);
            }
            foreach (GameObject go in videoURLs) {
                go.SetActive(!setup);
            }
        }
    }

    public void turnOffVideoURL(bool setup)
    {
        foreach (GameObject go in videoURLs) {
            go.SetActive(setup);
        }
    }

    public void turnOffPlaceHolders(bool onOff) {
        toShowPlaceHolders = onOff;
        foreach (GameObject p in placeHoldersDotsLines) {
            p.SetActive(onOff);
        }
    }

    public void turnOffStickers(bool onOff)
    {
        toShowStickers = onOff;
        foreach (GameObject sticker in stickerObjects) {
            sticker.SetActive(onOff);
        }
    }

    public void turnOffModels(bool setup)
    {
        foreach (GameObject go in models) {
            go.SetActive(setup);
        }
    }

    public void turnOffNavMeshes(bool setup)
    {
        foreach (GameObject go in navigateMesh)
        {
            MeshRenderer[] mrs = go.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in mrs) {
                m.enabled = setup;
            }
        }
    }

    public void AddCloudToReco(GameObject go)
    {
        if (activeReco != null)
        {
            go.transform.parent = activeReco.transform;
            plyObjects.Add(go);
        }
        else Debug.Log("activeReco null");
    }

    public void setNewModelObjectId(string objectParams)
    {
        if (modelToServer != null) {
            modelToServer.GetComponent<Mover>().objectId = objectParams;
        }
    }

    public GameObject get3dFromLocal(string id)
    {
        GameObject temp = null;
        if (PlayerPrefs.HasKey(id))
        {
            temp = new GameObject(PlayerPrefs.GetString(id));
            temp.transform.position = new Vector3(
                PlayerPrefs.GetFloat(id + "coordx"),
                PlayerPrefs.GetFloat(id + "coordy"),
                PlayerPrefs.GetFloat(id + "coordz")
                );
            temp.transform.rotation = new Quaternion(
                PlayerPrefs.GetFloat(id + "orix"),
                PlayerPrefs.GetFloat(id + "oriy"),
                PlayerPrefs.GetFloat(id + "oriz"),
                PlayerPrefs.GetFloat(id + "oriw")
                );
        }
        return temp;
    }

    public string getCurrentRecoId() {
        return lastLocalizedRecoId;
    }
}
