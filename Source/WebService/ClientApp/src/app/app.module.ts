import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule, Provider } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgToggleModule } from '@nth-cloud/ng-toggle';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { AddEditAdminComponent } from './components/management/admin/admin-add-edit.component';
import { BingoBoardAddEditComponent } from './components/management/bingo/boards/bingo-board-add-edit.component';
import { BoardCardComponent } from './components/management/bingo/boards/board-card/board-card.component';
import { EditBoardCardComponent } from './components/management/bingo/boards/edit-board-card/edit-board-card.component';
import { BingoTileAddEditComponent } from './components/management/bingo/tiles/bingo-tile-add-edit.component';
import { EditTileCardComponent } from './components/management/bingo/tiles/edit-tile-card/edit-tile-card.component';
import { TileCardComponent } from './components/management/bingo/tiles/tile-card/tile-card.component';
import { FormInputComponent } from './shared/components/form/form-input/form-input.component';
import { FormTextAreaComponent } from './shared/components/form/form-textarea/form-textarea.component';
import { LayoutComponent } from './shared/components/layout/layout.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';
import { ToastContainerComponent } from './shared/components/toast/toast-container.component';
import { AppInitializer } from './shared/middleware/app.initializer';
import { TokenInterceptor } from './shared/middleware/token.interceptor';
import { SafeHtmlPipe } from './shared/safe-html.pipe';

const httpInterceptors: Provider = [
  { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
];
export function RunInitialization(appInitilaizer: AppInitializer): () => void {
  return () => appInitilaizer.initialize();
}
const appInitializers: Provider = [
  { provide: APP_INITIALIZER, useFactory: RunInitialization, deps: [AppInitializer], multi: true }
];

@NgModule({
  declarations: [
    //#region PAGE COMPONENTS

    LoginComponent,
    DashboardComponent,

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

    //#endregion

    //#region SHARED COMPONENTS

    LayoutComponent,
    NavMenuComponent,
    ToastContainerComponent,
    FormInputComponent,
    FormTextAreaComponent,

    //#endregion

    //#region SHARED PIPES

    SafeHtmlPipe

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
