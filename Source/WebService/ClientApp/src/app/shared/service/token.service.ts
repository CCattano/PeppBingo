import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private _userID: number;
  private _tokenTTL: Date;
  private _token: string;
  private _haveToken: boolean;

  /**
   * The name of the auth token cookie set by the server
   */
  public static readonly authTokenName: string = 'PeppAuthToken';

  /**
   * The name of the access token cookie set by the client
   */
  public static readonly accessTokenName: string = 'PeppBingoToken';

  /**
   * The UserID provided in the JWT
   */
  public get userID(): number {
    return this._userID;
  }
  /**
   * The TTL provided in the JWT
   */
  public get tokenTTL(): Date {
    return this._tokenTTL;
  }
  /**
   * The token provided in the JWT
   */
  public get token(): string {
    return this._token;
  }
  /**
   * Whether or not the service contains token data
   */
  public get haveToken(): boolean {
    return this._haveToken;
  };

  /**
   * Set the token in this singleton service
   *
   * If writeToCookie is true the provided token will be
   * added to the user's browser's long term storage as a cookie
   * @param token
   * @param writeToCookie
   */
  public setToken(token: string, writeToCookie: boolean = false): void {
    this._token = token;
    this._haveToken = true;

    //Decode provided token
    this._decodeToken(token);

    //Set token in cookie
    if (writeToCookie) {
      this._writeToCookie(TokenService.accessTokenName);
    }
  }

  /**
   * Remove the JWT from cookie storage
   */
  public removeToken(removeAuthToken: boolean): void {
    const tokenName: string = removeAuthToken
      ? TokenService.authTokenName
      : TokenService.accessTokenName;
    this._writeToCookie(tokenName, true);
    this._haveToken = false;
    this._token = null;
    this._userID = 0;
    this._tokenTTL = null;
  }

  private _writeToCookie(tokenName: string, expireImmediately: boolean = false) {
    const maxAge: number = expireImmediately
      ? 0
      : 60 * 60 * 24; // 1 day worth of seconds
    const cookieParts: string[] = [
      // Cookie name
      `${tokenName}=${this._token}`,
      // Max age of cookie
      `max-age=${maxAge}`,
      // Path for coookie
      'path=/'
    ];
    const cookie: string = cookieParts.join(';');
    document.cookie = cookie;
  }

  private _decodeToken(token: string): void {
    //Token anatomy is 'encodedHeader.encodedBody.encodedSignature'
    const encodedToken: string = this._base64UrlDecode(token);
    const encodedTokenBody: string = encodedToken.split('.')[1];
    const decodedTokenBody: string = this._base64UrlDecode(encodedTokenBody);

    const tokenObj: { UserID: number, ExpirationDateTime: Date } = JSON.parse(decodedTokenBody);

    this._userID = tokenObj.UserID;
    this._tokenTTL = new Date(tokenObj.ExpirationDateTime);
  }

  private _base64UrlDecode(token: string) {
    // Replace non-url compatible chars with base64 standard chars
    token = token
      .replace(/-/g, '+')
      .replace(/_/g, '/');

    // Pad out with standard base64 required padding characters
    var pad = token.length % 4;
    if (pad) {
      if (pad === 1) {
        throw new Error('InvalidLengthError: token base64url string is the wrong length to determine padding');
      }
      token += new Array(5 - pad).join('=');
    }

    const decodedToken: string = atob(token);
    return decodedToken;
  }
}
