using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Services_Metadata))]
    public partial class CMS_Services
    {
        public class CMS_Services_Metadata
        {

            [Display(Name = "Tên")]
            [StringLength(200, ErrorMessage = "Tên không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            public string Name { get; set; }

            [Display(Name = "URL")]
            public string URL { get; set; }

            [Display(Name = "Mô tả")]
            public string Description { get; set; }

            [Display(Name = "Công khai")]
            public string Publish { get; set; }

            [Display(Name = "Nguồn dữ liệu")]
            public string Source { get; set; }

            [Display(Name = "Lĩnh vực")]
            [Required(ErrorMessage = "Chưa chọn lĩnh vực")]
            public int TypeOfMapID { get; set; }

        }
    }
}




