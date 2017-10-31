using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIT.Interfaces;
//using WIT.Business.Entities;
using System.Linq.Dynamic;
using WIT.Data.Models;

namespace WIT.Data.Services
{
    public class LibraryDataService : EntityFrameworkService //, ICustomerDataService
    {
        public List<Customer> GetCustomers(string code, string name, int currentPageNumber, int pageSize, string sortDirection, string sortExpression, out int totalRows)
        {
     
            if (sortExpression.Length == 0) sortExpression = "CompanyName";

            if (sortDirection.Length == 0) sortDirection = "ASC";

            sortExpression = sortExpression + " " + sortDirection;            
         
            var customerQuery = db.Customers.AsQueryable();

            if (customerCode != null && customerCode.Trim().Length > 0)
            {
                customerQuery = customerQuery.Where(c => c.CustomerCode.StartsWith(customerCode));
            }

            if (companyName != null && companyName.Trim().Length > 0)
            {
                customerQuery = customerQuery.Where(c => c.CompanyName.StartsWith(companyName));
            }

            totalRows = customerQuery.Count();

            List<Customer> customers = customerQuery.OrderBy(sortExpression).Skip((currentPageNumber - 1) * pageSize).Take(pageSize).ToList();

            return customers;

        }
    }
}
