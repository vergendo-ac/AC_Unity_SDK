# Org.OpenAPITools.Api.SeriesApi

All URIs are relative to *http://developer.vergendo.com:5000/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetSeries**](SeriesApi.md#getseries) | **GET** /series | Get series reconstruction status by task id
[**PostSeries**](SeriesApi.md#postseries) | **POST** /series | Add new series to server to make reconstruction
[**PutSeries**](SeriesApi.md#putseries) | **PUT** /series | Upload series images to server


<a name="getseries"></a>
# **GetSeries**
> List&lt;OneOfobjectobjectobject&gt; GetSeries (string accept, List<Guid> taskId)

Get series reconstruction status by task id

Get series reconstruction status by task id

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class GetSeriesExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://developer.vergendo.com:5000/api";
            var apiInstance = new SeriesApi(config);
            var accept = accept_example;  // string |  (default to "application/vnd.myplace.v2+json")
            var taskId = new List<Guid>(); // List<Guid> | uuid. Several task ids could be specified

            try
            {
                // Get series reconstruction status by task id
                List<OneOfobjectobjectobject> result = apiInstance.GetSeries(accept, taskId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SeriesApi.GetSeries: " + e.Message );
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
 **taskId** | [**List&lt;Guid&gt;**](Guid.md)| uuid. Several task ids could be specified | 

### Return type

[**List&lt;OneOfobjectobjectobject&gt;**](OneOfobjectobjectobject.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Status returned |  -  |
| **400** |  |  -  |
| **500** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="postseries"></a>
# **PostSeries**
> InlineResponse200 PostSeries (string accept, int? notificationId = null, InlineObject3 inlineObject3 = null)

Add new series to server to make reconstruction

Add new series to server to make reconstruction asynchronously. Accept series description and wait images upload

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class PostSeriesExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://developer.vergendo.com:5000/api";
            var apiInstance = new SeriesApi(config);
            var accept = accept_example;  // string |  (default to "application/vnd.myplace.v2+json")
            var notificationId = 56;  // int? | Firebase cloud message id. If specified the client will get push notification when series process is finished (optional) 
            var inlineObject3 = new InlineObject3(); // InlineObject3 |  (optional) 

            try
            {
                // Add new series to server to make reconstruction
                InlineResponse200 result = apiInstance.PostSeries(accept, notificationId, inlineObject3);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SeriesApi.PostSeries: " + e.Message );
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
 **notificationId** | **int?**| Firebase cloud message id. If specified the client will get push notification when series process is finished | [optional] 
 **inlineObject3** | [**InlineObject3**](InlineObject3.md)|  | [optional] 

### Return type

[**InlineResponse200**](InlineResponse200.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Series task created |  -  |
| **400** |  |  -  |
| **500** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="putseries"></a>
# **PutSeries**
> OneOfobjectobject PutSeries (string accept, Guid taskId, List<System.IO.Stream> file)

Upload series images to server

Upload files to server at the series reconstruction. Send an empty request to notify a server that all images are uploaded. In that case the server will set files in a queue to be processed

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class PutSeriesExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://developer.vergendo.com:5000/api";
            var apiInstance = new SeriesApi(config);
            var accept = accept_example;  // string |  (default to "application/vnd.myplace.v2+json")
            var taskId = new Guid(); // Guid | uuid. Only one task_id could be specified
            var file = new List<System.IO.Stream>(); // List<System.IO.Stream> | 

            try
            {
                // Upload series images to server
                OneOfobjectobject result = apiInstance.PutSeries(accept, taskId, file);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SeriesApi.PutSeries: " + e.Message );
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
 **taskId** | [**Guid**](Guid.md)| uuid. Only one task_id could be specified | 
 **file** | **List&lt;System.IO.Stream&gt;**|  | 

### Return type

[**OneOfobjectobject**](OneOfobjectobject.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/json, text/plain

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Files uploaded at the series |  -  |
| **400** | Bad request |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

