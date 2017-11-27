using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using WIT.Data.Models;
using System.Runtime.Serialization;

namespace WIT.Business.Entities
{
    public class ItemField
    {
        // from LItemProp entity
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Help { get; set; }
        public LPropTypeEnum PropType { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public bool GridHide { get; set; }
        public bool Required { get; set; }
        public bool Multiple { get; set; }
        public bool Disabled { get; set; }
        public bool IsSortable {
            get {
                return 
                    Id == 0 ||                              // sort by key always allowed
                    PropType == LPropTypeEnum.String ||
                    PropType == LPropTypeEnum.Integer ||
                    PropType == LPropTypeEnum.Decimal ||
                    PropType == LPropTypeEnum.Date ||
                    PropType == LPropTypeEnum.Time ||
                    PropType == LPropTypeEnum.DateTime
                    ;
            }
        }

        public ItemField()
        {
        }

        public ItemField(LItemProp prop)
        {
            if (prop != null)
            {
                Id = prop.ItemPropID;
                Key = string.Format("p{0}", prop.ItemPropID);
                Name = prop.Name;
                Help = prop.Description;
                PropType = prop.PropType;
                ItemTypeID = prop.ItemTypeID;
                GridHide = prop.GridHide;
                Required = prop.Required;
                Multiple = prop.Multiple;
            }
        }
    }


    public class ItemValue : ItemField
    {
        // from LITemPropValueNnnn entity
        public dynamic Value { get; set; }

        public string ValidationError { get; set; }

        public ItemValue(LItemProp prop, List<LItemValue> values)
            : base(prop)
        {
            if (values.Count > 0)
            {
                switch (prop.PropType)
                {
                    case LPropTypeEnum.String:
                        Value = values[0]?.LItemValueString?.Value;
                        break;

                    case LPropTypeEnum.Text:
                        Value = values[0]?.LItemValueText?.Value;
                        break;

                    case LPropTypeEnum.Date:
                    case LPropTypeEnum.DateTime:
                    case LPropTypeEnum.Time:
                        Value = values[0]?.LItemValueDateTime?.Value;
                        break;

                    case LPropTypeEnum.Integer:
                        Value = values[0]?.LItemValueInteger?.Value;
                        break;

                    case LPropTypeEnum.Decimal:
                        Value = values[0]?.LItemValueDecimal?.Value;
                        break;

                    case LPropTypeEnum.Item:
                        Value = values
                            .OrderBy(v => v.ItemValueID)
                            .Take(Multiple ? values.Count : 1)
                            .Select(v => new {
                                ItemId = v.LItemValueItem.ItemID,
                                Key = v.LItemValueItem.Key
                            });
                        break;

                    // TODO: implement other types
                    case LPropTypeEnum.Hyperlink:
                    case LPropTypeEnum.Image:
                    case LPropTypeEnum.Video:
                        break;
                }
            }
        }
    }

    public class ItemEntity
    {
        // from LItem entity
        public long Id { get; set; }
        public string Key { get; set; }
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

        public ItemEntity(LItem existingItem)
        {
            if (existingItem != null)
            {
                Id = existingItem.ItemID;
                Key = existingItem.Key;
                CreatedBy = existingItem.CreatedBy;
                CreatedOn = existingItem.CreatedOn;
                UpdatedBy = existingItem.UpdatedBy;
                UpdatedOn = existingItem.UpdatedOn;

                Name = existingItem.LItemType.Name;
                Help = existingItem.LItemType.Description;

                Values = new List<ItemValue>(existingItem.LItemType.LItemProps.Count + 2);

                foreach (var prop in existingItem.LItemType.LItemProps.OrderBy(p => p.ItemPropID))
                {
                    var propValues = prop.LItemValues.Where(v => v.ItemID == existingItem.ItemID)
                        .OrderBy(v => v.ItemValueID)
                        .ToList()
                        ;

                    Values.Add(new ItemValue(prop, propValues));
                }
            }
        }
    }

    public class ItemGrid
    {
        public GridInfo GridInfo { get; set; }

        public List<ItemField> Fields { get; set; }

        public List<Dictionary<string,object>> Data { get; set; }

        public ItemGrid(GridInfo gridInfo)
        {
            GridInfo = gridInfo;
            Fields = new List<ItemField>();
            Data = new List<Dictionary<string, object>>();

            // fields
            Fields.Add(new ItemField()
            {
                Id = -1,
                Key = "id",
                Name = "Id",
                PropType = LPropTypeEnum.Integer,
                GridHide = true
            });

            Fields.Add(new ItemField()
            {
                Id = 0,
                Key = "key",
                Name = "Key",
                PropType = LPropTypeEnum.String,
                GridHide = false
            });
        }
    }
}
