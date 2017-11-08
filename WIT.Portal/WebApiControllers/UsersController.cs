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
            return BaseAction(request, (transaction) => {
                this.ValidateToken(request, transaction);

                User existingItem = _db.Users.FirstOrDefault(i => i.UserID == transaction.CurrentUserID);

                if (existingItem == null)
                {
                    transaction.ReturnMessage = string.Format("User '{0}' not found", transaction.CurrentUserEmail);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                existingItem.Password = null;

                transaction.Data = new UserItem(existingItem);
            });
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("RegisterUser")]      
        [HttpPost]
        public HttpResponseMessage RegisterUser(HttpRequestMessage request, [FromBody] UserItem item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                if (!UserValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                User existingItem = _db.Users.Add(new User());

                existingItem.FirstName = item.FirstName;
                existingItem.LastName = item.LastName;
                existingItem.EmailAddress = item.EmailAddress;
                existingItem.AddressLine1 = item.AddressLine1;
                existingItem.AddressLine2 = item.AddressLine2;
                existingItem.City = item.City;
                existingItem.State = item.State;
                existingItem.ZipCode = item.ZipCode;
                existingItem.Password = item.Password;

                existingItem.CreatedBy = transaction.CurrentUserEmail;
                existingItem.CreatedOn = DateTime.Now;
                existingItem.UpdatedBy = transaction.CurrentUserEmail;
                existingItem.UpdatedOn = DateTime.Now;

                _db.SaveChanges();

                existingItem.Password = null;

                transaction.Data = new UserItem(existingItem);
            });
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("UpdateProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateProfile(HttpRequestMessage request, [FromBody] UserItem item)
        {
            return BaseAction(request, (transaction) => {

                transaction.Data = item;

                this.ValidateToken(request, transaction);

                if (!UserValidator.Check(_db, item))
                {
                    transaction.ReturnMessage = "Please correct all errors.";
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                User existingItem = _db.Users.FirstOrDefault(i => i.UserID == transaction.CurrentUserID);

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

                existingItem.Password = null;

                transaction.Data = new UserItem(existingItem);
            });
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody] UserItem item)
        {
            string token = "";

            HttpResponseMessage response = BaseAction(request, (transaction) => {

                User existingItem = _db.Users.FirstOrDefault(i => i.EmailAddress == item.EmailAddress);

                if (existingItem == null || existingItem.Password != item.Password)
                {
                    transaction.ReturnMessage = "Invalid login or password.";
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                existingItem.Password = null;

                transaction.Data = new UserItem(existingItem);
                transaction.CurrentUserID = existingItem.UserID;
                transaction.CurrentUserEmail = existingItem.EmailAddress;

                //transaction.IsAuthenicated = true;

                token = TokenManager.CreateToken(transaction);
            });

            if (response.StatusCode == HttpStatusCode.OK)
            {
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                response.Headers.Add("Authorization", token);
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
            HttpResponseMessage response = BaseAction(request, (transaction) => {

                this.ValidateToken(request, transaction);

                User existingItem = _db.Users.FirstOrDefault(i => i.UserID == transaction.CurrentUserID && i.EmailAddress == transaction.CurrentUserEmail);

                if (existingItem == null)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                existingItem.Password = null;

                transaction.Data = new UserItem(existingItem);
            });

            if (response.StatusCode == HttpStatusCode.OK)
            {
                response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                response.Headers.Add("Authorization", request.Headers.Authorization.ToString());
            }

            return response;
        }
    }
}