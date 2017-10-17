import { TransactionalInformation } from './transactionalinformation.entity';

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
    public CreatedOn: Date;
    public UpdatedOn: Date;
    public CreatedBy: string;
    public UpdatedBy: string;
    public customers: Array<Customer>;
}
