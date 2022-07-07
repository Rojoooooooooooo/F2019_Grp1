using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetParadise.Extras
{
    public static class Expiration
    {
        public static DateTime MONTH { get { return DateTime.UtcNow.AddMonths(1); } }
        public static DateTime WEEK { get { return DateTime.UtcNow.AddDays(7); } }
        public static DateTime SHORT { get { return DateTime.UtcNow.AddMinutes(3); } }
    }
}