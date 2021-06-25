using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public enum OrderStatus
    {
        Unknown = 0,
        Created = 1,
        OrderConfirmed = 2,
        Delivered = 3,
        Refunded = 4,
        Cancelled = 5
    }
}
