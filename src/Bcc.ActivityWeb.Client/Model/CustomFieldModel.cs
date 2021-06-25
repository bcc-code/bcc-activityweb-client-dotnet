using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class CustomFieldModel
    {
        public int ActivityId { get; set; }

        public List<CustomFieldChoiceModel> Choices { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public string FieldId { get; set; }
        public int FunctionalityId { get; set; }
        public string Label { get; set; }
        public string Max { get; set; }
        public int? MaxAge { get; set; }

        public int? MaxLength { get; set; }
        public string Min { get; set; }

        public int? MinAge { get; set; }
        public int? MinLength { get; set; }
        public bool Required { get; set; }

        public bool? ShowInTicketControl { get; set; }

        public bool ShowToEndUsers { get; set; }
        public int Type { get; set; }
        public bool UseMax { get; set; }

        public bool UseMin { get; set; }

    }

    public class CustomFieldChoiceModel
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
    }
}
