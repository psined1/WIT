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


namespace WIT.Portal.WebApiControllers
{
    [RoutePrefix("api/library")]
    public class LibraryController : BaseApiController
    {

        //[Inject]
        //public IProductFeatureDataService _customerDataService { get; set; }

        private WIT.Data.Models.Entities _db = new Entities();

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        #region ProductFeature

        /// <summary>
        /// Get ProductFeatures
        /// </summary>
        /// <param name="request"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [Route("GetProductFeatures")]
        [HttpPost]
        public HttpResponseMessage GetProductFeatures(HttpRequestMessage request, [FromBody] ProductFeatureList list)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo();

            try
            {
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

                response = Request.CreateResponse(HttpStatusCode.OK, transaction);
            }

            catch (HttpResponseException ex)
            {
                response = Request.CreateResponse(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage = ex.ToString();
                response = Request.CreateResponse(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }


        /// <summary>
        /// Get ProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("GetProductFeature")]
        [HttpPost]
        public HttpResponseMessage GetProductFeature(HttpRequestMessage request, [FromBody] ProductFeature item)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo();

            try
            {
                ProductFeature existingItem = _db.ProductFeatures.Where(i => i.ProductFeatureId == item.ProductFeatureId).FirstOrDefault();

                if (existingItem == null)
                {
                    response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Content = new StringContent(string.Format("Product feature {0} not found", item.Code))
                    };
                    throw new HttpResponseException(response);
                }

                transaction.Data = existingItem;

                response = Request.CreateResponse(HttpStatusCode.OK, transaction);
            }

            catch (HttpResponseException ex)
            {
                if (response == null)
                    response = Request.CreateResponse(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage = ex.ToString();
                response = Request.CreateResponse(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }

        /// <summary>
        /// Get ProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("DeleteProductFeature")]
        [HttpPost]
        public HttpResponseMessage DeleteProductFeature(HttpRequestMessage request, [FromBody] ProductFeature item)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo();

            try
            {
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

                response = Request.CreateResponse(HttpStatusCode.OK, transaction);
            }

            catch (HttpResponseException ex)
            {
                response = Request.CreateResponse(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage = ex.ToString();
                response = Request.CreateResponse(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }

        #endregion  // ProductFeature
    }
}