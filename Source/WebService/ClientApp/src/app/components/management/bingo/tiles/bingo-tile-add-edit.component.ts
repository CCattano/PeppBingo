import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { faPlusCircle, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { AdminApi } from '../../../../shared/api/admin.api';
import { BoardTileDto } from '../../../../shared/dtos/board-tile.dto';
import { UserDto } from '../../../../shared/dtos/user.dto';
import { BoardTileVM } from '../viewmodel/board-data.viewmodel';

@Component({
  templateUrl: './bingo-tile-add-edit.component.html',
  styleUrls: ['./shared-tile-styles.scss']
})
export class BingoTileAddEditComponent implements OnInit {
  public _tiles: BoardTileVM[] = [];

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faPlusCircle': faPlusCircle,
  };

  /**
   * A map that contains user's display names by their userID
   */
  private _displayNamesById: Map<number, string> = new Map();

  /**
   * The board the tiles belong to
   */
  public _boardID: number;

  public readonly boardName: string = 'Edit Tiles';

  constructor(
    activatedRoute: ActivatedRoute,
    private _adminApi: AdminApi
  ) {
    this._boardID = +(activatedRoute.snapshot.params.boardID as string);
    const boardName: string = window.history.state.boardName;
    if (boardName)
      this.boardName = boardName.toLowerCase().includes('board')
        ? boardName
        : `${boardName} Board`;
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    await this._getTileData();
  }

  public _onAddNewClick(): void {
    this._addEmptyTile();
  }

  /**
   * Event handler for when changes to a board are successfully made
   *
   */
  public async _onSaveSuccess(index: number): Promise<void> {
    const changedTile: BoardTileVM = this._tiles[index];

    const usersToFetch: { [id: string]: number; } = {};
    if (!this._displayNamesById.has(changedTile.createdBy))
      usersToFetch[changedTile.createdBy] = changedTile.createdBy;
    if (!this._displayNamesById.has(changedTile.modBy))
      usersToFetch[changedTile.modBy] = changedTile.modBy;

    if (Object.keys(usersToFetch).length) {
      const users: UserDto[] =
        await this._adminApi.getUsersByUserIDs(Object.values(usersToFetch));
      users.forEach(user => this._displayNamesById.set(user.userID, user.displayName));
    }

    changedTile.createdByName = this._displayNamesById.get(changedTile.createdBy);
    changedTile.modByName = this._displayNamesById.get(changedTile.modBy);
  }

  /**
   * Event handler for the cancel click event fired from cards being edited
   *
   * If the tile was new and changes were canceled removes the tile from the list
   * @param index
   */
  public _onCancelClick(index: number): void {
    if (this._tiles[index].isNew)
      this._tiles.splice(index, 1);
  }

  private _addEmptyTile(): void {
    const newTile: BoardTileVM = {
      isActive: true,
      createdByName: 'You',
      modByName: 'You',
      createdDateTime: new Date(new Date().toISOString()),
      modDateTime: new Date(new Date().toISOString()),
      isNew: true,
      editing: true
    } as BoardTileVM;
    this._tiles.unshift(newTile);
  }

  private async _getTileData(): Promise<void> {
    const tiles: BoardTileDto[] = await this._adminApi.getTilesForBoard(this._boardID);
    if (!tiles?.length) {
      this._addEmptyTile();
      return;
    }
    const allUserIDs: { [id: string]: number; } = {};
    tiles.forEach(tiles => {
      allUserIDs[tiles.createdBy] = tiles.createdBy;
      allUserIDs[tiles.modBy] = tiles.modBy;
    });
    const users: UserDto[] =
      await this._adminApi.getUsersByUserIDs(Object.values(allUserIDs));
    users.forEach(user => this._displayNamesById.set(user.userID, user.displayName));
    this._tiles = tiles
      .sort((a, b) => a.createdDateTime > b.createdDateTime ? -1 : 1)
      .map(tileDto => ({
        ...tileDto,
        createdByName: this._displayNamesById.get(tileDto.createdBy),
        modByName: this._displayNamesById.get(tileDto.modBy)
      } as BoardTileVM));
  }
}
