import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase, PagedRequestDto,
} from 'shared/paged-listing-component-base';
import {
    ProductsServiceProxy,
    ProductDto,
    ProductDtoPagedResultDto,
    ReportsServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { SalesComponent } from '../sales/sales.component';
import { CreateProductDialogComponent } from './create-product/create-product-dialog.component';
import { EditProductDialogComponent } from './edit-product/edit-product-dialog.component';
import { options } from 'toastr';

@Component({
    templateUrl: './products.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
      ]
})
export class ProductsComponent extends PagedListingComponentBase<ProductDto> {
    products: ProductDto[] = [];
    keyword = '';
    isActive: boolean | null;
   
    constructor(
        injector: Injector,
        private _productService: ProductsServiceProxy,
        private _reportService: ReportsServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    list(
        request: PagedRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {

        this._productService
            .getAll('', request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: ProductDtoPagedResultDto) => {
                this.products = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    delete(product: ProductDto): void {
        abp.message.confirm(
            this.l('ProductDeleteWarningMessage', product.name),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._productService
                        .delete(product.id)
                        .pipe(
                            finalize(() => {
                                abp.notify.success(this.l('Successfully'));
                                this.refresh();
                            })
                        )
                        .subscribe(() => { });
                }
            }
        );
    }



    createProduct(): void {
        this.showCreateOrEditProductDialog();
    }

    callRelatorio(tipo: string, arquivo: string): void {
     
        abp.message.confirm(
           
            this.l("Deseja gerar o relatório diário como " + arquivo + "?"),
            
            undefined,
            (result: boolean) => {
                if (result == null) {
                    result = false;
                }
               
                this._reportService.getAll(tipo, result)
                    .pipe(
                        finalize(() => {
                           // abp.notify.success(this.l('SuccessfullyDeleted'));
                            this.refresh();
                        })
                    )
                    .subscribe((data) => {
                        let downloadLink = document.createElement('a');
                        downloadLink.href = window.URL.createObjectURL(data.body);

                        downloadLink.setAttribute('download', "Relatorio." + arquivo);
                        document.body.appendChild(downloadLink);
                        downloadLink.click();
                        downloadLink.remove();
                    });
            }, {
                title: "Gerar Relatorio",
            buttons: ["Relatorio para Compras?", "Relatorio para Vendas?"],
        }
        );
    }
  



    editProduct(product: ProductDto): void {
        this.showCreateOrEditProductDialog(product.id);
    }

    sellProduct(product: ProductDto): void {
        this.showSellProductDialog(product.id);
    }

    showCreateOrEditProductDialog(id?: number): void {
        let createOrEditProductDialog;
        if (id === undefined || id <= 0) {
            createOrEditProductDialog = this._dialog.open(CreateProductDialogComponent);
        } else {
            createOrEditProductDialog = this._dialog.open(EditProductDialogComponent, {
                data: id
            });
        }

        createOrEditProductDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }

    showSellProductDialog(id?: number): void {
        let sellProductDialog;
        if (id !== undefined || id > 0) {
            sellProductDialog = this._dialog.open(SalesComponent, {
                data: id
            });
        }

        sellProductDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }
}
