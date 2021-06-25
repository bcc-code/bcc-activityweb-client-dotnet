using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    [Serializable]
    public class CustomFieldInfo
    {

        private string _name;

        public int ActivityId { get; set; }

        public string Choices { get; set; }

        public string DefaultValue { get; set; }

        public string Description { get; set; }

        public bool? DescriptionAsLabelInForm { get; set; }
        public int FieldId { get; set; }

        public int FunctionalityId { get; set; }

        public string GroupFilter { get; set; }

        public string Label { get; set;  }

        public string Max { get; set; }

        public int? MaxAge { get; set; }

        public int? MaxLength { get; set; }

        public string Min { get; set; }

        public int? MinAge { get; set; }

        public int? MinLength { get; set; }

        public string Name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_name))
                    return _name;
                return Label;
            }
            set => _name = value;
        }

        public int Order { get; set; }

        public bool Required { get; set; }

        public bool? ShowInTicketControl { get; set; }

        public bool ShowToEndUsers { get; set; }

        public int Type { get; set; }

        public bool UseMax { get; set; }

        public bool UseMin { get; set; }
    }
}
