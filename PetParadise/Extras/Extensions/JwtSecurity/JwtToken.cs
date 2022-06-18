using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PetParadise.Extras.Extensions.JwtSecurity
{
    public class JwtToken
    {
        public string Value { get; private set; }

        public JwtToken(string token, TokenValidationParameters validationParams)
        {
            if (token == null) this.Value = null; 
            if (!this.ValidateToken(token, validationParams))
            { 
                this.Value = null;
            }
            Value = token;
        }
        private bool ValidateToken(string token, TokenValidationParameters validationParams)
        {
            try
            {
                SecurityToken validatedToken;
                IPrincipal principal = new JwtSecurityTokenHandler()
                                            .ValidateToken(token,
                                                            validationParams, out validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}