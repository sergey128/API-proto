using System;
using System.Configuration;
using System.IO;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Xunit;

namespace AIG.Science.Backend.Test
{
    public abstract class TestCaseBase : IDisposable, IUseFixture<SelfHostFixture>
    {
        SelfHostFixture fixture;

        protected TestHttpClient client;
        public TestCaseBase()
        {
            client = new TestHttpClient();
            client.BaseAddress = new Uri(SelfHostFixture.HostBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (client != null)
                client.Dispose();
        }

        #endregion


        #region IUseFixture<SelfHostFixture> Members

        public void SetFixture(SelfHostFixture data)
        {
            fixture = data;
        }

        #endregion
    }
}
