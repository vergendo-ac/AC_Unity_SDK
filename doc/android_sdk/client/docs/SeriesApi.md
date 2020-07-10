# SeriesApi

All URIs are relative to *http://developer.vergendo.com:5000/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**getSeries**](SeriesApi.md#getSeries) | **GET** /series | Get series reconstruction status by task id
[**postSeries**](SeriesApi.md#postSeries) | **POST** /series | Add new series to server to make reconstruction
[**putSeries**](SeriesApi.md#putSeries) | **PUT** /series | Upload series images to server


<a name="getSeries"></a>
# **getSeries**
> kotlin.Array&lt;OneOfLessThanObjectCommaObjectCommaObjectGreaterThan&gt; getSeries(accept, taskId)

Get series reconstruction status by task id

Get series reconstruction status by task id

### Example
```kotlin
// Import classes:
//import org.openapitools.client.infrastructure.*
//import org.openapitools.client.models.*

val apiInstance = SeriesApi()
val accept : kotlin.String = accept_example // kotlin.String | 
val taskId : kotlin.Array<java.util.UUID> =  // kotlin.Array<java.util.UUID> | uuid. Several task ids could be specified
try {
    val result : kotlin.Array<OneOfLessThanObjectCommaObjectCommaObjectGreaterThan> = apiInstance.getSeries(accept, taskId)
    println(result)
} catch (e: ClientException) {
    println("4xx response calling SeriesApi#getSeries")
    e.printStackTrace()
} catch (e: ServerException) {
    println("5xx response calling SeriesApi#getSeries")
    e.printStackTrace()
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **kotlin.String**|  | [default to &quot;application/vnd.myplace.v2+json&quot;]
 **taskId** | [**kotlin.Array&lt;java.util.UUID&gt;**](java.util.UUID.md)| uuid. Several task ids could be specified |

### Return type

[**kotlin.Array&lt;OneOfLessThanObjectCommaObjectCommaObjectGreaterThan&gt;**](OneOfLessThanObjectCommaObjectCommaObjectGreaterThan.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

<a name="postSeries"></a>
# **postSeries**
> InlineResponse200 postSeries(accept, notificationId, inlineObject3)

Add new series to server to make reconstruction

Add new series to server to make reconstruction asynchronously. Accept series description and wait images upload

### Example
```kotlin
// Import classes:
//import org.openapitools.client.infrastructure.*
//import org.openapitools.client.models.*

val apiInstance = SeriesApi()
val accept : kotlin.String = accept_example // kotlin.String | 
val notificationId : kotlin.Int = 56 // kotlin.Int | Firebase cloud message id. If specified the client will get push notification when series process is finished
val inlineObject3 : InlineObject3 =  // InlineObject3 | 
try {
    val result : InlineResponse200 = apiInstance.postSeries(accept, notificationId, inlineObject3)
    println(result)
} catch (e: ClientException) {
    println("4xx response calling SeriesApi#postSeries")
    e.printStackTrace()
} catch (e: ServerException) {
    println("5xx response calling SeriesApi#postSeries")
    e.printStackTrace()
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **kotlin.String**|  | [default to &quot;application/vnd.myplace.v2+json&quot;]
 **notificationId** | **kotlin.Int**| Firebase cloud message id. If specified the client will get push notification when series process is finished | [optional]
 **inlineObject3** | [**InlineObject3**](InlineObject3.md)|  | [optional]

### Return type

[**InlineResponse200**](InlineResponse200.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

<a name="putSeries"></a>
# **putSeries**
> OneOfLessThanObjectCommaObjectGreaterThan putSeries(accept, taskId, file)

Upload series images to server

Upload files to server at the series reconstruction. Send an empty request to notify a server that all images are uploaded. In that case the server will set files in a queue to be processed

### Example
```kotlin
// Import classes:
//import org.openapitools.client.infrastructure.*
//import org.openapitools.client.models.*

val apiInstance = SeriesApi()
val accept : kotlin.String = accept_example // kotlin.String | 
val taskId : java.util.UUID = 38400000-8cf0-11bd-b23e-10b96e4ef00d // java.util.UUID | uuid. Only one task_id could be specified
val file : kotlin.Array<java.io.File> = BINARY_DATA_HERE // kotlin.Array<java.io.File> | 
try {
    val result : OneOfLessThanObjectCommaObjectGreaterThan = apiInstance.putSeries(accept, taskId, file)
    println(result)
} catch (e: ClientException) {
    println("4xx response calling SeriesApi#putSeries")
    e.printStackTrace()
} catch (e: ServerException) {
    println("5xx response calling SeriesApi#putSeries")
    e.printStackTrace()
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accept** | **kotlin.String**|  | [default to &quot;application/vnd.myplace.v2+json&quot;]
 **taskId** | [**java.util.UUID**](.md)| uuid. Only one task_id could be specified |
 **file** | **kotlin.Array&lt;java.io.File&gt;**|  |

### Return type

[**OneOfLessThanObjectCommaObjectGreaterThan**](OneOfLessThanObjectCommaObjectGreaterThan.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: application/json, text/plain

