using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class RegistrationModelBase
    {
        public int ActivityId { get; set; }
        public int RegistrationId { get; set; }
        public UserModelBase User { get; set; }

    }
}
