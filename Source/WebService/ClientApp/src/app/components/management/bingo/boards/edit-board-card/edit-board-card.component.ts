import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faSave, faTrash, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { AdminApi } from '../../../../../shared/api/admin.api';
import { BoardDto } from '../../../../../shared/dtos/board.dto';
import { ToastService } from '../../../../../shared/service/toast.service';
import { BoardVM } from '../../viewmodel/board-data.viewmodel';
import { EditBoardForm } from './edit-board.form';

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
  public readonly saveSuccess: EventEmitter<number> = new EventEmitter<number>();

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

  constructor(
    private _adminApi: AdminApi,
    private _toastService: ToastService
  ) {
  }

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
  public async _onSaveClick(): Promise<void> {
    if (!this._boardForm.form.valid) {
      this._boardForm.form.markAllAsTouched();
      return;
    }
    if (this._boardForm.form.pristine)
      this.board.editing = false;
    let newBoard: BoardDto;
    if (this.board.isNew) {
      const boardToCreate: BoardDto = new BoardDto();
      boardToCreate.name = this._boardForm.controls.name.value;
      boardToCreate.description = this._boardForm.controls.description.value;
      newBoard = await this._createBoard(boardToCreate);
    } else {
      const boardToUpdate: BoardDto = {
        ...this.board,
        name: this._boardForm.controls.name.value,
        description: this._boardForm.controls.description.value
      };
      newBoard = await this._updateBoard(boardToUpdate);
    }
    if (!newBoard) return;
    Object.keys(newBoard).forEach((key: string) =>
      (this.board as any)[key] = (newBoard as any)[key]);
    this.board.isNew = false;
    this.board.editing = false;
    this.saveSuccess.emit(this.index);
  }

  /**
   * Event handler for the Save click event
   */
  public _onCancelClick(): void {
    this.board.editing = false;
    this.cancelClick.emit(this.index);
  }

  private async _createBoard(board: BoardDto): Promise<BoardDto> {
    return await this._adminApi.createNewBoard(board).catch(() => {
      this._toastService.showDangerToast({
        header: 'An Error Occurred!',
        body: 'We couldn\'t create your board. Please try again.',
        ttlMs: 5000
      });
      return null;
    });
  }

  private async _updateBoard(board: BoardDto): Promise<BoardDto> {
    return await this._adminApi.updateBoard(board).catch(() => {
      this._toastService.showDangerToast({
        header: 'An Error Occurred!',
        body: 'We couldn\'t update this board. Please try again.',
        ttlMs: 5000
      });
      return null;
    });
  }
}
