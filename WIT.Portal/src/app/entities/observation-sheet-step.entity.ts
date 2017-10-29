import { BaseEntity } from './base.entity';

export class ObservationSheetStep extends BaseEntity {
    public id: number;
    public sort: number;
    public name: string;
    public description: string;
    public hash: number;

    constructor()
    constructor(rhs : ObservationSheetStep)
    constructor(rhs?: ObservationSheetStep) {
        super(rhs);

        if (!rhs) {
            this.id = 0;
            this.hash = new Date().valueOf() ^ Math.random() * 1000000000;
        }
    }

    public get caption() {
        return this.name || 'Unnamed step';
    }

    public get isValid() {
        return ((this.name || "") !== "");
    }
}
