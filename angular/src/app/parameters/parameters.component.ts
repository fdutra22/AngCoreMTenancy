import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase, PagedRequestDto,
} from 'shared/paged-listing-component-base';
import {
    ParameterDto,
    ParametersServiceProxy,
    ParameterDtoPagedResultDto,
} from '@shared/service-proxies/service-proxies';
import { CreateParametersComponent } from './create-parameters/create-parameters.component';
import { EditParametersComponent } from './edit-parameters/edit-parameters.component';

@Component({
    templateUrl: './parameters.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
    ]
})
export class ParametersComponent extends PagedListingComponentBase<ParameterDto> {
    parameters: ParameterDto[] = [];
    keyword = '';
    isActive: boolean | null;

    constructor(
        injector: Injector,
        private _parameterService: ParametersServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    list(
        request: PagedRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {

        this._parameterService
            .getAll('', request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: ParameterDtoPagedResultDto) => {
                this.parameters = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    delete(parameter: ParameterDto): void {
        abp.message.confirm(
            this.l('ProductDeleteWarningMessage', parameter.id),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._parameterService
                        .delete(parameter.id)
                        .pipe(
                            finalize(() => {
                                abp.notify.success(this.l('SuccessfullyDeleted'));
                                this.refresh();
                            })
                        )
                        .subscribe(() => { });
                }
            }
        );
    }

    createParameter(): void {
        this.showCreateOrEditParameterDialog();
    }

    editParameter(parameter: ParameterDto): void {
        this.showCreateOrEditParameterDialog(parameter.id);
    }

    showCreateOrEditParameterDialog(id?: number): void {
        let createOrEditParameterDialog;
        if (id === undefined || id <= 0) {
            createOrEditParameterDialog = this._dialog.open(CreateParametersComponent);
        } else {
            createOrEditParameterDialog = this._dialog.open(EditParametersComponent, {
                data: id
            });
        }

        createOrEditParameterDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }
}

