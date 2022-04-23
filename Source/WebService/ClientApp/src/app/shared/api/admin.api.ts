import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
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
    const queryParam: string = userIDs.map(id => `userID=${id}`).join('&');
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
   * Get all bingo board metadata for all boards maintained in the application
   */
  public async getAllBoards(): Promise<BoardDto[]> {
    return await this._http.get<BoardDto[]>('Admin/Boards').toPromise();
  }

  //#endregion
}

