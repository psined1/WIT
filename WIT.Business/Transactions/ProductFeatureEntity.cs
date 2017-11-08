using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class ProductFeatureItem : BaseEntity
    {
        public int ProductFeatureID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductFeatureItem() { }
        public ProductFeatureItem(ProductFeature rhs)
        {
            CreatedOn = rhs.CreatedOn;
            UpdatedOn = rhs.UpdatedOn;
            CreatedBy = rhs.CreatedBy;
            UpdatedBy = rhs.UpdatedBy;

            ProductFeatureID = rhs.ProductFeatureID;
            Code = rhs.Code;
            Name = rhs.Name;
            Description = rhs.Description;
        }
    }

    public class ProductFeatureList
    {
        public GridInfo GridInfo { get; set; }
        public List<ProductFeatureItem> Items { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public ProductFeatureList()
        {
            Items = new List<ProductFeatureItem>();
            GridInfo = new GridInfo();
        }
    }
}
