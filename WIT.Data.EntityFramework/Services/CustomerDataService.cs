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
    public class CustomerDataService : EntityFrameworkService, ICustomerDataService
    {
        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customer"></param>
        public void CreateCustomer(Customer customer)
        {
            DateTime now = DateTime.Now;
            customer.DateCreated = now;
            customer.DateUpdated = now;
            db.Customers.Add(customer);
        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            customer.DateUpdated = DateTime.Now;
        }

        /// <summary>
        /// Validate Duplicate Customer
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        public Boolean ValidateDuplicateCustomer(string customerCode)
        {
            Customer customer = db.Customers.Where(c => c.CustomerCode == customerCode).SingleOrDefault();
            if (customer == null) return true;
            return false;
        }

        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer GetCustomer(int customerID)
        {
            Customer customer = db.Customers.Where(c => c.CustomerID == customerID).FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// Get Customers
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        public List<Customer> GetCustomers(string customerCode)
        {
            List<Customer> customers = db.Customers.Where(c => c.CustomerCode == customerCode).ToList();
            return customers;
        }

        /// <summary>
        /// Get Customers
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="companyName"></param>
        /// <param name="currentPageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortDirection"></param>
        /// <param name="sortExpression"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        public List<Customer> GetCustomers(string customerCode, string companyName, int currentPageNumber, int pageSize, string sortDirection, string sortExpression, out int totalRows)
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
