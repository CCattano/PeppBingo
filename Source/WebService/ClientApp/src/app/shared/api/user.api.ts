import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDto } from '../dtos/user.dto';
import {UserSubmissionStatus} from '../enums/user-submission-state.enum';

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
   * Log suspicious in-game behaviour for the authenticated user
   */
  public async logSuspiciousBehaviour(): Promise<void> {
    return await this._http.post<void>('User/LogSuspiciousBehaviour', null).toPromise();
  }

  /**
   * Determine whether the user can or cannot
   * submit bingos for leaderboard advancement
   */
  public async getUserSubmissionStatus(): Promise<UserSubmissionStatus> {
    return await this._http.get<UserSubmissionStatus>('User/GetUserSubmissionStatus').toPromise();
  }

  /**
   * Update the server to denote that the authenticated user
   * can no longer submit bingos for leaderboard advancement
   */
  public async markUserAsBingoSubmitted(): Promise<void> {
    return await this._http.post<null>('User/MarkUserAsBingoSubmitted', null).toPromise();
  }
}

