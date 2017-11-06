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
using System.Security.Claims;
using Ninject;
using WIT.Data.Models;

namespace WIT.Portal.WebApiControllers
{
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {

        //[Inject]
        //public IUserDataService _userDataService { get; set; }

        private WitEntities _db = new WitEntities();

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        /*[Route("Ping")]
        [HttpPost]   
        public HttpResponseMessage Ping([FromBody] UserInformation userInformation)
        {
            TransactionalInformation transaction = new TransactionalInformation();

            userInformation = new UserInformation();
            userInformation.ReturnStatus = true;
            userInformation.ReturnMessage.Add("Ping was successful");

            var response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);          
            return response;
        }*/

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("GetProfile")]
        [HttpPost]
        public HttpResponseMessage GetProfile(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo();

            try
            {
                this.ValidateToken(request, transaction);

                User existingItem = _db.Users.Where(i => i.UserID == transaction.CurrentUserID).FirstOrDefault();

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("User '{0}' not found", transaction.CurrentUserEmail);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

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

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("RegisterUser")]      
        [HttpPost]
        public HttpResponseMessage RegisterUser(HttpRequestMessage request, [FromBody] User item)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo()
            {
                Data = item
            };

            try
            {
                //this.ValidateToken(request, transaction);

                if (!UserValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                item.CreatedBy = transaction.CurrentUserEmail;
                item.CreatedOn = DateTime.Now;
                item.UpdatedBy = transaction.CurrentUserEmail;
                item.UpdatedOn = DateTime.Now;

                _db.Users.Add(item);
                _db.SaveChanges();

                transaction.Data = item;

                response = Request.CreateResponse(HttpStatusCode.OK, transaction);

                /*
                 * UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.RegisterUser(userInformation, out transaction);
                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                string tokenString = TokenManager.CreateToken(userInformation);

                response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                response.Headers.Add("Authorization", tokenString);
                */
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
        /// Update Profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateProfile(HttpRequestMessage request, [FromBody] User item)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo()
            {
                Data = item
            };

            try
            {
                this.ValidateToken(request, transaction);

                /*
                 * UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.UpdateProfile(userInformation, out transaction);
                if (transaction.ReturnStatus == false)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);
                */

                if (!UserValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                User existingItem = _db.Users.Where(i => i.UserID == transaction.CurrentUserID).FirstOrDefault();

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("User '{0}' not found", transaction.CurrentUserEmail);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                existingItem.FirstName = item.FirstName;
                existingItem.LastName = item.LastName;
                existingItem.AddressLine1 = item.AddressLine1;
                existingItem.AddressLine2 = item.AddressLine2;
                existingItem.City = item.City;
                existingItem.State = item.State;
                existingItem.ZipCode = item.ZipCode;

                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

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

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody] User item)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo();

            try
            {
                /*
                 * UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.Login(userInformation, out transaction);
                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                */

                User existingItem = _db.Users.Where(i => i.EmailAddress == item.EmailAddress).FirstOrDefault();

                if (existingItem == null || existingItem.Password != item.Password)
                {
                    transaction.ReturnMessage = "Invalid login or password.";
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                transaction.CurrentUserID = existingItem.UserID;
                transaction.CurrentUserEmail = existingItem.EmailAddress;
                transaction.IsAuthenicated = true;

                response = Request.CreateResponse(HttpStatusCode.OK, transaction);
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                response.Headers.Add("Authorization", TokenManager.CreateToken(transaction));
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
        /// Authenicate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("Authenicate")]
        [HttpPost]
        public HttpResponseMessage Authenicate(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo();

            try
            {
                this.ValidateToken(request, transaction);

                /*
                 * 
                 * UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.Authenicate(userInformation, out transaction);
                if (transaction.ReturnStatus == false)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                string tokenString = request.Headers.Authorization.ToString();
                */

                User existingItem = _db.Users.Where(i => i.UserID == transaction.CurrentUserID && i.EmailAddress == transaction.CurrentUserEmail).FirstOrDefault();

                if (existingItem == null)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                transaction.CurrentUserID = existingItem.UserID;
                transaction.CurrentUserEmail = existingItem.EmailAddress;
                transaction.IsAuthenicated = true;

                response = Request.CreateResponse(HttpStatusCode.OK, transaction);
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                response.Headers.Add("Authorization", request.Headers.Authorization.ToString());
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
    }
}