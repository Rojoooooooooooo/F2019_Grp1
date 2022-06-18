using PetParadise.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace PetParadise.Extras.Extensions.Models
{
    public static class AccountCredentialExtensions
    {
        public static bool IsActive(this account_credential account) => !account.IsArchived;
        public static void ArchiveAccount(this account_credential account)
        {
            account.IsArchived = true;
            account.login_sessions = new Collection<login_sessions>(); // remove sessions
        } 
            
    }
}