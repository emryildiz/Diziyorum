//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diziyorum
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBLYORUM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBLYORUM()
        {
            this.TBLKULLANICI = new HashSet<TBLKULLANICI>();
        }
    
        public int YORUMID { get; set; }
        public string KULLANICIAD { get; set; }
        public string MAIL { get; set; }
        public string YORUMICERIK { get; set; }
        public Nullable<int> YORUMBLOG { get; set; }
    
        public virtual TBLBLOG TBLBLOG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBLKULLANICI> TBLKULLANICI { get; set; }
    }
}
