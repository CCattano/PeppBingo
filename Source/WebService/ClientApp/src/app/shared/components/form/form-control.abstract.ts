import { Directive, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { filter, map, tap } from 'rxjs/operators';

@Directive()
export abstract class FormControlAbstract implements OnInit, OnDestroy {
  /**
 * Private backing var for public control setter
 */
  protected _control: FormControl;

  /**
   * The name of the form control provided
   *
   * Calculated in the [control] setter
   */
  protected controlName: string;

  /**
   * The form control this component will manage the value of
   */
  @Input()
  public set control(value: FormControl) {
    const ctrlName: string =
      Object.keys(value.parent.controls)
        .find(key => (value.parent as FormGroup).controls[key] === value);
    this.controlName = ctrlName[0].toUpperCase() + ctrlName.slice(1);
    this._control = value;
  }
  /**
   * The form control this component will manage the value of
   */
  public get control(): FormControl {
    return this._control;
  }

  /**
 * The error message displayed in the template for the control
 */
  public errorMsg: string;

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
          this.errorMsg = '';
        return shouldContinue;
      }),
      tap(() => this._processControlError()),
      map(() => null)
    );
  }

  private _processControlError(): void {
    const errName: string = Object.keys(this.control.errors)[0];
    switch (errName.toLowerCase()) {
      case 'required':
        this.errorMsg = `${this.controlName} is required`;
        break;
      case 'maxlength':
        const maxLenErr: {
          actualLength: number,
          requiredLength: number;
        } = this.control.errors[errName];
        this.errorMsg = `${this.controlName} must be less than ${maxLenErr.requiredLength} characters`;
        break;
      default:
        this.errorMsg = `${this.controlName} is invalid`;
        break;
    }
  }
};
