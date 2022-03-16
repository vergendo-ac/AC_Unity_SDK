 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ServerUrlsNs
{

    public class URLSData : MonoBehaviour
    {
        public static URLSData Instance { get; private set; }
        public ServerURLs URLs;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Debug.LogError("Pseudo-singletone already initialized");
            URLs.Load();
        }

    }
}
