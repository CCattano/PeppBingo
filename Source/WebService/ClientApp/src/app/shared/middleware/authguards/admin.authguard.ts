import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { TwitchApi } from '../../api/twitch.api';
import { UserApi } from '../../api/user.api';
import { UserDto } from '../../dtos/user.dto';
import { TokenService } from '../../service/token.service';

@Injectable({
  providedIn: 'root'
})
export class AdminAuthGuard implements CanActivate {
  constructor(
    private _router: Router,
    private _tokenSvc: TokenService,
    private _twitchApi: TwitchApi,
    private _userApi: UserApi
  ) {
  }

  public async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    if (this._tokenSvc.haveToken) {
      if (this._tokenSvc.tokenIsExpired) {
        await this._twitchApi.refreshToken();
      } else {
        const user: UserDto = (await this._userApi.getUser().catch(() => null));
        if (user?.isAdmin)
          return true;
        else
          this._router.navigateByUrl('');
      }
    } else {
      this._router.navigateByUrl('login');
    }
    return false;
  }
}
