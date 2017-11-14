﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WitEntities : DbContext
    {
        public WitEntities()
            : base("name=WitEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ProductFeature> ProductFeatures { get; set; }
        public virtual DbSet<ProductClass> ProductClasses { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<LItem> LItems { get; set; }
        public virtual DbSet<LItemProp> LItemProps { get; set; }
        public virtual DbSet<LItemPropValue> LItemPropValues { get; set; }
        public virtual DbSet<LItemPropValueDateTime> LItemPropValueDateTimes { get; set; }
        public virtual DbSet<LItemPropValueDecimal> LItemPropValueDecimals { get; set; }
        public virtual DbSet<LItemPropValueInteger> LItemPropValueIntegers { get; set; }
        public virtual DbSet<LItemPropValueString> LItemPropValueStrings { get; set; }
        public virtual DbSet<LItemPropValueText> LItemPropValueTexts { get; set; }
        public virtual DbSet<LItemType> LItemTypes { get; set; }
    }
}
