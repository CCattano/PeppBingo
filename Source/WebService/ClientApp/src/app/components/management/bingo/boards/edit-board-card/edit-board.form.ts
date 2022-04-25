import { FormControl, FormGroup, Validators } from '@angular/forms'
import { BoardDto } from '../../../../../shared/dtos/board.dto'

type EditableBoardFields = Required<Pick<BoardDto, 'name' | 'description'>>
export class EditBoardForm {
  /**
   * The controls available for this form
   */
  public readonly controls: { [p in keyof EditableBoardFields]: FormControl } = {
    name: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50)
    ]),
    description: new FormControl(null, [
      Validators.required,
      Validators.maxLength(150)
    ])
  }

  /**
   * The FormGroup maintained by this class
   */
  public readonly form: FormGroup;

  constructor() {
    this.form = new FormGroup(this.controls);
  }
}
