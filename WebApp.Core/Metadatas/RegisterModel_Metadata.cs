using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Core.EF
{
    public class RegisterViewModel
    {
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "(*) Bạn phải nhập họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "(*) Bạn phải nhập số Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "(*) Bạn phải nhập tài khoản")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "(*) Bạn phải nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "(*) Bạn phải nhập mật khẩu")]
        [DataType(DataType.Password)]
        [CompareAttribute("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string RePassword { get; set; }
    }
}
