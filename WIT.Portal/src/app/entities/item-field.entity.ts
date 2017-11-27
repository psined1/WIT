
export enum LPropTypeEnum {
    String = 0,
    Integer = 2,
    Decimal = 3,
    Date = 4,
    Time = 5,
    DateTime = 6,
    Text = 1,
    Item = 100,
    Image = 7,
    Video = 8,
    Hyperlink = 9,
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
    public disabled: Boolean;
    public isSortable: Boolean;

    constructor()
    constructor(rhs: ItemField)
    constructor(rhs?: ItemField) {

        if (rhs) {
            this.id = rhs.id;
            this.key = rhs.key;
            this.name = rhs.name;
            this.help = rhs.help;
            this.propType = rhs.propType;
            this.valueItemTypeID = rhs.valueItemTypeID;
            this.gridHide = rhs.gridHide;
            this.required = rhs.required;
            this.multiple = rhs.multiple;
            this.disabled = rhs.disabled;
            this.isSortable = rhs.isSortable;
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

