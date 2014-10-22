using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace AIG.Science.Backend.Test
{
	public class TestHttpClient : HttpClient 
	{
		public static string ApiBaseUrl { get; private set; }

		/// <summary>Options to use by test client (de)serializer. Must be equivalent to those used by server (see Web API project).</summary>
		public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings();

		static TestHttpClient()
		{
            //ApiBaseUrl = ConfigurationManager.ConnectionStrings["apiBaseUrl"].ConnectionString;
            //if (!ApiBaseUrl.EndsWith("/"))
            //    ApiBaseUrl += "/";

			// include type definition to serialized JSON to be able to deserialize complex objects in tests
			JsonSettings.TypeNameHandling = TypeNameHandling.Objects;
			// pretty, intended view
			JsonSettings.Formatting = Formatting.Indented; 

			Trace.TraceInformation("Using {0} as a base url", ApiBaseUrl);
		}
	}
}
