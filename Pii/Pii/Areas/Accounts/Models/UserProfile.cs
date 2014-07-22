using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;
using Pii.Areas.Accounts.Enums;

namespace Pii.Areas.Accounts.Models
{
    [Table("UserProfile")]
    public partial class UserProfile
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(100)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Quyền hạn của user
        /// </summary>
        /// <returns></returns>
        public UserRoles GetRole()
        {
            var role = Roles.IsUserInRole(this.UserName, "administrators") ? UserRoles.Administrators
            : Roles.IsUserInRole(this.UserName, "manager") ? UserRoles.Managers
            : UserRoles.Staff;
            return role;
        }
    }
}