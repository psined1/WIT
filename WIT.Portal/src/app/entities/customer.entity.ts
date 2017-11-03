import { BaseEntity } from './base.entity';
import { Address } from './address.entity';
import { ListBase, GridInfo } from './grid-info.entity';

export class Customer extends BaseEntity {
    public customerID: number;
    public customerCode: string;
    public companyName: string;
    public addressLine1: string;
    public addressLine2: string;
    public city: string;
    public state: string;
    public zipCode: string;
    public phoneNumber: string;

    constructor()
    constructor(rhs: Customer)
    constructor(rhs?: Customer) {
        super(rhs);
        if (!rhs) {
            this.customerID = 0;
        }
    }

    public get cityAndState(): string {
        let city = this.city || '';
        let state = this.state || '';
        if (city === '' && state === '')
            return '';
        else if (city === '')
            return state;
        else if (state === '')
            return city;
        else
            return city + ', ' + state; 
    }

    public get address(): Address {
        let address = new Address();
        address.addressLine1 = this.addressLine1;
        address.addressLine2 = this.addressLine2;
        address.city = this.city;
        address.state = this.state;
        address.zipCode = this.zipCode;
        return address;
    }

    public set address(rhs: Address) {
        this.addressLine1 = rhs.addressLine1;
        this.addressLine2 = rhs.addressLine2;
        this.city = rhs.city;
        this.state = rhs.state;
        this.zipCode = rhs.zipCode;
    }
}

export class CustomerList extends ListBase {
    public items: Array<Customer>;
    public customerCode: string;
    public companyName: string;

    constructor()
    constructor(rhs: CustomerList)
    constructor(rhs?: CustomerList) {

        super(rhs);

        if (!this.items || !Array.isArray(this.items)) {
            this.items = new Array<Customer>();
        } else {
            this.items = this.items.map(v => new Customer(v));
        }
    }
}

