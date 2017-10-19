
export class DataGridSortInformation {

    public Direction: number;
    public Column: string;
    public SortDirection: string;

}

export class DataGridButton {

    //public ButtonClicked: string;
    //public ItemSelected: number;
    public Name: string;
    public Text: string;

}

export class DataGridEventInformation {
    public EventType: string;
    public ItemSelected: number;
    public Column: DataGridColumn;
    public Button: DataGridButton;
    public Direction: number;
    public SortDirection: string;
    public SortExpression: string;
    public PageSize: number;
    public CurrentPageNumber: number;
}

export class DataGridColumn {

    name: string;
    description: string;
    //options: any;
    cellWidth: string;
    textAlign: string;

    hyperLink: Boolean;
    //singleButton: Boolean;
    //multiButton: Boolean;
    disableSorting: Boolean;   
    formatDate: Boolean;
    formatDateTime: Boolean;

    //buttonText: string;
    buttons: DataGridButton[] = [];

    constructor(name, description, options) {

        this.name = name;
        this.description = description;
        let config = JSON.parse(options);

        this.cellWidth = config.width;
        this.textAlign = config.textAlign;
        this.hyperLink = config.hyperLink;    

        if (this.hyperLink != true) {
            this.hyperLink = false;
        }

        //this.singleButton = this.options[0].singleButton;
        //this.multiButton = this.options[0].multiButton;

        //if (this.singleButton != true) {
        //    this.singleButton = false;
        //}

        //if (this.singleButton == true) {
        //    this.buttonText = this.options[0].buttonText;
        //}

        //if (this.multiButton != true) {
        //    this.multiButton = false;
        //}

        if (config.buttons && config.buttons.length) {
            let items = config.buttons.length;
            for (let i = 0; i < items; i++) {
                let button = new DataGridButton();
                button.Name = config.buttons[i].name;
                button.Text = config.buttons[i].text;
                this.buttons.push(button);
            }
        }

        this.disableSorting = config.disableSorting;
        if (this.disableSorting != true) {
            this.disableSorting = false;
        }

        this.formatDate = config.formatDate;
        if (this.formatDate != true) {
            this.formatDate = false;
        }

        this.formatDateTime = config.formatDateTime;
        if (this.formatDateTime != true) {
            this.formatDateTime = false;
        }

    }
}

export class DataGridSorter {
    public direction: number;
    public key: string;
    constructor() {
        this.direction = 1;
    }
    sort(key, data) {
        if (this.key === key) {
            this.direction = this.direction * -1;
        }
        else {
            this.direction = 1;
        }
        this.key = key;

        let sortInformation = new DataGridSortInformation();
        sortInformation.Column = key;
        sortInformation.Direction = this.direction;
        if (this.direction == -1) {
            sortInformation.SortDirection = "DESC";
        }
        else {
            sortInformation.SortDirection = "ASC";
        }

        return sortInformation;
       
    }
}