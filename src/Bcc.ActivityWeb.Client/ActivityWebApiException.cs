using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Bcc.ActivityWeb.Client
{
    public class ActivityWebApiException : Exception
    {
        public ActivityWebApiException()
        {
        }

        public ActivityWebApiException(string message) : base(message)
        {
        }

        public ActivityWebApiException(string message, Exception innerException) : base(message, innerException)
        {
        }


        public HttpStatusCode StatusCode { get; set; }

        public string Response { get; set; }

        public string RequestUri { get; set; }
    }
}
