using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Documents_Metadata))]
    public partial class CMS_Documents
    {
        public class CMS_Documents_Metadata
        {

            [Display(Name = "Số hiệu")]
            [Required(ErrorMessage = "Vui lòng nhập số hiệu văn bản")]
            public string DocumentNumber { get; set; }

            [Display(Name = "Loại văn bản")]
            [Required(ErrorMessage = "Chọn loại văn bản")]
            public Int16 TypeOfDocumentID { get; set; }

            [Display(Name = "Lĩnh vực")]
            [Required(ErrorMessage = "Chọn lĩnh vực")]
            public Int16 AreaOfDocument { get; set; }

            [Display(Name = "Tổ chức")]
            [Required(ErrorMessage = "Chọn tổ chức")]
            public Int16 OrganizationID { get; set; }

            [Display(Name = "Trích yếu")]
            [Required(ErrorMessage = "Không được trống")]
            public string Abstract { get; set; }

            [Display(Name = "Người ký")]
            public string Signer { get; set; }

            [Display(Name = "Nội dung toàn văn")]
            //[Required(ErrorMessage = "Không được trống")]
            public string Contents { get; set; }

            [Display(Name = "File đính kèm")]
            public string FileName { get; set; }

            [Display(Name = "Công bố")]
            public bool Publish { get; set; }

            [Display(Name = "Ngày ban hành")]
            public DateTime IssuedDate { get; set; }

            [Display(Name = "Ngày hiệu lực")]
            public DateTime EffectiveDate { get; set; }

            [Display(Name = "Hiệu lực")]
            public DateTime Effective { get; set; }

            [Display(Name = "Ghi chú")]
            public DateTime Description { get; set; }

        }
    }
}




