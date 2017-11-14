using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;

namespace WIT.Business.Entities
{
    public class ItemField
    {
        // from LItemProp entity
        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Help { get; set; }
        public LPropTypeEnum PropType { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public bool GridHide { get; set; }
        public LValueRuleEnum ValueRule { get; set; }

        public ItemField()
        {
        }

        public ItemField(LItemProp prop)
        {
            Id = prop.ItemPropID;
            Key = string.Format("p{0}", prop.ItemPropID);
            Name = prop.Name;
            Help = prop.Description;
            PropType = prop.PropType;
            ItemTypeID = prop.ItemTypeID;
            GridHide = prop.GridHide;
            ValueRule = prop.ValueRule;
        }
    }

    public class ItemValue : ItemField
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
        public List<ItemValue> Values { get; set; }

        // obsolete
        public Dictionary<string, string> ValidationErrors { get; set; }
    }

    public class ItemGrid
    {
        public GridInfo GridInfo { get; set; }

        public List<ItemField> Fields { get; set; }

        public List<Dictionary<string,object>> Data { get; set; }
    }
}
