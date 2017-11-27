set identity_insert LItemType on;
insert LItemType (ItemTypeID, Name) values 
	(1, 'Product class')
;
set identity_insert LItemType off;
go

set identity_insert LItemProp on;
insert LItemProp (ItemPropID, ItemTypeID, Name, PropType, [Required]) values 
	(1, 1, 'Name', 0, 1),
	(2, 1, 'Description', 0, 0)
;
set identity_insert LItemProp off;
go

set identity_insert LItem on;
insert LItem (ItemID, ItemTypeID, [Key]) values 
	(1, 1, 'CLS1'),
	(2, 1, 'CLS2'),
	(3, 1, 'CLS3')
;
set identity_insert LItem off;
go

set identity_insert LItemValue on;
insert LItemValue (ItemValueID, ItemID, ItemPropID) values 
	(1, 1, 1),
	(2, 2, 1),
	(3, 3, 1)
;
set identity_insert LItemValue off;
go

insert LItemValueString (ItemValueID, Value) values 
	(1, 'Class 1'),
	(2, 'Class 2'),
	(3, 'Class 3')
;
go

-----------------------------------------------------------------------------------------------------
set identity_insert LItemType on;
insert LItemType (ItemTypeID, Name) values 
	(2, 'Product feature')
;
set identity_insert LItemType off;
go

set identity_insert LItemProp on;
insert LItemProp (ItemPropID, ItemTypeID, Name, PropType, [Required]) values
	(3, 2, 'Name', 0, 1),
	(4, 2, 'Description', 0, 0)
;
set identity_insert LItemProp off;
go

set identity_insert LItem on;
insert LItem (ItemID, ItemTypeID, [Key]) values 
	(4, 2, 'FEAT1'),
	(5, 2, 'FEAT2'),
	(6, 2, 'FEAT3')
;
set identity_insert LItem off;
go

set identity_insert LItemValue on;
insert LItemValue (ItemValueID, ItemID, ItemPropID) values 
	(4, 4, 3),
	(5, 5, 3),
	(6, 6, 3),
	(7, 6, 4)
;
set identity_insert LItemValue off;
go

insert LItemValueString (ItemValueID, Value) values 
	(4, 'Feature 1'),
	(5, 'Feature 2'),
	(6, 'Feature 3'),
	(7, 'Feature 3 description')
;
go



