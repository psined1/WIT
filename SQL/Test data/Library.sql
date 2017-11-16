set identity_insert LItemType on;
insert LItemType (ItemTypeID, Name) select 1, 'Product class';
set identity_insert LItemType off;
go

set identity_insert LItemProp on;
insert LItemProp (ItemPropID, ItemTypeID, Name, PropType, ValueRule) select 1, 1, 'Name', 0, 1;
insert LItemProp (ItemPropID, ItemTypeID, Name, PropType, ValueRule) select	2, 1, 'Description', 0, 1;
set identity_insert LItemProp off;
go

set identity_insert LItem on;
insert LItem (ItemID, ItemTypeID, [Key]) select 1, 1, 'CLS1';
insert LItem (ItemID, ItemTypeID, [Key]) select 2, 1, 'CLS2';
insert LItem (ItemID, ItemTypeID, [Key]) select 3, 1, 'CLS3';
set identity_insert LItem off;
go

set identity_insert LItemValue on;
insert LItemValue (ItemValueID, ItemID, ItemPropID) select 1, 1, 1;
insert LItemValue (ItemValueID, ItemID, ItemPropID) select 2, 2, 1;
insert LItemValue (ItemValueID, ItemID, ItemPropID) select 3, 3, 1;
set identity_insert LItemValue off;
go

insert LItemValueString (ItemValueID, Value) select 1, 'Class 1';
insert LItemValueString (ItemValueID, Value) select 2, 'Class 2';
insert LItemValueString (ItemValueID, Value) select 3, 'Class 3';
go

-----------------------------------------------------------------------------------------------------
set identity_insert LItemType on;
insert LItemType (ItemTypeID, Name) select 2, 'Product feature';
set identity_insert LItemType off;
go

set identity_insert LItemProp on;
insert LItemProp (ItemPropID, ItemTypeID, Name, PropType, ValueRule) select 3, 2, 'Name', 0, 1;
insert LItemProp (ItemPropID, ItemTypeID, Name, PropType, ValueRule) select	4, 2, 'Description', 0, 1;
set identity_insert LItemProp off;
go

set identity_insert LItem on;
insert LItem (ItemID, ItemTypeID, [Key]) select 4, 2, 'FEAT1';
insert LItem (ItemID, ItemTypeID, [Key]) select 5, 2, 'FEAT2';
insert LItem (ItemID, ItemTypeID, [Key]) select 6, 2, 'FEAT3';
set identity_insert LItem off;
go

set identity_insert LItemValue on;
insert LItemValue (ItemValueID, ItemID, ItemPropID) select 4, 4, 3;
insert LItemValue (ItemValueID, ItemID, ItemPropID) select 5, 5, 3;
insert LItemValue (ItemValueID, ItemID, ItemPropID) select 6, 6, 3;
insert LItemValue (ItemValueID, ItemID, ItemPropID) select 7, 6, 4;
set identity_insert LItemValue off;
go

insert LItemValueString (ItemValueID, Value) select 4, 'Feature 1';
insert LItemValueString (ItemValueID, Value) select 5, 'Feature 2';
insert LItemValueString (ItemValueID, Value) select 6, 'Feature 3';
insert LItemValueString (ItemValueID, Value) select 7, 'Feature 3 description';
go



