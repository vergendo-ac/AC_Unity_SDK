# Org.OpenAPITools.Api.LocalizationApi

All URIs are relative to *http://developer.vergendo.com:5000/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetObjects**](LocalizationApi.md#getobjects) | **POST** /get_objects | Return objects
[**Localize**](LocalizationApi.md#localize) | **POST** /localizer/localize | Localize camera
[**Prepare**](LocalizationApi.md#prepare) | **GET** /localizer/prepare | Prepare localization session


<a name="getobjects"></a>
# **GetObjects**
> Object GetObjects (string accept, string type = null, string sceneFormat = null, System.IO.Stream file = null)

Return objects

Localize uploaded image and return objects info and scene

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class GetObjectsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://developer.vergendo.com:5000/api";
            var apiInstance = new LocalizationApi(config);
            var accept = accept_example;  // string |  (default to "application/vnd.myplace.v1+json")
            var type = type_example;  // string | If not specified type is sticker (optional)  (default to sticker)
            var sceneFormat = sceneFormat_example;  // string | If not specified scene_format is none (optional)  (default to none)
            var file = BINARY_DATA_HERE;  // System.IO.Stream | An image to make a query by (optional) 

            try
            {
                // Return objects
                Object result = apiInstance.GetObjects(accept, type, sceneFormat, file);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LocalizationApi.GetObjects: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **string**|  | [default to &quot;application/vnd.myplace.v1+json&quot;]
 **type** | **string**| If not specified type is sticker | [optional] [default to sticker]
 **sceneFormat** | **string**| If not specified scene_format is none | [optional] [default to none]
 **file** | **System.IO.Stream****System.IO.Stream**| An image to make a query by | [optional] 

### Return type

**Object**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/json, text/plain

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Scene and objects info returned |  -  |
| **400** | Bad request |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="localize"></a>
# **Localize**
> LocalizationResult Localize (string accept, LocalizerLocalizeDescription description, System.IO.Stream image)

Localize camera

Localize uploaded image. Return camera pose and optional objects info and scene. Objects info and scene are sent on coordinate system change.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class LocalizeExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://developer.vergendo.com:5000/api";
            var apiInstance = new LocalizationApi(config);
            var accept = accept_example;  // string |  (default to "application/vnd.myplace.v2+json")
            var description = new LocalizerLocalizeDescription(); // LocalizerLocalizeDescription | 
            var image = BINARY_DATA_HERE;  // System.IO.Stream | A JPEG-encoded query image

            try
            {
                // Localize camera
                LocalizationResult result = apiInstance.Localize(accept, description, image);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LocalizationApi.Localize: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **string**|  | [default to &quot;application/vnd.myplace.v2+json&quot;]
 **description** | [**LocalizerLocalizeDescription**](LocalizerLocalizeDescription.md)|  | 
 **image** | **System.IO.Stream****System.IO.Stream**| A JPEG-encoded query image | 

### Return type

[**LocalizationResult**](LocalizationResult.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data, image/jpeg
 - **Accept**: application/json, text/plain

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Camera pose and optional objects info and scene |  -  |
| **400** | Bad request |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="prepare"></a>
# **Prepare**
> Status Prepare (string accept, double lat, double lon, double? alt = null, double? dop = null)

Prepare localization session

Prepare for localization for given geolocation. Causes server to load nearby reconstructions for localization. Returns an error when localization in this location is not possible.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class PrepareExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://developer.vergendo.com:5000/api";
            var apiInstance = new LocalizationApi(config);
            var accept = accept_example;  // string |  (default to "application/vnd.myplace.v2+json")
            var lat = 1.2D;  // double | GPS latitude
            var lon = 1.2D;  // double | GPS longitude
            var alt = 1.2D;  // double? | GPS altitude (optional) (optional) 
            var dop = 1.2D;  // double? | GPS HDOP (optional) (optional) 

            try
            {
                // Prepare localization session
                Status result = apiInstance.Prepare(accept, lat, lon, alt, dop);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling LocalizationApi.Prepare: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **string**|  | [default to &quot;application/vnd.myplace.v2+json&quot;]
 **lat** | **double**| GPS latitude | 
 **lon** | **double**| GPS longitude | 
 **alt** | **double?**| GPS altitude (optional) | [optional] 
 **dop** | **double?**| GPS HDOP (optional) | [optional] 

### Return type

[**Status**](Status.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, text/plain

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Status |  -  |
| **400** | Bad request |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

