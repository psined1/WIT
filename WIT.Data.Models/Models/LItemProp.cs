//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WIT.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LItemProp
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LItemProp()
        {
            this.LItemValues = new HashSet<LItemValue>();
            this.LItemTypeLookups = new HashSet<LItemTypeLookup>();
        }
    
        public int ItemPropID { get; set; }
        public int ItemTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public LPropTypeEnum PropType { get; set; }
        public Nullable<int> ValueItemTypeID { get; set; }
        public bool GridHide { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Required { get; set; }
        public bool Multiple { get; set; }
        public bool Unique { get; set; }
        public bool UpperCase { get; set; }
        public int Radix { get; set; }
    
        public virtual LItemType LItemType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LItemValue> LItemValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LItemTypeLookup> LItemTypeLookups { get; set; }
    }
}
