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

        /// <summary>
        /// Get ProductFeatures
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userInformation"></param>
        /// <returns></returns>
        [Route("GetProductFeatures")]
        [HttpPost]
        public HttpResponseMessage GetProductFeatures(HttpRequestMessage request, [FromBody] ProductFeatureInformation customerInformation)
        {

            TransactionalInformation transaction = new TransactionalInformation();

            string customerCode = customerInformation.ProductFeatureCode;
            string companyName = customerInformation.CompanyName;
            int currentPageNumber = customerInformation.CurrentPageNumber;
            int pageSize = customerInformation.PageSize;
            string sortExpression = customerInformation.SortExpression;
            string sortDirection = customerInformation.SortDirection;

            int totalRows = 0;

            ProductFeatureBusinessService customerBusinessService = new ProductFeatureBusinessService(_customerDataService);
            List<ProductFeature> customers = customerBusinessService.GetProductFeatures(customerCode, companyName, currentPageNumber, pageSize, sortDirection, sortExpression, out totalRows, out transaction);      
            if (transaction.ReturnStatus == false)
            {
                var badResponse = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
                return badResponse;
            }

            customerInformation = new ProductFeatureInformation();        
            customerInformation.ReturnStatus = transaction.ReturnStatus;
            customerInformation.TotalRows = totalRows;
            customerInformation.TotalPages = Utilities.CalculateTotalPages(totalRows, pageSize);
            customerInformation.ReturnMessage.Add("page " + currentPageNumber + " of " + customerInformation.TotalPages + " returned at " + DateTime.Now.ToString());
            customerInformation.ProductFeatures = customers;

            var response = Request.CreateResponse<ProductFeatureInformation>(HttpStatusCode.OK, customerInformation);
            return response;

        }

        /// <summary>
        /// Get ProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerInformation"></param>
        /// <returns></returns>
        [Route("GetProductFeature")]
        [HttpPost]
        public HttpResponseMessage GetProductFeature(HttpRequestMessage request, [FromBody] ProductFeatureInformation customerInformation)
        {

            TransactionalInformation transaction = new TransactionalInformation();

            int customerID = customerInformation.ProductFeatureID;          

            ProductFeatureBusinessService customerBusinessService = new ProductFeatureBusinessService(_customerDataService);
            customerInformation = customerBusinessService.GetProductFeature(customerID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                var badResponse = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
                return badResponse;
            }
         
            customerInformation.ReturnStatus = transaction.ReturnStatus;           

            var response = Request.CreateResponse<ProductFeatureInformation>(HttpStatusCode.OK, customerInformation);
            return response;

        }


        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerInformation"></param>
        /// <returns></returns>
        [Route("UpdateProductFeature")]
        [HttpPost]
        public HttpResponseMessage UpdateProductFeature(HttpRequestMessage request, [FromBody] ProductFeatureInformation customerInformation)
        {

            TransactionalInformation transaction = new TransactionalInformation();

            try
            {
                transaction = this.ValidateToken(request, customerInformation);

                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                ProductFeatureBusinessService customerBusinessService = new ProductFeatureBusinessService(_customerDataService);
                customerBusinessService.UpdateProductFeature(customerInformation, out transaction);

                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                customerInformation.ReturnStatus = true;
                customerInformation.ReturnMessage = transaction.ReturnMessage;

                return Request.CreateResponse<ProductFeatureInformation>(HttpStatusCode.OK, customerInformation);
            }

            catch (HttpResponseException ex)
            {
                return Request.CreateResponse<TransactionalInformation>(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.ToString());
                return Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
            }
        }

        /// <summary>
        /// Delete ProductFeature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerInformation"></param>
        /// <returns></returns>
        [Route("DeleteProductFeature")]
        [HttpPost]
        public HttpResponseMessage DeleteProductFeature(HttpRequestMessage request, [FromBody] ProductFeatureInformation customerInformation)
        {

            TransactionalInformation transaction = new TransactionalInformation();

            try
            {
                transaction = this.ValidateToken(request, customerInformation);

                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                ProductFeatureBusinessService customerBusinessService = new ProductFeatureBusinessService(_customerDataService);
                customerBusinessService.DeleteProductFeature(customerInformation, out transaction);

                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                customerInformation.ReturnStatus = true;
                customerInformation.ReturnMessage = transaction.ReturnMessage;

                return Request.CreateResponse<ProductFeatureInformation>(HttpStatusCode.OK, customerInformation);
            }

            catch (HttpResponseException ex)
            {
                return Request.CreateResponse<TransactionalInformation>(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.ToString());
                return Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
            }
        }



    }
}