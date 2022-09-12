using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using ServerUrlsNs;

public class ZoneController : MonoBehaviour
{
    public class Zone
    {
        public string latitude;
        public string longitude;
        public string radiusKm;

        public Zone(string lat, string lon, string rad) 
        {
            this.latitude  = lat;
            this.longitude = lon;
            this.radiusKm  = rad;
        }
    }

    public class PlaceInfo
    {
        public Zone   zone;                         // zone params
        public string placeName;                    // zone name
        // aux zone params
        public string skin;
        public string noInstructions;
        public string noStartAR;

        public PlaceInfo(string pl, string lat = null, string lon = null, string rad = null)
        {
            placeName = pl;
            zone = new Zone(lat, lon, rad);
        }
    }


    const float permissionTime = 8;
    float permissionTimer = 0;

    float lat = 0;
    float lon = 0;
    bool  geoStarted;

    UISplashController uispc;

    public ServerURLs URLs;
    string apiURL => URLs.CurrentUrl;

    void Start()
    {
        uispc = GetComponent<UISplashController>();
        string ver = Application.version;
        if (PlayerPrefs.HasKey("bver"))
        {
            if (!(PlayerPrefs.GetString("bver").Equals(ver)))
            {
                UnityWebRequest.ClearCookieCache();
                Caching.ClearCache();
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetString("bver", ver);
            }
        }
        else
        {
            UnityWebRequest.ClearCookieCache();
            Caching.ClearCache();
            //PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("bver", ver);
        }

        Debug.Log("Version " + Application.version + " bver " + ver);

#if PLATFORM_ANDROID || UNITY_IOS
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
#endif
        Input.location.Start();
    }

    void FixedUpdate()
    {
        permissionTimer += Time.fixedDeltaTime;
#if PLATFORM_ANDROID || UNITY_IOS
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation) && !geoStarted)
        {
            StartCoroutine(GetUIParams(AppParamsParser));
            geoStarted = true;
        }
