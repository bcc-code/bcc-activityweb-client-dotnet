using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class OrderLineModel
    {
        public ActivityEventModel Activity { get; set; }

        public int ActivityId { get; set; }

        public DateTime Created { get; set; }

        public decimal Discount { get; set; }

        public decimal FullPrice { get; set; }

        public UserModel OrderedForUser { get; set; }

        public int OrderedForUserId { get; set; }

        public int OrderItemId { get; set; }

        public string OrderNumber { get; set; }

        public decimal Price { get; set; }

        public DateTime? PriceAdjustment { get; set; }

        public string ProductName { get; set; }

        public string QrCode { get; set; }

        public int Vat { get; set; }

        public decimal VatAmount { get; set; }
    }
}
