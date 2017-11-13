using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class ItemProperty
    {
        // from LItemProp entity
        public long Id { get; set; }
        public string Name { get; set; }
        public LPropType PropType { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public bool GridHide { get; set; }
        public LValueRule ValueRule { get; set; }

        // from LITemPropValueNnnn entity
        public dynamic Value { get; set; }
    }


    public class ItemEntity
    {
        public long Id { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public Dictionary<string, string> ValidationErrors { get; set; }
    }
}
