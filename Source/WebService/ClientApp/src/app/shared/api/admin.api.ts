import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs/operators';
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
   * Updates and existing board with new information
   * @param board
   */
  public async updateBoard(board: BoardDto): Promise<BoardDto> {
    return await this._http.put<BoardDto>('Admin/UpdateBoard', board)
      .pipe(tap(board => this._convertDateStringsToDates(board)))
      .toPromise();
  }

  private _convertDateStringsToDates(board: BoardDto): void {
    if (!board) return;
    board.createdDateTime = new Date((board.createdDateTime as any as string).endsWith('Z') ? board.createdDateTime : board.createdDateTime + 'Z');
    board.modDateTime = new Date((board.modDateTime as any as string).endsWith('Z') ? board.modDateTime : board.modDateTime + 'Z');
  }
  //#endregion
}

