using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Configuration;
using System.IO;
using WIT.Business.Entities;
using WIT.Interfaces;
using WIT.Business.Common;
using FluentValidation.Results;
using WIT.Data.Models;

namespace WIT.Business
{
    public class CustomerBusinessService
    {
        private ICustomerDataService _customerDataService;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerBusinessService(ICustomerDataService customerDataService)
        {
            _customerDataService = customerDataService;
        }

        /// <summary>
        /// Import Customers
        /// </summary>
        /// <param name="transaction"></param>
        public void ImportCustomers(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            try
            {

                string importFileName = ConfigurationManager.AppSettings["CustomerImportData"];

                System.IO.StreamReader csv_file = File.OpenText(importFileName);

                _customerDataService.CreateSession();
                _customerDataService.BeginTransaction();

                Boolean firstLine = true;
                int customerRecordsAdded = 0;

                while (csv_file.Peek() >= 0)
                {
                    // read and add a line
                    string line = csv_file.ReadLine();
                    string[] columns = line.Split('\t');
                    if (firstLine == false)
                    {
                        if (ImportCustomer(columns) == true)
                            customerRecordsAdded++;
                    }
                    firstLine = false;
                }

                _customerDataService.CommitTransaction(true);

                csv_file.Close();

                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add(customerRecordsAdded.ToString() + " customer successfully imported.");

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _customerDataService.CloseSession();
            }
        }

        /// <summary>
        /// Import Customer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        private Boolean ImportCustomer(string[] columns)
        {

            Customer customer = new Customer();

            customer.CustomerCode = ReplaceNullValue(columns[0].Trim());
            customer.CompanyName = ReplaceNullValue(columns[1].Trim());
            customer.AddressLine1 = ReplaceNullValue(columns[4].Trim());
            customer.AddressLine2 = string.Empty;
            customer.City = ReplaceNullValue(columns[5].Trim());
            customer.State = ReplaceNullValue(columns[6].Trim());
            customer.ZipCode = ReplaceNullValue(columns[7].Trim());          
            customer.PhoneNumber = ReplaceNullValue(columns[9].Trim());

            Boolean valid = _customerDataService.ValidateDuplicateCustomer(customer.CustomerCode);
            if (valid)
            {
                _customerDataService.CreateCustomer(customer);
            }

            return valid;

        }


        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="customerInformation"></param>
        /// <param name="transaction"></param>
        public void UpdateCustomer(CustomerInformation customerInformation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            Customer customer = new Customer();

            try
            {
                customer.CustomerCode = customerInformation.CustomerCode;
                customer.CompanyName = customerInformation.CompanyName;
                customer.CustomerID = customerInformation.CustomerID;

                CustomerBusinessRules customerBusinessRules = new CustomerBusinessRules(_customerDataService, customer);
                ValidationResult results = customerBusinessRules.Validate(customer);

                bool validationSucceeded = results.IsValid;
                IList<ValidationFailure> failures = results.Errors;

                if (validationSucceeded == false)
                {
                    //transaction = ValidationErrors.PopulateValidationErrors(failures);
                    transaction.PopulateValidationErrors(failures);
                    return;
                }

                _customerDataService.CreateSession();
                _customerDataService.BeginTransaction();

                if (customerInformation.CustomerID == 0 )
                {                                      
                    customer.AddressLine1 = customerInformation.AddressLine1;
                    customer.AddressLine2 = customerInformation.AddressLine2;
                    customer.City = customerInformation.City;
                    customer.State = customerInformation.State;
                    customer.ZipCode = customerInformation.ZipCode;
                    customer.PhoneNumber = customerInformation.PhoneNumber;

                    _customerDataService.CreateCustomer(customer);
                    _customerDataService.CommitTransaction(true);

                    customerInformation.CustomerID = customer.CustomerID;

                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Customer was successfully created at " + DateTime.Now.ToString());

                }
                else
                {

                    Customer existingCustomer = _customerDataService.GetCustomer(customerInformation.CustomerID);
                    existingCustomer.CustomerCode = customerInformation.CustomerCode;
                    existingCustomer.CompanyName = customerInformation.CompanyName;
                    existingCustomer.AddressLine1 = customerInformation.AddressLine1;
                    existingCustomer.AddressLine2 = customerInformation.AddressLine2;
                    existingCustomer.City = customerInformation.City;
                    existingCustomer.State = customerInformation.State;
                    existingCustomer.ZipCode = customerInformation.ZipCode;
                    existingCustomer.PhoneNumber = customerInformation.PhoneNumber;

                    _customerDataService.UpdateCustomer(existingCustomer);
                    _customerDataService.CommitTransaction(true);

                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Customer was successfully updated at " + DateTime.Now.ToString());

                }
              
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                transaction.ReturnMessage.Add(errorMessage);
                transaction.ReturnStatus = false;
            }
            finally
            {
                _customerDataService.CloseSession();
            }

            return;


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
        public List<Customer> GetCustomers(string customerCode, string companyName, int currentPageNumber, int pageSize, string sortDirection, string sortExpression, out int totalRows, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<Customer> customers = new List<Customer>();

            totalRows = 0;

            try
            {
                _customerDataService.CreateSession();
                customers = _customerDataService.GetCustomers(customerCode, companyName, currentPageNumber,pageSize, sortDirection,sortExpression, out totalRows);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _customerDataService.CloseSession();
            }

            return customers;

        }

        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public CustomerInformation GetCustomer(int customerID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            CustomerInformation customerInformation = new CustomerInformation();

  
            try
            {
                _customerDataService.CreateSession();
                Customer customer = _customerDataService.GetCustomer(customerID);
                customerInformation = PopulateCustomerInformation(customer);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                _customerDataService.CloseSession();
            }

            return customerInformation;

        }

        /// <summary>
        /// Populate Customer Information
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private CustomerInformation PopulateCustomerInformation(Customer customer)
        {
            CustomerInformation customerInformation = new CustomerInformation();
            customerInformation.AddressLine1 = ReplaceNullValue(customer.AddressLine1);
            customerInformation.AddressLine2 = ReplaceNullValue(customer.AddressLine2);
            customerInformation.City = ReplaceNullValue(customer.City);
            customerInformation.CompanyName = ReplaceNullValue(customer.CompanyName);
            customerInformation.CustomerCode = ReplaceNullValue(customer.CustomerCode);
            customerInformation.CustomerID = customer.CustomerID;
            customerInformation.CreatedOn = customer.CreatedOn;
            customerInformation.UpdatedOn = customer.UpdatedOn;
            customerInformation.CreatedBy = customer.CreatedBy;
            customerInformation.UpdatedBy = customer.UpdatedBy;
            customerInformation.PhoneNumber = ReplaceNullValue(customer.PhoneNumber);
            customerInformation.State = ReplaceNullValue(customer.State);
            customerInformation.ZipCode = ReplaceNullValue(customer.ZipCode);

            return customerInformation;

        }

        /// <summary>
        /// Replace NULL value 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private string ReplaceNullValue(string inputString)
        {
            if (inputString == "NULL") return string.Empty;
            return inputString;
        }



    }

}
