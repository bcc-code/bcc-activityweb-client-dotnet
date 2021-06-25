using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class ActivityBaseModel
    {
        public int ActivityId { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? Finish { get; set; }

        public string Location { get; set; }

        public string Name { get; set; }

        public int? MainActivity { get; set; }
    }
}
