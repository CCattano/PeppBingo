import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay, tap } from 'rxjs/operators';
import { UserDto } from '../dtos/user.dto';

@Injectable({
  providedIn: 'root'
})
export class UserApi {
  constructor(private _http: HttpClient) {
  }

  /**
   * Fetches user info for the user authenticated via Twitch Auth integration
   */
  public async getUser(): Promise<UserDto> {
    return await this._http.get<UserDto>('User/GetUser').toPromise();
  }

  /**
   * Fetches users who's display name contains the name provided
   * *Note: Only usable by authenticated users
   * @param name
   */
  public async searchUsersByName(name: string): Promise<UserDto[]> {
    return await this._http.get<UserDto[]>(`User/SearchUsersByName?name=${name}`).toPromise();
  }

  /**
   * Fetches users who are marked as Administrators in the system
   */
  public async getAdmins(): Promise<UserDto[]> {
    //return await this._http.get<UserDto[]>("User/Admins").toPromise();
    return await of([
      ...new Array(12).fill(0).map((_, i) => ({
        displayName: `TORGUTAN_TORRES${i + 1}`,
        isAdmin: true,
        profileImageUri: 'https://static-cdn.jtvnw.net/jtv_user_pictures/28589ef5-43c3-468d-a839-f5c6f8bb4421-profile_image-300x300.png'
      }) as UserDto)
    ]).pipe(delay(750))
      .toPromise();
  }

  /**
   * Updates the user with the userID specified to no longer be considered an Admin by the application
   * @param userID
   */
  public async removeAdminPermissionForUser(userID: number): Promise<void> {
    //return await this._http.put<void>("User/RemoveAdminPermission", { userID }).toPromise();
    return of(null)
      .pipe(
        tap(() => console.log('In removeAdminPermissionForUser req')),
        delay(3000)
      )
      .toPromise();
  }

  /**
   * Updates the user with the userID specified to be considered an Admin by the application
   * @param userID
   */
  public async grantAdminPermissionForUser(userID: number): Promise<void> {
    //return await this._http.put<void>("User/GrantAdminPermission", { userID }).toPromise();
    return of(null)
      .pipe(
        tap(() => console.log('In grantAdminPermissionForUser req')),
        delay(3000)
      )
      .toPromise();
  }
}

