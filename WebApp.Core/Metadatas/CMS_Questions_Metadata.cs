using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Questions_Metadata))]
    public partial class CMS_Questions
    {
        public class CMS_Questions_Metadata
        {

            [Display(Name = "Tiêu đề")]
            [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
            public string Title { get; set; }


            [Display(Name = "Nội dung")]
            [Required(ErrorMessage = "Nội dung không được trống")]
            public string Contents { get; set; }


            [Display(Name = "Câu trả lời")]
            public string Answer { get; set; }


            [Display(Name = "File đính kèm")]
            public string FileName { get; set; }


            [Display(Name = "Công bố")]
            public bool Publish { get; set; }

            [Display(Name = "Loại câu hỏi")]
            [Required(ErrorMessage = "Chưa chọn loại câu hỏi")]
            public bool TypeOfQuestionID { get; set; }


        }
    }
}




