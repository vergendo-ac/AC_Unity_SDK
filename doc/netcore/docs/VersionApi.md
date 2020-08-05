# Org.OpenAPITools.Api.VersionApi

All URIs are relative to *http://developer.vergendo.com:5000/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**ServerVersionGet**](VersionApi.md#serverversionget) | **GET** /server_version | Get server version


<a name="serverversionget"></a>
# **ServerVersionGet**
> string ServerVersionGet (string body = null)

Get server version

Get server version

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class ServerVersionGetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://developer.vergendo.com:5000/api";
            var apiInstance = new VersionApi(config);
            var body = 2.7-1-g926829c AC-178-add_versioning 2020.06.19 13:12;  // string |  (optional) 

            try
            {
                // Get server version
                string result = apiInstance.ServerVersionGet(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling VersionApi.ServerVersionGet: " + e.Message );
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
 **body** | **string**|  | [optional] 

### Return type

**string**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: */*, text/plain

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

