using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class OrderSearchCriteriaDto
    {
        public string Reference { get; set; }

        public bool IncludeOrdersFromSpouse { get; set; } = true;
    }
}
