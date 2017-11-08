using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class UserItem : BaseEntity
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }


        public UserItem() { }
        public UserItem(User rhs)
        {
            CreatedOn = rhs.CreatedOn;
            UpdatedOn = rhs.UpdatedOn;
            CreatedBy = rhs.CreatedBy;
            UpdatedBy = rhs.UpdatedBy;

            UserID = rhs.UserID;
            FirstName = rhs.FirstName;
            LastName = rhs.LastName;
            EmailAddress = rhs.EmailAddress;
            AddressLine1 = rhs.AddressLine1;
            AddressLine2 = rhs.AddressLine2;
            City = rhs.City;
            State = rhs.State;
            ZipCode = rhs.ZipCode;
            Password = rhs.Password;
        }
    }

    public class UserList
    {
        public GridInfo GridInfo { get; set; }
        public List<UserItem> Items { get; set; }

        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }

        public UserList()
        {
            Items = new List<UserItem>();
            GridInfo = new GridInfo();
        }
    }
}
