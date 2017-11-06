import { BaseEntity } from './base.entity';
import { Address } from './address.entity';

export class User extends BaseEntity {
    public userID: number;
    public firstName: string;
    public lastName: string;
    public emailAddress: string;
    public addressLine1: string;
    public addressLine2: string;
    public city: string;     
    public state: string;
    public zipCode: string;
    public password: string;    
    public passwordConfirmation: string;

    constructor()
    constructor(rhs: User)
    constructor(rhs?: User) {
        super(rhs);
        if (!rhs) {
            this.userID = 0;
        }
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

