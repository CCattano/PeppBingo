import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs/operators';
import { BoardTileDto } from '../dtos/board-tile.dto';
import { BoardDto } from '../dtos/board.dto';
import { UserDto } from '../dtos/user.dto';

@Injectable({
  providedIn: 'root'
})
export class AdminApi {
  constructor(private _http: HttpClient) {
  }

  //#region Admin User Data Endpoints

  /**
   * Fetches users who's display name contains the name provided
   * @param name
   */
  public async searchUsersByName(name: string): Promise<UserDto[]> {
    return await this._http.get<UserDto[]>(`Admin/SearchUsersByName?name=${name}`).toPromise();
  }

  /**
 * Get all bingo board metadata for all boards maintained in the application
 */
  public async getUsersByUserIDs(userIDs: number[]): Promise<UserDto[]> {
    const path: string = 'Admin/GetUsersByIDs';
    const queryParam: string = userIDs.map(id => `userIDs=${id}`).join('&');
    return await this._http.get<UserDto[]>(`${path}?${queryParam}`).toPromise();
  }

  /**
   * Fetches users who are marked as Administrators in the system
   */
  public async getAdmins(): Promise<UserDto[]> {
    return await this._http.get<UserDto[]>('Admin/Admins').toPromise();
  }

  /**
   * Updates the user with the userID specified to no longer be considered an Admin by the application
   * @param userID
   */
  public async revokeAdminPermissionForUser(userID: number): Promise<void> {
    return await this._http.put<null>('Admin/RevokeAdminPermission', userID).toPromise();
  }

  /**
   * Updates the user with the userID specified to be considered an Admin by the application
   * @param userID
   */
  public async grantAdminPermissionForUser(userID: number): Promise<void> {
    return await this._http.put<null>('Admin/GrantAdminPermission', userID).toPromise();
  }

  //#endregion

  //#region Admin Game Data Endpoints

  /**
   * Create a new bingo board
   * @param board
   */
  public async createNewBoard(board: BoardDto): Promise<BoardDto> {
    return await this._http.post<BoardDto>('Admin/CreateBoard', board)
      .pipe(tap(board => this._convertDateStringsToDates(board)))
      .toPromise();
  }

  /**
   * Get all bingo board metadata for all boards maintained in the application
   */
  public async getAllBoards(): Promise<BoardDto[]> {
    return await this._http.get<BoardDto[]>('Admin/Boards')
      .pipe(tap(boards => boards?.forEach(board => this._convertDateStringsToDates(board))))
      .toPromise();
  }

  /**
   * Updates an existing board with new information
   * @param board
   */
  public async updateBoard(board: BoardDto): Promise<BoardDto> {
    return await this._http.put<BoardDto>('Admin/UpdateBoard', board)
      .pipe(tap(board => this._convertDateStringsToDates(board)))
      .toPromise();
  }

  public async deleteBoard(boardID: number): Promise<void> {
    return this._http.delete<null>(`Admin/DeleteBoard?boardID=${boardID}`).toPromise();
  }

  /**
   * Create a new board tile for the board specified
   * @param tile
   */
  public async createNewBoardTile(boardID: number, tile: BoardTileDto): Promise<BoardTileDto> {
    return await this._http.post<BoardTileDto>(`Admin/CreateBoardTile?boardID=${boardID}`, tile)
      .pipe(tap(board => this._convertDateStringsToDates(board)))
      .toPromise();
  }

  /**
   * Get all board tiles for a given board
   * @param boardID
   */
  public async getTilesForBoard(boardID: number): Promise<BoardTileDto[]> {
    return await this._http.get<BoardTileDto[]>(`Admin/Tiles?boardID=${boardID}`)
      .pipe(tap(tiles => tiles?.forEach(tile => this._convertDateStringsToDates(tile))))
      .toPromise();
  }

  /**
   * Updates an existing board tile with new information
   * @param tileZ
   */
  public async updateBoardTile(tile: BoardTileDto, showLoader: boolean = false): Promise<BoardTileDto> {
    return this._http.put<BoardTileDto>('Admin/UpdateBoardTile', tile)
      .pipe(tap(board => this._convertDateStringsToDates(board)))
      .toPromise();
  }

  /**
   * Deletes a board tile with the given tileID
   * @param tileID
   */
  public async deleteBoardTile(tileID: number): Promise<void> {
    return this._http.delete<null>(`Admin/DeleteBoardTile?tileID=${tileID}`).toPromise();
  }

  private _convertDateStringsToDates(gameData: BoardDto | BoardTileDto): void {
    if (!gameData) return;
    gameData.createdDateTime =
      new Date((gameData.createdDateTime as any as string).endsWith('Z')
        ? gameData.createdDateTime
        : gameData.createdDateTime + 'Z');
    gameData.modDateTime =
      new Date((gameData.modDateTime as any as string).endsWith('Z')
        ? gameData.modDateTime
        : gameData.modDateTime + 'Z');
  }
  //#endregion
}

