export class BoardTileDto {
  /**
   * The internal ID that represent this bingo board tile
   */
  public tileID: number;
  /**
   * The text content of this bingo board tile to be displayed on the board
   */
  public text: string;
  /**
   * Flag indicating if this tile is to be used as the center tile on the bingo board
   */
  public isFreeSpace: boolean
  /**
   * Flag indicating if this tile is enabled for use in bingo games
   */
  public isActive: boolean;
  /**
   * When the board tile was created
   */
  public createdDateTime: Date;
  /**
   * The UserID of who created the board tile
   */
  public createdBy: number;
  /**
   * When details about the bingo board tile were last edited
   */
  public modDateTime: Date;
  /**
   * Who last edited details about the bingo board tile
   */
  public modBy: number;
}
