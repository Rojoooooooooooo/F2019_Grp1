using PetParadise.Extras.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace PetParadise.Extras.Error
{
    public class HttpErrorContent : IHttpActionResult
    {
        public HttpRequestMessage Request{ get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public string Message { get; private set; }

        public HttpErrorContent(HttpRequestMessage request, HttpStatusCode code, HttpError message) {
            StatusCode = code;
            Message = message.Message;
            Request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            string res = "{\"error\": { \"message\":\"" + Message + "\"} }";
            var response = new HttpResponseMessage()
            {
                StatusCode = this.StatusCode,
                Content = new StringContent(res, System.Text.Encoding.UTF8, "application/json"),
                RequestMessage = Request
            };
            return Task.FromResult(response);
        }
    }
}