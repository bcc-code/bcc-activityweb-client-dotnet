using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class UserActivityProductGroup
    {
        public int ProductGroupId { get; set; }
        public string ProductName { get; set; }
        public ProductGroupType Type { get; set; }
        public List<UserActivityProductPrice> ProductPrices { get; set; }
    }
}
