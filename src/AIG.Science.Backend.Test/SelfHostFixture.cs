using System;
using System.Configuration;
using System.IO;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace AIG.Science.Backend.Test
{
    public class SelfHostFixture : IDisposable
    {
        public static string HostBaseAddress { get; private set; }

        HttpSelfHostServer server;

        static SelfHostFixture()
        {
            HostBaseAddress = ConfigurationManager.AppSettings["HostBaseAddress"];
            if (!HostBaseAddress.EndsWith("/"))
                HostBaseAddress += "/";
        }

        public SelfHostFixture()
        {
            server = CreateHost(HostBaseAddress);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (server != null)
            {
                server.CloseAsync().Wait();
                server.Dispose();
            }
        }

        #endregion

        static HttpSelfHostServer CreateHost(string address)
        {
            string dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["DataDirectory"] ?? "");
            dataDirectory = Path.GetFullPath(dataDirectory); // otherwise EF throws "Invalid value for key 'attachdbfilename'" if relative path parts are present
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

            // Create normal config
            var config = new HttpSelfHostConfiguration(address);
            config.EnableSystemDiagnosticsTracing();

            AIG.Science.Backend.WebApiConfig.Register(config);

            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();

            return server;
        }
    }
}
