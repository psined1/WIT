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
        protected TransactionalInformation ValidateToken(HttpRequestMessage request, TransactionalInformation info)
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
        }
    }
}