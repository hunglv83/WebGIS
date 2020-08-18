using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Notifications_Metadata))]
    public partial class CMS_Notifications
    {
        public class CMS_Notifications_Metadata
        {

            [Display(Name = "Tiêu đề")]
            [StringLength(200, ErrorMessage = "Tiêu đề không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
            public string Title { get; set; }

           
            [Display(Name = "Thông tin chi tiết")]
            public string Contents { get; set; }


            [Display(Name = "Công khai")]
            public string Publish { get; set; }


        }
    }
}




