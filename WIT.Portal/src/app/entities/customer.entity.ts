import { TransactionalInformation } from '../entities/transactionalInformation.entity';

export class Customer extends TransactionalInformation {
    public customerID: number;
    public companyName: string;
    public customerCode: string;
    public addressLine1: string;
    public addressLine2: string;
    public city: string;
    public state: string;
    public zipCode: string;
    public phoneNumber: string;
    public createdOn: Date;
    public updatedOn: Date;
    public createdBy: string;
    public updatedBy: string;
    public customers: Array<Customer>;
}
