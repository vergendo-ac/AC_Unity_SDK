# Org.OpenAPITools.Api.ObjectApi

All URIs are relative to *http://developer.vergendo.com:5000/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**AddObject**](ObjectApi.md#addobject) | **POST** /object | Add an object


<a name="addobject"></a>
# **AddObject**
> Object AddObject (string accept, System.IO.Stream image, ObjectDescription description)

Add an object

Add a new user object into database

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class AddObjectExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://developer.vergendo.com:5000/api";
            var apiInstance = new ObjectApi(config);
            var accept = accept_example;  // string |  (default to "application/vnd.myplace.v1+json")
            var image = BINARY_DATA_HERE;  // System.IO.Stream | 
            var description = new ObjectDescription(); // ObjectDescription | 

            try
            {
                // Add an object
                Object result = apiInstance.AddObject(accept, image, description);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ObjectApi.AddObject: " + e.Message );
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
 **image** | **System.IO.Stream****System.IO.Stream**|  | 
 **description** | [**ObjectDescription**](ObjectDescription.md)|  | 

### Return type

**Object**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Object processed |  -  |
| **400** |  |  -  |
| **500** |  |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

