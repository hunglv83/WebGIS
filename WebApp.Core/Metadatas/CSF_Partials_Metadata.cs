using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CSF_Partials_Metadata))]
    public partial class CSF_Partials
    {
        public class CSF_Partials_Metadata
        {

            [Display(Name = "Tên Partial")]
            [StringLength(200, ErrorMessage = "Tên Control không quá 200 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên Control hehe")]
            public string Name { get; set; }

            [Display(Name = "File PartialView")]          
            public string Key { get; set; }

            [Display(Name = "Module")]
            [Required(ErrorMessage = "Vui lòng chọn module Abc")]
            public int ModuleID { get; set; }
          
            [Display(Name = "Controller")]
            [Required(ErrorMessage = "Vui lòng chọn Controller")]
            public string Controller { get; set; }

        }
    }
}




