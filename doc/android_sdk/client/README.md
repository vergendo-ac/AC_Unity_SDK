# org.openapitools.client - Kotlin client library for Augmented City API

## Requires

* Kotlin 1.3.41
* Gradle 4.9

## Build

First, create the gradle wrapper script:

```
gradle wrapper
```

Then, run:

```
./gradlew check assemble
```

This runs all tests and packages the library.

## Features/Implementation Notes

* Supports JSON inputs/outputs, File inputs, and Form inputs.
* Supports collection formats for query parameters: csv, tsv, ssv, pipes.
* Some Kotlin and Java types are fully qualified to avoid conflicts with types defined in OpenAPI definitions.
* Implementation of ApiClient is intended to reduce method counts, specifically to benefit Android targets.

<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *http://developer.vergendo.com:5000/api*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*LocalizationApi* | [**getObjects**](docs/LocalizationApi.md#getobjects) | **POST** /get_objects | Return objects
*LocalizationApi* | [**localize**](docs/LocalizationApi.md#localize) | **POST** /localizer/localize | Localize camera
*LocalizationApi* | [**prepare**](docs/LocalizationApi.md#prepare) | **GET** /localizer/prepare | Prepare localization session
*ObjectApi* | [**addObject**](docs/ObjectApi.md#addobject) | **POST** /object | Add an object
*SeriesApi* | [**getSeries**](docs/SeriesApi.md#getseries) | **GET** /series | Get series reconstruction status by task id
*SeriesApi* | [**postSeries**](docs/SeriesApi.md#postseries) | **POST** /series | Add new series to server to make reconstruction
*SeriesApi* | [**putSeries**](docs/SeriesApi.md#putseries) | **PUT** /series | Upload series images to server


<a name="documentation-for-models"></a>
## Documentation for Models

 - [org.openapitools.client.models.InlineObject](docs/InlineObject.md)
 - [org.openapitools.client.models.InlineObject1](docs/InlineObject1.md)
 - [org.openapitools.client.models.InlineObject2](docs/InlineObject2.md)
 - [org.openapitools.client.models.InlineObject3](docs/InlineObject3.md)
 - [org.openapitools.client.models.InlineObject4](docs/InlineObject4.md)
 - [org.openapitools.client.models.InlineResponse200](docs/InlineResponse200.md)
 - [org.openapitools.client.models.InlineResponse2001](docs/InlineResponse2001.md)
 - [org.openapitools.client.models.InlineResponse2001Status](docs/InlineResponse2001Status.md)
 - [org.openapitools.client.models.LocalizerLocalizeDescription](docs/LocalizerLocalizeDescription.md)
 - [org.openapitools.client.models.LocalizerLocalizeDescriptionGps](docs/LocalizerLocalizeDescriptionGps.md)
 - [org.openapitools.client.models.ObjectDescription](docs/ObjectDescription.md)
 - [org.openapitools.client.models.ObjectDescriptionPlaceholder](docs/ObjectDescriptionPlaceholder.md)
 - [org.openapitools.client.models.ObjectDescriptionPlaceholderProjections](docs/ObjectDescriptionPlaceholderProjections.md)
 - [org.openapitools.client.models.ObjectDescriptionSticker](docs/ObjectDescriptionSticker.md)


<a name="documentation-for-authorization"></a>
## Documentation for Authorization

All endpoints do not require authorization.
