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
    public class ProductFeatureValidator
    {
        private WitEntityValidator<ProductFeature> _validator = new WitEntityValidator<ProductFeature>();
        private WitEntities _db = null;

        private ProductFeatureValidator(WitEntities db)
        {
            _db = db;

            _validator.RuleFor(a => a.Code).NotEmpty().WithMessage("Code is required.");

            _validator.RuleSet("new", () =>
            {
                _validator.RuleFor(a => a.Code).Must(NotExist).WithMessage("Code already exists.");
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
            string ruleSet = item.ProductFeatureID == 0 ? "default,new" : "default";
            item.ValidationErrors = new ProductFeatureValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class ProductClassValidator
    {
        private WitEntityValidator<ProductClass> _validator = new WitEntityValidator<ProductClass>();
        private WitEntities _db = null;

        private ProductClassValidator(WitEntities db)
        {
            _db = db;

            _validator.RuleFor(a => a.Code).NotEmpty().WithMessage("Code is required.");

            _validator.RuleSet("new", () =>
            {
                _validator.RuleFor(a => a.Code).Must(NotExist).WithMessage("Code already exists.");
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
            item.ValidationErrors = new ProductClassValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class ProductValidator
    {
        private WitEntityValidator<Product> _validator = new WitEntityValidator<Product>();
        private WitEntities _db = null;

        private ProductValidator(WitEntities db)
        {
            _db = db;

            _validator.RuleFor(a => a.ProductCode).NotEmpty().WithMessage("Code is required.");

            _validator.RuleSet("new", () =>
            {
                _validator.RuleFor(a => a.ProductCode).Must(NotExist).WithMessage("Code already exists.");
            });
        }

        /// <summary>
        /// Validate Duplicate Code
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        private bool NotExist(string code)
        {
            return !_db.Products.Any(a => a.ProductCode == code);
        }

        public static bool Check(WitEntities db, Product item)
        {
            string ruleSet = item.ProductID == 0 ? "default,new" : "default";
            item.ValidationErrors = new ProductValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class CustomerValidator
    {
        private WitEntityValidator<Customer> _validator = new WitEntityValidator<Customer>();
        private WitEntities _db = null;

        private CustomerValidator(WitEntities db)
        {
            _db = db;

            _validator.RuleFor(a => a.CustomerCode).NotEmpty().WithMessage("Code is required.");
            _validator.RuleFor(a => a.CompanyName).NotEmpty().WithMessage("Name is required.");

            _validator.RuleSet("new", () =>
            {
                _validator.RuleFor(a => a.CustomerCode).Must(NotExist).WithMessage("Code already exists.");
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
            item.ValidationErrors = new CustomerValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }
}
