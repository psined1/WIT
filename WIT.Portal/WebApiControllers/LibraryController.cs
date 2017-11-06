using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WIT.Business.Entities;
using WIT.Interfaces;
using WIT.Portal.TokenManagement;
using WIT.Business;
using WIT.Business.Common;
using System.Security.Claims;
using Ninject;
using WIT.Data.Models;
using System.Linq.Dynamic;
using FluentValidation.Results;

namespace WIT.Portal.WebApiControllers
{
    [RoutePrefix("api/library")]
    public class LibraryController : BaseApiController
    {

        //[Inject]
        //public IProductFeatureDataService _customerDataService { get; set; }

        private WitEntities _db = new WitEntities();

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        #region ProductFeature

        /// <summary>
        /// GetProductFeatures
        /// </summary>
        /// <param name="request"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("GetProductFeatures")]
        [HttpPost]
        public HttpResponseMessage GetProductFeatures(HttpRequestMessage request, [FromBody] ProductFeatureList list)
        {
            return BaseAction(request, (transaction) => {

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortExpression))
                    list.GridInfo.SortExpression = "Code";

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortDirection))
                    list.GridInfo.SortDirection = "ASC";

                var q = _db.ProductFeatures.AsQueryable();

                if (!string.IsNullOrWhiteSpace(list.Code))
                {
                    q = q.Where(i => i.Code.StartsWith(list.Code));
                }

                if (!string.IsNullOrWhiteSpace(list.Name))
                {
                    q = q.Where(i => i.Name.StartsWith(list.Name));
                }

                list.GridInfo.TotalRows = q.Count();

                list.Items = q
                    .OrderBy(string.Format("{0} {1}", list.GridInfo.SortExpression, list.GridInfo.SortDirection))
                    .Skip((list.GridInfo.CurrentPageNumber - 1) * list.GridInfo.PageSize)
                    .Take(list.GridInfo.PageSize)
                    .ToList()
                    ;

