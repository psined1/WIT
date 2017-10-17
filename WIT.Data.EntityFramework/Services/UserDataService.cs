using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIT.Interfaces;
//using WIT.Business.Entities;
using WIT.Data.Services;
using WIT.Data.Models;

namespace WIT.Data.Services
{
   
    /// <summary>
    /// User Data Service
    /// </summary>
    public class UserDataService : EntityFrameworkService, IUserDataService
    {
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(User user)
        {
            DateTime now = DateTime.Now;
            user.CreatedOn = now;
            user.UpdatedOn = now;
            db.Users.Add(user);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            user.UpdatedOn = DateTime.Now;
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public User GetUser(string emailAddress)
        {
            User user = db.Users.Where(u => u.EmailAddress == emailAddress).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GetUser(int userID)
        {
            User user = db.Users.Where(u => u.UserID == userID).FirstOrDefault();
            return user;
        }

    }
}
