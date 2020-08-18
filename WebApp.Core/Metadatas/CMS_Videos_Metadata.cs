using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Videos_Metadata))]
    public partial class CMS_Videos
    {
        public class CMS_Videos_Metadata
        {

            [Display(Name = "Tiêu đề")]
            [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
            public string Name { get; set; }

            [Display(Name = "Chuyên mục video")]
            [Required(ErrorMessage = "Chọn loại hình ảnh")]
            public Int16 TypeOfVideoID { get; set; }

            [Display(Name = "Mô tả")]
            public Int16 Description { get; set; }

            [Display(Name = "File video")]
            public string FileName { get; set; }

            [Display(Name = "Công bố")]
            public bool Publish { get; set; }

            [Display(Name = "Ảnh đại diện")]
            public bool AvatarPicture { get; set; }

        }
    }
}




