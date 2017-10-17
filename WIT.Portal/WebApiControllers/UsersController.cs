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

        [Inject]
        public IUserDataService _userDataService { get; set; }

        [Route("Ping")]
        [HttpPost]   
        public HttpResponseMessage Ping([FromBody] UserInformation userInformation)
        {
            TransactionalInformation transaction = new TransactionalInformation();

            userInformation = new UserInformation();
            userInformation.ReturnStatus = true;
            userInformation.ReturnMessage.Add("Ping was successful");

            var response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);          
            return response;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("RegisterUser")]      
        [HttpPost]
        public HttpResponseMessage RegisterUser(HttpRequestMessage request, [FromBody] UserInformation userInformation)
        {
            HttpResponseMessage response = null;
            TransactionalInformation transaction = new TransactionalInformation();

            try
            {
                UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.RegisterUser(userInformation, out transaction);
                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                string tokenString = TokenManager.CreateToken(userInformation);

                response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                response.Headers.Add("Authorization", tokenString);
            }

            catch (HttpResponseException ex)
            {
                response = Request.CreateResponse<TransactionalInformation>(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.ToString());
                response = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userInformation"></param>
        /// <returns></returns>
        [Route("GetProfile")]
        [HttpPost]
        public HttpResponseMessage GetProfile(HttpRequestMessage request, [FromBody] UserInformation userInformation)
        {
            HttpResponseMessage response = null;
            TransactionalInformation transaction = new TransactionalInformation();

            try
            {
                transaction = this.ValidateToken(request, userInformation);

                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.GetProfile(userInformation, out transaction);
                if (transaction.ReturnStatus == false)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);
            }

            catch (HttpResponseException ex)
            {
                response = Request.CreateResponse<TransactionalInformation>(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.ToString());
                response = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userInformation"></param>
        /// <returns></returns>
        [Route("UpdateProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateProfile(HttpRequestMessage request, [FromBody] UserInformation userInformation)
        {
            HttpResponseMessage response = null;
            TransactionalInformation transaction = new TransactionalInformation();

            try
            {
                transaction = this.ValidateToken(request, userInformation);

                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.UpdateProfile(userInformation, out transaction);
                if (transaction.ReturnStatus == false)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);
            }

            catch (HttpResponseException ex)
            {
                response = Request.CreateResponse<TransactionalInformation>(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.ToString());
                response = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userInformation"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody] UserInformation userInformation)
        {
            HttpResponseMessage response = null;
            TransactionalInformation transaction = new TransactionalInformation();

            try
            {
                UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.Login(userInformation, out transaction);
                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                string tokenString = TokenManager.CreateToken(userInformation);

                response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                response.Headers.Add("Authorization", tokenString);
            }

            catch (HttpResponseException ex)
            {
                response = Request.CreateResponse<TransactionalInformation>(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.ToString());
                response = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }

        /// <summary>
        /// Authenicate
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userInformation"></param>
        /// <returns></returns>
        [Route("Authenicate")]
        [HttpPost]
        public HttpResponseMessage Authenicate(HttpRequestMessage request, [FromBody] UserInformation userInformation)
        {
            HttpResponseMessage response = null;
            TransactionalInformation transaction = new TransactionalInformation();

            try
            {
                transaction = this.ValidateToken(request, userInformation);

                if (!transaction.ReturnStatus)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                UserBusinessService userBusinessService = new UserBusinessService(_userDataService);
                userBusinessService.Authenicate(userInformation, out transaction);
                if (transaction.ReturnStatus == false)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                string tokenString = request.Headers.Authorization.ToString();

                response = Request.CreateResponse<UserInformation>(HttpStatusCode.OK, userInformation);
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                response.Headers.Add("Authorization", tokenString);
            }

            catch (HttpResponseException ex)
            {
                response = Request.CreateResponse<TransactionalInformation>(ex.Response.StatusCode, transaction);
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage.Add(ex.ToString());
                response = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }
    }
}