#endif
        if (!geoStarted && permissionTimer > permissionTime) 
        {
            uispc.NoGeoPermisson();
            geoStarted = true;
        }
    }

    IEnumerator CheckLocationS()
    {
        if (!Input.location.isEnabledByUser) {
            Debug.Log("GEO is disabled!");
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
        // Service hasn't initialized for 2 seconds
        if (maxWait < 1)
        {
            Debug.Log("GEO: timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("GEO: Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("GEO: la=" + Input.location.lastData.latitude
                      + " lo:" + Input.location.lastData.longitude
                      + " al:" + Input.location.lastData.altitude
                      + " hd:" + Input.location.lastData.horizontalAccuracy
                      + " tm:" + Input.location.lastData.timestamp);
            lat = Input.location.lastData.latitude;
            lon = Input.location.lastData.longitude;
        }
    }

    public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
    {
        double rlat1 = Math.PI * lat1 / 180;
        double rlat2 = Math.PI * lat2 / 180;
        double theta = lon1 - lon2;
        double rtheta = Math.PI * theta / 180;
        double dist =
            Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
            Math.Cos(rlat2) * Math.Cos(rtheta);
        dist = Math.Acos(dist);
        dist = dist * 180 / Math.PI;
        dist = dist * 60 * 1.1515;
        switch (unit)
        {
            case 'K': //Kilometers -> default
                return dist * 1.609344;
            case 'N': //Nautical Miles 
                return dist * 0.8684;
            case 'M': //Miles
                return dist;
        }
        return dist;
    }

    IEnumerator GetUIParams(Action<string> getJsonParams)
    {
#if !UNITY_EDITOR
        yield return StartCoroutine(CheckLocationS());
#endif
        string req = apiURL + "/media/resources/ACV/props_by_gps.json"; // Default params; molfetta.props_by_gps.json
        var sw = UnityWebRequest.Get(req);
        yield return sw.SendWebRequest();

        if (sw.isNetworkError || sw.isHttpError)
        {
            Debug.Log("GetUIParams: err=" + sw.error);
        }
        else
        {
            Debug.Log("GetUIParams: json=" + sw.downloadHandler.text);
            getJsonParams(sw.downloadHandler.text);
        }
    }

    void AppParamsParser(string jsonS)
    {
        var jsonParse = JSON.Parse(jsonS);
        string pl, la, lo, ra;
        int count = -1;
        List<PlaceInfo> placeInfoList = new List<PlaceInfo>();
        do
        {
            count++;
            pl = jsonParse[count]["place"];
            la = jsonParse[count]["zone"]["latitude"];
            lo = jsonParse[count]["zone"]["longitude"];
            ra = jsonParse[count]["zone"]["radius_km"];
            if (pl != null || (la != null && lo != null))
            {
                PlaceInfo newPlace = new PlaceInfo(pl, la, lo, ra);

                newPlace.skin           = jsonParse[count]["skin"];
                newPlace.noInstructions = jsonParse[count]["no_instructions"];
                newPlace.noStartAR      = jsonParse[count]["no_start-AR"];
				
                placeInfoList.Add(newPlace);

				Debug.Log("AppParamsParser: zone[" + count + "] - " + placeInfoList[count]);
            } else
				break;
        } while (true);
		
        Debug.Log("AppParamsParser: #zones = " + placeInfoList.Count);

        DoCheckLocation(placeInfoList.ToArray());
    }

    public void DoCheckLocation(PlaceInfo[] places)
    {
        PlaceInfo rPlace = null;                    // the right place just within a zone
        PlaceInfo oPlace = null;                    // other place as a separate zone "all other places", outside of all other zones
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        for (int i = 0; i < places.Length; i++) 
        {
            if (places[i].placeName.ToLower().Contains("all ") ||
                string.IsNullOrEmpty(places[i].zone.radiusKm))
            {
                oPlace = places[i];
                Debug.Log("DoCheckLocation: found oPlace=" + oPlace.placeName + ", [" + i + "]");
            }
            else
            {
                float la = 0;
                float.TryParse(places[i].zone.latitude, out la);
                float lo = 0;
                float.TryParse(places[i].zone.longitude, out lo);
                float ra = 0;
                float.TryParse(places[i].zone.radiusKm, out ra);
                Debug.Log("DoCheckLocation: place[" + i + "] " + places[i].placeName + ", geo: la=" + la + ", lo=" + lo + ", ra=" + ra);

                if (DistanceTo(lat, lon, la, lo) < ra)
                {
                    rPlace = places[i];
                    Debug.Log("DoCheckLocation: found rPlace=" + rPlace.placeName + ", [" + i + "]");
                }
            }
        }

        if (rPlace != null)
        {
            SetPlace(rPlace);                       // we found zone that fits the current location
        }
        else if (oPlace != null) 
        { 
            SetPlace(oPlace);                       // we found zone that fits the current location, outside of all others
        }
        else if (places.Length >= 1)                // if there're at least one entry, which doesn't fit the current location
        { 
            SetNotInPlace();
        }
        else
        {
            PlaceInfo defParams = new PlaceInfo("");
            SetPlace(defParams);                    // the file has no zones, activate default params
        }

        // Finally set UI panels in according to the params
        uispc.SetZoneControllerUI();
    }

    void SetNotInPlace()
    {
        PlayerPrefs.SetInt("NotInPlace", 1);
    }

    void SetPlace(PlaceInfo place) 
    {
        PlayerPrefs.SetInt("NotInPlace", 0);

        if (!string.IsNullOrEmpty(place.skin))
        {
            PlayerPrefs.SetString("skin", place.skin);
        }
        else
        {
            PlayerPrefs.SetString("skin", "");
        }

        if (!string.IsNullOrEmpty(place.noStartAR) && place.noStartAR.Equals("1"))
        {
            PlayerPrefs.SetInt("NoStartAR", 1);
        }
        else
        {
            PlayerPrefs.SetInt("NoStartAR", 0);
        }

        if (!string.IsNullOrEmpty(place.noInstructions) && place.noInstructions.Equals("1"))
        {
            PlayerPrefs.SetInt("Instruction", 0);
            StartCoroutine(StartARScene());
        }
        else
        {
            PlayerPrefs.SetInt("Instruction", 1);
        }
    }

    IEnumerator StartARScene()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
