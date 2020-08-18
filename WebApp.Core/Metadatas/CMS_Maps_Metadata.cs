using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_Maps_Metadata))]
    public partial class CMS_Maps
    {
        public class CMS_Maps_Metadata
        {

            [Display(Name = "Tên")]
            [StringLength(200, ErrorMessage = "Tên không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            public string Name { get; set; }

           
            [Display(Name = "Mô tả")]
            public string Description { get; set; }


            [Display(Name = "Loại bản đồ")]
            [Required(ErrorMessage = "Vui lòng chọn loại bản đồ")]
            public int TypeOfMapID { get; set; }

        }
    }
}




