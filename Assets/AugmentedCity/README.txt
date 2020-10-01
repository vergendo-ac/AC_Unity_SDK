Unity version 2019.2.11 or higher.
Before importing the package, please do the following in the project:

SDK uses ARFoundation systems, so you have to install packages(Window -> Package manager):
ARFoundation 2.1.4
ARCore XR Plugin 2.1.1
ARKit XR Plugin 2.1.2 

Set project settings for Android and ARcore:
Allow 'unsafe' Code - true
Minimum API Level: Android 8.0
Target Architectures - ARM64
Delete VulcanAPI from Graphics API (ARCore doesnt suuport).

Set project settings for iOS and ARKit:
Allow 'unsafe' Code - true
other settings standart for ARKit with Unity.

---------------------------
SDK description:
ACityAPI.cs is the main class, which take frame form AR camera, get gps location, interacts with API and converts the coordinates and data received from the API into Unity ARScene coordinates and convenient string data.
For getting placeholders, where you can place 3d model you have to start "ARLocation" method of ACityAPI.cs and give him an callback Action(other method), which will execute after all data will be received. This method have to recieve array of objects ACityAPI.StickerInfo.

ACityAPI.StickerInfo structure:

StickerInfo {
        Vector3[] positions;   // array of four 3d coordinates in Unity scene of placeholders
        string sPath;             // URL of sticker
        string sText;	         // Text of sticker
}

You can see the implementation of working with the class ACityAPI  in the demo scene in GetPlaceHolders.cs
Also you can change server API in the GameObject to which ACityAPI.cs is attached.
And you can check localization status when localization started with method ACityAPI.getLocalizationStatus(). 
It is implemented through enum structure ACityAPI.LocalizationStatus and can take seven statuses:
NotStarted,
GetGPSData,
NoGPSData,
WaitForAPIAnswer,
ServerError,
CantLocalize,
Ready

You can built DemoScene on Android or iOS and get a simple application that localizes and shows placeholders on scanned buildings.

-----------------------
You can use method ACityAPI.uploadFrame  - to recieve json string with camera and objects from API
.
uploadFrame (string framePath, string apiURL, float langitude, float latitude, Action<string> getJsonCameraObjects)
Where are:
framePath - path of *.jpg file to be uploaded
apiURL - full URL of request, use "http://developer.augmented.city/api/localizer/localize" as default
langitude - langitude GPS data
latitude - latitude GPS data
getJsonCameraObjects - Action which will recieve JSON string data







