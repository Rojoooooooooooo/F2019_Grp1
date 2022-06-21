using PetParadise.Extras.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PetParadise.Extras.Extensions.JwtSecurity
{
    public static class JwtSecurityTokenExtensions
    {
        public static PayloadModel GetPayload(this JwtToken token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token.Value);

            PayloadModel payload = new PayloadModel();
            payload.JTI = jwt.Claims.Where(c => c.Type.Equals("jti")).First().Value;
            payload.UserId = jwt.Claims.Where(c => c.Type.Equals("userId")).First().Value;
            payload.Username = jwt.Claims.Where(c => c.Type.Equals("username")).First().Value;
            payload.Expiration = jwt.Claims.Where(c => c.Type.Equals("exp")).First().Value;
            payload.Issuer = jwt.Claims.Where(c => c.Type.Equals("iss")).First().Value;
            payload.Audience = jwt.Claims.Where(c => c.Type.Equals("aud")).First().Value;

            Debug.WriteLine("payload: " + payload.Username);
            return payload;
        }

        public async static Task<PayloadModel> GetPayloadAsync(this JwtToken token)
        {
            try
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jwt = handler.ReadJwtToken(token.Value);

                        PayloadModel payload = new PayloadModel();
                        payload.JTI = jwt.Claims.First(claim => claim.Type.Equals("jti")).Value;
                        payload.UserId = jwt.Claims.First(claim => claim.Type.Equals("userId")).Value;
                        payload.Username = jwt.Claims.First(claim => claim.Type.Equals("username")).Value;
                        payload.Expiration = jwt.Claims.First(claim => claim.Type.Equals("exp")).Value;
                        payload.Audience = jwt.Claims.First(claim => claim.Type.Equals("aud")).Value;
                        payload.Issuer = jwt.Claims.First(claim => claim.Type.Equals("iss")).Value;

                        return payload;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.StackTrace);
                        return null;
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}