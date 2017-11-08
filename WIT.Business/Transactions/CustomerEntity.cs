using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class CustomerItem : BaseEntity
    {
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }

        public CustomerItem() { }
        public CustomerItem(Customer rhs)
        {
            CreatedOn = rhs.CreatedOn;
            UpdatedOn = rhs.UpdatedOn;
            CreatedBy = rhs.CreatedBy;
            UpdatedBy = rhs.UpdatedBy;

            CustomerID = rhs.CustomerID;
            CustomerCode = rhs.CustomerCode;
            CompanyName = rhs.CompanyName;
            AddressLine1 = rhs.AddressLine1;
            AddressLine2 = rhs.AddressLine2;
            City = rhs.City;
            State = rhs.State;
            ZipCode = rhs.ZipCode;
            PhoneNumber = rhs.PhoneNumber;
        }
    }

    public class CustomerList
    {
        public GridInfo GridInfo { get; set; }
        public List<CustomerItem> Items { get; set; }

        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }

        public CustomerList()
        {
            Items = new List<CustomerItem>();
            GridInfo = new GridInfo();
        }
    }
}
