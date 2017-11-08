using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class ProductItem : BaseEntity
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }

        //public int? ProductClassID { get; set; }
        public string ProductClass { get; set; }

        //public int? ProductFeatureID { get; set; }
        public string ProductFeature { get; set; }

        public ProductItem() { }
        public ProductItem(Product rhs)
        {
            CreatedOn = rhs.CreatedOn;
            UpdatedOn = rhs.UpdatedOn;
            CreatedBy = rhs.CreatedBy;
            UpdatedBy = rhs.UpdatedBy;

            ProductID = rhs.ProductID;
            ProductCode = rhs.ProductCode;
            ProductName = rhs.ProductName;
            Description = rhs.Description;

            //ProductClassID = rhs.ProductClassID;
            ProductClass = rhs.ProductClass?.Code;

            //ProductFeatureID = rhs.ProductFeatureID;
            ProductFeature = rhs.ProductFeature?.Code;
        }
    }

    public class ProductList
    {
        public GridInfo GridInfo { get; set; }
        public List<ProductItem> Items { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public ProductList()
        {
            Items = new List<ProductItem>();
            GridInfo = new GridInfo();
        }
    }
}
