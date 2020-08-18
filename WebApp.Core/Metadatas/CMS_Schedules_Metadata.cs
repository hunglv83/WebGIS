using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Schedules_Metadata))]
    public partial class CMS_Schedules
    {
        public class CMS_Schedules_Metadata
        {

            [Display(Name = "Tiêu đề")]
            [StringLength(200, ErrorMessage = "Tiêu đề không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
            public string Title { get; set; }

           
            [Display(Name = "Nội dung")]
            public string Contents { get; set; }

            [Display(Name = "Thời gian bắt đầu")]
            [Required(ErrorMessage = "Vui lòng nhập thời gian bắt đầu")]
            [DataType(DataType.DateTime)]
            public DateTimeOffset StartDate { get; set; }

            [Display(Name = "Thời gian kết thúc")]
            [DataType(DataType.DateTime)]
            public DateTimeOffset EndDate { get; set; }

            [Display(Name = "Địa điểm")]
            [StringLength(500, ErrorMessage = "Địa điểm không quá 500 ký tự")]
            public string Place { get; set; }

            [Display(Name = "Chuyên viên")]
            public string Participants { get; set; }

            [Display(Name = "Người chuẩn bị")]
            [StringLength(200, ErrorMessage = "Người chuẩn bị không quá 200 ký tự")]
            public string UserPrepare { get; set; }

            [Display(Name = "Lãnh đạo")]
            public string Leaders { get; set; }

            [Display(Name = "Lãnh đạo bộ")]
            public string Ministry_leaders { get; set; }

        }
    }
}




