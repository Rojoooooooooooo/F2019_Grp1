//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SummaryReport.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClinicProfile
    {
        public string Id { get; set; }
        public string ClinicName { get; set; }
        public string VetFirstName { get; set; }
        public string VetMiddleName { get; set; }
        public string VetLastName { get; set; }
        public string Line { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string contactId { get; set; }
        public string Contact { get; set; }
        public Nullable<int> Followers { get; set; }
        public Nullable<double> Rating { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
    }
}