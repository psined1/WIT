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
    public class UserBusinessRules : AbstractValidator<User>
    {
        private User _user;
        private Boolean _emailAddressIsValid = true;
        private Boolean _passwordConfirmationEntered = true;

        /// <summary>
        /// Account Business Rules
        /// </summary>
        public UserBusinessRules(IUserDataService userDataService, User user, string passwordConfirmation)
        {
            userDataService.CreateSession();

            if (user.EmailAddress.Length > 0)
            {
                _user = userDataService.GetUser(user.EmailAddress);
                if (_user != null) _emailAddressIsValid = false;
            }

            userDataService.CloseSession();

            passwordConfirmation = passwordConfirmation.Trim();
            if (passwordConfirmation == null || passwordConfirmation == string.Empty || passwordConfirmation.Length == 0)
            {
                _passwordConfirmationEntered = false;
            }

            RuleFor(a => a.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(a => a.LastName).NotEmpty().WithMessage("Last Name is required.");
            RuleFor(a => a.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(a => a.Password).Must(PasswordConfirmationRequired).WithMessage("Password Confirmation is required.");
            RuleFor(a => a.Password).Matches(passwordConfirmation).WithMessage("Passwords do not match");
            RuleFor(a => a.EmailAddress).NotEmpty().WithMessage("Email Address is required.");
            RuleFor(a => a.EmailAddress).EmailAddress().WithMessage("Invalid Email Address.");
            RuleFor(a => a.EmailAddress).Must(ValidateDuplicateEmailAddress).WithMessage("Email Address already exists.");          


        }

        /// <summary>
        /// User Business Rules
        /// </summary>
        /// <param name="userDataService"></param>
        /// <param name="user"></param>
        //public UserBusinessRules(IUserDataService userDataService, User user)
        //{
        //    RuleFor(a => a.FirstName).NotEmpty().WithMessage("First Name is required.");
        //    RuleFor(a => a.LastName).NotEmpty().WithMessage("Last Name is required.");          
        //}


        public UserBusinessRules()
        {
            RuleFor(a => a.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(a => a.LastName).NotEmpty().WithMessage("Last Name is required.");
        }

        /// <summary>
        /// Validate Duplicate Email Address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        private bool ValidateDuplicateEmailAddress(string emailAddress)
        {
            return _emailAddressIsValid;

        }

        /// <summary>
        /// Validation of Password Confirmation
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool PasswordConfirmationRequired(string password)
        {
            return _passwordConfirmationEntered;

        }

    }
}

