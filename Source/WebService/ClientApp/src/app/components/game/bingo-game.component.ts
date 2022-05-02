import { Component, OnDestroy, OnInit } from '@angular/core';
import { fromEvent, Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map, tap } from 'rxjs/operators';
import { GameApi } from '../../shared/api/game.api';
import { GameTileDto } from '../../shared/dtos/game-tile.dto';
import { GameTileVM } from './NewFolder/game-tile.viewmodel';

enum BreakpointsEnum {
  xs = 0,
  sm = 576,
  md = 768,
  lg = 992,
  xl = 1200,
  xxl = 1400
}

@Component({
  templateUrl: './bingo-game.component.html',
  styleUrls: ['./bingo-game.component.scss']
})
export class BingoGameComponent implements OnInit, OnDestroy {
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
   * Enum reference captures for use in template
   */
  public readonly breakpointsEnum: typeof BreakpointsEnum = BreakpointsEnum;

  /**
   * Observable constructed from the window.onresize event
   */
  private _resizeEvent$: Observable<Event> = fromEvent(window, 'resize');
  /**
   * Subscription to the resize event observable
   */
  private _resizeSub: Subscription;
  /**
   * The current breakpoint the page width is associated with
   *
   * Used to determine when to display mobile vs. desktop bingo board
   */
  public _currBreakpoint: BreakpointsEnum;

  /**
   * Bool flag that indicated we are in a mobile
   * width and in a landscape orientation
   */
  public _isMobileLandscape: boolean;

  constructor(private _gameApi: GameApi) {
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    this._currBreakpoint = this._calcViewport();
    this._isMobileLandscape = this._calcIsMobileLandscape(this._currBreakpoint);
    this._resizeSub = this._initResizeEventPipeline().subscribe();
    await this._getGameData();
  }

  /**
   * @inheritdoc
   */
  public ngOnDestroy(): void {
    this._resizeSub?.unsubscribe();
    this._resizeSub = null;
  }

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

  //#region Data Initalization Functions

  /**
   * Fetch all data necessary to play a round of bingo
   */
  private async _getGameData(): Promise<void> {
    // Fetch the board to play with first
    const activeBoardID: number = await this._gameApi.getActiveBoardID();
    this._noActiveBoard = !activeBoardID;
    // If a active board hasn't been set yet bail here
    if (this._noActiveBoard) return;
    // If we have a board get its board info and tile info
    await Promise.all([
      this._getBoardName(activeBoardID),
      this._getBoardTiles(activeBoardID)
    ]);
    // Construct the board to be displayed
    this._makeBoard();
  }

  private async _getBoardName(activeBoardID: number): Promise<void> {
    const name: string = await this._gameApi.getBoardNameByID(activeBoardID);
    this._boardName = name;
  }

  private async _getBoardTiles(activeBoardID: number): Promise<void> {
    const tiles: GameTileDto[] =
      await this._gameApi.getActiveBoardTilesByBoardID(activeBoardID);
    this._tiles = tiles;
    const freeSpaceIdx: number = this._tiles.findIndex(tile => tile.isFreeSpace);
    if (freeSpaceIdx >= 0)
      this._freeSpace = this._tiles.splice(freeSpaceIdx, 1).shift();
  }

  //#endregion

  private _makeBoard(): void {
    const tiles: GameTileDto[] = this._tiles.slice(0);
    // Shuffle the array befor grabbing
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

  //#region Resize Functions
  private _calcViewport(): BreakpointsEnum {
    const width: number = document.documentElement.clientWidth;
    if (width >= BreakpointsEnum.xxl)
      return BreakpointsEnum.xxl;
    else if (width >= BreakpointsEnum.xl)
      return BreakpointsEnum.xl;
    else if (width >= BreakpointsEnum.lg)
      return BreakpointsEnum.xl;
    else if (width >= BreakpointsEnum.md)
      return BreakpointsEnum.md;
    else if (width >= BreakpointsEnum.sm)
      return BreakpointsEnum.sm;
    else
      return BreakpointsEnum.xs;
  }

  private _calcIsMobileLandscape(breakpoint: BreakpointsEnum): boolean {
    return breakpoint < BreakpointsEnum.lg && window.matchMedia("(orientation: landscape)").matches;
  }

  private _initResizeEventPipeline(): Observable<void> {
    return this._resizeEvent$.pipe(
      debounceTime(250),
      distinctUntilChanged(),
      map(() => this._calcViewport()),
      filter((breakpoint: BreakpointsEnum) =>
        this._currBreakpoint !== breakpoint),
      tap((breakpoint: BreakpointsEnum) => {
        this._currBreakpoint = breakpoint;
        this._isMobileLandscape = this._calcIsMobileLandscape(breakpoint);
      }),
      map(() => null)
    );
  }

  private _checkForWinCondition() {
    //Check all rows, columns, and diagonals
    let topLToBottomRDiagCount = 0;
    let bottomLToTopRDiagCount = 0;
    let blttrRow = 0;
    let blttrCol = 4;
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
        alert("You've Won!");
        return;
      }
      //check top-left to bottom-right diag coordinate
      if (this._board[row][row].isSelected)
        topLToBottomRDiagCount++;

      if (this._board[blttrRow][blttrCol].isSelected)
        bottomLToTopRDiagCount++;

      blttrRow++;
      blttrCol--;
    }
    //If no rows or cols had 5 in a row check diagonals
    if (topLToBottomRDiagCount === 5 || bottomLToTopRDiagCount === 5)
      alert("You've Won!");
  }
  //#endregion
}
