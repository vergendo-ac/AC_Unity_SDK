# ObjectApi

All URIs are relative to *http://developer.vergendo.com:5000/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**addObject**](ObjectApi.md#addObject) | **POST** /object | Add an object


<a name="addObject"></a>
# **addObject**
> kotlin.Any addObject(accept, image, description)

Add an object

Add a new user object into database

### Example
```kotlin
// Import classes:
//import org.openapitools.client.infrastructure.*
//import org.openapitools.client.models.*

val apiInstance = ObjectApi()
val accept : kotlin.String = accept_example // kotlin.String | 
val image : java.io.File = BINARY_DATA_HERE // java.io.File | 
val description : ObjectDescription =  // ObjectDescription | 
try {
    val result : kotlin.Any = apiInstance.addObject(accept, image, description)
    println(result)
} catch (e: ClientException) {
    println("4xx response calling ObjectApi#addObject")
    e.printStackTrace()
} catch (e: ServerException) {
    println("5xx response calling ObjectApi#addObject")
    e.printStackTrace()
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **kotlin.String**|  | [default to &quot;application/vnd.myplace.v1+json&quot;]
 **image** | **java.io.File**|  |
 **description** | [**ObjectDescription**](ObjectDescription.md)|  |

### Return type

[**kotlin.Any**](kotlin.Any.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/json

