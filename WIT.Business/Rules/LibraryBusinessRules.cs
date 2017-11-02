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
    public class ProductFeatureValidator : AbstractValidator<ProductFeature>
    {
        private WitEntities _db = null;

        public ProductFeatureValidator(WitEntities db)
        {
            _db = db;

            RuleFor(a => a.Code).NotEmpty().WithMessage("Code is required.");
            RuleFor(a => a.Code).Must(NotExist).WithMessage("Code already exists.");
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
    }
}
