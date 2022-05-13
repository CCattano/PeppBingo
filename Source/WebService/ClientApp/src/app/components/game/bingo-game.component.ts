import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {GameApi} from '../../shared/api/game.api';
import {GameTileDto} from '../../shared/dtos/game-tile.dto';
import {PlayerHub} from '../../shared/hubs/player/player.hub';
import {LeaderboardSubmissionFlowComponent} from '../leaderboard/submission-flow/leaderboard-submission-flow.component';
import {GameTileVM} from '../../shared/viewmodels/game-tile.viewmodel';
import {BingoSubmissionEvent} from '../../shared/hubs/player/events/bingo-submission.event';
import {TokenService} from '../../shared/service/token.service';
import {LeaderboardVoteFlowComponent} from '../leaderboard/vote-flow/leaderboard-vote-flow.component';
import {Observable, Subject, timer} from 'rxjs';
import {map, scan, switchMap, take, tap} from 'rxjs/operators';

@Component({
  templateUrl: './bingo-game.component.html',
  styleUrls: ['./bingo-game.component.scss']
})
export class BingoGameComponent implements OnInit, OnDestroy {
  @ViewChild(LeaderboardSubmissionFlowComponent, {static: true})
  private readonly _leaderboardSubmissionFlowComponent: LeaderboardSubmissionFlowComponent;

  @ViewChild(LeaderboardVoteFlowComponent, {static: true})
  private readonly _leaderboardVoteFlowComponent: LeaderboardVoteFlowComponent;

  /**
   * Bool flag indicating an admin has not set an active board to play
   */
  public _noActiveBoard: boolean;

  /**
   * The name of the board being played currently
   */
  public _boardName: string;

  /**
   * All active tiles available for this board
   */
  public _tiles: GameTileDto[];
  /**
   * The designated free-space for this board, if it exists
   */
  public _freeSpace: GameTileDto;

  /**
   * The board displayed in the template
   */
  public _board: GameTileVM[][];

  /**
   * The ID of the board currently being played
   */
  public _activeBoardID: number;

  /**
   * Array containing metadata related to another
   * player's board that has gotten a bingo
   */
  public readonly _voteRequests: BingoSubmissionEvent[] = [];

  private _cannotSubmitBingo: boolean = false;

  private readonly _resetActiveSource: Subject<number> = new Subject<number>();
  public readonly resetActiveTimer$: Observable<number>;
  public _resetIsActive: boolean = false;

  constructor(private _gameApi: GameApi,
              private _playerHub: PlayerHub,
              private _tokenService: TokenService) {
    this.resetActiveTimer$ = this._initResetActivePipe();
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    await this._registerHubEventHandlers();
    await this._getGameData();
  }

  /**
   * @inheritdoc
   */
  public ngOnDestroy(): void {
    this._playerHub.unregisterAllHandlers();
  }

  //#region UI Event Handlers

  /**
   * Event handler for when a list group item
   * is selected in the mobile bingo board layout
   * @param selectedTile
   */
  public _onTileClick(selectedTile: GameTileVM): void {
    selectedTile.isSelected = !selectedTile.isSelected;
    this._checkForWinCondition();
  }

  /**
   * Event handler for when the reset board button is clicked
   * in either the mobile or desktop bingo board layout
   */
  public _onResetBoardClick(): void {
    if (!this._resetIsActive) return;
    this._makeBoard();
  }

  /**
   * Event handler for when the user has closed the leaderboard
   * submission modal whether by complete submission or cancel
   */
  public _onLeaderboardSubmissionComplete(bingoWasApproved: boolean): void {
    if (!this._cannotSubmitBingo)
      this._cannotSubmitBingo = bingoWasApproved;
    // Short-term: Reset board
    this._makeBoard();
    // TODO: Long-term: Enable board shuffling for 30s
    // If board is never shuffled at end of 30s perform shuffle automatically
  }

  /**
   * Event handler for the Vote button.
   * Opens the vote modal
   */
  public _onOpenVoteModalClick(): void {
    this._leaderboardVoteFlowComponent.openModal();
  }

  //#endregion

  //#region Data Initialization Functions

  /**
   * Fetch all data necessary to play a round of bingo
   */
  private async _getGameData(activeBoardID: number = null): Promise<void> {
    // Fetch the board to play with first
    const boardID: number = activeBoardID || await this._gameApi.getActiveBoardID();
    this._activeBoardID = boardID;
    this._noActiveBoard = !boardID;
    // If an active board hasn't been set yet bail here
    if (this._noActiveBoard) return;
    // If we have a board get its board info and tile info
    await Promise.all([
      this._getBoardName(boardID),
      this._getBoardTiles(boardID)
    ]);
    // Construct the board to be displayed
    this._makeBoard();
  }

  private async _getBoardName(activeBoardID: number): Promise<void> {
    this._boardName = await this._gameApi.getBoardNameByID(activeBoardID);
  }

