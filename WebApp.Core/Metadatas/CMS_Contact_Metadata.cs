using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Contact_Metadata))]
    public partial class CMS_Contact
    {
        public class CMS_Contact_Metadata
        {

            [Display(Name = "Tên")]
            [StringLength(200, ErrorMessage = "Tên không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            public string Name { get; set; }

            [Display(Name = "Địa chỉ")]
            [StringLength(500, ErrorMessage = "Địa chỉ không quá 500 ký tự")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập email")]
            [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email chưa đúng")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [RegularExpression("([0-9]+)", ErrorMessage = "Chỉ nhập số")]
            [Display(Name = "Mobile")]
            public string Mobile { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
            [Display(Name = "Tiêu đề")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Nội dung không được trống")]
            [Display(Name = "Nội dung")]
            public string Contents { get; set; }

        }
    }
}




