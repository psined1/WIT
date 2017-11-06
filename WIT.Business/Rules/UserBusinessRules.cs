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
    public class UserValidator : WitEntityValidator<User>
    {
        /// <summary>
        /// Account Business Rules
        /// </summary>
        public UserValidator(WitEntities db)
        {
            _db = db;

            // default rules
            RuleFor(a => a.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(a => a.LastName).NotEmpty().WithMessage("Last Name is required.");

            RuleSet("register", () => {
                RuleFor(a => a.EmailAddress)
                    .NotEmpty().WithMessage("Email Address is required.")
                    .EmailAddress().WithMessage("Invalid Email Address.")
                    .Must(NotExist).WithMessage("Email Address already exists.")
                    ;

                RuleFor(a => a.PasswordConfirmation).NotEmpty().WithMessage("Password Confirmation is required.");

                RuleFor(a => a.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .Matches(a => (a.PasswordConfirmation ?? String.Empty).Trim()).WithMessage("Passwords do not match")
                    ;
            });
        }

        private bool NotExist(string emailAddress)
        {
            return !_db.Users.Any(a => a.EmailAddress == emailAddress);
        }

        public static bool Check(WitEntities db, User item)
        {
            string ruleSet = item.UserID == 0 ? "default,register" : "default";
            item.ValidationErrors = new UserValidator(db).CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }
}

