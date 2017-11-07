using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace WIT.Business
{
    public class WitEntityValidator<T> : AbstractValidator<T>
    {
        public Dictionary<string, string> CheckErrors(T item, string ruleSet)
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
