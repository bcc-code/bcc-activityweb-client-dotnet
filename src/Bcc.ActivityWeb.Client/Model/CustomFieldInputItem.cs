using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class CustomFieldGroupedInputItem
    {
        public int CustomFieldId { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public int GroupedFieldId { get; set; }
    }
}
