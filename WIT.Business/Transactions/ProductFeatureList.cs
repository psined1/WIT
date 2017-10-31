using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class ProductFeatureList
    {
        public GridInfo GridInfo { get; set; }
        public List<ProductFeature> Items { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public ProductFeatureList()
        {
            Items = new List<ProductFeature>();
            GridInfo = new GridInfo();
        }
    }
}
