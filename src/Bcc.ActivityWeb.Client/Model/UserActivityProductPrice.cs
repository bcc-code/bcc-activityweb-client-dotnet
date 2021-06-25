using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class UserActivityProductPrice : ProductPriceBaseModel
    {
        public bool IsRegistered { get; set; }

        public int AvailableRemaining { get; set; }

        public string TicketUrl { get; set; }

    }
}
