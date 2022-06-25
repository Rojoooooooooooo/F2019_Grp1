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
    using System.ComponentModel.DataAnnotations;

    public partial class owner_profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public owner_profile()
        {
            this.owner_contact = new HashSet<owner_contact>();
            this.pet_profile = new HashSet<pet_profile>();
        }
    

        public string Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        [MaxLength(35, ErrorMessage = "First name must be 2-35 characters.")]
        [MinLength(2, ErrorMessage = "First name must be 2-35 characters.")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        [MaxLength(35, ErrorMessage = "Middle name must be 2-35 characters.")]
        public string MiddleName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        [MinLength(2, ErrorMessage = "Last name must be 2-35 characters.")]
        [MaxLength(35, ErrorMessage = "Last name must be 2-35 characters.")]
        public string LastName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<owner_contact> owner_contact { get; set; }
        public virtual owner_address owner_address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pet_profile> pet_profile { get; set; }
    }
}
