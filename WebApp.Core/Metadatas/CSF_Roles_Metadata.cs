using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CSF_Roles_Metadata))]
    public partial class CSF_Roles
    {
        public class CSF_Roles_Metadata
        {
            [Display(Name = "Tên nhóm")]
            [StringLength(20, ErrorMessage = "Tên nhóm không quá 20 ký tự")]
            [Required(ErrorMessage = "Vui lòng nhập tên nhóm")]
            public string Name { get; set; }
        }
    }
}




