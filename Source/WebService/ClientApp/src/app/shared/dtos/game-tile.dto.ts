export class GameTileDto {
  /**
   * The text content of this bingo board tile to be displayed on the board
   */
  public text: string;
  /**
   * Flag indicating if this tile is to be used as the center tile on the bingo board
   */
  public isFreeSpace: boolean
}
