import { Component } from '@angular/core';
import { FormControlAbstract } from '../form-control.abstract';

@Component({
  selector: 'form-textarea',
  templateUrl: './form-textarea.component.html',
  styleUrls: ['./form-textarea.component.scss']
})
export class FormTextAreaComponent extends FormControlAbstract {
  constructor() {
    super();
  }
}
