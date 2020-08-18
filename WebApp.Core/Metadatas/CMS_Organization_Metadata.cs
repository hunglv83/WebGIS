using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Organization_Metadata))]
    public partial class CMS_Organization
    {
        public class CMS_Organization_Metadata
        {

            [Display(Name = "Tên cơ quan/tổ chức")]
            [StringLength(500, ErrorMessage = "Tên cơ quan/tổ chức không quá 500 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên cơ quan/tổ chức")]
            public string Name { get; set; }

            [Display(Name = "Tên viết tắt")]
            public string ShortName { get; set; }

            [Display(Name = "Cơ quan/tổ chức cha")]
            [Required(ErrorMessage = "Vui lòng chọn cha")]
            public int ParentID { get; set; }

            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Mobile")]
            public string Mobile { get; set; }

            [Display(Name = "Website")]
            public string Website { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }

        }
    }
}




