import { Component, OnInit } from "@angular/core";
import { UserApi } from '../../shared/api/user.api';
import { UserDto } from '../../shared/dtos/user.dto';
import { TokenService } from '../../shared/service/token.service';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  /** Temp for testing */
  public user: UserDto;

  constructor(private _userApi: UserApi) {
  }

  /**
   * @inheritdoc
   */
  public async ngOnInit(): Promise<void> {
    this.user = await this._userApi.getUser();
  }
}
