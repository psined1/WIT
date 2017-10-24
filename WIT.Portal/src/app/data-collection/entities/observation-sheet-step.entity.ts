export class ObservationSheetStep {
    public id: number;
    public sort: number;
    public name: string;
    //public deleted: Boolean;
    public hash: number;

    constructor() {
        this.id = 0;
        //this.deleted = false;
        this.hash = new Date().valueOf() ^ Math.random() * 1000000000;
    }

    public get caption() {
        return this.name || 'Unnamed step';
    }
}
