import { BoardTileDto } from '../../../../shared/dtos/board-tile.dto';
import { BoardDto } from '../../../../shared/dtos/board.dto';

class BoardDataViewModel {
  /**
   * The name of the player who created the tile
   */
  public createdByName: string;
  /**
   * The name of the player who last modified the tile
   */
  public modByName: string;
  /**
   * Flag indicating if the tile is being actively edited
   */
  public editing: boolean = false;
  /**
   * Flag indicating if this is a new board being created
   */
  public isNew: boolean = false;
}
export type BoardVM = (BoardDto & BoardDataViewModel);
export type BoardTileVM = BoardTileDto & BoardDataViewModel;
