### OpenAPI

https://developer.augmented.city/doc#operation/localize

### Requirements

Unity version 2021.1.19f or higher.
Before importing the package, please do the following in the project:

SDK uses ARFoundation systems, so you have to install packages (Window -> Package manager):
- ARFoundation 4.1.9
- ARCore XR Plugin 4.1.9
- ARKit XR Plugin 4.1.9

Set project settings for Android and ARcore:
- Allow 'unsafe' Code - true
- Minimum API Level: Android 8.0
- Target Architectures - ARM64
- Delete VulcanAPI from Graphics API (ARCore doesn’t support).

Set project settings for iOS and ARKit:
- Allow 'unsafe' Code - true
- other settings standart for ARKit with Unity.


### Usage details

ACityAPI.cs is the main class, which takes frame from AR camera, gets gps location,
 interacts with API and converts the coordinates and data received from the API into
 Unity ARScene coordinates and convenient string data.
For getting placeholders, where you can place 3D model you have to start "ARLocation"
 method of ACityAPI.cs and give it a callback Action (other method), which will be
 executed after all data have been received. This method have to receive array of objects
 ACityAPI.StickerInfo.

ACityAPI.StickerInfo structure:

StickerInfo {
	Vector3[] positions;	// array of four 3D coordinates in Unity scene of placeholders
	string sPath;			// URL of sticker
	string sText;			// Text of sticker
}

You can see the implementation of working with the class ACityAPI in the demo scene in GetPlaceHolders.cs.
Also you can change server API in the GameObject to which ACityAPI.cs is attached.
And you can check localization status when localization started with method ACityAPI.getLocalizationStatus().
It is implemented through enum structure ACityAPI.LocalizationStatus and can take seven states:
	NotStarted,
	GetGPSData,
	NoGPSData,
	WaitForAPIAnswer,
	ServerError,
	CantLocalize,
	Ready.

You can build DemoScene on Android/iOS and get a simple application that localizes and shows placeholders on scanned buildings.
