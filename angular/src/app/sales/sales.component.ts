//@Component({
//  selector: 'app-sales',
//  templateUrl: './sales.component.html',
//  styleUrls: ['./sales.component.css']
//})
//export class SalesComponent implements OnInit {

//  constructor() { }

//  ngOnInit() {
//  }

//}

import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ProductDto, SaleDto,
    SaleProductsServiceProxy,
    ProductsServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './sales.component.html',
    styles: [
        `
      mat-form-field {
        width: 100%;
      }
      mat-checkbox {
        padding-bottom: 5px;
      }
    `
    ]
})
export class SalesComponent extends AppComponentBase
    implements OnInit {
    saving = false;
    product: ProductDto = new ProductDto();
    sale: SaleDto = new SaleDto();

    constructor(
        injector: Injector,
        public _saleProductService: SaleProductsServiceProxy,
        public _productService: ProductsServiceProxy,
        private _dialogRef: MatDialogRef<SalesComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._productService.get(this._id).subscribe((result: ProductDto) => {
            this.product = result;
        });
    }

    save(): void {
        this.saving = true;
     
        this.sale.carroId = this.product.id;
        this._saleProductService
            .create(this.sale)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close(true);
            });
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }
}



