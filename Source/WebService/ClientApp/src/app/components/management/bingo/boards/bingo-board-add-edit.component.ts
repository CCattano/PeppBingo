import { Component, OnInit } from '@angular/core';
import { faPlusCircle, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { AdminApi } from '../../../../shared/api/admin.api';
import { BoardDto } from '../../../../shared/dtos/board.dto';
import { UserDto } from '../../../../shared/dtos/user.dto';
import { BoardVM } from './viewmodels/board.viewmodel';

@Component({
  templateUrl: './bingo-board-add-edit.component.html',
  styleUrls: ['./shared-board-styles.scss']
})
export class BingoBoardAddEditComponent implements OnInit {

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faPlusCircle': faPlusCircle
  };

  /**
   * Boards available to be modified
   */
  public _boards: BoardVM[] = [];

  constructor(private _adminApi: AdminApi) {
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    await this._getBoardData();
  }

  /**
   * Click event handler for adding a new board
   */
  public _onAddClick(): void {
    this._addEmptyBoard();
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

  private _addEmptyBoard(): void {
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
   * Fetches board data and then user data associated with those boards.
   */
  private async _getBoardData(): Promise<void> {
    debugger;
    const boards: BoardDto[] = await this._adminApi.getAllBoards();
    if (!boards?.length) {
      this._addEmptyBoard();
      return;
    }
    const allUserIDs: { [id: string]: number; } = {};
    boards.forEach(board => {
      allUserIDs[board.createdBy] = board.createdBy;
      allUserIDs[board.modBy] = board.modBy;
    });
    const users: UserDto[] =
      await this._adminApi.getUsersByUserIDs(Object.values(allUserIDs));
    const displayNamesById: Map<number, string> = new Map();
    users.forEach(user => displayNamesById.set(user.userID, user.displayName));
    this._boards = boards.map(boardDto => ({
      ...boardDto,
      createdByName: displayNamesById.get(boardDto.createdBy),
      modByName: displayNamesById.get(boardDto.modBy)
    } as BoardVM));
  }
}