  private async _getBoardTiles(activeBoardID: number): Promise<void> {
    this._tiles = await this._gameApi.getActiveBoardTilesByBoardID(activeBoardID);
    const freeSpaceIdx: number = this._tiles.findIndex(tile => tile.isFreeSpace);
    if (freeSpaceIdx >= 0)
      this._freeSpace = this._tiles.splice(freeSpaceIdx, 1).shift();
  }

  //#endregion

  //#region Gameplay Functions

  private _makeBoard(): void {
    const tiles: GameTileDto[] = this._tiles.slice(0);
    // Shuffle the array before grabbing
    // random items out of it for the board
    for (let i: number = tiles.length - 1; i > 0; i--) {
      const j: number = Math.floor(Math.random() * (i + 1));
      [tiles[i], tiles[j]] = [tiles[j], tiles[i]];
    }
    this._board = [];
    for (let row: number = 0; row < 5; row++) {
      this._board[row] = [];
      for (let col: number = 0; col < 5; col++) {
        if (row === 2 && col === 2 && this._freeSpace) {
          this._board[row].push({
            ...this._freeSpace,
            isSelected: false
          } as GameTileVM);
          continue;
        }
        const randomIndex: number =
          Math.floor(Math.random() * (tiles.length));
        const tile: GameTileDto = tiles.splice(randomIndex, 1).shift();
        this._board[row].push({
          ...tile,
          isSelected: false
        } as GameTileVM);
      }
    }
  }

  private _checkForWinCondition() {
    //Check all rows, columns, and diagonals
    let topLToBotRDiagCount = 0;
    let botLToTopRDiagCount = 0;
    for (let row = 0; row < 5; row++) {
      let selectedRowCount = 0;
      let selectedColCount = 0;
      //check all columns for this row
      for (let col = 0; col < 5; col++) {
        //horizontal check: check all columns for this row
        if (this._board[row][col].isSelected)
          selectedRowCount++;
        //vertical check: check all rows for this column
        if (this._board[col][row].isSelected)
          selectedColCount++;
      }
      //Check if nested for-loop found winner
      if (selectedRowCount === 5 || selectedColCount === 5) {
        this._leaderboardSubmissionFlowComponent.openSubmissionFlowModal(this._cannotSubmitBingo);
        return;
      }
      //check top-left to bottom-right diag coordinate
      if (this._board[row][row].isSelected)
        topLToBotRDiagCount++;

      if (this._board[Math.abs(row - 4)][row].isSelected)
        botLToTopRDiagCount++;
    }
    //If no rows or cols had 5 in a row check diagonals
    if (topLToBotRDiagCount === 5 || botLToTopRDiagCount === 5)
      this._leaderboardSubmissionFlowComponent.openSubmissionFlowModal(this._cannotSubmitBingo);
  }

  private _initResetActivePipe(): Observable<number> {
    return this._resetActiveSource.asObservable().pipe(
      tap(() => {
        this._resetIsActive = true;
        this._makeBoard();
      }),
      map((timeRemaining?: number) => timeRemaining || 30),
      switchMap((timeRemaining: number) =>
        timer(0, 1000).pipe(
          scan((acc, _) => --acc, timeRemaining),
          take(timeRemaining + 1),
          tap((second: number) => {
            if (second === 0)
              this._resetIsActive = false;
          })
        )
      )
    );
  }

  //#endregion

  //#region SignalR Functions

  private async _registerHubEventHandlers(): Promise<void> {
    await this._playerHub.connect();
    this._playerHub.registerLatestActiveBoardIDHandler(this._onLatestActiveBoardID);
    this._playerHub.registerBingoSubmissionHandler(this._onBingoSubmission);
    this._playerHub.registerCancelSubmissionHandler(this._onSubmissionCancel);
    this._playerHub.registerResetBoardHandler(this._onResetBoardEvent);
  }

  private _onLatestActiveBoardID = (activeBoardID: number): void => {
    this._getGameData(activeBoardID);
  };

  public _onBingoSubmission = (submission: BingoSubmissionEvent): void => {
    // This Hub event is sent to all users who are NOT the user that initiated the event
    // We are able to distinguish the initiator by their Hub connection ID
    // But if that initiator had multiple tabs open they will have different HubConnIDs per tab
    // So they could receive this event and approve themselves from a separate tab
    // So we're double-checking here that the UserID of the event
    // initiator in the submission payload is not the same as the
    // UserID of the user running the app which we have in our tokenSvc
    if(this._tokenService.userID === submission.userID)
      return; // Nice try m8.

    this._voteRequests.push(submission);
  }

  private _onSubmissionCancel = (hubConnID: string): void => {
    const canceledReqIndex: number =
      this._voteRequests.findIndex(req => req.submitterConnectionID === hubConnID);
    if (canceledReqIndex >= 0)
      this._voteRequests.splice(canceledReqIndex, 1);
  }

  private _onResetBoardEvent = (timeRemaining?: number): void => {
    this._resetActiveSource.next(timeRemaining);
  }

  //#endregion
}
