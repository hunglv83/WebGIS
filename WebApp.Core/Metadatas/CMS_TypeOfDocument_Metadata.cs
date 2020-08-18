using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CMS_TypeOfDocument_Metadata))]
    public partial class CMS_TypeOfDocument
    {
        public class CMS_TypeOfDocument_Metadata
        {

            [Display(Name = "Tên")]
            [Required(ErrorMessage = "Vui lòng nhập tên loại văn bản!")]
            public string Name { get; set; }

            [Display(Name = "Mô tả")]
            public string Description { get; set; }
        }
    }
}