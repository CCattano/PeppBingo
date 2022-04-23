import { Component } from '@angular/core';
import { faPlusCircle, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { BoardDto } from '../../../../shared/dtos/board.dto';
import { BoardVM } from './viewmodels/board.viewmodel';

@Component({
  templateUrl: './bingo-board-add-edit.component.html',
  styleUrls: ['./shared-board-styles.scss']
})
export class BingoBoardAddEditComponent {

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faPlusCircle': faPlusCircle
  };

  /**
   * Boards available to be modified
   */
  public _boards: BoardVM[] = [
    {
      boardID: 1,
      name: 'Horror',
      description: 'Board of Horror tropes. Used only on Mondays',
      tileCount: 45,
      createdDateTime: new Date(new Date().setHours(-5)),
      createdBy: 1,
      createdByName: 'TORTUGAN_TORRES',
      modDateTime: new Date(new Date().setHours(-1)),
      modBy: 1,
      modByName: 'TORTUGAN_TORRES'
    } as BoardVM,
    {
      boardID: 2,
      name: 'RPG',
      description: 'Board of RPG tropes. Used only on Thursdays',
      tileCount: 25,
      createdDateTime: new Date(new Date().setHours(-4)),
      createdBy: 2,
      createdByName: 'WheelChairBanditTTV',
      modDateTime: new Date(new Date().setHours(-4)),
      modBy: 2,
      modByName: 'WheelChairBanditTTV'
    } as BoardVM
  ];

  /**
   * Click event handler for adding a new board
   */
  public _onAddClick(): void {
    const newBoard: BoardVM = {
      createdByName: 'You',
      modByName: 'You',
      createdDateTime: new Date(),
      modDateTime: new Date(),
      isNew: true,
      editing: true
    } as BoardVM;
    this._boards.push(newBoard);
  }

  /**
   * Event handler for when changes to a board are successfully made
   *
   * If all boards are in a non-editing state then the boards are
   * sorted alphabetically by name
   */
  public _onEditSaveSuccess(): void {
    if (this._boards.every(board => !board.editing))
      this._boards = this._boards.sort((a, b) => a.name > b.name ? 1 : -1);
  }

  /**
   * Event handler for when a user cancels changes to a board
   *
   * Checks if the board that changes were canceled for was a new
   * board in the process of being created.
   *
   * If it was it is removed from the array of boards in the template
   * @param index
   */
  public _onEditCancled(index: number): void {
    if (this._boards[index].isNew)
      this._boards.splice(index, 1);
  }
}
