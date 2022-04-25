import { Component } from '@angular/core';
import { FormControlAbstract } from '../form-control.abstract';

@Component({
  selector: 'form-input',
  templateUrl: './form-input.component.html',
  styleUrls: ['./form-input.component.scss']
})
export class FormInputComponent extends FormControlAbstract {
  constructor() {
    super();
  }
}
