using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class CommonApiResponse
    {
        public List<string> Errors { get; set; }

        public bool Failed => !Success;

        public bool HasWarnings { get; set; }
        public bool Success { get; set; }
        public List<string> Warnings { get; set; }

        protected CommonApiResponse()
        {
        }

    }

    public class CommonApiResponse<T> : CommonApiResponse
    {
        public T Value { get; set; }

        protected CommonApiResponse()
        {
        }
    }
}
