﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIT.Data.Models
{
    public partial class ProductFeature
    {
        [NotMapped]
        public Dictionary<string, string> ValidationErrors { get; set; }
    }

    public partial class ProductClass
    {
        [NotMapped]
        public Dictionary<string, string> ValidationErrors { get; set; }
    }

    public partial class Product
    {
        [NotMapped]
        public Dictionary<string, string> ValidationErrors { get; set; }
    }

    public partial class Customer
    {
        [NotMapped]
        public Dictionary<string, string> ValidationErrors { get; set; }
    }

    public partial class User
    {
        [NotMapped]
        public Dictionary<string, string> ValidationErrors { get; set; }

        [NotMapped]
        public string PasswordConfirmation { get; set; }
    }
}