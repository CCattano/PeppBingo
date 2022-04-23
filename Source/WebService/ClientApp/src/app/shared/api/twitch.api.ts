import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TokenService } from '../service/token.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

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
   * This will cause the user to leave whatever page they're on
   */
  public async refreshToken(): Promise<void> {
    await this._http.post('Twitch/RefreshToken', null)
      .pipe(catchError(() => {
        // If there's been an error refreshing then we want to clear our cookies
        this._tokenService.clearAllCookies();
        return of(null);
      }))
      .toPromise();
    // Angular can't receive redirect requests
    // It tries to handle them and fetch the html of the redirect uri itself
    // instead of letting the browser handle it
    // In our case we're just trying to get our initializer to fire again
    // So a page refresh after posting is sufficient
    window.location.reload(true);
  }
}
