import { Component, OnInit } from '@angular/core';

import { LibraryService } from '../../services/library.service';

import { AlertBoxComponent } from '../../shared/alertbox.component';
import { ConfirmYesNoComponent } from '../../shared/confirm-yes-no/confirm-yes-no.component';

import { TransactionInfo } from '../../entities/transaction-info.entity';

import { IItemData } from '../../entities/grid-info.entity';
import { ItemField, LPropTypeEnum } from '../../entities/item-field.entity';


@Component({
    selector: 'app-item',
    templateUrl: './item.component.html',
    styles: []
})
export class ItemComponent implements OnInit {

    constructor() { }

    ngOnInit() {
    }
}
