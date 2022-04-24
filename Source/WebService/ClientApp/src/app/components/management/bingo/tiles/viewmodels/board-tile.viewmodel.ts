import { BoardTileDto } from '../../../../../shared/dtos/board-tile.dto';

export class BoardTileVM extends BoardTileDto {
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
}
