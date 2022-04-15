import { Component, OnInit } from "@angular/core";
import { TokenService } from '../../shared/service/token.service';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  /** Temp for testing */
  public diagnostic: Object;

  constructor(private _tokenService: TokenService) {
  }

  /**
   * @inheritdoc
   */
  public ngOnInit(): void {
    this.diagnostic = {
      haveToken: this._tokenService.haveToken,
      token: this._tokenService.token,
      tokenTTL: this._tokenService.tokenTTL,
      userID: this._tokenService.userID
    };
  }
}
