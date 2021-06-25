using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class RegistrationModel
    {
        public int ActivityId { get; set; }

        public string Comments { get; set; }

        public int? CreditedTeamId { get; set; }
        public string CreditedTeamOfficialName { get; set; }

        public Dictionary<string, string> CustomFields { get; set; }

        public IEnumerable<RegistrationDetailModel> Details { get; set; }

        public bool IsReserve { get; set; }

        public DateTime RegisteredDate { get; set; }
        public int RegistrationId { get; set; }

        public bool SelfRegistered { get; set; }

        public UserModel User { get; set; }

        public int UserId { get; set; }

    }
}
