using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Links_Metadata))]
    public partial class CMS_Links
    {
        public class CMS_Links_Metadata
        {

            [Display(Name = "Tiêu đề")]
            [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
            public string Name { get; set; }

            [Display(Name = "Đường dẫn")]
            [Required(ErrorMessage = "Không được trống")]
            public string Url { get; set; }

            [Display(Name = "Hình ảnh")]
            public string Picture { get; set; }

            [Display(Name = "Hiển thị")]
            public bool Shows { get; set; }

            [Display(Name = "Thứ tự")]
            [Required(ErrorMessage = "Không được trống")]
            [RegularExpression("([0-9]+)", ErrorMessage = "Chỉ nhập số")]
            public Int32 Order { get; set; }
        }
    }
}




