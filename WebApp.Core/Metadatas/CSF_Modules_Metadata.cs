using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CSF_Modules_Metadata))]
    public partial class CSF_Modules
    {
        public class CSF_Modules_Metadata
        {

            [Display(Name = "Tên")]
            [Required(ErrorMessage = "Vui lòng nhập tên")]
            public string Name { get; set; }

            [Display(Name = "Mô tả")]
            public string Description { get; set; }

            [Display(Name = "Kích hoạt")]
            public string IsActive { get; set; }
        }
    }
}