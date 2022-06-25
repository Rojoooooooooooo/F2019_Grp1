using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace PetParadise.Extras.Error
{
    public class HttpError
    {
        private HttpError(string message) { Message = message; }

        public string Message { get; private set; }

        public static HttpError LoginAuthError { get { return new HttpError("Invalid username / password."); } }
        public static HttpError InvalidSession { get { return new HttpError("Invalid session token."); } }
        public static HttpError UserExists { get { return new HttpError("Username or email is invalid."); } }
        //public HttpError LoginError { get { return new HttpError("Invalid username / password.") } }
        //public HttpError LoginError { get { return new HttpError("Invalid username / password.") } }

    }
}