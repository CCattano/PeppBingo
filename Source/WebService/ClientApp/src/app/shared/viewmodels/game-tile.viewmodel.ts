import { GameTileDto } from '../dtos/game-tile.dto';

export class GameTileVM extends GameTileDto {
  public isSelected: boolean = false;
}
