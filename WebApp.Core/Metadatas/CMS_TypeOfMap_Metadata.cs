using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_TypeOfMap_Metadata))]
    public partial class CMS_TypeOfMap
    {
        public class CMS_TypeOfMap_Metadata
        {

            [Display(Name = "Tên")]
            [StringLength(200, ErrorMessage = "Tên không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            public string Name { get; set; }

           
            [Display(Name = "Mô tả")]
            public string Description { get; set; }


          

        }
    }
}




