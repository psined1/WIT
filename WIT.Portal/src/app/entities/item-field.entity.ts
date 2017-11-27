import { BaseEntity } from './base.entity';

export enum LPropTypeEnum {

    String = 0,
    Text = 1,
    Integer = 2,
    Decimal = 3,
    Date = 4,
    Time = 5,
    DateTime = 6,
    Image = 7,
    Video = 8,
    Hyperlink = 9,
    Item = 100,
}

export class ItemField {

    public id: number;
    public key: string;
    public name: string;
    public help: string;
    public propType: LPropTypeEnum;
    public valueItemTypeID?: number;
    public gridHide: Boolean;
    public required: Boolean;
    public multiple: Boolean;
    public unique: Boolean;
    public upperCase: Boolean;
    public disabled: Boolean;
    public isSortable: Boolean;
    public isItemKey: Boolean;

    constructor()
    constructor(rhs: ItemField)
    constructor(rhs?: ItemField) {

        if (rhs) {

            this.id = rhs.id;
            this.key = rhs.key;
            this.name = rhs.name;
            this.help = rhs.help;
            this.propType = rhs.propType;
            this.gridHide = rhs.gridHide;
            this.required = rhs.required;
            this.multiple = rhs.multiple;
            this.unique = rhs.unique;
            this.upperCase = rhs.upperCase;
            this.disabled = rhs.disabled;
            this.isSortable = rhs.isSortable;
            this.isItemKey = rhs.isItemKey;

            if (rhs.valueItemTypeID)
                this.valueItemTypeID = rhs.valueItemTypeID;
        } else {

            this.key = null;
            this.name = null;
        }

        this.propType = this.propType || LPropTypeEnum.String;
        this.required = !!this.required;
        this.multiple = !!this.multiple;
        this.id = this.id || 0;
        this.disabled = !!this.disabled;
        this.isSortable = !!this.isSortable;
    }
}

export class ItemValue extends ItemField
{
    // from LITemPropValueNnnn entity
    public value: any;
    public validationError?: string;

    constructor()
    constructor(rhs: ItemValue)
    constructor(rhs?: ItemValue) {

        super(rhs);

        if (rhs) {
            this.value = rhs.value;
            if (rhs.validationError)
                this.validationError = rhs.validationError;
        }
    }
}

export interface IItemData {
    id: number;
}

export class ItemEntity extends BaseEntity implements IItemData {

    // from LItem
    public id: number;

    // from LItemType entity
    public name: string;
    public help: string;

    // from LItemProp/Value
    public fields: Array<ItemValue>;

    constructor()
    constructor(rhs: ItemEntity)
    constructor(rhs?: ItemEntity) {

        super(rhs);

        if (rhs) {
            if (!rhs.id) this.id = 0;
        }

        this.fields = this.fields || [];
    }
}

