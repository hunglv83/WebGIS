using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_TypeOfQuestion_Metadata))]
    public partial class CMS_TypeOfQuestion
    {
        public class CMS_TypeOfQuestion_Metadata
        {

            [Display(Name = "Tên")]
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            public string Name { get; set; }

            [Display(Name = "Mô tả")]
            public string Description { get; set; }
        }
    }
}