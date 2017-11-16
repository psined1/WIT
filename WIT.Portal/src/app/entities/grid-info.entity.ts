export class GridInfo {

    public totalRows: number;
    public pageSize: number;
    public currentPageNumber: number;
    public sortId: number;
    public sortExpression: string;
    public sortDirection: string;
    public filter: string;
    public id: number;

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
            this.id = rhs.id;
            this.sortId = rhs.sortId;
        } else {
            this.totalRows = 0;
            this.pageSize = 0;
            this.id = 0;

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