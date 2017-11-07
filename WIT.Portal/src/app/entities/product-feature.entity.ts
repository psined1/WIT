import { BaseEntity } from './base.entity';
import { ListBase, GridInfo } from './grid-info.entity';

export class ProductFeature extends BaseEntity {
    public productFeatureID: number;
    public code: string;
    public name: string;
    public description: string;

    constructor()
    constructor(rhs: ProductFeature)
    constructor(rhs?: ProductFeature) {
        super(rhs);
        if (!rhs) {
            this.productFeatureID = 0;
        }
    }

    public get caption() {
        return this.code + '-' + this.name || '';
    }

    public get isValid() {
        return ((this.code || "") !== "");
    }
}

export class ProductFeatureList extends ListBase {
    public items: Array<ProductFeature>;
    public code: string;
    public name: string;

    constructor()
    constructor(rhs: ProductFeatureList)
    constructor(rhs?: ProductFeatureList) {

        super(rhs);

        if (!this.items || !Array.isArray(this.items)) {
            this.items = new Array<ProductFeature>();
        } else {
            this.items = this.items.map(v => new ProductFeature(v));
        }
    }
}

