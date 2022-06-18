using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PetParadise.Controllers.ApiControllers
{
    public class PetController : ApiController
    {

        [Route('owner/pet')]
        [HttpPost]
        public IHttpActionResult AddPet() {

        }
    }
}
