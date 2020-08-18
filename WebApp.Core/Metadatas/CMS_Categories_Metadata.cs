using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Categories_Metadata))]
    public partial class CMS_Categories
    {
        public class CMS_Categories_Metadata
        {

            [Display(Name = "Tên")]
            [StringLength(200, ErrorMessage = "Tên không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            public string NAME { get; set; }

            [Display(Name = "Key")]
            public string KEY { get; set; }

            [Display(Name = "Mô tả")]
            public string DESCRIPTION { get; set; }


            [Display(Name = "Ảnh đại diện")]
            public string PICTURE { get; set; }

            [Display(Name = "Công khai")]
            public string PUBLISH { get; set; }

            [Display(Name = "Thứ tự")]
            public string ORDERS { get; set; }

            [Display(Name = "Loại tin tức cha")]
            public int PARENTCATE { get; set; }


        }
    }
}




