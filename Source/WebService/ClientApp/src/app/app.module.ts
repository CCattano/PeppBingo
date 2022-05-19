import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {APP_INITIALIZER, NgModule, Provider} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserModule} from '@angular/platform-browser';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NgToggleModule} from '@nth-cloud/ng-toggle';
import {AppRoutingModule} from './app-routing.module';
import {BingoGameComponent} from './components/game/bingo-game.component';
import {
  LeaderboardSubmissionFlowComponent
} from './components/leaderboard/submission-flow/leaderboard-submission-flow.component';
import {LoginComponent} from './components/login/login.component';
import {AddEditAdminComponent} from './components/management/admin/admin-add-edit.component';
import {BingoBoardAddEditComponent} from './components/management/bingo/boards/bingo-board-add-edit.component';
import {BoardCardComponent} from './components/management/bingo/boards/board-card/board-card.component';
import {EditBoardCardComponent} from './components/management/bingo/boards/edit-board-card/edit-board-card.component';
import {BingoTileAddEditComponent} from './components/management/bingo/tiles/bingo-tile-add-edit.component';
import {EditTileCardComponent} from './components/management/bingo/tiles/edit-tile-card/edit-tile-card.component';
import {TileCardComponent} from './components/management/bingo/tiles/tile-card/tile-card.component';
import {LiveControlsComponent} from './components/management/live-controls/live-controls.component';
import {FormInputComponent} from './shared/components/form/form-input/form-input.component';
import {FormTextAreaComponent} from './shared/components/form/form-textarea/form-textarea.component';
import {LayoutComponent} from './shared/components/layout/layout.component';
import {NavMenuComponent} from './shared/components/nav-menu/nav-menu.component';
import {ToastContainerComponent} from './shared/components/toast/toast-container.component';
import {AppInitializer} from './shared/middleware/app.initializer';
import {TokenInterceptor} from './shared/middleware/token.interceptor';
import {SafeHtmlPipe} from './shared/safe-html.pipe';
import {LeaderboardVoteFlowComponent} from './components/leaderboard/vote-flow/leaderboard-vote-flow.component';
import {DesktopBingoGridComponent} from './shared/components/bingo-grid/desktop/desktop-bingo-grid.component';
import {MobileBingoGridComponent} from './shared/components/bingo-grid/mobile/mobile-bingo-grid.component';
import {BingoGridContainerComponent} from './shared/components/bingo-grid/bingo-grid-container.component';
import {LeaderboardStandingsComponent} from './components/leaderboard/standings/leaderboard-standings.component';
import {BindOnceDirective} from './shared/directives/bind-once.directive';
import {AboutComponent} from './components/about/about.component';

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

    // -- Admin Management
    AddEditAdminComponent,

    // -- Bingo Board Management
    BingoBoardAddEditComponent,
    BoardCardComponent,
    EditBoardCardComponent,

    // -- Bingo Tile Management
    BingoTileAddEditComponent,
    TileCardComponent,
    EditTileCardComponent,

    // -- Live Controls
    LiveControlsComponent,

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
    FormInputComponent,
    FormTextAreaComponent,
    BingoGridContainerComponent,
    DesktopBingoGridComponent,
    MobileBingoGridComponent,

    //#endregion

    //#region SHARED PIPES

    SafeHtmlPipe,

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
    ReactiveFormsModule,
    NgToggleModule,
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
