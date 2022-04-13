using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ServerUrlsNs
{

    [System.Serializable]
    public class ServerURLs
    {
        public const int defaultUrlId = 1;
        public const int defaultUrlIndex = defaultUrlId - 1;

        public  int CustomUrlId => CustomUrlIndex + 1;
        public  int CustomUrlIndex = 12;

        public  string LocalizerURL => CurAPIUrl + "/api/localizer/localize";  //"https://developer.augmented.city/api/localizer/localize";

        const string CustomUrlKey = "ApiUrl";
        const string UrlIdKey = "Tnumber";

        public int CurUrlId = defaultUrlId;
        public int CurUrlIndex => CurUrlId - 1;
        public string CurAPIUrl => URLsArr[CurUrlIndex];

        /*
          BundleFullURL = PlayerPrefs.GetString("ApiUrl") + "/media/3d/" + ABName + "/ios/bundle";
          BundleFullURL = PlayerPrefs.GetString("ApiUrl") + "/media/3d/" + ABName + "/android/bundle";
        */
        const string iosPathPart = "/ios/bundle";
        const string androidPathPart = "/android/bundle";
#if UNITY_IOS
        string osPart = iosPathPart;
#endif
#if PLATFORM_ANDROID
        string osPart = androidPathPart;
#endif

        public string GetMediaURL(string assetId)
        {
            string ret = CurAPIUrl + @"/media/3d/" + assetId + osPart;
            return ret;
        }

        public void SetCurUrlIndex(int newIndex)
        {
            CurUrlId = IndexToUrlId(newIndex);
            Debug.Log($"Assigned CurUrlId= {CurUrlId} ");
        }

        public void SetCurUrlId(int newUrlId)
        {
            CurUrlId = newUrlId;
            Debug.Log($"Assigned CurUrlId= {CurUrlId} ");
        }

        public string CurrentUrl => URLsArr[CurUrlIndex];

        public int IndexToUrlId(int urlIndex)
        {
            return urlIndex + 1;
        }

        public int UrlIdToIndex(int urlId)
        {
            return urlId - 1;
        }

        public void SetCustomUrl(string newUrl)
        {
            URLsArr[CustomUrlIndex] = newUrl;
        }

        /*
            1 https://developer.augmented.city
            2 http://185.162.94.228:25000
            3 https://developer.mirror.augmented.city
            4 http://185.162.94.228:35000
            5 https://developer.reserve.augmented.city
            6 http://178.249.69.197:36000
            7 http://178.249.69.197:37000
            8 https://developer.testing.augmented.city
            9 http://185.162.94.228:15000
            10 http://185.162.94.228:16000
            11 http://185.162.94.228:17000
        */
       //[SerializeField]
       public string[] URLsArr = {
           "https://developer.augmented.city",             // id = 0,1 (defaultUrlId)
           "http://185.162.94.228:25000",                  // id = 2
           "https://developer.mirror.augmented.city",      // id = 3
           "http://185.162.94.228:35000",                  // id = 4
           "https://developer.reserve.augmented.city",     // id = 5
           "http://178.249.69.197:36000",                  // id = 6
           "http://178.249.69.197:37000",                  // id = 7
           "https://developer.testing.augmented.city",     // id = 8
           "http://185.162.94.228:15000",                  // id = 9
           "http://185.162.94.228:16000",                  // id = 10
           "http://185.162.94.228:17000",                  // id = 11
           "custom url"                                    // customUrl id = 12, index = 11
        };
        public int Count => URLsArr.Length;

        public void Load()
        {
            CurUrlId = PlayerPrefs.GetInt(UrlIdKey, defaultUrlId);
            Debug.Log($"ServerURLs::Load: Loading from playerPrefs {UrlIdKey} = {CurUrlId}");
            if (CurUrlId == CustomUrlId) {
                URLsArr[CustomUrlIndex] = PlayerPrefs.GetString(CustomUrlKey, URLsArr[defaultUrlIndex]);
            }
            Debug.Log($"ServerURLs: loading from PlayerPrefs done, id={CurUrlId}/url={CurAPIUrl}");
        }

        public void Save()
        {
            Debug.Log($"ServerURLs::Save: Saving to playerPrefs {UrlIdKey} = {CurUrlId}");
            PlayerPrefs.SetInt(UrlIdKey, CurUrlId);
            if (CurUrlId == CustomUrlId)
            {
                Debug.Log("Saving Custom URL");
                PlayerPrefs.SetString(CustomUrlKey, CurAPIUrl);
            }
        }
    }
}
