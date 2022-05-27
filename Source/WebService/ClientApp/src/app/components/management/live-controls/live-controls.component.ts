import {HttpErrorResponse} from '@angular/common/http';
import {Component, OnDestroy, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {faEdit, faRadiationAlt, IconDefinition} from '@fortawesome/free-solid-svg-icons';
import {Observable, Subject, timer} from 'rxjs';
import {map, scan, switchMap, take, tap} from 'rxjs/operators';
import {AdminApi} from '../../../shared/api/admin.api';
import {GameApi} from '../../../shared/api/game.api';
import {BoardDto} from '../../../shared/dtos/board.dto';
import {AdminHub} from '../../../shared/hubs/admin.hub';
import {ToastService} from '../../../shared/service/toast.service';
import {NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';

@Component({
  templateUrl: './live-controls.component.html',
  styleUrls: ['./live-controls.component.scss']
})
export class LiveControlsComponent implements OnInit, OnDestroy {
  @ViewChild('resetBoardsConfirmationModal', {static: true})
  private readonly _resetBoardsConfirmationModalContent: TemplateRef<any>;

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
   * The last time a reset event occurred
   */
  public _lastResetEventDateTime: Date;

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faEdit': faEdit,
    'faRadiationAlt': faRadiationAlt
  };

  public _latestBoardCooldownIsActive: boolean = false;
  public _resetBoardCooldownIsActive: boolean = false;
  private _latestBoardCooldownSource: Subject<number> = new Subject<number>();
  private _resetBoardsCooldownSource: Subject<number> = new Subject<number>();
  public readonly latestBoardCooldownTime$: Observable<number>;
  public readonly resetBoardCooldownTime$: Observable<number>;

  private _activeModalRef: NgbModalRef;

  constructor(
    private _adminHub: AdminHub,
    private _adminApi: AdminApi,
    private _gameApi: GameApi,
    private _toastService: ToastService,
    private _ngbModal: NgbModal
  ) {
    this.latestBoardCooldownTime$ =
      this._initCooldownPipeline(this._latestBoardCooldownSource.asObservable()
        .pipe(map((timeout: number) => [false, timeout])));
    this.resetBoardCooldownTime$ =
      this._initCooldownPipeline(this._resetBoardsCooldownSource.asObservable()
        .pipe(map((timeout: number) => [true, timeout])));
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
    if (this._latestBoardCooldownIsActive || !this._boards?.length) return;
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

  /**
   * Event handler for the Nuke 'Em button in the template
   *
   * Opens a modal to confirm the reset boards actions to prevent
   * accidental resets
   */
  public _onResetAllBoardsClick(): void {
    this._activeModalRef =
      this._ngbModal.open(this._resetBoardsConfirmationModalContent, {
        animation: true,
        backdrop: 'static',
        centered: true,
        keyboard: false,
        size: 'lg'
      });
  }

  /**
   * Event handle for the reset board confirmation button
   *
   * Sends and event to the server to be broadcast to all
   * connected players to reset their boards
   */
  public async _onConfirmResetClick(): Promise<void> {
    await this._adminApi.resetAllBoards();
    this._activeModalRef.close();
  }

  /**
   * Event handler for the cancel reset button
   *
   * Closes the confirmation modal
   */
  public _onCancelResetClick(): void {
    this._activeModalRef.close();
  }

  private _initCooldownPipeline(sourceObs: Observable<[boolean, number?]>): Observable<number> {
    return sourceObs.pipe(
      tap(([forResetEvt, _]: [boolean, number?]) => {
        if (forResetEvt)
          this._resetBoardCooldownIsActive = true;
        else
          this._latestBoardCooldownIsActive = true
      }),
      map(([_, timeRemaining]: [boolean, number?]) => [_, timeRemaining || 30] as [boolean, number?]),
      switchMap(([forResetEvt, timeRemaining]: [boolean, number?]) =>
        timer(0, 1000).pipe(
          scan((acc, _) => --acc, timeRemaining),
          take(timeRemaining + 1),
          tap((second: number) => {
            if (second === 0) {
              if (forResetEvt)
                this._resetBoardCooldownIsActive = false;
              else
                this._latestBoardCooldownIsActive = false;
            }
          })
        )
      )
    );
  }

  //#region SignalR Hub Events

  private async _registerHubEventHandlers(): Promise<void> {
    await this._adminHub.connect();
    this._adminHub.registerOnStartSetActiveBoardCooldown(this._onStartSetActiveBoardCooldown);
    this._adminHub.registerOnLatestActiveBoardIDHandler(this._onEmitLatestActiveBoardID);
    const knowLastReset: boolean =
      this._adminHub.registerOnStartResetAllBoardsCooldown(this._onStartResetAllBoardsCooldown);
    if(!knowLastReset) {
      this._lastResetEventDateTime = await this._adminApi.getLastResetDatetime();
    }
  }

  private _onEmitLatestActiveBoardID = (activeBoardID: number) => {
    if (this._activeBoard?.boardID === activeBoardID) return;
    this._activeBoard = this._newActiveBoard =
      this._boards.find(board => board.boardID === activeBoardID);
  };

  private _onStartSetActiveBoardCooldown = (timeRemaining: number = null) => {
    this._latestBoardCooldownSource.next(timeRemaining);
  };

  private _onStartResetAllBoardsCooldown = (evtDateTime: Date, timeRemaining: number = null) => {
    this._lastResetEventDateTime = evtDateTime;
    this._resetBoardsCooldownSource.next(timeRemaining);
  };

  //#endregion
}
