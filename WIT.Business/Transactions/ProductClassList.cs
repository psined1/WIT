using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class ProductClassList
    {
        public GridInfo GridInfo { get; set; }
        public List<ProductClass> Items { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public ProductClassList()
        {
            Items = new List<ProductClass>();
            GridInfo = new GridInfo();
        }
    }
}
