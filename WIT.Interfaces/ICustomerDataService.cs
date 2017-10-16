using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WIT.Business.Entities;
using WIT.Data.Models;

namespace WIT.Interfaces
{
    public interface ICustomerDataService : IDataRepository, IDisposable
    {
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);       
        Customer GetCustomer(int customerID);
        List<Customer> GetCustomers(string customerCode, string companyName, int currentPageNumber, int pageSize, string sortDirection, string sortExpression, out int totalRows);
        List<Customer> GetCustomers(string customerCode);
        Boolean ValidateDuplicateCustomer(string customerCode);

    }
}
