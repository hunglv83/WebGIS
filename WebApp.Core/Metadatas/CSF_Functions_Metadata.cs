using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Core.EF
{
    [MetadataTypeAttribute(typeof(CSF_Functions_Metadata))]
    public partial class CSF_Functions
    {
        internal sealed class CSF_Functions_Metadata
        {
            [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
            public string Name { get; set; }
        }

    }
}
