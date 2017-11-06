using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WIT.Business.Entities;
using WIT.Portal.TokenManagement;
using System.Security.Claims;
using WIT.Business;

namespace WIT.Portal.WebApiControllers
{
    public abstract class BaseApiController : ApiController
    {
        /*protected TransactionalInformation ValidateToken(HttpRequestMessage request, TransactionalInformation info)
        {
            TransactionalInformation transaction = new TransactionalInformation()
            {
                ReturnStatus = false
            };

            if (request.Headers.Authorization == null)
            {
                transaction.ReturnMessage.Add("Your session is invalid.");
                return transaction;
            }

            string tokenString = request.Headers.Authorization.ToString();
            ClaimsPrincipal principal = TokenManager.ValidateToken(tokenString);

            if (principal == null)
            {
                transaction.ReturnMessage.Add("Your session is invalid.");
                return transaction;
            }

            int userID = principal.GetUserID();

            if (userID == 0)
            {
                transaction.ReturnMessage.Add("Your session is invalid.");
                return transaction;
            }

            info.CurrentUserID = userID;
            info.CurrentUserEmail = principal.GetUserEmail();

            transaction.ReturnStatus = true;

            return transaction;
        }*/

        protected void ValidateToken(HttpRequestMessage request, TransactionInfo transaction)
        {
            //transaction.IsAuthenicated = false;
            transaction.CurrentUserID = 0;
            transaction.CurrentUserEmail = null;

            if (request.Headers.Authorization != null)
            {
                string tokenString = request.Headers.Authorization.ToString();
                ClaimsPrincipal principal = TokenManager.ValidateToken(tokenString);

                if (principal != null)
                {
                    int userID = principal.GetUserID();

                    if (userID != 0)
                    {
                        //transaction.IsAuthenicated = true;
                        transaction.CurrentUserID = userID;
                        transaction.CurrentUserEmail = principal.GetUserEmail();
                        return;
                    }
                }
            }

            transaction.ReturnMessage = "Your session is invalid. Please re-login.";
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        protected HttpResponseMessage BaseAction(HttpRequestMessage request, Action<TransactionInfo> controllerAction)
        {
            HttpResponseMessage response = null;
            TransactionInfo transaction = new TransactionInfo();

            try
            {
                controllerAction(transaction);

                response = Request.CreateResponse(HttpStatusCode.OK, transaction);
            }

            catch (HttpResponseException ex)
            {
                response = request.CreateResponse(ex.Response.StatusCode, transaction);
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                    response.Headers.Add("Authorization", "");
                }
            }

            catch (Exception ex)
            {
                transaction.ReturnMessage = ex.ToString();
                response = request.CreateResponse(HttpStatusCode.BadRequest, transaction);
            }

            return response;
        }
    }
}