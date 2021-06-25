using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Bcc.ActivityWeb.Client.Tests
{
    public class TestHttpClientFactory : IHttpClientFactory
    {

        public HttpClient CreateClient(string name = null)
        {
            return new HttpClient();
        }
    }
}
