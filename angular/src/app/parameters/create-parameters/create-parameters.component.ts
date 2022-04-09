import { Component, Injector, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ParameterDto,
    ParametersServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './create-parameters.component.html',
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
export class CreateParametersComponent extends AppComponentBase
    implements OnInit {
    saving = false;
    parameter: ParameterDto = new ParameterDto();

    constructor(
        injector: Injector,
        public _parameterService: ParametersServiceProxy,
        private _dialogRef: MatDialogRef<CreateParametersComponent>
    ) {
        super(injector);
    }

    ngOnInit(): void {
    }

    save(): void {
        this.saving = true;

        this._parameterService
            .create(this.parameter)
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
