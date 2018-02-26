import { Component, OnInit, Input, Output, OnChanges, ElementRef } from '@angular/core';

@Component({
    selector: 'multiselect',
    templateUrl: 'multiselect.component.html',
    host: {
        '(document:click)': 'handleClick($event)',
    }
})

export class MultiselectComponent implements OnInit {

    elementRef: ElementRef;

    @Input() options: any[] = [];
    @Input() displayProp: any;
    @Input() idProp: any;
    @Input() searchLimit = 25;
    @Input() selectionLimit = 0;
    @Input() showSelectAll = true;
    @Input() showUnselectAll = true;
    @Input() showSearch = true;
    @Input() disabled = false;
    @Input() labels: any;
    @Input() classesBtn = ['btn-block', 'btn-default'];
    showTooltip = '=?';
    placeholder = '@?';

    searchFilter = '';
    open = false;

    resolvedOptions: any[] = [];
    selectedOptions: any[] = [];
    unselectedOptions: any[] = [];

    constructor(myElement: ElementRef) {
        this.elementRef = myElement;
    }

    ngOnInit() {
        this.updateSelectionLists();
    }

    private handleClick($event) {
        if (!this.elementRef.nativeElement.contains($event.target)) {
            this.open = false;
        }
    }

    private getRecursiveProperty(object, path) {
        return path.split('.').reduce((object, x) => {
            if (object) {
                return object[x];
            } else {
                return null;
            }
        }, object)
    }

    private updateSelectionLists() {
        /*if (!$ngModelCtrl.$viewValue) {
            if (this.selectedOptions) {
                this.selectedOptions = [];
            }
            this.unselectedOptions = this.resolvedOptions.slice(); // Take a copy
        } else {
            this.selectedOptions = this.resolvedOptions.filter(el => {
                const id = this.getId(el);
                for (let i = 0; i < $ngModelCtrl.$viewValue.length; i++) {
                    const selectedId = this.getId($ngModelCtrl.$viewValue[i]);
                    if (id === selectedId) {
                        return true;
                    }
                }
                return false;
            });
            this.unselectedOptions = this.resolvedOptions.filter(el => {
                return this.selectedOptions.indexOf(el) < 0;
            });
        }*/
    }

    toggleDropdown() {
        this.open = !this.open;
        this.resolvedOptions = this.options;
        this.updateSelectionLists();
    }

    isEmpty(value) {
        if (value) {
            return (value.length === 0);
        } else {
            return true;
        }
    }

    getButtonText() {
        if (this.selectedOptions && this.selectedOptions.length === 1) {
            return this.getDisplay(this.selectedOptions[0]);
        }
        if (this.selectedOptions && this.selectedOptions.length > 1) {
            var totalSelected = Array.isArray(this.selectedOptions) ? this.selectedOptions.length : 0;
            if (totalSelected === 0) {
                return this.labels && this.labels.select ? this.labels.select : (this.placeholder || 'Select');
            } else {
                return totalSelected + ' ' + (this.labels && this.labels.itemsSelected ? this.labels.itemsSelected : 'selected');
            }
        } else {
            return this.labels && this.labels.select ? this.labels.select : (this.placeholder || 'Select');
        }
    }

    selectAll() {
        this.selectedOptions = this.resolvedOptions.slice(); // Take a copy;
        this.unselectedOptions = [];
    }

    unselectAll() {
        this.selectedOptions = [];
        this.unselectedOptions = this.resolvedOptions.slice(); // Take a copy;
    }

    toggleItem(item) {
        if (typeof this.selectedOptions === 'undefined') {
            this.selectedOptions = [];
        }
        const selectedIndex = this.selectedOptions.indexOf(item);
        const currentlySelected = (selectedIndex !== -1);
        if (currentlySelected) {
            this.unselectedOptions.push(this.selectedOptions[selectedIndex]);
            this.selectedOptions.splice(selectedIndex, 1);
        } else if (!currentlySelected && (this.selectionLimit === 0 || this.selectedOptions.length < this.selectionLimit)) {
            const unselectedIndex = this.unselectedOptions.indexOf(item);
            this.unselectedOptions.splice(unselectedIndex, 1);
            this.selectedOptions.push(item);
        }
    }

    getId(item) {
        if (typeof item === 'string') {
            return item;
        } else if (typeof item === 'object') {
            if (this.idProp) {
                return this.getRecursiveProperty(item, this.idProp);
            } else {
                console.error('Multiselect: when using objects as model, a idProp value is mandatory.');
                return '';
            }
        } else {
            return item;
        }
    }

    getDisplay(item) {
        if (typeof item === 'string') {
            return item;
        } else if (typeof item === 'object') {
            if (this.displayProp) {
                return this.getRecursiveProperty(item, this.displayProp);
            } else {
                console.error('Multiselect: when using objects as model, a displayProp value is mandatory.');
                return '';
            }
        } else {
            return item;
        }
    }

    isSelected(item) {
        if (!this.selectedOptions) {
            return false;
        }
        const itemId = this.getId(item);
        for (let i = 0; i < this.selectedOptions.length; i++) {
            const selectedElement = this.selectedOptions[i];
            if (this.getId(selectedElement) === itemId) {
                return true;
            }
        }
        return false;
    }

    updateOptions() {
        if (typeof this.options === 'function') {
            this.options().then((resolvedOptions) => {
                this.resolvedOptions = resolvedOptions;
                this.updateSelectionLists();
            });
        }
    }

    // This search function is optimized to take into account the search limit.
    // Using angular limitTo filter is not efficient for big lists, because it still runs the search for
    // all elements, even if the limit is reached
    search() {
        var counter = 0;
        return function (item) {
            if (counter > this.searchLimit) {
                return false;
            }
            const displayName = this.getDisplay(item);
            if (displayName) {
                const result = displayName.toLowerCase().indexOf(this.searchFilter.toLowerCase()) > -1;
                if (result) {
                    counter++;
                }
                return result;
            }
        }
    }
}

