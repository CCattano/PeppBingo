import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
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
}

