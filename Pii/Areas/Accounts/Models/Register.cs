using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Pii.Areas.Accounts.Enums;

namespace Pii.Areas.Accounts.Models
{
    public class Register
    {
        #region Properties
        [Key]
        public int UserId { get; set; }

        public string GetRoleName() { return Roles.GetRolesForUser(UserName)[0]; }
        [Required]
        [Display(Name = "User name")]
        [MinLength(5)]
        //[RegularExpression(@"([a-zA-Z0-9]+)", ErrorMessage = "{0} only use A-Z or a-z or 0-9")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
        
        //[Required]
        //[MinLength(5)]
        //[Display(Name = "Email")]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        //[Required]
        //public string ClientCapcha { get; set; }

        //public bool ValidCapchat(object value)
        //{
        //    if (value == null) return false;
        //    if (value.ToString().ToLower() != this.ClientCapcha.ToLower()) return false;
        //    return true;
        //}
        #endregion
    }
}