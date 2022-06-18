using Microsoft.IdentityModel.Tokens;
using PetParadise.Extras.Extensions;
using PetParadise.Extras.Extensions.JwtSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PetParadise.Extras.Extensions.HttpRequestHeaders
{
    
    public static class HttpRequestHeadersExtensions
    {
        private const string SESSION_HEADER = "Session-Token";

        public static bool HasSessionTokenHeader(
            this System.Net.Http.Headers.HttpRequestHeaders headers)=>
                headers.Contains(SESSION_HEADER);
        
        public static string GetSessionToken(
            this System.Net.Http.Headers.HttpRequestHeaders headers) => 
                headers.GetValues(SESSION_HEADER).First();
        
        public static bool IsSessionValid(
            this System.Net.Http.Headers.HttpRequestHeaders headers, 
            TokenValidationParameters parameters)
        {
            string sessionToken = headers.GetValues(SESSION_HEADER).First();
            JwtToken token = new JwtToken(sessionToken, parameters);
            return token != null;
        }

    }
}