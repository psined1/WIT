using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class CustomerList
    {
        public GridInfo GridInfo { get; set; }
        public List<Customer> Items { get; set; }

        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }

        public CustomerList()
        {
            Items = new List<Customer>();
            GridInfo = new GridInfo();
        }
    }
}
