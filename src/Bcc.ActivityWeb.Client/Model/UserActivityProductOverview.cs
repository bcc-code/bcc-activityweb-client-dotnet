using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class UserActivityProductOverview
    {
        public int UserId { get; set; }
        public ActivityBaseModel ActivityInfo { get; set; }
        public List<UserActivityProductGroup> ProductGroups { get; set; }
        public bool IsRegistered { get; set; }
        public List<string> OrderNumbers { get; set; }
    }
}
