import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {APP_INITIALIZER, NgModule, Provider} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {AppRoutingModule} from './app-routing.module';
import {BingoGameComponent} from './components/game/bingo-game.component';
import {
  LeaderboardSubmissionFlowComponent
} from './components/leaderboard/submission-flow/leaderboard-submission-flow.component';
import {LoginComponent} from './components/login/login.component';
import {LayoutComponent} from './shared/components/layout/layout.component';
import {NavMenuComponent} from './shared/components/nav-menu/nav-menu.component';
import {ToastContainerComponent} from './shared/components/toast/toast-container.component';
import {AppInitializer} from './shared/middleware/app.initializer';
import {TokenInterceptor} from './shared/middleware/token.interceptor';
import {LeaderboardVoteFlowComponent} from './components/leaderboard/vote-flow/leaderboard-vote-flow.component';
import {LeaderboardStandingsComponent} from './components/leaderboard/standings/leaderboard-standings.component';
import {BindOnceDirective} from './shared/directives/bind-once.directive';
import {AboutComponent} from './components/about/about.component';
import {BingoGridContainerComponent} from './shared/components/bingo-grid/bingo-grid-container.component';
import {DesktopBingoGridComponent} from './shared/components/bingo-grid/desktop/desktop-bingo-grid.component';
import {MobileBingoGridComponent} from './shared/components/bingo-grid/mobile/mobile-bingo-grid.component';

const httpInterceptors: Provider = [
  { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
];
export function RunInitialization(appInitializer: AppInitializer): () => void {
  return () => appInitializer.initialize();
}
const appInitializers: Provider = [
  { provide: APP_INITIALIZER, useFactory: RunInitialization, deps: [AppInitializer], multi: true }
];

@NgModule({
  declarations: [
    //#region PAGE COMPONENTS

    LoginComponent,
    AboutComponent,

    // -- Bingo Game
    BingoGameComponent,

    // -- Leaderboard
    LeaderboardSubmissionFlowComponent,
    LeaderboardVoteFlowComponent,
    LeaderboardStandingsComponent,

    //#endregion

    //#region SHARED COMPONENTS

    LayoutComponent,
    NavMenuComponent,
    ToastContainerComponent,
    BingoGridContainerComponent,
    DesktopBingoGridComponent,
    MobileBingoGridComponent,

    //#endregion

    //#region SHARED DIRECTIVES

    BindOnceDirective

    //#endregion
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FontAwesomeModule,
    NgbModule,
    FormsModule
  ],
  providers: [
    //#region INTERCEPTORS

    httpInterceptors,

    //#endregion

    //#region INITIALIZERS

    appInitializers

    //#endregion
  ],
  bootstrap: [LayoutComponent]
})
export class AppModule { }
