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
        public int Radix { get; set; }


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

        /*public bool IsItemKey
        {
            get
            {
                return Unique && Required && !Multiple;
            }
        }*/

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
                Radix = prop.Radix;
            }
        }

        public void UpdateItemProp(WitEntities _db, LItemProp existingField, TransactionInfo transaction)
        {
            existingField.UpdatedBy = transaction.CurrentUserEmail;

            existingField.Name = Name;
            existingField.Description = Help;

            existingField.PropType = PropType;
            //existingField.ValueItemTypeID { get; set; }
            existingField.GridHide = GridHide;
            existingField.Required = Required;
            existingField.Multiple = Multiple;
            existingField.Unique = Unique;
            existingField.UpperCase = UpperCase;
            existingField.Radix = Radix;
        }
    }


    public interface IValue
    {
        long Id { get; set; }
    }

    public class LookupValue : IValue
    {
        public long Id { get; set; }
        //public long ItemId { get; set; }
        public object Key { get; set; }
    }

    public class StringValue : IValue
    {
        public long Id { get; set; }
        public string Value { get; set; }
    }

    public class DateTimeValue : IValue
    {
        public long Id { get; set; }
        public DateTime? Value { get; set; }
    }

    public class IntegerValue : IValue
    {
        public long Id { get; set; }
        public int? Value { get; set; }
    }

    public class DecimalValue : IValue
    {
        public long Id { get; set; }
        public decimal? Value { get; set; }
    }

    public class ItemValue : ItemField
    {
        // from LITemPropValueNnnn entity
        public object Value { get; set; }

        public string ValidationError { get; set; }

        public ItemValue()
        {
        }

        public static object GetValue(WitEntities _db, LItemValue fromValue)
        {
            if (fromValue != null)
            {
                switch (fromValue.LItemProp.PropType)
                {
                    case LPropTypeEnum.String:
                        return fromValue.LItemValueString?.Value;

                    case LPropTypeEnum.Text:
                        return fromValue.LItemValueText?.Value;

                    case LPropTypeEnum.Date:
                    case LPropTypeEnum.DateTime:
                    case LPropTypeEnum.Time:
                        return fromValue.LItemValueDateTime?.Value;

                    case LPropTypeEnum.Integer:
                        return fromValue.LItemValueInteger?.Value;

                    case LPropTypeEnum.Decimal:
                        return fromValue.LItemValueDecimal?.Value;

                    case LPropTypeEnum.Item:
                        {
                            var lookupItem = fromValue.LItemValueItem;

                            if (lookupItem != null)
                            {
                                var lookupItemProp = _db.LItemProps.FirstOrDefault(p =>
                                    p.ItemTypeID == lookupItem.ItemTypeID &&
                                    !p.Multiple && p.Required && p.Unique
                                );

                                if (lookupItemProp != null)
                                {
                                    var lookupValues = _db.LItemValues
                                        .OrderBy(v => v.ItemValueID)
                                        .Where(v => v.ItemPropID == lookupItemProp.ItemPropID && v.ItemID == lookupItem.ItemID)
                                        .Select(v => v.LItemValueString.Value)
                                        .ToArray()
                                        ;

                                    return string.Join(", ", lookupValues);
                                }
                            }
                        }
                        break;

                    // TODO: implement other types
                    case LPropTypeEnum.Hyperlink:
                    case LPropTypeEnum.Image:
                    case LPropTypeEnum.Video:
                        break;
                }
            }

            return null;
        }

        public ItemValue(WitEntities _db, LItemProp prop, LItem existingItem)
            : base(prop)
        {
            var itemValues = existingItem.LItemValues.Where(v => v.ItemPropID == Id)
                .OrderBy(v => v.ItemValueID)
                .ToList()
                ;

            var values = new List<object>(itemValues.Count);

            foreach (var value in itemValues)
            {
                if (!Multiple)
                {
                    Value = GetValue(_db, value);
                    return;
                }

                values.Add(GetValue(_db, value));
            }

            Value = values;
        }

        public ItemValue(LItemProp prop)
            : base(prop)
        {
        }

        public static void UpdateValue(
            WitEntities _db,
            LItem existingItem,
            LItemProp prop,
            ItemValue updatedField,
            TransactionInfo transaction)
        {
            var value = updatedField?.Value;

            if (value == null)
            {
                return;
            }

            switch (prop.PropType)
            {
                case LPropTypeEnum.String:
                    {
                        string newValue = value as string;
                        if (newValue != null)
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
                    break;

                case LPropTypeEnum.Text:
                    {
                        string newValue = value as string;
                        if (newValue != null)
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
                    break;

                case LPropTypeEnum.Date:
                case LPropTypeEnum.DateTime:
                case LPropTypeEnum.Time:
                    existingItem.LItemValues.Add(new LItemValue()
                    {
                        ItemPropID = prop.ItemPropID,
                        LItemValueDateTime = new LItemValueDateTime()
                        {
                            Value = (DateTime)value
                        }
                    });
                    break;

                case LPropTypeEnum.Integer:
                    existingItem.LItemValues.Add(new LItemValue()
                    {
                        ItemPropID = prop.ItemPropID,
                        LItemValueInteger = new LItemValueInteger()
                        {
                            Value = (int)value
                        }
                    });
                    break;

                case LPropTypeEnum.Decimal:
                    existingItem.LItemValues.Add(new LItemValue()
                    {
                        ItemPropID = prop.ItemPropID,
                        LItemValueDecimal = new LItemValueDecimal()
                        {
                            Value = (decimal)value
                        }
                    });
                    break;

                case LPropTypeEnum.Item:
                    {
                        var lookupItemProp = _db.LItemProps.FirstOrDefault(p =>
                            p.ItemTypeID == prop.ValueItemTypeID &&
                            !p.Multiple && p.Required && p.Unique
                        );

                        var values = value as string ?? "";

                        foreach (var newValue in values.Split(','))
                        {
                            var lookupItem = _db.LItemValueStrings.FirstOrDefault(s => 
                                s.Value == newValue && 
                                s.LItemValue.ItemPropID == lookupItemProp.ItemPropID &&
                                s.LItemValue.LItem.ItemTypeID == prop.ValueItemTypeID
                                );

                            if (lookupItem != null)
                            {
                                existingItem.LItemValues.Add(new LItemValue()
                                {
                                    ItemPropID = prop.ItemPropID,
                                    LItemValueItem = lookupItem.LItemValue.LItem
                                });
                            }
                        }
                    }
                    break;

                // TODO: implement other types
                case LPropTypeEnum.Hyperlink:
                case LPropTypeEnum.Image:
                case LPropTypeEnum.Video:
                    break;
            }
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

        public ItemEntity(WitEntities _db, LItem existingItem)
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

                foreach (var prop in existingItem.LItemType.LItemProps.OrderBy(p => p.Radix))
                {
                    Fields.Add(new ItemValue(_db, prop, existingItem));
                }
            }
        }

        public ItemEntity(WitEntities _db, LItemType existingItemType)
        {
            if (existingItemType != null)
            {
                ItemTypeId = existingItemType.ItemTypeID;
                Name = existingItemType.Name;
                Help = existingItemType.Description;

                Fields = new List<ItemValue>(existingItemType.LItemProps.Count + 1);

                foreach (var prop in existingItemType.LItemProps.OrderBy(p => p.Radix))
                {
                    Fields.Add(new ItemValue(prop));
                }
            }
        }

        public void UpdateItemValues(WitEntities _db, LItem existingItem, TransactionInfo transaction)
        {
            _db.LItemValues.RemoveRange(existingItem.LItemValues);

            foreach (var prop in existingItem.LItemType.LItemProps)
            {
                var updatedField = Fields
                    .FirstOrDefault(f => f.Id == prop.ItemPropID)
                    ;

                ItemValue.UpdateValue(_db, existingItem, prop, updatedField, transaction);
            }
        }

        public void UpdateItemType(WitEntities _db, LItemType existingItem, TransactionInfo transaction)
        {
            existingItem.Name = Name;
            existingItem.Description = Help;

            // delete fields which are no longer
            foreach(var oldField in existingItem.LItemProps)
            {
                if (!Fields.Any(f => f.Id == oldField.ItemPropID))
                    existingItem.LItemProps.Remove(oldField);
            }

            // update existing or add new fields
            int i = 0;
            foreach (var f in Fields)
            {
                f.Radix = i++;

                var existingField = existingItem.LItemProps.FirstOrDefault(p => p.ItemPropID == f.Id);
                if (existingField == null)
                {
                    existingField = new LItemProp() {
                        CreatedBy = transaction.CurrentUserEmail
                    };
                    existingItem.LItemProps.Add(existingField);
                }

                f.UpdateItemProp(_db, existingField, transaction);
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
