import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { TokenService } from '../service/token.service';

@Injectable({
  providedIn: 'root'
})
export class TwitchApi {
  constructor(private _http: HttpClient,
              private _tokenService: TokenService) {
  }

  /**
   * Send a refresh token request to the server
   *
   * This is a fire-and-forget request
   *
   * The server response will be a Http501 Permanent Redirect
   *
   * This will cause the user to leave whatever page they're on
   */
  public async refreshToken(): Promise<void> {
    await this._http.post('Twitch/RefreshToken', null).toPromise();
  }
}
