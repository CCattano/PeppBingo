import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { TwitchApi } from '../../api/twitch.api';
import { TokenService } from '../../service/token.service';

@Injectable({
  providedIn: 'root'
})
export class CommonAuthGuard implements CanActivate {
  constructor(
    private _router: Router,
    private _tokenSvc: TokenService,
    private _twitchApi: TwitchApi
  ) {
  }

  public async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    //Check if token exists
    if (this._tokenSvc.haveToken) {
      if (this._tokenSvc.tokenIsExpired)
        await this._twitchApi.refreshToken();
      else
        return true;
    } else {
      if (state.url == '/login')
        return true;
      else
        this._router.navigateByUrl('login');
    }
    return false;
  }
}
