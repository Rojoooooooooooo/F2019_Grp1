using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetParadise.Models.Body
{
    public class PetInformationModel
    {
        public string Id{ get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public int CategoryId { get; set; }
        public int BreedId { get; set; }
        public string Color { get; set; }
    }
}