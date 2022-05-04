import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { faEdit, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { Observable, Subject, timer } from 'rxjs';
import { scan, switchMap, take, tap } from 'rxjs/operators';
import { AdminApi } from '../../../shared/api/admin.api';
import { GameApi } from '../../../shared/api/game.api';
import { BoardDto } from '../../../shared/dtos/board.dto';
import { AdminHub } from '../../../shared/hubs/admin.hub';
import { ToastService } from '../../../shared/service/toast.service';

@Component({
  templateUrl: './live-controls.component.html',
  styleUrls: ['./live-controls.component.scss']
})
export class LiveControlsComponent implements OnInit, OnDestroy {
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

  private _cooldownSource: Subject<number> = new Subject<number>();
  public _cooldownIsActive: boolean = false;
  public readonly cooldownTime$: Observable<number>;

  constructor(
    private _adminHub: AdminHub,
    private _adminApi: AdminApi,
    private _gameApi: GameApi,
    private _toastService: ToastService
  ) {
    this.cooldownTime$ = this._initCooldownPipeline();
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    await this._registerHubEventHandlers();
    this._boards = await this._adminApi.getAllBoards();
    this._boards = this._boards.filter(board => board.tileCount >= 25);
    const activeBoardID: number = await this._gameApi.getActiveBoardID();
    if (activeBoardID)
      this._activeBoard = this._newActiveBoard = this._boards.find(board => board.boardID === activeBoardID);
  }

  /**
   * @inheritdoc
   */
  public ngOnDestroy(): void {
    this._adminHub.unregisterAllHandlers();
  }

  /**
   * Click event handler for the change
   * active board edit icon in the template
   */
  public _onChangeActiveBoardClick(): void {
    if (this._cooldownIsActive || !this._boards?.length) return;
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

  private _initCooldownPipeline(): Observable<number> {
    return this._cooldownSource.asObservable().pipe(
      tap(() => this._cooldownIsActive = true),
      switchMap((timeRemaining: number) =>
        timer(0, 1000)
          .pipe(
            scan((acc, _) => --acc, timeRemaining),
            take(30),
            tap((second: number) => {
              if (second === 0) this._cooldownIsActive = false;
            }),
          ))
    );
  }

  //#region SignalR Hub Events

  private async _registerHubEventHandlers(): Promise<void> {
    await this._adminHub.connect();
    this._adminHub.registerOnTriggerSetActiveBoardCooldown(this._onTriggerSetActiveBoardCooldown);
    this._adminHub.registerOnLatestActiveBoardIDHandler(this._onEmitLatestActiveBoardID);
  }

  private _onEmitLatestActiveBoardID = (activeBoardID: number) => {
    if (this._activeBoard?.boardID === activeBoardID) return;
    this._activeBoard = this._newActiveBoard =
      this._boards.find(board => board.boardID === activeBoardID);
  };

  private _onTriggerSetActiveBoardCooldown = (timeRemaining: number = null) => {
    this._cooldownSource.next(timeRemaining || 30);
  };

  //#endregion
}
