using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WIT.Business.Entities;
using System.Configuration;
using WIT.Interfaces;
using WIT.Data.Models;

namespace WIT.Business
{
    public class CustomerBusinessRules : AbstractValidator<Customer>
    {      
        private Boolean _validCustomerCode = true;
       
        public CustomerBusinessRules(ICustomerDataService customerDataService, Customer customer)
        {
           
            if (customer.CustomerCode != null && customer.CustomerCode.Trim().Length > 0)
            {
                customerDataService.CreateSession();
                List<Customer> customers = customerDataService.GetCustomers(customer.CustomerCode);
                customerDataService.CloseSession();
                foreach(Customer existingCustomer in customers)
                {
                    if (existingCustomer.CustomerID != customer.CustomerID)
                    {
                        _validCustomerCode = false;
                        break;
                    }
                }
            }

            RuleFor(a => a.CustomerCode).NotEmpty().WithMessage("Customer Code is required.");
            RuleFor(a => a.CompanyName).NotEmpty().WithMessage("Company Name is required.");                    
            RuleFor(a => a.CustomerCode).Must(ValidateDuplicateCustomerCode).WithMessage("Customer Code already exists.");

        }

        /// <summary>
        /// Validate Duplicate Customer Code
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        private bool ValidateDuplicateCustomerCode(string customerCode)
        {
            return _validCustomerCode;
        }

    }
}


