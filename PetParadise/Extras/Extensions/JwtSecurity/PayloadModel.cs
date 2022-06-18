using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetParadise.Extras.Extensions.JwtSecurity
{
    public class PayloadModel
    {
        public string JTI { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Expiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}