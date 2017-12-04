truncate table LItemValueString
truncate table LItemValueText
truncate table LItemValueInteger
truncate table LItemValueDecimal
truncate table LItemValueDateTime
delete LItemValue
delete LItem
delete LItemProp
delete LItemType
go

set identity_insert LItemType on;
insert LItemType (ItemTypeID, Name) values 
	(1, 'Product class'),
	(2, 'Product feature')
;
set identity_insert LItemType off;
go

set identity_insert LItemProp on;
insert LItemProp (ItemPropID, ItemTypeID, Name, PropType, [Required], [Unique], UpperCase, Multiple, Radix) values 
	(1, 1, 'Code', 0, 1, 1, 1, 0, 0),
	(2, 1, 'Name', 0, 1, 0, 0, 0, 1),
	(3, 1, 'Description', 1, 0, 0, 0, 0, 2),
	
	(4, 2, 'Code', 0, 1, 1, 1, 0, 0),
	(5, 2, 'Name', 0, 1, 0, 0, 0, 1),
	(6, 2, 'Description', 1, 0, 0, 0, 0, 2)
;
set identity_insert LItemProp off;
go

set identity_insert LItem on;
insert LItem (ItemID, ItemTypeID) values 
	(1, 1),
	(2, 1),
	(3, 1),
	
	(4, 2),
	(5, 2),
	(6, 2)
;
set identity_insert LItem off;
go

set identity_insert LItemValue on;
insert LItemValue (ItemValueID, ItemID, ItemPropID) values 
	(1, 1, 1), (2, 1, 2),
	(3, 2, 1), (4, 2, 2),
	(5, 3, 1), (6, 3, 2),
	
	(7, 4, 4), (8, 4, 5),
	(9, 5, 4), (10, 5, 5),
	(11, 6, 4), (12, 6, 5), (13, 6, 6)
;
set identity_insert LItemValue off;
go

insert LItemValueString (ItemValueID, Value) values 
	(1, 'CLS1'), (2, 'Class 1'),
	(3, 'CLS2'), (4, 'Class 2'),
	(5, 'CLS3'), (6, 'Class 3'),

	(7, 'FEAT1'), (8, 'Feature 1'),
	(9, 'FEAT2'), (10, 'Feature 2'),
	(11, 'FEAT3'), (12, 'Feature 3')
;
go

insert LItemValueText (ItemValueID, Value) values 
	(13, 'Feature 3 description')
;
go




