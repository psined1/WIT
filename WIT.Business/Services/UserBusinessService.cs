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
        public void RegisterUser(UserInformation userInformation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation()
            {
                ReturnStatus = false
            };

            try
            {
                _userDataService.CreateSession();
                _userDataService.BeginTransaction();

                UserBusinessRules userBusinessRules = new UserBusinessRules(_userDataService, userInformation);
                ValidationResult results = userBusinessRules.ValidateRegisterUser();

                if (!results.IsValid)
                {
                    transaction.PopulateValidationErrors(results.Errors);
                    return;
                }

                User user = new User();
                user.EmailAddress = userInformation.EmailAddress;
                user.FirstName = userInformation.FirstName;
                user.LastName = userInformation.LastName;
                user.Password = userInformation.Password;
                user.AddressLine1 = userInformation.AddressLine1;
                user.AddressLine2 = userInformation.AddressLine2;
                user.City = userInformation.City;
                user.State = userInformation.State;
                user.ZipCode = userInformation.ZipCode;
                user.CreatedBy = userInformation.EmailAddress;
                user.UpdatedBy = userInformation.EmailAddress;

                _userDataService.CreateUser(user);
                _userDataService.CommitTransaction(true);

                populateFromUser(userInformation, user);

                userInformation.CurrentUserID = user.UserID;
                userInformation.CurrentUserEmail = user.EmailAddress;

                userInformation.IsAuthenicated = true;
                userInformation.ReturnMessage.Clear();
                userInformation.ReturnMessage.Add("Profile successfully created.");
                userInformation.ReturnStatus = true;

                transaction.ReturnStatus = true;
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.Message);
            }

            finally
            {
                _userDataService.CloseSession();
            }
        }

        /// <summary>
        /// Get Profile
        /// </summary>
        /// <param name="userInformation"></param>
        /// <param name="transaction"></param>
        public void GetProfile(UserInformation userInformation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation()
            {
                ReturnStatus = false
            };

            try
            {
                _userDataService.CreateSession();

                User user = _userDataService.GetUser(userInformation.CurrentUserID);

                populateFromUser(userInformation, user);

                userInformation.ReturnMessage.Clear();
                userInformation.ReturnMessage.Add("Success.");
                userInformation.ReturnStatus = true;

                transaction.ReturnStatus = true;
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.Message);
            }

            finally
            {
                _userDataService.CloseSession();
            }
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="userInformation"></param>
        /// <param name="transaction"></param>
        public void UpdateProfile(UserInformation userInformation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation()
            {
                ReturnStatus = false
            };

            try
            {
                _userDataService.CreateSession();
                _userDataService.BeginTransaction();

                UserBusinessRules userBusinessRules = new UserBusinessRules(_userDataService, userInformation);
                ValidationResult results = userBusinessRules.ValidateUpdateProfile();

                if (!results.IsValid)
                {
                    transaction.PopulateValidationErrors(results.Errors);
                    return;
                }

                User user = _userDataService.GetUser(userInformation.CurrentUserID);
                
                user.FirstName = userInformation.FirstName;
                user.LastName = userInformation.LastName;
                user.AddressLine1 = userInformation.AddressLine1;
                user.AddressLine2 = userInformation.AddressLine2;
                user.City = userInformation.City;
                user.State = userInformation.State;
                user.ZipCode = userInformation.ZipCode;
                user.UpdatedBy = userInformation.CurrentUserEmail;
                user.UpdatedOn = DateTime.Now;

                _userDataService.UpdateUser(user);
                _userDataService.CommitTransaction(true);

                populateFromUser(userInformation, user);

                userInformation.ReturnMessage.Clear();
                userInformation.ReturnMessage.Add("Profile successfully updated.");
                userInformation.ReturnStatus = true;

                transaction.ReturnStatus = true;
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.Message);
            }

            finally
            {
                _userDataService.CloseSession();
            }
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public void Login(UserInformation userInformation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation()
            {
                ReturnStatus = false
            };

            try
            {
                _userDataService.CreateSession();

                User user = _userDataService.GetUser(userInformation.EmailAddress);

                if (user == null || user.Password != userInformation.Password)
                {
                    transaction.ReturnMessage.Add("Invalid login or password.");
                    return;
                }

                populateFromUser(userInformation, user);

                userInformation.CurrentUserID = user.UserID;
                userInformation.CurrentUserEmail = user.EmailAddress;

                userInformation.IsAuthenicated = true;
                userInformation.ReturnStatus = true;
                userInformation.ReturnMessage.Clear();
                userInformation.ReturnMessage.Add("Login successful.");

                transaction.ReturnStatus = true;
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.Message);
            }

            finally
            {
                _userDataService.CloseSession();
            }
        }

        /// <summary>
        /// Authenicate
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public void Authenicate(UserInformation userInformation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation()
            {
                ReturnStatus = false
            };

            try
            {
                _userDataService.CreateSession();

                User user = _userDataService.GetUser(userInformation.CurrentUserID);
                if (user == null)
                {
                    transaction.ReturnMessage.Add("Invalid session");
                    transaction.ReturnStatus = false;
                    return;
                }

                populateFromUser(userInformation, user);

                userInformation.IsAuthenicated = true;
                userInformation.ReturnStatus = true;
                userInformation.ReturnMessage.Clear();
                userInformation.ReturnMessage.Add("Valid session.");

                transaction.ReturnStatus = true;
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.Message);
            }

            finally
            {
                _userDataService.CloseSession();
            }
        }

        private void populateFromUser(UserInformation userInformation, User user)
        {
            userInformation.UserID = user.UserID;
            userInformation.EmailAddress = user.EmailAddress;
            userInformation.FirstName = user.FirstName;
            userInformation.LastName = user.LastName;
            userInformation.AddressLine1 = user.AddressLine1 ?? String.Empty;
            userInformation.AddressLine2 = user.AddressLine2 ?? String.Empty;
            userInformation.City = user.City ?? String.Empty;
            userInformation.State = user.State ?? String.Empty;
            userInformation.ZipCode = user.ZipCode ?? String.Empty;

            userInformation.Password = String.Empty;
            userInformation.PasswordConfirmation = String.Empty;
        }
    }
}
