import { TransactionalInformation } from '../../entities/transactionalInformation.entity';

export class ObservationSheetStep extends TransactionalInformation {
    public id: number;
    public sort: number;
    public name: string;
    public description: string;
    public hash: number;

    constructor() {
        super();
        this.id = 0;
        //this.deleted = false;
        this.hash = new Date().valueOf() ^ Math.random() * 1000000000;
    }

    public get caption() {
        return this.name || 'Unnamed step';
    }

    public get isValid() {
        return ((this.name || "") !== "");
    }
}
