using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    
    public class AuthResponse
    {
        
        public int TeamId { get; set; }

        
        public string Token { get; set; }

        
        public UserModel User { get; set; }
    }
}
