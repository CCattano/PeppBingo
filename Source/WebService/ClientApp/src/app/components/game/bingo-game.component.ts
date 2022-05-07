import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {GameApi} from '../../shared/api/game.api';
import {GameTileDto} from '../../shared/dtos/game-tile.dto';
import {PlayerHub} from '../../shared/hubs/player/player.hub';
import {LeaderboardSubmissionFlowComponent} from '../leaderboard/submission-flow/leaderboard-submission-flow.component';
import {GameTileVM} from '../../shared/viewmodels/game-tile.viewmodel';
import {BingoSubmissionEvent} from '../../shared/hubs/player/events/bingo-submission.event';
import {TokenService} from '../../shared/service/token.service';

@Component({
  templateUrl: './bingo-game.component.html',
  styleUrls: ['./bingo-game.component.scss']
})
export class BingoGameComponent implements OnInit, OnDestroy {
  @ViewChild(LeaderboardSubmissionFlowComponent, {static: true})
  private readonly _leaderboardSubmissionFlowComponent: LeaderboardSubmissionFlowComponent;
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
   * Array containing metadata related to another
   * player's board that has gotten a bingo
   */
  public readonly _voteRequests: BingoSubmissionEvent[] = [];

  constructor(private _gameApi: GameApi,
              private _playerHub: PlayerHub,
              private _tokenService: TokenService) {
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
    this._makeBoard();
  }

  /**
   * Event handler for when the user has closed the leaderboard
   * submission modal whether by complete submission or cancel
   */
  public _onLeaderboardSubmissionComplete(): void {
    // Short-term: Reset board
    this._makeBoard();
    // TODO: Long-term: Enable board shuffling for 30s
    // If board is never shuffled at end of 30s perform shuffle automatically
  }

  //#endregion

  //#region Data Initialization Functions

  /**
   * Fetch all data necessary to play a round of bingo
   */
  private async _getGameData(activeBoardID: number = null): Promise<void> {
    // Fetch the board to play with first
    const boardID: number = activeBoardID || await this._gameApi.getActiveBoardID();
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
        this._leaderboardSubmissionFlowComponent.openSubmissionFlowModal();
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
      this._leaderboardSubmissionFlowComponent.openSubmissionFlowModal();
  }

  //#endregion

  //#region SignalR Functions

  private async _registerHubEventHandlers(): Promise<void> {
    await this._playerHub.connect();
    this._playerHub.registerEmitLatestActiveBoardIDHandler(this._onEmitLatestActiveBoardID);
    this._playerHub.registerEmitBingoSubmissionHandler(this._onEmitBingoSubmission);
  }

  private _onEmitLatestActiveBoardID = (activeBoardID: number): void => {
    this._getGameData(activeBoardID);
  };

  public _onEmitBingoSubmission = (submission: BingoSubmissionEvent): void => {
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

  //#endregion
}
