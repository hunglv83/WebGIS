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
    
    public partial class CMS_Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMS_Categories()
        {
            this.CMS_News = new HashSet<CMS_News>();
        }
    
        public short ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string PICTURE { get; set; }
        public bool PUBLISH { get; set; }
        public string KEY { get; set; }
        public Nullable<int> ORDERS { get; set; }
        public Nullable<int> PARENTCATE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMS_News> CMS_News { get; set; }
    }
}