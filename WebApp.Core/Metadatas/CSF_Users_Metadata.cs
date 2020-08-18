using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CSF_Users_Metadata))]
    public partial class CSF_Users
    {
        public class CSF_Users_Metadata
        {

            [Display(Name = "Tên đăng nhập")]
            [StringLength(20, ErrorMessage = "Tên đăng nhập không quá 20 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
            public string UserName { get; set; }

            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [Display(Name = "Họ và tên")]
            [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
            [StringLength(30, ErrorMessage = "Họ và tên không quá 30 ký tự")]
            public string FullName { get; set; }

            [Display(Name = "Email")]
            [Required(ErrorMessage = "Vui lòng nhập email")]
            [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email chưa đúng")]
            public string Email { get; set; }
        }
    }
}




