import { Injectable, Injector } from '@angular/core';
import { TokenService } from '../service/token.service';

@Injectable()
export class AppInitializer {
  private _tokenSvc: TokenService;

  constructor(injector: Injector) {
    this._tokenSvc = injector.get(TokenService);
  }

  /**
   * Run setup logic for the application on the initial bootstrapping
   * of the framework before the request route resolves
   */
  public initialize(): void {
    let token: string = this._tryGetTokenValueByName(TokenService.authTokenName);

    // If we've found a token under the PeppAuthToken workflow
    // Then the user has just finished the authentication workflow
    // and does not need to be considered for the refresh workflow
    // Set the 1 min auth token as a 1 day app access token and leave
    if (token) {
      this._tokenSvc.removeToken(true);
      this._tokenSvc.setToken(token, true);
      return;
    }

    token = this._tryGetTokenValueByName(TokenService.accessTokenName);

    /*
     * If we find a token under the access token name
     * we want to store it in our token service
     * our common auth guard will check to determine
     * if it is expired and needs refreshed or not
     */
    if (token) {
      this._tokenSvc.setToken(token);
    }
    /*
     * If we find no auth or access token to set in our token service
     * Then we will let the route handling determine there is no token
     * in our auth guard and that will send us to the login page
     */
  }

  /**
   * Attempt to fetch a cookie of the provided name and return its value
   * @param tokenName
   */
  private _tryGetTokenValueByName(tokenName: string): string {
    return document.cookie
      .split(';')
      .find(row => row.startsWith(tokenName))
      ?.split('=')
      .pop();
  }
}
