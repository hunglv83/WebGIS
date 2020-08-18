using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Photos_Metadata))]
    public partial class CMS_Photos
    {
        public class CMS_Photos_Metadata
        {

            [Display(Name = "Tiêu đề")]
            [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
            public string Name { get; set; }

            [Display(Name = "Loại hình ảnh")]
            [Required(ErrorMessage = "Chọn loại hình ảnh")]
            public Int16 TypeOfPhotoID { get; set; }

            [Display(Name = "Mô tả")]
            public Int16 Description { get; set; }

            [Display(Name = "File ảnh")]
            public string FileName { get; set; }

            [Display(Name = "Công bố")]
            public bool Publish { get; set; }

        }
    }
}




