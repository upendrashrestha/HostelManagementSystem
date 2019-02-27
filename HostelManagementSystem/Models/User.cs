using HostelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HostelManagementSystem.Models
{
    public class User
    {

        [Required(ErrorMessage = "Required Username")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Username Must be Minimum 2 Charaters")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Password")]
        [MaxLength(30, ErrorMessage = "Password cannot be Greater than 30 Charaters")]
        [StringLength(31, MinimumLength = 7, ErrorMessage = "Password Must be Minimum 7 Charaters")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        public DateTime CreateOn { get; set; }

        public t_staff Staff { get; set; }

        public bool Active { get; set; }

        public string UserId { get; set; }

        /*[Required(ErrorMessage = "Required EmailID")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter Valid Email ID")]
        public string EmailID { get; set; }*/
    }
}