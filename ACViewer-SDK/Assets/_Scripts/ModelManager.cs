using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelManager : MonoBehaviour
{
    string bundleName;
    public GameObject ABloader;
    public string modelPath;
    GameObject activeModel;
    GetPlaceHoldersDev gph;
    UIManager uim;
    public GameObject activeButtonImage;
    public GameObject groundPlane;
    public GameObject planeForObjects;
    public GameObject shadowForObjects;

    public Color gk;
    public Image actImage;
    public List<AssetBundle> bundles = new List<AssetBundle>();
    public List<string> loadingBunles = new List<string>();
    bool editModeOn;
    public float timeForLongTap = 2f;
    GameObject modelToDelete;
    string objectIdToDelete;
    GameObject ground;
    PlaneManager pm;
    [HideInInspector]
    public GameObject pl;
    [HideInInspector]
    public GameObject shadowObj;

    void Start()
    {
        bundleName = "trans2tank";  //FixMe: why?
        gph = GetComponent<GetPlaceHoldersDev>();
        uim = GetComponent<UIManager>();
        pm  = GetComponent<PlaneManager>();

        //modelPath = Application.streamingAssetsPath + "/";
        //Debug.Log(Application.streamingAssetsPath);
    }

    public void ClearBundles()
    {
        bundles.Clear();
    }

    public void setModel(string bName)
    {
        bundleName = bName;
        Debug.Log($"ModelManager::setModel('{bundleName}')");
    }


    public void firstButton(GameObject act)
    {
        activeButtonImage = act;
        activeButtonImage.SetActive(true);
        actImage = activeButtonImage.GetComponentInChildren<Image>();
        setActiveButton(activeButtonImage);
        setColorImage(actImage);
    }

    public void setActiveButton(GameObject ImageObj)
    {
        activeButtonImage.SetActive(false);
        ImageObj.SetActive(true);
        //ImageObj.GetComponentInParent<Image>().color = Color.white;
        activeButtonImage = ImageObj;
    }

    public void setColorImage(Image imag)
    {
        actImage.color = gk;
        imag.color = Color.white;
        actImage = imag;
    }

    public void CreateNewModel()
    {
        GameObject aRcamera;
        aRcamera = Camera.main.gameObject;
        GameObject go = new GameObject("temp");
        go.transform.position = aRcamera.transform.position;
        go.transform.Translate(aRcamera.transform.forward * 3.5f);  // create it in 3.5m ahead of the camera

        float yGround = pm.getPlaneY();
        Debug.Log("NewObj: yGround=" + yGround);

        go.transform.position = new Vector3(go.transform.position.x,
                                            yGround,
                                            go.transform.position.z);

        activeModel = Instantiate(ABloader);
        activeModel.GetComponent<AssetLoader>().ABName = bundleName;
        activeModel.GetComponent<Mover>().setLocked(false);
        activeModel.transform.position = go.transform.position;

        pl = Instantiate(planeForObjects);
        pl.transform.position = new Vector3(activeModel.transform.position.x,
                                            activeModel.transform.position.y + 0.5f,  // make it above the ground by 0.5m
                                            activeModel.transform.position.z);
        ground = Instantiate(groundPlane);
        groundPlane.transform.position = new Vector3(aRcamera.transform.position.x,
                                                     yGround,
                                                     aRcamera.transform.position.z);
        shadowObj = Instantiate(shadowForObjects);
        shadowObj.transform.position = activeModel.transform.position;
        Destroy(go);
    }

    public bool GetEditMode()
    {
        return editModeOn;
    }

    public void SetEditMode(bool mode)
    {
        editModeOn = mode;
    }

}
