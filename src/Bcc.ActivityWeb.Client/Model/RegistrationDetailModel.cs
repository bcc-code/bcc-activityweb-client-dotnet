using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class RegistrationDetailModel
    {
        public int ActivityId { get; set; }
        public bool Approved { get; set; }
        public string Description { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal Hours { get; set; }
        public DateTime? HoursRegisteredAt { get; set; }
        public int Id { get; set; }
        public bool IsEndUserRegistration { get; set; }
        public string Location { get; set; }
        public int Pause { get; set; }
        public int RegistrationId { get; set; }
        public DateTime? StartTime { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }

    }
}
