export class BingoSubmissionEvent {
  /*
   * The SignalR Hub Connection ID for this user.
   * Used so that the Vote event is not sent to the user requesting the vote
   */
  public submitterConnectionID: string;
  /*
   * The internal UserID of the user
   * Used so the user cannot have multiple tabs open and vote for themselves
   */
  public userID: number;
  /*
   * The board tiles that compose the vote requestor's board at the time of bingo submission
   */
  public boardTiles: TileDetail[];
}

class TileDetail
{
  /*
   * The text of the bingo tile
   */
  public text: string;
  /*
   * Bool flag indicating if tile is selected
   */
  public isSelected: boolean;
  /*
   * The row the bingo tile is associated with
   */
  public row: number;
  /*
   * The column the bingo tile is associated with
   */
  public column: number;
}
