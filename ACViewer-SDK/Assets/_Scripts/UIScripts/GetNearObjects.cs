using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetNearObjects : MonoBehaviour
{

    const float radius = 200f;
    float lat = 0;
    float lon = 0;
    [SerializeField] private GameObject notInPlacePanel;
    string aPIURL;


    void Start()
    {
        aPIURL = GetComponent<ACityAPIDev>().GetApiURL();
        GetGeoObjects();
    }

    public void GetGeoObjects() 
    {
        StartCoroutine(GetObjects());
    }

    IEnumerator checkLocationS()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("geo not enabled");
        }
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 4;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(0.5f);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            lat = Input.location.lastData.latitude;
            lon = Input.location.lastData.longitude;
        }
    }


    IEnumerator GetObjects()
    {
#if !UNITY_EDITOR
            yield return StartCoroutine(checkLocationS());
#endif
        // Example https://developer.augmented.city/rpc/get_near_placeholders?p_latitude=59.907008&p_longitude=30.298400&p_radius=200
        if (lat == 0 && lon == 0) 
        {
            notInPlacePanel.SetActive(true);
            yield break;
        }
        string request = aPIURL + "/rpc/get_near_placeholders?p_latitude=" + lat + "&p_longitude= " + lon + "&p_radius=" + radius;
        Debug.Log("request GETOBJECTS = " + request);
        var sw = UnityWebRequest.Get(request);
        yield return sw.SendWebRequest();
        if (sw.isNetworkError || sw.isHttpError)
        {
            Debug.Log(sw.error);
        }
        else
        {
            Debug.Log("GETOBJECTS Answer = " + sw.downloadHandler.text);
            if (sw.downloadHandler.text.ToLower().Equals("null"))
            {
                notInPlacePanel.SetActive(true);
            }
            else 
            {
                notInPlacePanel.SetActive(false);
            }
        }
    }


}
