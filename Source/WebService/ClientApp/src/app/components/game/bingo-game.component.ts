import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {GameApi} from '../../shared/api/game.api';
import {GameTileDto} from '../../shared/dtos/game-tile.dto';
import {PlayerHub} from '../../shared/hubs/player/player.hub';
import {LeaderboardSubmissionFlowComponent} from '../leaderboard/submission-flow/leaderboard-submission-flow.component';
import {GameTileVM} from '../../shared/viewmodels/game-tile.viewmodel';
import {BingoSubmissionEvent} from '../../shared/hubs/player/events/bingo-submission.event';
import {TokenService} from '../../shared/service/token.service';
import {LeaderboardVoteFlowComponent} from '../leaderboard/vote-flow/leaderboard-vote-flow.component';
import {of} from 'rxjs';
import {switchMap, tap} from 'rxjs/operators';
import {UserApi} from '../../shared/api/user.api';
import {UserSubmissionStatus} from '../../shared/enums/user-submission-state.enum';
import {BingoGridContainerComponent} from '../../shared/components/bingo-grid/bingo-grid-container.component';

interface ILocalStorageContent {
  /**
   * The ID of the board the grid of
   * tiles has been constructed from
   */
  boardID: number;
  /**
   * The last board made for this user
   */
  boardGrid: GameTileVM[][];
  /**
   * The ID of the most recent reset
   * event this player was apart of
   */
  resetEventID: string;
}

@Component({
  templateUrl: './bingo-game.component.html',
  styleUrls: ['./bingo-game.component.scss']
})
export class BingoGameComponent implements OnInit, OnDestroy {
  private static readonly _localStorageKey: string = 'PeppBingo_Board';

  @ViewChild(LeaderboardSubmissionFlowComponent, {static: true})
  private readonly _leaderboardSubmissionFlowComponent: LeaderboardSubmissionFlowComponent;

  @ViewChild(LeaderboardVoteFlowComponent, {static: true})
  private readonly _leaderboardVoteFlowComponent: LeaderboardVoteFlowComponent;

  @ViewChild(BingoGridContainerComponent)
  private readonly _bingoGridContainerComponent: BingoGridContainerComponent;

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

  private _userSubmissionStatus: UserSubmissionStatus =
    UserSubmissionStatus.CanSubmitBingo;
  private _resetEventID: string;

  constructor(private _gameApi: GameApi,
              private _userApi: UserApi,
              private _playerHub: PlayerHub,
              private _tokenService: TokenService) {
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    await this._registerHubEventHandlers();
    await this._initGameData();
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
    const localStorageContent: ILocalStorageContent = {
      boardID: this._activeBoardID,
      boardGrid: this._board,
      resetEventID: this._resetEventID
    };
    window.localStorage.setItem(BingoGameComponent._localStorageKey, JSON.stringify(localStorageContent));
    this._checkForWinCondition();
  }

  /**
   * Event handler for when the user has closed the leaderboard
   * submission modal whether by complete submission or cancel
   */
  public _onLeaderboardSubmissionComplete(userSubmissionStatus: UserSubmissionStatus): void {
    this._userSubmissionStatus = userSubmissionStatus;
    // Only reset the board if a bingo was approved by the community.
    // Otherwise, keep the same board on rejections and cancelled submissions.
    // Only a successful bingo or a Mod reset event can get a player a new board.
    if (userSubmissionStatus === UserSubmissionStatus.AlreadySubmitted)
      this._makeBoard();
  }

  /**
   * Event handler for the Vote button.
   * Opens the vote modal
   */
  public _onOpenVoteModalClick(): void {
    this._leaderboardVoteFlowComponent.openModal();
  }

  public _onScreenshotClick(): void {
    this._bingoGridContainerComponent.getScreenshotOfBoard();
  }
  //#endregion

  //#region Data Initialization Functions

  /**
   * Fetch all data necessary to play a round of bingo
   */
  private async _initGameData(activeBoardID: number = null): Promise<void> {
    // Fetch the board to play with first
    const boardID: number = activeBoardID || await this._gameApi.getActiveBoardID();

    this._activeBoardID = boardID;
    this._noActiveBoard = !boardID;
    // If an active board hasn't been set yet bail here
    if (this._noActiveBoard) return;

    // If we have a board get its board info and tile info
    await Promise.all([
      this._getBoardName(boardID),
      this._getBoardTiles(boardID),
      this._getCurrentResetID(),
      this._getSubmissionStatus()
    ]);

    // Determine if this is a returning user with a still valid board
    const boardWasMade: boolean = this._tryGetBoardFromLocalStorage();
    if (!boardWasMade) {
      // If a board could not be provided by LS data
      // Mark that as suspicious on the server for this user
      // Then determine if that means they cannot submit
      of(null).pipe(
        switchMap(() => this._userApi.logSuspiciousBehaviour()),
        switchMap(() => this._userApi.getUserSubmissionStatus()),
        tap((userSubmissionStatus: UserSubmissionStatus) => {
          this._userSubmissionStatus = userSubmissionStatus;
          this._makeBoard();
        })
      ).subscribe();
    }
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

  private async _getCurrentResetID(): Promise<void> {
    this._resetEventID = await this._gameApi.getCurrentResetID();
  }

  private async _getSubmissionStatus(): Promise<void> {
    this._userSubmissionStatus = await this._userApi.getUserSubmissionStatus();
  }

  private _tryGetBoardFromLocalStorage(): boolean {
    const boardContent: string =
      window.localStorage.getItem(BingoGameComponent._localStorageKey);
    if (!boardContent) return false;
    const {boardID, boardGrid, resetEventID} =
      JSON.parse(boardContent) as ILocalStorageContent;
    if (this._resetEventID !== resetEventID) {
      // This user had a board in LS but since it was made a reset has happened
      // This board is stale, so we will construct a new board for them
      this._makeBoard();
    } else if (this._activeBoardID !== boardID) {
      // This user had a board in LS but since it was made the active board has changed
      // So we cannot give them this board to play with. Let's make a new one.
      this._makeBoard();
    } else {
      this._board = boardGrid;
    }
    // We'll return true to indicate were able to set
    // this user up with a board based on their LS data
    return true;
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
    const localStorageContent: ILocalStorageContent = {
      boardID: this._activeBoardID,
      boardGrid: this._board,
      resetEventID: this._resetEventID
    };
    window.localStorage.setItem(BingoGameComponent._localStorageKey, JSON.stringify(localStorageContent));
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
        this._leaderboardSubmissionFlowComponent.openSubmissionFlowModal(this._userSubmissionStatus);
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
      this._leaderboardSubmissionFlowComponent.openSubmissionFlowModal(this._userSubmissionStatus);
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
    this._userSubmissionStatus = UserSubmissionStatus.CanSubmitBingo;
    this._initGameData(activeBoardID).then(null);
  };

  private _onResetBoardEvent = (resetEventID: string): void => {
    this._resetEventID = resetEventID;
    this._userSubmissionStatus = UserSubmissionStatus.CanSubmitBingo;
    this._makeBoard();
  }

  private _onBingoSubmission = (submission: BingoSubmissionEvent): void => {
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

  //#endregion
}
