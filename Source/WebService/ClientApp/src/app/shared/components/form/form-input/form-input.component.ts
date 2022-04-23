import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';

@Component({
  selector: 'form-input',
  templateUrl: './form-input.component.html',
  styleUrls: ['./form-input.component.scss']
})
export class FormInputComponent implements OnInit, OnDestroy {
  /**
   * Private backing var for public control setter
   */
  private _control: FormControl;

  /**
   * The form control this component will manage the value of
   */
  @Input()
  public set control(value: FormControl) {
    const ctrlName: string =
      Object.keys(value.parent.controls)
        .find(key => (value.parent as FormGroup).controls[key] === value);
    this._controlName = ctrlName[0].toUpperCase() + ctrlName.slice(1);
    this._control = value;
  }
  public get control(): FormControl {
    return this._control;
  }

  /**
   * The label to be applied above the form control
   */
  @Input()
  public label: string;

  /**
   * The label to be applied above the form control
   */
  @Input()
  public placeholder: string;

  /**
   * The error message displayed in the template for the control
   */
  public _errorMsg: string;

  private _controlName: string;

  private _statusChangesSubscription: Subscription;

  /**
   * @inheritdoc
   */
  public ngOnInit(): void {
    this._statusChangesSubscription = this._initStatusChangesPipeline().subscribe();
    this._control.updateValueAndValidity();
  }

  /**
   * @inheritdoc
   */
  public ngOnDestroy(): void {
    this._statusChangesSubscription?.unsubscribe();
    this._statusChangesSubscription = null;
  }

  private _initStatusChangesPipeline(): Observable<void> {
    return this.control.statusChanges.pipe(
      filter((status: string) => {
        const shouldContinue: boolean = status.toLowerCase() !== 'valid';
        if (!shouldContinue)
          this._errorMsg = '';
        return shouldContinue;
      }),
      tap(() => this._processControlError()),
      map(() => null)
    );
  }

  private _processControlError(): void {
    console.log(this._control);
    const errName: string = Object.keys(this.control.errors)[0];
    switch (errName.toLowerCase()) {
      case 'required':
        this._errorMsg = `${this._controlName} is required`;
        break;
      case 'maxlength':
        const maxLenErr: {
          actualLength: number,
          requiredLength: number;
        } = this.control.errors[errName];
        this._errorMsg = `${this._controlName} must be less than ${maxLenErr.requiredLength} characters`;
        break;
      default:
        this._errorMsg = `${this._controlName} is invalid`;
        break;
    }
  }
}
