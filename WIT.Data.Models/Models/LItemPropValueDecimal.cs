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
    
    public partial class LItemPropValueDecimal
    {
        public long ItemPropValueID { get; set; }
        public decimal Value { get; set; }
    
        public virtual LItemPropValue LItemPropValue { get; set; }
    }
}