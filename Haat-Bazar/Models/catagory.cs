//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Haat_Bazar.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class catagory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public catagory()
        {
            this.Products = new HashSet<Product>();
        }
    
        public int catId { get; set; }
        public string catName { get; set; }
        public string catImage { get; set; }
        public int catStatus { get; set; }
        public Nullable<int> admin_id { get; set; }
    
        public virtual admin_table admin_table { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
