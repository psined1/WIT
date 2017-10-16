using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using WIT.Business.Entities;

namespace WIT.Portal.TokenManagement
{
    public class TokenManager
    {
        /// <summary>
        /// Create Token
        /// </summary>
        /// <param name="userInformation"></param>
        /// <returns></returns>
        public static string CreateToken(UserInformation userInformation)
        {
            string userID = userInformation.UserID.ToString();
    
            var claims = new List<Claim>();
            claims.Add(CreateClaim("UserID", userID));
   
            var tokenHandler = new JwtSecurityTokenHandler();

            DateTime now = DateTime.UtcNow.AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Hours);

            var symmetricKey = GetBytes("WIT.Token");
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new ClaimsIdentity(claims)),
                TokenIssuerName = "WIT.Token",
                AppliesToAddress = "http://localhost",
                Lifetime = new Lifetime(now, now.AddDays(1)),
                SigningCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(symmetricKey),
                                                             "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                                                            "http://www.w3.org/2001/04/xmlenc#sha256")
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        /// <summary>
        /// Validate Token
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public static ClaimsPrincipal ValidateToken(string tokenString)
        {
            var symmetricKey = GetBytes("WIT.Token");

            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidIssuer = "WIT.Token",
                ValidAudience = "http://localhost",
                ValidateLifetime = true,
                ValidateIssuer = true,
                RequireExpirationTime = true,
                IssuerSigningKey = new InMemorySymmetricSecurityKey(symmetricKey)

            };

            SecurityToken token = new JwtSecurityToken();
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(tokenString, validationParameters, out token);

                return principal;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
            }

            return null;

        }

        /// <summary>
        /// Create Claim
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value);
        }

        /// <summary>
        /// Get Bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

        }

        /// <summary>
        /// Get UserID
        /// </summary>
        /// <param name="tokenString"></param>
        /// <returns></returns>
        public static int GetUserID(string tokenString)
        {
            ClaimsPrincipal principal = TokenManager.ValidateToken(tokenString);
            var claim = principal.Claims.Where(p => p.Type == "UserID").SingleOrDefault();
            if (claim == null)
            {
                return 0;
            }

            int userID = Convert.ToInt32(claim.Value);

            return userID;

        }



    }
}
