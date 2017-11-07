using System;
using System.Linq;
using FluentValidation;
using WIT.Data.Models;

namespace WIT.Business
{
    public class UserValidator //: WitEntityValidator<User>
    {
        private WitEntityValidator<User> _validator = new WitEntityValidator<User>();
        private WitEntities _db = null;

        /// <summary>
        /// Account Business Rules
        /// </summary>
        public UserValidator(WitEntities db)
        {
            _db = db;

            // default rules
            _validator.RuleFor(a => a.FirstName).NotEmpty().WithMessage("First Name is required.");
            _validator.RuleFor(a => a.LastName).NotEmpty().WithMessage("Last Name is required.");

            _validator.RuleSet("register", () => {
                _validator.RuleFor(a => a.EmailAddress)
                    .NotEmpty().WithMessage("Email Address is required.")
                    .EmailAddress().WithMessage("Invalid Email Address.")
                    .Must(NotExist).WithMessage("Email Address already exists.")
                    ;

                _validator.RuleFor(a => a.PasswordConfirmation).NotEmpty().WithMessage("Password Confirmation is required.");

                _validator.RuleFor(a => a.Password)
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
            item.ValidationErrors = new UserValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }
}

