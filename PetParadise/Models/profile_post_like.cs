//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetParadise.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class profile_post_like
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string ProfileId { get; set; }
    
        public virtual profile_post profile_post { get; set; }
    }
}