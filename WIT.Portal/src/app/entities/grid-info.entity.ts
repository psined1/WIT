import { ItemField } from './item-field.entity';

// obsolete
export class GridInfo {

    public totalRows: number;
    public pageSize: number;
    public currentPageNumber: number;
    public sortId: number;              // library item property ID, -1 = itemId, 0 = key
    public sortExpression: string;      // obsolete, use sortId
    public sortDirection: string;
    public filter: string;
    public itemTypeId: number;

    constructor()
    constructor(rhs: GridInfo)
    constructor(rhs?: GridInfo) {

        if (rhs) {
            this.totalRows = rhs.totalRows;
            this.pageSize = rhs.pageSize;
            this.currentPageNumber = rhs.currentPageNumber;
            this.sortExpression = rhs.sortExpression;
            this.sortDirection = rhs.sortDirection;
            this.filter = rhs.filter;
            this.itemTypeId = rhs.itemTypeId;
            this.sortId = rhs.sortId;
        } else {
            this.totalRows = 0;
            this.pageSize = 0;
            this.itemTypeId = 0;

            this.reset();
        }

        if (this.currentPageNumber > this.totalPages) {
            this.currentPageNumber = this.totalPages;
        }
    }

    public get totalPages(): number {
        if (!this.totalRows || this.totalRows == NaN) return 1;
        if (!this.pageSize || this.pageSize == NaN) return 1;
        return Math.floor(this.totalRows / this.pageSize) + (this.totalRows % this.pageSize ? 1 : 0);
    }

    public reset(): void {
        this.currentPageNumber = 1;
        this.sortDirection = "";
        this.sortExpression = "";
        this.filter = "";
        this.sortId = 0;
    }
}

export class ItemGrid {
    public gridInfo: GridInfo;
    public fields: Array<ItemField>;
    public data: Array<any>;

    constructor()
    constructor(rhs: ItemGrid)
    constructor(rhs?: ItemGrid) {
        if (rhs) {
            for (let k in rhs) this[k] = rhs[k];
            this.gridInfo = new GridInfo(rhs.gridInfo);
        }

        if (!this.data || !Array.isArray(this.data)) {
            this.data = [];
        }

        if (!this.fields || !Array.isArray(this.fields)) {
            this.fields = [];
        }

        if (!this.gridInfo || !(this.gridInfo instanceof GridInfo)) {
            this.gridInfo = new GridInfo();
        }
    }
}


// obsolete
export class ListBase {

    public gridInfo: GridInfo;

    constructor()
    constructor(rhs: ListBase)
    constructor(rhs?: ListBase) {

        if (rhs) {
            for (let k in rhs) this[k] = rhs[k];
            this.gridInfo = new GridInfo(rhs.gridInfo);
        } else {
            this.gridInfo = new GridInfo();
        }
    }
}