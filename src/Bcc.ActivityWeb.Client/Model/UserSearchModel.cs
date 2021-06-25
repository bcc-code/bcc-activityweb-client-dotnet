using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class UserSearchModel
    {

        public string Address { get; set; }

        public int Age { get; set; }

        public int? CreditedTeamId { get; set; }


        public string Email { get; set; }

        public string FirstName { get; set; }


        public string FullName { get; set; }


        public IEnumerable<int> GroupIds { get; set; }


        public string LastName { get; set; }

        public bool? Local { get; set; }

        public string Organization { get; set; }


        public bool? Registered { get; set; }

        public bool ScanIdMatch { get; set; }

  
        public int UserId { get; set; }
    }
}
