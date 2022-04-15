import { Component } from '@angular/core';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  public readonly twitchLoginUri: string =
    'https://id.twitch.tv/oauth2/authorize' +
    '?client_id=cszi01mpjo6ru18b1a08t9wkadbfqp' +
    `&redirect_uri=${window.location.protocol}//${window.location.host}/Twitch/AccessCode` +
    '&response_type=code' +
    '&scope=user_read';

  constructor() {
  }
}
