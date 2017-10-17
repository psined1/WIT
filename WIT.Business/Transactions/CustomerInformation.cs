using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class CustomerInformation : TransactionalInformation
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
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public List<Customer> Customers { get; set; }
    }

}
