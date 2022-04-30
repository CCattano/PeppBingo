import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BoardTileDto } from '../../../../../shared/dtos/board-tile.dto';

type EditableTileFields = Required<Pick<BoardTileDto, 'text' | 'isFreeSpace' | 'isActive'>>;
export class EditTileForm {
  /**
 * The controls available for this form
 */
  public readonly controls: { [p in keyof EditableTileFields]: FormControl } = {
    text: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50)
    ]),
    isFreeSpace: new FormControl(false),
    isActive: new FormControl(true)
  };

  /**
   * The FormGroup maintained by this class
   */
  public readonly form: FormGroup;

  constructor() {
    this.form = new FormGroup(this.controls);
  }
}
