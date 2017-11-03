export class Address {  
    public addressLine1: string;
    public addressLine2: string;
    public city: string;
    public state: string;
    public zipCode: string;   

    constructor()
    constructor(rhs: Address)
    constructor(rhs?: Address) {

        if (rhs) {
            this.addressLine1 = rhs.addressLine1;
            this.addressLine2 = rhs.addressLine2;
            this.city = rhs.city;
            this.state = rhs.state;
            this.zipCode = rhs.zipCode;
        }
    }
}