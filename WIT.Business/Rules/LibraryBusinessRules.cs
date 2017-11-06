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
    public class ProductFeatureValidator : WitEntityValidator<ProductFeature>
    {
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
        /// Validate Duplicate Code
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        private bool NotExist(string code)
        {
            return !_db.ProductFeatures.Any(a => a.Code == code);
        }

        public static bool Check(WitEntities db, ProductFeature item)
        {
            string ruleSet = item.ProductFeatureId == 0 ? "default,new" : "default";
            item.ValidationErrors = new ProductFeatureValidator(db).CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class ProductClassValidator : WitEntityValidator<ProductClass>
    {
        private ProductClassValidator(WitEntities db)
        {
            _db = db;

            RuleFor(a => a.Code).NotEmpty().WithMessage("Code is required.");

            RuleSet("new", () =>
            {
                RuleFor(a => a.Code).Must(NotExist).WithMessage("Code already exists.");
            });
        }

        /// <summary>
        /// Validate Duplicate Code
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        private bool NotExist(string code)
        {
            return !_db.ProductFeatures.Any(a => a.Code == code);
        }

        public static bool Check(WitEntities db, ProductClass item)
        {
            string ruleSet = item.ProductClassID == 0 ? "default,new" : "default";
            item.ValidationErrors = new ProductClassValidator(db).CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class CustomerValidator : WitEntityValidator<Customer>
    {
        private CustomerValidator(WitEntities db)
        {
            _db = db;

            RuleFor(a => a.CustomerCode).NotEmpty().WithMessage("Code is required.");
            RuleFor(a => a.CompanyName).NotEmpty().WithMessage("Name is required.");

            RuleSet("new", () =>
            {
                RuleFor(a => a.CustomerCode).Must(NotExist).WithMessage("Code already exists.");
            });
        }

        /// <summary>
        /// Validate Duplicate Code
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        private bool NotExist(string customerCode)
        {
            return !_db.Customers.Any(a => a.CustomerCode == customerCode);
        }

        public static bool Check(WitEntities db, Customer item)
        {
            string ruleSet = item.CustomerID == 0 ? "default,new" : "default";
            item.ValidationErrors = new CustomerValidator(db).CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }
}
