using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CSF_Pages_Metadata))]
    public partial class CSF_Pages
    {
        public class CSF_Pages_Metadata
        {

            [Display(Name = "Tên trang")]
            [StringLength(200, ErrorMessage = "Tên trang không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
            public string Name { get; set; }

            [Display(Name = "Key")]
            [StringLength(100, ErrorMessage = "Tên key không quá 100 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập key")]
            public string Key { get; set; }


            [Display(Name = "Biểu tượng")]
            public string Icon { get; set; }

            [Display(Name = "Thứ tự")]
            [RegularExpression("([0-9]+)", ErrorMessage = "Chỉ nhập số")]
            [Required(ErrorMessage = "Vui lòng nhập thứ tự")]
            public string Order { get; set; }

            [Display(Name = "Module")]
            [Required(ErrorMessage = "Vui lòng chọn module")]
            public int ModuleID { get; set; }

            [Display(Name = "Trang cha")]
            public int ParentID { get; set; }

            [Display(Name = "Kích hoạt")]
            public int IsActive { get; set; }

            [Display(Name = "Trang trắng")]
            public int IsBlank { get; set; }

            [Display(Name = "Trang quản trị")]
            public int IsAdmin { get; set; }

        }
    }
}




