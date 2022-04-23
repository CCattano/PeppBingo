export class BoardDto {
  /**
   * The internal ID that represent this bingo board
   */
  public boardID: number;
  /**
   * The name of the bingo board
   */
  public name: string;
  /**
   * A brief dsescription about the bingo board
   */
  public description: string;
  /**
   * Quantity of active tiles available for this board
   */
  public tileCount: number;
  /**
   * When the board was created
   */
  public createdDateTime: Date;
  /**
   * The UserID of who created the board
   */
  public createdBy: number;
  /**
   * When details about the bingo board were last edited
   */
  public modDateTime: Date;
  /**
   * Who last edited details about the bingo board
   */
  public modBy: number;
}
