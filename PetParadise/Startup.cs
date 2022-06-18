using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;

[assembly: OwinStartup(typeof(PetParadise.Startup))]

namespace PetParadise
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            var key = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_SESSION_KEY"]);
            var issuer = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_ISSUER"]);
            var audience = Environment.ExpandEnvironmentVariables(
                ConfigurationManager.AppSettings["JWT_AUDIENCE"]);

            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    }
                });
        }
    }
}
