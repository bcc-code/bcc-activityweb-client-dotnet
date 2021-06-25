using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public abstract class ProductPriceBaseModel
    {
        public int Id { get; set; }

        public int Gender { get; set; }

        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public int Total { get; set; }

    }
}
