using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetParadise.Models.Body
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression("^(?=[a-zA-Z0-9._]{6,18}$)(?!.*[_.]{2})[^_.].*[^_.]$", ErrorMessage = "Invalid username.")]
        [MinLength(8, ErrorMessage = "Username must be 8-16 characters.")]
        [MaxLength(16, ErrorMessage = "Username must be 8-16 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(16, ErrorMessage = "Password must be 8-128 characters.")]
        [MaxLength(128, ErrorMessage = "Password must be 8-128 characters.")]
        public string Password { get; set; }

    }
}