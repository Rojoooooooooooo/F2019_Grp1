using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetParadise.Extras
{
    public static class Expiration
    {
        public static DateTime MONTH { get { return DateTime.Now.AddMonths(1); } }
        public static DateTime WEEK { get { return DateTime.Now.AddDays(7); } }
        public static DateTime SHORT { get { return DateTime.Now.AddMinutes(3); } }
    }
}