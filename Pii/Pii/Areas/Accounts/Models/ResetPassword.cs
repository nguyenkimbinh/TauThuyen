using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Pii.Areas.Accounts.Models.ViewModels
{
    public class ResetPassword
    {
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "{0} không để trống!")]
        [StringLength(100, ErrorMessage = "{0} phải lớn hơn {2} ký tự.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không chính xác.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
        public string Email { get; set; }
    }
}