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
        private WitEntityValidator<ProductFeatureItem> _validator = new WitEntityValidator<ProductFeatureItem>();
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

        public static bool Check(WitEntities db, ProductFeatureItem item)
        {
            string ruleSet = item.ProductFeatureID == 0 ? "default,new" : "default";
            item.ValidationErrors = new ProductFeatureValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class ProductClassValidator
    {
        private WitEntityValidator<ProductClassItem> _validator = new WitEntityValidator<ProductClassItem>();
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
            return !_db.ProductClasses.Any(a => a.Code == code);
        }

        public static bool Check(WitEntities db, ProductClassItem item)
        {
            string ruleSet = item.ProductClassID == 0 ? "default,new" : "default";
            item.ValidationErrors = new ProductClassValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class ProductValidator
    {
        private WitEntityValidator<ProductItem> _validator = new WitEntityValidator<ProductItem>();
        private WitEntities _db = null;

        private ProductValidator(WitEntities db)
        {
            _db = db;

            _validator.RuleFor(a => a.ProductCode).NotEmpty().WithMessage("Code is required.");
            _validator.RuleFor(a => a.ProductClass).Must(ProductClassExistsOrEmpty).WithMessage("Incorrect value");
            _validator.RuleFor(a => a.ProductFeature).Must(ProductFeatureExistsOrEmpty).WithMessage("Incorrect value");

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

        private bool ProductClassExistsOrEmpty(string productClass)
        {
            return
                string.IsNullOrWhiteSpace(productClass) ||
                _db.ProductClasses.Any(a => a.Code == productClass)
                ;
        }

        private bool ProductFeatureExistsOrEmpty(string productFeature)
        {
            return
                string.IsNullOrWhiteSpace(productFeature) ||
                _db.ProductFeatures.Any(a => a.Code == productFeature)
                ;
        }

        public static bool Check(WitEntities db, ProductItem item)
        {
            string ruleSet = item.ProductID == 0 ? "default,new" : "default";
            item.ValidationErrors = new ProductValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class CustomerValidator
    {
        private WitEntityValidator<CustomerItem> _validator = new WitEntityValidator<CustomerItem>();
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

        public static bool Check(WitEntities db, CustomerItem item)
        {
            string ruleSet = item.CustomerID == 0 ? "default,new" : "default";
            item.ValidationErrors = new CustomerValidator(db)._validator.CheckErrors(item, ruleSet);
            return item.ValidationErrors.Count == 0;
        }
    }

    public class ItemTypeValidator
    {
        private WitEntityValidator<ItemEntity> _validator = new WitEntityValidator<ItemEntity>();
        private WitEntities _db = null;

        private ItemTypeValidator(WitEntities db)
        {
            _db = db;

            _validator.RuleSet("delete", () =>
            {
            });

            _validator.RuleSet("new", () =>
            {
            });

            _validator.RuleSet("update", () =>
            {
            });
        }

        public static bool Check(WitEntities db, ItemEntity item)
        {
            //item.ValidationErrors = new ItemValidator(db)._validator.CheckErrors(item, "delete");
            //return item.ValidationErrors.Count == 0;

            return true;    // TODO: implement
        }

        public static bool CheckDelete(WitEntities db, ItemEntity item)
        {
            //item.ValidationErrors = new ItemValidator(db)._validator.CheckErrors(item, "delete");
            //return item.ValidationErrors.Count == 0;

            return true;    // TODO: implement
        }
    }

    public class ItemValidator
    {
        private WitEntityValidator<ItemEntity> _validator = new WitEntityValidator<ItemEntity>();
        private WitEntities _db = null;

        private ItemValidator(WitEntities db)
        {
            _db = db;

            _validator.RuleSet("delete", () =>
            {
            });

            _validator.RuleSet("new", () =>
            {
            });

            _validator.RuleSet("update", () =>
            {
            });
        }

        public static bool Check(WitEntities db, ItemEntity item)
        {
            //item.ValidationErrors = new ItemValidator(db)._validator.CheckErrors(item, "delete");
            //return item.ValidationErrors.Count == 0;

            return true;    // TODO: implement
        }

        public static bool CheckDelete(WitEntities db, ItemEntity item)
        {
            //item.ValidationErrors = new ItemValidator(db)._validator.CheckErrors(item, "delete");
            //return item.ValidationErrors.Count == 0;

            return true;    // TODO: implement
        }
    }
}
