import { BaseEntity } from './base.entity';
import { ListBase, GridInfo } from './grid-info.entity';

export class Product extends BaseEntity {
    productID: number;
    productCode: string;
    productName: string;
    description: string;
    productClassID: number;
    productFeatureID: number;

    constructor()
    constructor(rhs: Product)
    constructor(rhs?: Product) {
        super(rhs);
        if (!rhs) {
            this.productID = 0;
        }
    }
}

export class ProductList extends ListBase {
    public items: Array<Product>;
    public code: string;
    public name: string;

    constructor()
    constructor(rhs: ProductList)
    constructor(rhs?: ProductList) {

        super(rhs);

        if (!this.items || !Array.isArray(this.items)) {
            this.items = new Array<Product>();
        } else {
            this.items = this.items.map(v => new Product(v));
        }
    }
}
