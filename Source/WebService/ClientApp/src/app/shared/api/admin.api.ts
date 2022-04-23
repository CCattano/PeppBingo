import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDto } from '../dtos/user.dto';

@Injectable({
  providedIn: 'root'
})
export class AdminApi {
  constructor(private _http: HttpClient) {
  }

  /**
   * Fetches users who's display name contains the name provided
   * *Note: Only usable by authenticated users
   * @param name
   */
  public async searchUsersByName(name: string): Promise<UserDto[]> {
    return await this._http.get<UserDto[]>(`Admin/SearchUsersByName?name=${name}`).toPromise();
  }

  /**
   * Fetches users who are marked as Administrators in the system
   */
  public async getAdmins(): Promise<UserDto[]> {
    return await this._http.get<UserDto[]>("Admin/Admins").toPromise();
  }

  /**
   * Updates the user with the userID specified to no longer be considered an Admin by the application
   * @param userID
   */
  public async revokeAdminPermissionForUser(userID: number): Promise<void> {
    return await this._http.put<null>("Admin/RevokeAdminPermission", userID).toPromise();
  }

  /**
   * Updates the user with the userID specified to be considered an Admin by the application
   * @param userID
   */
  public async GrantAdminPermissionForUser(userID: number): Promise<void> {
    return await this._http.put<null>("Admin/GrantAdminPermission", userID).toPromise();
  }
}

