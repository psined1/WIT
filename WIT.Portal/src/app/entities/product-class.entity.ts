import { BaseEntity } from './base.entity';
import { ListBase, GridInfo } from './grid-info.entity';

export class ProductClass extends BaseEntity {
    public productClassID: number;
    public code: string;
    public name: string;
    public description: string;

    constructor()
    constructor(rhs: ProductClass)
    constructor(rhs?: ProductClass) {
        super(rhs);
        if (!rhs) {
            this.productClassID = 0;
        }
    }

    public get caption() {
        return this.code + '-' + this.name || '';
    }

    public get isValid() {
        return ((this.code || "") !== "");
    }
}

export class ProductClassList extends ListBase {
    public items: Array<ProductClass>;
    public code: string;
    public name: string;

    constructor()
    constructor(rhs: ProductClassList)
    constructor(rhs?: ProductClassList) {

        super(rhs);

        if (!this.items || !Array.isArray(this.items)) {
            this.items = new Array<ProductClass>();
        } else {
            this.items = this.items.map(v => new ProductClass(v));
        }
    }
}

