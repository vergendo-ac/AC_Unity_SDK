# LocalizationApi

All URIs are relative to *http://developer.vergendo.com:5000/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**getObjects**](LocalizationApi.md#getObjects) | **POST** /get_objects | Return objects
[**localize**](LocalizationApi.md#localize) | **POST** /localizer/localize | Localize camera
[**prepare**](LocalizationApi.md#prepare) | **GET** /localizer/prepare | Prepare localization session


<a name="getObjects"></a>
# **getObjects**
> kotlin.Any getObjects(accept, file, type, sceneFormat)

Return objects

Localize uploaded image and return objects info and scene

### Example
```kotlin
// Import classes:
//import org.openapitools.client.infrastructure.*
//import org.openapitools.client.models.*

val apiInstance = LocalizationApi()
val accept : kotlin.String = accept_example // kotlin.String | 
val file : java.io.File = BINARY_DATA_HERE // java.io.File | An image to make a query by
val type : kotlin.String = type_example // kotlin.String | If not specified type is sticker
val sceneFormat : kotlin.String = sceneFormat_example // kotlin.String | If not specified scene_format is none
try {
    val result : kotlin.Any = apiInstance.getObjects(accept, file, type, sceneFormat)
    println(result)
} catch (e: ClientException) {
    println("4xx response calling LocalizationApi#getObjects")
    e.printStackTrace()
} catch (e: ServerException) {
    println("5xx response calling LocalizationApi#getObjects")
    e.printStackTrace()
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **kotlin.String**|  | [default to &quot;application/vnd.myplace.v1+json&quot;]
 **file** | **java.io.File**| An image to make a query by |
 **type** | **kotlin.String**| If not specified type is sticker | [optional] [default to &quot;sticker&quot;] [enum: sticker, placeholder]
 **sceneFormat** | **kotlin.String**| If not specified scene_format is none | [optional] [default to &quot;none&quot;] [enum: none, render, json_2d, json_3d]

### Return type

[**kotlin.Any**](kotlin.Any.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/json

<a name="localize"></a>
# **localize**
> kotlin.Any localize(accept, description, image)

Localize camera

Localize uploaded image. Return camera pose and optional objects info and scene. Objects info and scene are sent on coordinate system change.

### Example
```kotlin
// Import classes:
//import org.openapitools.client.infrastructure.*
//import org.openapitools.client.models.*

val apiInstance = LocalizationApi()
val accept : kotlin.String = accept_example // kotlin.String | 
val description : LocalizerLocalizeDescription =  // LocalizerLocalizeDescription | 
val image : java.io.File = BINARY_DATA_HERE // java.io.File | A JPEG-encoded query image
try {
    val result : kotlin.Any = apiInstance.localize(accept, description, image)
    println(result)
} catch (e: ClientException) {
    println("4xx response calling LocalizationApi#localize")
    e.printStackTrace()
} catch (e: ServerException) {
    println("5xx response calling LocalizationApi#localize")
    e.printStackTrace()
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **kotlin.String**|  | [default to &quot;application/vnd.myplace.v2+json&quot;]
 **description** | [**LocalizerLocalizeDescription**](LocalizerLocalizeDescription.md)|  |
 **image** | **java.io.File**| A JPEG-encoded query image |

### Return type

[**kotlin.Any**](kotlin.Any.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data, image/jpeg
 - **Accept**: application/json

<a name="prepare"></a>
# **prepare**
> InlineResponse2001 prepare(accept, lat, lon, alt, dop)

Prepare localization session

Prepare for localization for given geolocation. Causes server to load nearby reconstructions for localization. Returns an error when localization in this location is not possible.

### Example
```kotlin
// Import classes:
//import org.openapitools.client.infrastructure.*
//import org.openapitools.client.models.*

val apiInstance = LocalizationApi()
val accept : kotlin.String = accept_example // kotlin.String | 
val lat : kotlin.Double = 1.2 // kotlin.Double | GPS latitude
val lon : kotlin.Double = 1.2 // kotlin.Double | GPS longitude
val alt : kotlin.Double = 1.2 // kotlin.Double | GPS altitude (optional)
val dop : kotlin.Double = 1.2 // kotlin.Double | GPS HDOP (optional)
try {
    val result : InlineResponse2001 = apiInstance.prepare(accept, lat, lon, alt, dop)
    println(result)
} catch (e: ClientException) {
    println("4xx response calling LocalizationApi#prepare")
    e.printStackTrace()
} catch (e: ServerException) {
    println("5xx response calling LocalizationApi#prepare")
    e.printStackTrace()
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **kotlin.String**|  | [default to &quot;application/vnd.myplace.v2+json&quot;]
 **lat** | **kotlin.Double**| GPS latitude |
 **lon** | **kotlin.Double**| GPS longitude |
 **alt** | **kotlin.Double**| GPS altitude (optional) | [optional]
 **dop** | **kotlin.Double**| GPS HDOP (optional) | [optional]

### Return type

[**InlineResponse2001**](InlineResponse2001.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

