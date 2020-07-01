/**
* Augmented City API
* ## Description This is an API for the Augmented City platform ## Other resources For more information, please visit our website [https://www.augmented.city](https://www.augmented.city/) 
*
* The version of the OpenAPI document: 1.0.0
* Contact: support@vergendo.com
*
* NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
* https://openapi-generator.tech
* Do not edit the class manually.
*/
package org.openapitools.client.models

import org.openapitools.client.models.ObjectDescription

import com.squareup.moshi.Json
/**
 * 
 * @param image 
 * @param description 
 */

data class InlineObject (
    @Json(name = "image")
    val image: java.io.File,
    @Json(name = "description")
    val description: ObjectDescription
)

