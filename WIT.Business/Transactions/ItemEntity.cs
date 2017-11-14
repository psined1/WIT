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
        public string Help { get; set; }
        public LPropType PropType { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public bool GridHide { get; set; }
        public LValueRule ValueRule { get; set; }
    }

    public class ItemPropertyValue : ItemProperty
    {
        // from LITemPropValueNnnn entity
        public dynamic Value { get; set; }

        public string ValidationError { get; set; }
    }

    public class ItemEntity
    {
        // from LItem entity
        public long Id { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        // from LItemType entity
        public string Name { get; set; }
        public string Help { get; set; }

        // from LItemProp
        public List<ItemPropertyValue> Properties { get; set; }

        // obsolete
        public Dictionary<string, string> ValidationErrors { get; set; }
    }

    public class ItemGrid
    {
        // from LItemType entity:
        public int Id { get; set; }
        public string Name { get; set; }
        public string Help { get; set; }

        public GridInfo GridInfo { get; set; }

        public List<ItemProperty> Fields { get; set; }

        public List<Dictionary<string,object>> Data { get; set; }
    }
}
