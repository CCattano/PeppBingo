import { BoardDto } from '../../../../../shared/dtos/board.dto';

export class BoardVM extends BoardDto {
  /**
   * The name of the player who created the board
   */
  public createdByName: string;
  /**
   * The name of the player who last modified the board
   */
  public modByName: string;
  /**
   * Flag indicating if the board is being actively edited
   */
  public editing: boolean;
  /**
   *Flag indicating if this is a new board being created
   */
  public isNew: boolean;
}