                transaction.Data = list;
            });
        }


        /// <summary>
        /// GetProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("GetProductFeature")]
        [HttpPost]
        public HttpResponseMessage GetProductFeature(HttpRequestMessage request, [FromBody] ProductFeature item)
        {
            return BaseAction(request, (transaction) => {

                ProductFeature existingItem = _db.ProductFeatures.Where(i => i.ProductFeatureId == item.ProductFeatureId).FirstOrDefault();

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product feature {0} not found", item.Code);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                transaction.Data = existingItem;
            });
        }

        /// <summary>
        /// DeleteProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("DeleteProductFeature")]
        [HttpPost]
        public HttpResponseMessage DeleteProductFeature(HttpRequestMessage request, [FromBody] ProductFeature item)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                ProductFeature existingItem = _db.ProductFeatures.Where(i => i.ProductFeatureId == item.ProductFeatureId).FirstOrDefault();

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product feature {0} not found", item.Code);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _db.ProductFeatures.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = existingItem;
            });
        }

        /// <summary>
        /// UpdateProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateProductFeature")]
        [HttpPost]
        public HttpResponseMessage UpdateProductFeature(HttpRequestMessage request, [FromBody] ProductFeature item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!ProductFeatureValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                ProductFeature existingItem = _db.ProductFeatures.Where(i => i.ProductFeatureId == item.ProductFeatureId).FirstOrDefault();

                if (existingItem == null)
                {
                    existingItem = _db.ProductFeatures.Add(item);

                    existingItem.CreatedBy = transaction.CurrentUserEmail;
                    existingItem.CreatedOn = DateTime.Now;
                }

                else
                {
                    //existingItem.Code = item.Code;
                    existingItem.Name = item.Name;
                    existingItem.Description = item.Description;
                }

                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

                _db.SaveChanges();

                transaction.Data = existingItem;
            });
        }

        #endregion  // ProductFeature

        #region ProductClass

        /// <summary>
        /// GetProductClasss
        /// </summary>
        /// <param name="request"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("GetProductClasses")]
        [HttpPost]
        public HttpResponseMessage GetProductClasses(HttpRequestMessage request, [FromBody] ProductClassList list)
        {
            return BaseAction(request, (transaction) => {

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortExpression))
                    list.GridInfo.SortExpression = "Code";

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortDirection))
                    list.GridInfo.SortDirection = "ASC";

                var q = _db.ProductClasses.AsQueryable();

                if (!string.IsNullOrWhiteSpace(list.Code))
                {
                    q = q.Where(i => i.Code.StartsWith(list.Code));
                }

                if (!string.IsNullOrWhiteSpace(list.Name))
                {
                    q = q.Where(i => i.Name.StartsWith(list.Name));
                }

                list.GridInfo.TotalRows = q.Count();

                list.Items = q
                    .OrderBy(string.Format("{0} {1}", list.GridInfo.SortExpression, list.GridInfo.SortDirection))
                    .Skip((list.GridInfo.CurrentPageNumber - 1) * list.GridInfo.PageSize)
                    .Take(list.GridInfo.PageSize)
                    .ToList()
                    ;

                transaction.Data = list;
            });
        }


        /// <summary>
        /// GetProductClass
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("GetProductClass")]
        [HttpPost]
        public HttpResponseMessage GetProductClass(HttpRequestMessage request, [FromBody] ProductClass item)
        {
            return BaseAction(request, (transaction) => {

                ProductClass existingItem = _db.ProductClasses.Where(i => i.ProductClassID == item.ProductClassID).FirstOrDefault();

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product class {0} not found", item.Code);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                transaction.Data = existingItem;
            });
        }

        /// <summary>
        /// DeleteProductClass
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("DeleteProductClass")]
        [HttpPost]
        public HttpResponseMessage DeleteProductClass(HttpRequestMessage request, [FromBody] ProductClass item)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                ProductClass existingItem = _db.ProductClasses.Where(i => i.ProductClassID == item.ProductClassID).FirstOrDefault();

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Product class {0} not found", item.Code);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _db.ProductClasses.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = existingItem;
            });
        }

        /// <summary>
        /// UpdateProductClass
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateProductClass")]
        [HttpPost]
        public HttpResponseMessage UpdateProductClass(HttpRequestMessage request, [FromBody] ProductClass item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!ProductClassValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                ProductClass existingItem = _db.ProductClasses.Where(i => i.ProductClassID == item.ProductClassID).FirstOrDefault();

                if (existingItem == null)
                {
                    existingItem = _db.ProductClasses.Add(item);

                    existingItem.CreatedBy = transaction.CurrentUserEmail;
                    existingItem.CreatedOn = DateTime.Now;
                }

                else
                {
                    //existingItem.Code = item.Code;
                    existingItem.Name = item.Name;
                    existingItem.Description = item.Description;
                }

                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

                _db.SaveChanges();

                transaction.Data = existingItem;
            });
        }

        #endregion  // ProductClass

        #region Customer

        /// <summary>
        /// GetCustomers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("GetCustomers")]
        [HttpPost]
        public HttpResponseMessage GetCustomers(HttpRequestMessage request, [FromBody] CustomerList list)
        {
            return BaseAction(request, (transaction) => {

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortExpression))
                    list.GridInfo.SortExpression = "CustomerCode";

                if (string.IsNullOrWhiteSpace(list.GridInfo.SortDirection))
                    list.GridInfo.SortDirection = "ASC";

                var q = _db.Customers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(list.CustomerCode))
                {
                    q = q.Where(i => i.CustomerCode.StartsWith(list.CustomerCode));
                }

                if (!string.IsNullOrWhiteSpace(list.CompanyName))
                {
                    q = q.Where(i => i.CompanyName.StartsWith(list.CompanyName));
                }

                list.GridInfo.TotalRows = q.Count();

                list.Items = q
                    .OrderBy(string.Format("{0} {1}", list.GridInfo.SortExpression, list.GridInfo.SortDirection))
                    .Skip((list.GridInfo.CurrentPageNumber - 1) * list.GridInfo.PageSize)
                    .Take(list.GridInfo.PageSize)
                    .ToList()
                    ;

                transaction.Data = list;
            });
        }


        /// <summary>
        /// GetCustomer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("GetCustomer")]
        [HttpPost]
        public HttpResponseMessage GetCustomer(HttpRequestMessage request, [FromBody] Customer item)
        {
            return BaseAction(request, (transaction) => {

                Customer existingItem = _db.Customers.Where(i => i.CustomerID == item.CustomerID).FirstOrDefault();

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Customer {0} not found", item.CustomerCode);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                transaction.Data = existingItem;
            });
        }

        /// <summary>
        /// DeleteCustomer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("DeleteCustomer")]
        [HttpPost]
        public HttpResponseMessage DeleteCustomer(HttpRequestMessage request, [FromBody] Customer item)
        {
            return BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                Customer existingItem = _db.Customers.Where(i => i.CustomerID == item.CustomerID).FirstOrDefault();

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("Customer {0} not found", item.CustomerCode);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _db.Customers.Remove(existingItem);
                _db.SaveChanges();

                transaction.Data = existingItem;
            });
        }

        /// <summary>
        /// UpdateCustomer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateCustomer")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomer(HttpRequestMessage request, [FromBody] Customer item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!CustomerValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                Customer existingItem = _db.Customers.Where(i => i.CustomerID == item.CustomerID).FirstOrDefault();

                if (existingItem == null)
                {
                    existingItem = _db.Customers.Add(item);

                    existingItem.CreatedBy = transaction.CurrentUserEmail;
                    existingItem.CreatedOn = DateTime.Now;
                }

                else
                {
                    //existingItem.CustomerCode = item.CustomerCode;
                    existingItem.CompanyName = item.CompanyName;
                    existingItem.AddressLine1 = item.AddressLine1;
                    existingItem.AddressLine2 = item.AddressLine2;
                    existingItem.City = item.City;
                    existingItem.State = item.State;
                    existingItem.ZipCode = item.ZipCode;
                    existingItem.PhoneNumber = item.PhoneNumber;
                }

                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

                _db.SaveChanges();

                transaction.Data = existingItem;
            });
        }

        #endregion  // Customer
    }
}