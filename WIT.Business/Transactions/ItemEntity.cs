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
        public bool Unique { get; set; }
        public bool UpperCase { get; set; }
        public bool Disabled
        {
            get {
                return false;       // TODO: not implemented
            }
        }
                    
        public bool IsSortable {
            get {
                return 
                    PropType == LPropTypeEnum.String ||
                    PropType == LPropTypeEnum.Integer ||
                    PropType == LPropTypeEnum.Decimal ||
                    PropType == LPropTypeEnum.Date ||
                    PropType == LPropTypeEnum.Time ||
                    PropType == LPropTypeEnum.DateTime
                    ;
            }
        }

        public bool IsItemKey
        {
            get
            {
                return Unique && Required && !Multiple;
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
                Unique = prop.Unique;
                UpperCase = prop.UpperCase;
            }
        }
    }


    public class ItemValue : ItemField
    {
        // from LITemPropValueNnnn entity
        public dynamic Value { get; set; }

        public string ValidationError { get; set; }

        public ItemValue()
        {
        }

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
                            .Select(v => v.LItemValueItem.ItemID)
                            .ToArray();
                        break;

                    // TODO: implement other types
                    case LPropTypeEnum.Hyperlink:
                    case LPropTypeEnum.Image:
                    case LPropTypeEnum.Video:
                        break;
                }
            }
        }

        public ItemValue(LItemProp prop)
            : base(prop)
        {
        }

        public static void UpdateItemValue(ItemValue updatedField,
            WitEntities _db,
            LItem existingItem,
            LItemProp prop)
        {
            switch (prop.PropType)
            {
                case LPropTypeEnum.String:
                    UpdateValueString(updatedField, _db, existingItem, prop);
                    break;

                case LPropTypeEnum.Text:
                    UpdateValueText(updatedField, _db, existingItem, prop);
                    break;

                case LPropTypeEnum.Date:
                case LPropTypeEnum.DateTime:
                case LPropTypeEnum.Time:
                    UpdateValueDateTime(updatedField, _db, existingItem, prop);
                    break;

                case LPropTypeEnum.Integer:
                    UpdateValueInteger(updatedField, _db, existingItem, prop);
                    break;

                case LPropTypeEnum.Decimal:
                    UpdateValueDecimal(updatedField, _db, existingItem, prop);
                    break;

                case LPropTypeEnum.Item:
                    UpdateValueItem(updatedField, _db, existingItem, prop);
                    break;

                // TODO: implement other types
                case LPropTypeEnum.Hyperlink:
                case LPropTypeEnum.Image:
                case LPropTypeEnum.Video:
                    break;
            }
        }

        private static void UpdateValueString(
            ItemValue updatedField,
            WitEntities _db,
            LItem existingItem,
            LItemProp prop
            )
        {
            var currentValues = existingItem.LItemValues
                .Where(v => v.ItemPropID == prop.ItemPropID)
                .OrderBy(v => v.ItemValueID)
                ;

            string newValue = updatedField?.Value as string;
            if (newValue == null)
            {
                _db.LItemValues.RemoveRange(currentValues);
            }
            else
            {
                var currentValue = currentValues.FirstOrDefault();
                if (currentValue != null)
                {
                    if (currentValue.LItemValueString == null)
                    {
                        currentValue.LItemValueString = new LItemValueString()
                        {
                            Value = newValue
                        };
                    }
                    else if (currentValue.LItemValueString.Value != newValue)
                    {
                        currentValue.LItemValueString.Value = newValue;
                    }
                }
                else
                {
                    existingItem.LItemValues.Add(new LItemValue()
                    {
                        ItemPropID = prop.ItemPropID,
                        LItemValueString = new LItemValueString()
                        {
                            Value = newValue
                        }
                    });
                }
            }
        }

        private static void UpdateValueText(
            ItemValue updatedField,
            WitEntities _db,
            LItem existingItem,
            LItemProp prop
            )
        {
            var currentValues = existingItem.LItemValues
                .Where(v => v.ItemPropID == prop.ItemPropID)
                .OrderBy(v => v.ItemValueID)
                ;

            string newValue = updatedField?.Value as string;
            if (newValue == null)
            {
                _db.LItemValues.RemoveRange(currentValues);
            }
            else
            {
                var currentValue = currentValues.FirstOrDefault();
                if (currentValue != null)
                {
                    if (currentValue.LItemValueText == null)
                    {
                        currentValue.LItemValueText = new LItemValueText()
                        {
                            Value = newValue
                        };
                    }
                    else if (currentValue.LItemValueText.Value != newValue)
                    {
                        currentValue.LItemValueText.Value = newValue;
                    }
                }
                else
                {
                    existingItem.LItemValues.Add(new LItemValue()
                    {
                        ItemPropID = prop.ItemPropID,
                        LItemValueText = new LItemValueText()
                        {
                            Value = newValue
                        }
                    });
                }
            }
        }

        private static void UpdateValueDateTime(
            ItemValue updatedField,
            WitEntities _db,
            LItem existingItem,
            LItemProp prop
            )
        {
            var currentValues = existingItem.LItemValues
                .Where(v => v.ItemPropID == prop.ItemPropID)
                .OrderBy(v => v.ItemValueID)
                ;

            Nullable<DateTime> newValue = updatedField?.Value as Nullable<DateTime>;
            if (!newValue.HasValue)
            {
                _db.LItemValues.RemoveRange(currentValues);
            }
            else
            {
                var currentValue = currentValues.FirstOrDefault();
                if (currentValue != null)
                {
                    if (currentValue.LItemValueDateTime == null)
                    {
                        currentValue.LItemValueDateTime = new LItemValueDateTime()
                        {
                            Value = newValue.Value
                        };
                    }
                    else if (currentValue.LItemValueDateTime.Value != newValue.Value)
                    {
                        currentValue.LItemValueDateTime.Value = newValue.Value;
                    }
                }
                else
                {
                    existingItem.LItemValues.Add(new LItemValue()
                    {
                        ItemPropID = prop.ItemPropID,
                        LItemValueDateTime = new LItemValueDateTime()
                        {
                            Value = newValue.Value
                        }
                    });
                }
            }
        }

        private static void UpdateValueInteger(
            ItemValue updatedField,
            WitEntities _db,
            LItem existingItem,
            LItemProp prop
            )
        {
            var currentValues = existingItem.LItemValues
                .Where(v => v.ItemPropID == prop.ItemPropID)
                .OrderBy(v => v.ItemValueID)
                ;

            Nullable<int> newValue = updatedField?.Value as Nullable<int>;
            if (!newValue.HasValue)
            {
                _db.LItemValues.RemoveRange(currentValues);
            }
            else
            {
                var currentValue = currentValues.FirstOrDefault();
                if (currentValue != null)
                {
                    if (currentValue.LItemValueInteger == null)
                    {
                        currentValue.LItemValueInteger = new LItemValueInteger()
                        {
                            Value = newValue.Value
                        };
                    }
                    else if (currentValue.LItemValueInteger.Value != newValue.Value)
                    {
                        currentValue.LItemValueInteger.Value = newValue.Value;
                    }
                }
                else
                {
                    existingItem.LItemValues.Add(new LItemValue()
                    {
                        ItemPropID = prop.ItemPropID,
                        LItemValueInteger = new LItemValueInteger()
                        {
                            Value = newValue.Value
                        }
                    });
                }
            }
        }

        private static void UpdateValueDecimal(
            ItemValue updatedField,
            WitEntities _db,
            LItem existingItem,
            LItemProp prop
            )
        {
            var currentValues = existingItem.LItemValues
                .Where(v => v.ItemPropID == prop.ItemPropID)
                .OrderBy(v => v.ItemValueID)
                ;

            Nullable<decimal> newValue = updatedField?.Value as Nullable<decimal>;
            if (!newValue.HasValue)
            {
                _db.LItemValues.RemoveRange(currentValues);
            }
            else
            {
                var currentValue = currentValues.FirstOrDefault();
                if (currentValue != null)
                {
                    if (currentValue.LItemValueDecimal == null)
                    {
                        currentValue.LItemValueDecimal = new LItemValueDecimal()
                        {
                            Value = newValue.Value
                        };
                    }
                    else if (currentValue.LItemValueDecimal.Value != newValue.Value)
                    {
                        currentValue.LItemValueDecimal.Value = newValue.Value;
                    }
                }
                else
                {
                    existingItem.LItemValues.Add(new LItemValue()
                    {
                        ItemPropID = prop.ItemPropID,
                        LItemValueDecimal = new LItemValueDecimal()
                        {
                            Value = newValue.Value
                        }
                    });
                }
            }
        }

        private static void UpdateValueItem(
            ItemValue updatedField,
            WitEntities _db,
            LItem existingItem,
            LItemProp prop
            )
        {
        }
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
        public int ItemTypeId { get; set; }
        public string Name { get; set; }
        public string Help { get; set; }

        // from LItemProp
        public List<ItemValue> Fields { get; set; }

        // obsolete
        //public Dictionary<string, string> ValidationErrors { get; set; }

        public ItemEntity()
        {
            Fields = new List<ItemValue>();
        }

        public ItemEntity(LItem existingItem)
        {
            if (existingItem != null)
            {
                Id = existingItem.ItemID;
                CreatedBy = existingItem.CreatedBy;
                CreatedOn = existingItem.CreatedOn;
                UpdatedBy = existingItem.UpdatedBy;
                UpdatedOn = existingItem.UpdatedOn;

                ItemTypeId = existingItem.LItemType.ItemTypeID;
                Name = existingItem.LItemType.Name;
                Help = existingItem.LItemType.Description;

                Fields = new List<ItemValue>(existingItem.LItemType.LItemProps.Count + 1);

                foreach (var prop in existingItem.LItemType.LItemProps.OrderBy(p => p.ItemPropID))
                {
                    var propValues = prop.LItemValues.Where(v => v.ItemID == existingItem.ItemID)
                        .OrderBy(v => v.ItemValueID)
                        .ToList()
                        ;

                    Fields.Add(new ItemValue(prop, propValues));
                }
            }
        }

        public ItemEntity(LItemType existingItemType)
        {
            if (existingItemType != null)
            {
                ItemTypeId = existingItemType.ItemTypeID;
                Name = existingItemType.Name;
                Help = existingItemType.Description;

                Fields = new List<ItemValue>(existingItemType.LItemProps.Count + 1);

                foreach (var prop in existingItemType.LItemProps.OrderBy(p => p.ItemPropID))
                {
                    Fields.Add(new ItemValue(prop));
                }
            }
        }

        public void UpdateItemValues(WitEntities _db, LItem existingItem)
        {
            foreach (var prop in existingItem.LItemType.LItemProps)
            {
                var updatedField = Fields
                    .FirstOrDefault(f => f.Id == prop.ItemPropID)
                    ;

                ItemValue.UpdateItemValue(updatedField, _db, existingItem, prop);
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
                Id = 0,
                Key = "id",
                Name = "Id",
                PropType = LPropTypeEnum.Integer,
                GridHide = true
            });
        }
    }
}
