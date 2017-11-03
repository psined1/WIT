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
    public class ProductFeatureValidator : AbstractValidator<ProductFeature>
    {
        private WitEntities _db = null;

        private ProductFeatureValidator(WitEntities db)
        {
            _db = db;

            RuleFor(a => a.Code).NotEmpty().WithMessage("Code is required.");

            RuleSet("new", () =>
            {
                RuleFor(a => a.Code).Must(NotExist).WithMessage("Code already exists.");
            });
        }

        /// <summary>
        /// Validate Duplicate Customer Code
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        private bool NotExist(string code)
        {
            return _db.ProductFeatures.FirstOrDefault(a => a.Code == code) == null;
        }

        public static bool Check(WitEntities db, ProductFeature item)
        {
            string ruleSet = item.ProductFeatureId == 0 ? "default,new" : "default";

            ValidationResult results = new ProductFeatureValidator(db).Validate(item, ruleSet: ruleSet);

            if (!results.IsValid)
            {
                item.ValidationErrors = new Dictionary<string, string>();
                foreach (ValidationFailure error in results.Errors)
                {
                    if (!item.ValidationErrors.ContainsKey(error.PropertyName))
                        item.ValidationErrors.Add(error.PropertyName, error.ErrorMessage);
                }
            }

            return item.ValidationErrors.Count == 0;
        }
    }
}
