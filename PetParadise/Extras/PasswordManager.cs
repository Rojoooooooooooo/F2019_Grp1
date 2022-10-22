using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PetParadise.Extras
{
    public static class PasswordManager
    {
        private static int cost = 12;
        public static async Task<string> HashAsync(string password)
        {

            string s = await Task.Run(() => {
                string salt = BCrypt.Net.BCrypt.GenerateSalt(cost);
                return BCrypt.Net.BCrypt.HashPassword(password, salt);
            });
            return s;
        }
        public static async Task<bool> IsMatchedAsync(string input, string dbPassword)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(input, dbPassword));
        }
    }
}