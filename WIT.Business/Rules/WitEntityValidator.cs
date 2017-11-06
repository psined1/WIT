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
    public class WitEntityValidator<T> : AbstractValidator<T>
    {
        protected WitEntities _db = null;

        protected Dictionary<string, string> CheckErrors(T item, string ruleSet)
        {
            ValidationResult results = this.Validate(item, ruleSet: ruleSet);
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (!results.IsValid)
            {
                foreach (ValidationFailure error in results.Errors)
                {
                    if (!errors.ContainsKey(error.PropertyName))
                        errors.Add(error.PropertyName, error.ErrorMessage);
                }
            }

            return errors;
        }
    }
}
