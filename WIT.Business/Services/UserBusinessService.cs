using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIT.Business.Entities;
using WIT.Interfaces;
using WIT.Business.Common;
using FluentValidation.Results;
using FluentValidation;
using WIT.Data.Models;

namespace WIT.Business
{
    public class UserBusinessService
    {

        private IUserDataService _userDataService;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserBusinessService(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="userInformation"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public User RegisterUser(UserInformation userInformation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            User user = new User();

            try
            {

                user.EmailAddress = userInformation.EmailAddress;
                user.FirstName = userInformation.FirstName;
                user.LastName = userInformation.LastName;
                user.Password = userInformation.Password;
                user.AddressLine1 = string.Empty;
                user.AddressLine2 = string.Empty;
                user.City = string.Empty;
                user.State = string.Empty;
                user.ZipCode = string.Empty;

                UserBusinessRules userBusinessRules = new UserBusinessRules(_userDataService, user, userInformation.PasswordConfirmation);
                ValidationResult results = userBusinessRules.Validate(user);

                bool validationSucceeded = results.IsValid;
                IList<ValidationFailure> failures = results.Errors;

                if (validationSucceeded == false)
                {
                    //transaction = ValidationErrors.PopulateValidationErrors(failures);
                    transaction.PopulateValidationErrors(failures);
                    return user;
                }

                _userDataService.CreateSession();
                _userDataService.BeginTransaction();
                _userDataService.CreateUser(user);
                _userDataService.CommitTransaction(true);

                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("user successfully created.");

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                transaction.ReturnMessage.Add(errorMessage);
                transaction.ReturnStatus = false;
            }
            finally
            {
                _userDataService.CloseSession();
            }

            return user;


        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="userInformation"></param>
        /// <param name="transaction"></param>
        public void UpdateProfile(UserInformation userInformation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            User user = new User();

            try
            {

               
                user.FirstName = userInformation.FirstName;
                user.LastName = userInformation.LastName;           

                UserBusinessRules userBusinessRules = new UserBusinessRules();
                ValidationResult results = userBusinessRules.Validate(user);

                bool validationSucceeded = results.IsValid;
                IList<ValidationFailure> failures = results.Errors;

                if (validationSucceeded == false)
                {
                    //transaction = ValidationErrors.PopulateValidationErrors(failures);
                    transaction.PopulateValidationErrors(failures);
                    return;
                }

                _userDataService.CreateSession();
                _userDataService.BeginTransaction();

                User existingUser = _userDataService.GetUser(userInformation.UserID);
                existingUser.AddressLine1 = userInformation.AddressLine1;
                existingUser.AddressLine2 = userInformation.AddressLine2;
                existingUser.City = userInformation.City;
                existingUser.State = userInformation.State;
                existingUser.ZipCode = userInformation.ZipCode;

                _userDataService.UpdateUser(user);
                _userDataService.CommitTransaction(true);

                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Your profile was successfully updated.");

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                transaction.ReturnMessage.Add(errorMessage);
                transaction.ReturnStatus = false;
            }
            finally
            {
                _userDataService.CloseSession();
            }

            return;


        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public User Login(string emailAddress, string password, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            User user = new User();

            string errorMessage2 = string.Empty;

            try
            {

                errorMessage2 = "login 1";

                _userDataService.CreateSession();

                errorMessage2 = "login 2";

                user = _userDataService.GetUser(emailAddress);

                errorMessage2 = "login 3";

                if (user != null)
                {
                    if (user.Password != password)
                    {
                        transaction.ReturnMessage.Add("Invalid login.");
                        transaction.ReturnStatus = false;
                        return user;
                    }
                }
                else
                {
                    transaction.ReturnMessage.Add("Invalid login. " + emailAddress);
                    transaction.ReturnStatus = false;
                    user = new User();
                    return user;
                }


                errorMessage2 = "login 4";

                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("user successfully logged in.");

                if (user.AddressLine1 == null) user.AddressLine1 = string.Empty;
                if (user.AddressLine2 == null) user.AddressLine2 = string.Empty;
                if (user.City == null) user.City = string.Empty;
                if (user.State == null) user.State = string.Empty;
                if (user.ZipCode == null) user.ZipCode = string.Empty;

            }
            catch (Exception ex)
            {
                string errorMessage = string.Empty;
                if (ex.InnerException != null)
                {
                    errorMessage = ex.Message + " " + errorMessage2 + " " + ex.InnerException.ToString();
                }
                else
                {
                    errorMessage = ex.Message + " " + errorMessage2;
                }
                transaction.ReturnMessage.Add(errorMessage);
                transaction.ReturnStatus = false;
            }
            finally
            {
                _userDataService.CloseSession();
            }

            return user;


        }

        /// <summary>
        /// Authenicate
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public User Authenicate(int userID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            User user = new User();

            try
            {

                _userDataService.CreateSession();

                user = _userDataService.GetUser(userID);
                if (user == null)
                {
                    transaction.ReturnMessage.Add("Invalid session");
                    transaction.ReturnStatus = false;
                    return user;
                }

                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Valid session.");

                if (user.AddressLine1 == null) user.AddressLine1 = string.Empty;
                if (user.AddressLine2 == null) user.AddressLine2 = string.Empty;
                if (user.City == null) user.City = string.Empty;
                if (user.State == null) user.State = string.Empty;
                if (user.ZipCode == null) user.ZipCode = string.Empty;

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                transaction.ReturnMessage.Add(errorMessage);
                transaction.ReturnStatus = false;
            }
            finally
            {
                _userDataService.CloseSession();
            }

            return user;


        }




    }
}
