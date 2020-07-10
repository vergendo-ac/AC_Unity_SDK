/* 
 * Augmented City API
 *
 * ## Description This is an API for the Augmented City platform ## Other resources For more information, please visit our website [https://www.augmented.city](https://www.augmented.city/) 
 *
 * The version of the OpenAPI document: 1.0.0
 * Contact: support@vergendo.com
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using RestSharp;
using Xunit;

using Org.OpenAPITools.Client;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;

namespace Org.OpenAPITools.Test
{
    /// <summary>
    ///  Class for testing SeriesApi
    /// </summary>
    /// <remarks>
    /// This file is automatically generated by OpenAPI Generator (https://openapi-generator.tech).
    /// Please update the test case below to test the API endpoint.
    /// </remarks>
    public class SeriesApiTests : IDisposable
    {
        private SeriesApi instance;

        public SeriesApiTests()
        {
            instance = new SeriesApi();
        }

        public void Dispose()
        {
            // Cleanup when everything is done.
        }

        /// <summary>
        /// Test an instance of SeriesApi
        /// </summary>
        [Fact]
        public void InstanceTest()
        {
            // TODO uncomment below to test 'IsInstanceOfType' SeriesApi
            //Assert.IsType(typeof(SeriesApi), instance, "instance is a SeriesApi");
        }

        
        /// <summary>
        /// Test GetSeries
        /// </summary>
        [Fact]
        public void GetSeriesTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string accept = null;
            //List<Guid> taskId = null;
            //var response = instance.GetSeries(accept, taskId);
            //Assert.IsType<List<OneOfobjectobjectobject>> (response, "response is List<OneOfobjectobjectobject>");
        }
        
        /// <summary>
        /// Test PostSeries
        /// </summary>
        [Fact]
        public void PostSeriesTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string accept = null;
            //int? notificationId = null;
            //InlineObject3 inlineObject3 = null;
            //var response = instance.PostSeries(accept, notificationId, inlineObject3);
            //Assert.IsType<InlineResponse200> (response, "response is InlineResponse200");
        }
        
        /// <summary>
        /// Test PutSeries
        /// </summary>
        [Fact]
        public void PutSeriesTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string accept = null;
            //Guid taskId = null;
            //List<System.IO.Stream> file = null;
            //var response = instance.PutSeries(accept, taskId, file);
            //Assert.IsType<OneOfobjectobject> (response, "response is OneOfobjectobject");
        }
        
    }

}
