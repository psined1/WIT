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
    
    public partial class LItemValue
    {
        public long ItemValueID { get; set; }
        public long ItemID { get; set; }
        public int ItemPropID { get; set; }
    
        public virtual LItem LItem { get; set; }
        public virtual LItemProp LItemProp { get; set; }
        public virtual LItemValueDateTime LItemValueDateTime { get; set; }
        public virtual LItemValueDecimal LItemValueDecimal { get; set; }
        public virtual LItemValueInteger LItemValueInteger { get; set; }
        public virtual LItemValueString LItemValueString { get; set; }
        public virtual LItemValueText LItemValueText { get; set; }
        public virtual LItem LItemValueItem { get; set; }
    }
}
