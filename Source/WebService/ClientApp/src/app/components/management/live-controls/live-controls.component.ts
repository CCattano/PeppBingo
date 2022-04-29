import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { faEdit, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { AdminApi } from '../../../shared/api/admin.api';
import { BoardDto } from '../../../shared/dtos/board.dto';
import { ToastService } from '../../../shared/service/toast.service';

@Component({
  templateUrl: './live-controls.component.html',
  styleUrls: ['./live-controls.component.scss']
})
export class LiveControlsComponent implements OnInit {
  /**
   * All boards available to be selected as an active board
   */
  public _boards: BoardDto[];

  /**
   * The board that is currently the
   * active boards all users are playing
   */
  public _activeBoard: BoardDto;

  /**
   * Board chosen to be the new
   * active board in the template
   */
  public _newActiveBoard: BoardDto;

  /**
   * Bool flag indicating if we are currently
   * changing the active board users are to play with
   */
  public _editingActiveBoard: boolean = false;

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faEdit': faEdit,
  };

  constructor(
    private _adminApi: AdminApi,
    private _toastService: ToastService
  ) {
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    this._boards = await this._adminApi.getAllBoards();
    const activeBoardID: number = await this._adminApi.getActiveBoardID();
    if (activeBoardID)
      this._activeBoard = this._newActiveBoard = this._boards.find(board => board.boardID === activeBoardID);
  }

  /**
   * Click event handler for the change
   * active board edit icon in the template
   */
  public _onChangeActiveBoardClick(): void {
    if (!this._activeBoard)
      this._newActiveBoard =
        this._boards
          .sort((a, b) => a.createdDateTime > b.createdDateTime ? 1 : -1)[0];
    this._editingActiveBoard = true;
  }

  /**
   * Click event handler for the save
   * new board button in the template
   */
  public async _onSaveNewBoardClick(): Promise<void> {
    await this._adminApi.setActiveBoardID(this._newActiveBoard.boardID)
      .then(() => this._activeBoard = this._newActiveBoard)
      .catch((response: HttpErrorResponse) => this._toastService.showDangerToast({
        header: 'An Error Occurred',
        body: response.error,
        ttlMs: 5000
      }));
    this._editingActiveBoard = false;
  }

  /**
   * Click event handler for the cancel
   * new board button in the template
   */
  public _onCancelNewBoardClick(): void {
    this._editingActiveBoard = false;
    this._newActiveBoard = this._activeBoard;
  }
}
