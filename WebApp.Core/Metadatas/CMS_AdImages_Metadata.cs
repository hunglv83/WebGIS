using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_AdImages_Metadata))]
    public partial class CMS_AdImages
    {
        public class CMS_AdImages_Metadata
        {

            [Display(Name = "Tên")]
            [StringLength(200, ErrorMessage = "Tên 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            public string Name { get; set; }

            [Display(Name = "Mô tả")]
            public string Description { get; set; }

            [Display(Name = "Thứ tự")]
            public int Orders { get; set; }

            [Display(Name = "Công khai")]
            public int Publish { get; set; }

            [Display(Name = "Ảnh quảng cáo")]
            public string FileName { get; set; }

            [Display(Name = "Url")]
            public string Url { get; set; }

            [Display(Name = "Vị trí")]
            [Required(ErrorMessage = "Vui lòng chọn vị trí")]
            public string Location { get; set; }
        }
    }
}




