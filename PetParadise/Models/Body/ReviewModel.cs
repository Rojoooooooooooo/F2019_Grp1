using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetParadise.Models.Body
{
    public class ReviewModel
    {
        public string Id { get; set; }
        public string ClinicId { get; set; }
        public string ReviewerId { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt{ get; set; }
        
    }
}