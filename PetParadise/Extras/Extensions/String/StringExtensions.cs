using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PetParadise.Extras.Extensions.String
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string s) => new CultureInfo("en-US", false).TextInfo.ToTitleCase(s);
        
    }
}