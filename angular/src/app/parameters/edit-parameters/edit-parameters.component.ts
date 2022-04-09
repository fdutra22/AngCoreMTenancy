import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ParametersServiceProxy,
    ParameterDto
} from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './edit-parameters.component.html',
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
export class EditParametersComponent extends AppComponentBase
    implements OnInit {
    saving = false;
    parameters: ParameterDto = new ParameterDto();

    constructor(
        injector: Injector,
        public _parametersService: ParametersServiceProxy,
        private _dialogRef: MatDialogRef<EditParametersComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._parametersService.get(this._id).subscribe((result: ParameterDto) => {
            this.parameters = result;
        });
    }

    save(): void {
        this.saving = true;

        this._parametersService
            .update(this.parameters)
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
