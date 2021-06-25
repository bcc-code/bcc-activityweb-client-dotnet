using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class TicketModel
    {
        public List<string> GivesAccessTo { get; set; }
        public string QrCode { get; set; }
        public UserModel User { get; set; }
    }
}
