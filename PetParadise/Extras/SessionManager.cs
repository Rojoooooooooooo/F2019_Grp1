using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetParadise.Extras;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using PetParadise.Models;
using System.Security.Principal;
using System.Configuration;

namespace PetParadise.Extras
{
    public enum SessionType
    {
        SESSION = 0,
        ACCESS = 1
    }


    public class SessionManager
    {
        private string SessionKey;
        private string AccessKey;
        private string Issuer;
        private string Audience;
        private string UserId;
        private string Username;
        private int accountTypeId;

        public SessionManager()
        {
            this.SessionKey = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_SESSION_KEY"]);
            this.AccessKey = ""; // subject for cleanup soon
            this.Issuer = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_ISSUER"]);
            this.Audience = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_AUDIENCE"]);
        }

        public SessionManager(string id, string username, int accountTypeId)
        {
            /**
             * Make sure you have "env.config.json" on your root folder before you call on json key
             **/
            this.SessionKey = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_SESSION_KEY"]);
            this.AccessKey = ""; // subject for cleanup soon
            this.Issuer = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_ISSUER"]);
            this.Audience = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_AUDIENCE"]);
            this.UserId = id;
            this.Username = username;
            this.accountTypeId = accountTypeId;
        }
        private SigningCredentials Credentials(string key)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        }

        public List<Claim> CreateClaim()
        {
            try
            {
                var claims = new List<Claim>();

                claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens
                                    .JwtRegisteredClaimNames.Jti,
                                    Guid.NewGuid().ToString()));
                claims.Add(new Claim("userId", this.UserId));
                claims.Add(new Claim("username", this.Username));
                claims.Add(new Claim("accountTypeId", this.accountTypeId.ToString()));
                return claims;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return null;
            }
        }
        public Task<JwtSecurityToken> TokenizeAsync(SessionType type)
        {
            return Task.Run(() =>
            {
                try
                {
                    var claims = CreateClaim();
                    var signingCredentials = Credentials(type == 0 ?
                        this.SessionKey : this.AccessKey);
                    var expires = type == 0 ? Expiration.MONTH : Expiration.SHORT;
                    var token = new JwtSecurityToken(
                            Issuer,
                            Audience,
                            claims,
                            expires: expires,
                            signingCredentials: signingCredentials
                        );
                    return token;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                    return null;
                }
            });
        }
        public async Task<dynamic> TokenHandlerAsync(SessionType type)
        {
            return await Task.Run(async () => {
                SecurityToken token = await TokenizeAsync(type);
                return new JwtSecurityTokenHandler().WriteToken(token);
            });
        }

        public TokenValidationParameters CreateValidationParameters(SessionType type)
        {
            string encoding = type == 0 ? this.SessionKey : this.AccessKey; 
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = this.Issuer,
                ValidAudience = this.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(encoding))
            };
        }
        public async Task<bool> ValidateSessionToken(string token)
        {

            return await Task.Run(() =>
            {
                try
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    var validationParams = CreateValidationParameters(SessionType.SESSION);

                    SecurityToken validatedToken;
                    IPrincipal principal = tokenHandler.ValidateToken(token, validationParams, out validatedToken);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}