using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WIT.Business.Entities
{
    public class TransactionInfo
    {
        public string ReturnMessage { get; set; }
        public Boolean IsAuthenicated { get; set; }

        //[JsonIgnore]
        public int CurrentUserID { get; set; } // DP
        //[JsonIgnore]
        public string CurrentUserEmail { get; set; }

        public object Data { get; set; }

        public TransactionInfo()
        {
            IsAuthenicated = false;
            CurrentUserID = 0;
        }
    }
}
