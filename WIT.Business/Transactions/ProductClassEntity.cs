using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class ProductClassItem : BaseEntity
    {
        public int ProductClassID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductClassItem() { }
        public ProductClassItem(ProductClass rhs)
        {
            CreatedOn = rhs.CreatedOn;
            UpdatedOn = rhs.UpdatedOn;
            CreatedBy = rhs.CreatedBy;
            UpdatedBy = rhs.UpdatedBy;

            ProductClassID = rhs.ProductClassID;
            Code = rhs.Code;
            Name = rhs.Name;
            Description = rhs.Description;
        }
    }

    public class ProductClassList
    {
        public GridInfo GridInfo { get; set; }
        public List<ProductClassItem> Items { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public ProductClassList()
        {
            Items = new List<ProductClassItem>();
            GridInfo = new GridInfo();
        }
    }
}
