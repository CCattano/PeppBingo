import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GameTileDto } from '../dtos/game-tile.dto';

@Injectable({
  providedIn: 'root'
})
export class GameApi {
  constructor(private _http: HttpClient) {
  }

  /**
   * Get the boardID of the currently active board from the server
   */
  public async getActiveBoardID(): Promise<number> {
    return await this._http.get<number>('Game/GetActiveBoardID').toPromise();
  }

  /**
   * Get the name of the board with the boardID provided
   * @param boardID
   */
  public async getBoardNameByID(boardID: number): Promise<string> {
    return await this._http.get<string>(`Game/GetBoardNameByBoardID?boardID=${boardID}`).toPromise();
  }

  /**
   * Get all boardTiles for the board with the boardID provided
   * @param boardID
   */
  public async getActiveBoardTilesByBoardID(boardID: number): Promise<GameTileDto[]> {
    return await this._http.get<GameTileDto[]>(`Game/GetActiveBoardTilesByBoardID?boardID=${boardID}`).toPromise();
  }
}
