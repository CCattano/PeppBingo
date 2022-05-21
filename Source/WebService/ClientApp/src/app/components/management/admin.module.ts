import {NgModule} from '@angular/core';
import {AddEditAdminComponent} from './admin/admin-add-edit.component';
import {BingoBoardAddEditComponent} from './bingo/boards/bingo-board-add-edit.component';
import {BoardCardComponent} from './bingo/boards/board-card/board-card.component';
import {EditBoardCardComponent} from './bingo/boards/edit-board-card/edit-board-card.component';
import {BingoTileAddEditComponent} from './bingo/tiles/bingo-tile-add-edit.component';
import {EditTileCardComponent} from './bingo/tiles/edit-tile-card/edit-tile-card.component';
import {TileCardComponent} from './bingo/tiles/tile-card/tile-card.component';
import {LiveControlsComponent} from './live-controls/live-controls.component';
import {FormInputComponent} from '../../shared/components/form/form-input/form-input.component';
import {FormTextAreaComponent} from '../../shared/components/form/form-textarea/form-textarea.component';
import {SafeHtmlPipe} from '../../shared/safe-html.pipe';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {AdminAuthGuard} from '../../shared/middleware/authguards/admin.authguard';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgToggleModule} from '@nth-cloud/ng-toggle';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

const routes: Routes = [
  { path: 'mods/add-edit', pathMatch: 'full', component: AddEditAdminComponent, canActivate: [AdminAuthGuard] },
  { path: 'bingo/add-edit', pathMatch: 'full', component: BingoBoardAddEditComponent, canActivate: [AdminAuthGuard] },
  { path: 'bingo/board/:boardID/tiles/add-edit', pathMatch: 'full', component: BingoTileAddEditComponent, canActivate: [AdminAuthGuard] },
  { path: 'bingo/live-controls', pathMatch: 'full', component: LiveControlsComponent, canActivate: [AdminAuthGuard] },
];

@NgModule({
  declarations: [
    //#region PAGE COMPONENTS

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

    //#endregion

    //#region SHARED COMPONENTS

    FormInputComponent,
    FormTextAreaComponent,

    //#endregion

    //#region SHARED PIPES

    SafeHtmlPipe,

    //#endregion
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    FontAwesomeModule,
    NgToggleModule
  ],
})
export class AdminModule {

}
