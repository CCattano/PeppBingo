import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BoardVM } from '../viewmodels/board.viewmodel';
import { EditBoardForm } from './edit-board.form';
import { faSave, faTrash, IconDefinition } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-edit-board-card',
  templateUrl: './edit-board-card.component.html',
  styleUrls: ['../shared-board-styles.scss']
})
export class EditBoardCardComponent implements OnInit {
  /**
   * The board to apply modified values to
   */
  @Input()
  public board: BoardVM;

  /**
   * The index of the board being edited from the list
   */
  @Input()
  public index: number;

  /**
   * Event that emits the index of the board when changes are canceled by the user
   * This can be used to perform necessary cleanup such as splicing out a board
   * that was part of a canceled create operation
   */
  @Output()
  public readonly cancelClick: EventEmitter<number> = new EventEmitter<number>();

  /**
   * Event that emits when a save has been made successfully
   */
  @Output()
  public readonly saveSuccess: EventEmitter<void> = new EventEmitter<void>();

  /**
   * The form that holds all user changes made until saved of discarded
   */
  public readonly _boardForm: EditBoardForm = new EditBoardForm();

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faSave': faSave,
    'faTrash': faTrash
  };

  /**
   * @inheritdoc
   */
  public ngOnInit(): void {
    if (this.board)
      this._boardForm.form.reset(this.board);
  }

  /**
   * Event handler for the Save click event
   */
  public _onSaveClick(): void {
    if (!this._boardForm.form.valid) {
      this._boardForm.form.markAllAsTouched();
      return;
    }
    if (this._boardForm.form.pristine) return;
    console.log('save changes goes here');
    this.board.name = this._boardForm.controls.name.value;
    this.board.description = this._boardForm.controls.description.value;
    this.board.isNew = false;
    this.board.editing = false;
    this.saveSuccess.emit();
    console.log('test');
  }

  /**
   * Event handler for the Save click event
   */
  public _onCancelClick(): void {
    this.board.editing = false;
    this.cancelClick.emit(this.index);
  }
}
