using PetParadise.Models.Body.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetParadise.Models.Body
{
    [MetadataType(typeof(IClinicInformationModel))]
    public class ClinicInformationModel : IClinicInformationModel
    {
        public string Barangay { get; set; }

        public string City { get; set; }

        public string ClinicName { get; set; }

        public string Contact { get; set; }

        public string Country { get; set; }

        public string CurrentPassword { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Line { get; set; }

        public string MiddleName { get; set; }

        public string NewPassword { get; set; }
    }
}