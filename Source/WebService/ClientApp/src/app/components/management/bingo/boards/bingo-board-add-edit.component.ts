import { Component, OnInit } from '@angular/core';
import { faPlusCircle, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { AdminApi } from '../../../../shared/api/admin.api';
import { BoardDto } from '../../../../shared/dtos/board.dto';
import { UserDto } from '../../../../shared/dtos/user.dto';
import { BoardVM } from '../viewmodel/board-data.viewmodel';

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

  /**
   * A map that contains user's display names by their userID
   */
  private _displayNamesById: Map<number, string> = new Map();

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
   */
  public async _onSaveSuccess(index: number): Promise<void> {
    const changedBoard: BoardVM = this._boards[index];

    const usersToFetch: { [id: string]: number; } = {};
    if (!this._displayNamesById.has(changedBoard.createdBy))
      usersToFetch[changedBoard.createdBy] = changedBoard.createdBy;
    if (!this._displayNamesById.has(changedBoard.modBy))
      usersToFetch[changedBoard.modBy] = changedBoard.modBy;

    if (Object.keys(usersToFetch).length) {
      const users: UserDto[] =
        await this._adminApi.getUsersByUserIDs(Object.values(usersToFetch));
      users.forEach(user => this._displayNamesById.set(user.userID, user.displayName));
    }

    changedBoard.createdByName = this._displayNamesById.get(changedBoard.createdBy);
    changedBoard.modByName = this._displayNamesById.get(changedBoard.modBy);

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
  public _onEditCancelClick(index: number): void {
    if (this._boards[index].isNew)
      this._boards.splice(index, 1);
  }

  private _addEmptyBoard(): void {
    const newBoard: BoardVM = {
      createdByName: 'You',
      modByName: 'You',
      createdDateTime: new Date(new Date().toISOString()),
      modDateTime: new Date(new Date().toISOString()),
      isNew: true,
      editing: true
    } as BoardVM;
    this._boards.push(newBoard);
  }

  /**
   * Fetches board data and then user data associated with those boards.
   */
  private async _getBoardData(): Promise<void> {
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
    users.forEach(user => this._displayNamesById.set(user.userID, user.displayName));
    this._boards = boards.map(boardDto => ({
      ...boardDto,
      createdByName: this._displayNamesById.get(boardDto.createdBy),
      modByName: this._displayNamesById.get(boardDto.modBy)
    } as BoardVM));
  }
}
