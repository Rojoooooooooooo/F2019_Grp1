using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetParadise.Models.Body.Interfaces
{
    interface IClinicInformationModel
    {

        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        string ClinicName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        string MiddleName { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        string LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        string Line { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        string Barangay { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        string City { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s#'.,\-()]*$", ErrorMessage = @"Acceptable characters are: a-z A-Z 0-9#'.,\-()")]
        string Country { get; set; }

        [RegularExpression("[0-9]", ErrorMessage = "0-9 characters only.")]
        [MaxLength(15, ErrorMessage = "Max characters are fifteen (15).")]
        string Contact { get; set; }

        [EmailAddress(ErrorMessage = "Please use a valid email address.")]
        string Email { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(16, ErrorMessage = "New password must be 8-128 characters.")]
        [MaxLength(128, ErrorMessage = "New password must be 8-128 characters.")]
        string NewPassword { get; set; }

        string CurrentPassword { get; set; }
    }
}
