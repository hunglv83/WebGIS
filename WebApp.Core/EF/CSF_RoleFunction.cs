//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Core.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class CSF_RoleFunction
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public int FunctionID { get; set; }
    
        public virtual CSF_Functions CSF_Functions { get; set; }
        public virtual CSF_Roles CSF_Roles { get; set; }
    }
}
