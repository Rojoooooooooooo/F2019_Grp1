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
    
    public partial class profile_post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public profile_post()
        {
            this.profile_post_comment = new HashSet<profile_post_comment>();
            this.profile_post_like = new HashSet<profile_post_like>();
        }
    
        public string Id { get; set; }
        public string ProfileId { get; set; }
        public System.DateTime PostCreationDate { get; set; }
        public string PostContent { get; set; }
        public string MediaContent { get; set; }
        public int post_points { get; set; }
    
        public virtual pet_profile pet_profile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<profile_post_comment> profile_post_comment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<profile_post_like> profile_post_like { get; set; }
    }
}