using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class ActivityEventModel
    {
        public int ActivityId { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public DateTime Finish { get; set; }

        public string Location { get; set; }

        public int? MainActivityId { get; set; }

        public string Name { get; set; }

        public int? ResponsibleUserId { get; set; }

        public DateTime Start { get; set; }

        public string Status { get; set; }

        public int? TeamId { get; set; }
    }
}
