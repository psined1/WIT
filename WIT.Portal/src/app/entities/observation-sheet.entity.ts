import { BaseEntity } from './base.entity';
import { ObservationSheetStep } from './observation-sheet-step.entity';

export class ObservationSheet extends BaseEntity {
    public id: number;
    public name: string;
    public description: string;
    public steps: Array<ObservationSheetStep>;

    constructor()
    constructor(rhs: ObservationSheet)
    constructor(rhs?: ObservationSheet) {
        super(rhs);
        if (!rhs) {
            this.id = 0;
        }
        if (!this.steps || !Array.isArray(this.steps)) {
            this.steps = new Array<ObservationSheetStep>();
        } else {
            this.steps = this.steps.map(v => new ObservationSheetStep(v));
        }
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
