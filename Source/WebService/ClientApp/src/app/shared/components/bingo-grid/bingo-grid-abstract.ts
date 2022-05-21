import {Directive, EventEmitter, Input, Output} from '@angular/core';
import {GameTileVM} from '../../viewmodels/game-tile.viewmodel';

@Directive()
export abstract class BingoGridAbstract {
  /**
   * The board displayed in the template
   */
  @Input()
  public board: GameTileVM[][];

  /**
   * Event that emits whenever a bingo board tile is clicked
   */
  @Output()
  public readonly tileClick: EventEmitter<GameTileVM> = new EventEmitter<GameTileVM>();

  /**
   * Event handler for when a list group item
   * is selected in the mobile bingo board layout
   * @param selectedTile
   */
  public _onTileClick(selectedTile: GameTileVM): void {
    this.tileClick.emit(selectedTile);
  }

  public abstract getScreenshotOfBoard(): void;
}
