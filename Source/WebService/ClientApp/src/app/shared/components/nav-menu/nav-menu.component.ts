import { Component, OnInit } from '@angular/core';
import { UserApi } from '../../api/user.api';
import { UserDto } from '../../dtos/user.dto';
import { TokenService } from '../../service/token.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {
  /** User data for the currently authenticated user */
  public _user: UserDto;

  constructor(private _userApi: UserApi,
              private _tokenService: TokenService) {
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    if (this._tokenService.haveToken)
      this._user = await this._userApi.getUser();
  }
}
