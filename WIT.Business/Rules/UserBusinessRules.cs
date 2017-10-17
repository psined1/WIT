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
using FluentValidation.Results;

namespace WIT.Business
{
    public class UserBusinessRules : AbstractValidator<UserInformation>
    {
        private UserInformation _userInformation = null;
        private IUserDataService _userDataService = null;

        /// <summary>
        /// Account Business Rules
        /// </summary>
        public UserBusinessRules(IUserDataService userDataService, UserInformation userInformation)
        {
            _userInformation = userInformation;
            _userDataService = userDataService;

            _userInformation.PasswordConfirmation = (_userInformation.PasswordConfirmation ?? String.Empty).Trim();

            // default rules
            RuleFor(a => a.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(a => a.LastName).NotEmpty().WithMessage("Last Name is required.");

            RuleSet("register", () => {
                RuleFor(a => a.EmailAddress)
                    .NotEmpty().WithMessage("Email Address is required.")
                    .EmailAddress().WithMessage("Invalid Email Address.")
                    .Must(e => ValidateDuplicateEmailAddress(e)).WithMessage("Email Address already exists.")
                    ;

                RuleFor(a => a.PasswordConfirmation).NotEmpty().WithMessage("Password Confirmation is required.");

                RuleFor(a => a.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .Matches(_userInformation.PasswordConfirmation).WithMessage("Passwords do not match")
                    ;
            });
        }


        
        /// <summary>
        /// Validate Duplicate Email Address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        private bool ValidateDuplicateEmailAddress(string emailAddress)
        {
            return (
                string.IsNullOrWhiteSpace(_userInformation.EmailAddress) || 
                _userDataService.GetUser(_userInformation.EmailAddress) == null
                );
        }

        public ValidationResult ValidateRegisterUser()
        {
            return this.Validate(_userInformation, ruleSet: "default,register");
        }

        public ValidationResult ValidateUpdateProfile()
        {
            return this.Validate(_userInformation, ruleSet: "default");
        }
    }
}

