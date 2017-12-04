﻿
export class BaseEntity {
    public createdOn: Date;
    public createdBy: string;
    public updatedOn: Date;
    public updatedBy: string;
    public validationErrors: any;   // <-- obsolete

    constructor()
    constructor(rhs: BaseEntity)
    constructor(rhs?: BaseEntity) {

        if (rhs) {
            this.createdOn = rhs.updatedOn;
            this.createdBy = rhs.createdBy;
            this.updatedOn = rhs.updatedOn;
            this.updatedBy = rhs.updatedBy;

            // copy all properties
            for (let k in rhs)
                if (Object.prototype.hasOwnProperty.call(rhs, k))
                    this[k] = rhs[k];
        }

        this.validationErrors = this.validationErrors || {};
    }
}

