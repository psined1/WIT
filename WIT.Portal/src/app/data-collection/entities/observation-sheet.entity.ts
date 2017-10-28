import { TransactionalInformation } from '../../entities/transactionalInformation.entity';
import { ObservationSheetStep } from './observation-sheet-step.entity';

export class ObservationSheet extends TransactionalInformation {
    public id: number;
    public name: string;
    public description: string;
    public steps: Array<ObservationSheetStep>;

    constructor() {
        super();
        this.id = 0;
        this.steps = new Array<ObservationSheetStep>();
    }

    public get caption() {
        return this.name || 'Unnamed sheet';
    }

    public get isValid() {
        
        return ((this.name || "") !== "") &&
            (this.steps.filter(s => !s.isValid).length === 0)
            ;
    }
}
