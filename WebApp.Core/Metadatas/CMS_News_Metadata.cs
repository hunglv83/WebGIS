using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_News_Metadata))]
    public partial class CMS_News
    {
        public class CMS_News_Metadata
        {

            [Display(Name = "Tiêu đề")]
            [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
            public string TITLE { get; set; }

            [Display(Name = "Tóm tắt")]
            public string EXCERPT { get; set; }

            [Display(Name = "Nội dung")]
            [Required(ErrorMessage = "Nội dung không được trống")]
            public string CONTENTS { get; set; }

            [Display(Name = "Loại tin tức")]
            [Required(ErrorMessage = "Chọn loại tin tức")]
            public Int16 ID_CATEGORIES { get; set; }

            [Display(Name = "Ảnh đại diện")]
            public string PICTURE { get; set; }

        }
    }
}




