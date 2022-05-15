import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { BingoGameComponent } from './components/game/bingo-game.component';
import { LoginComponent } from './components/login/login.component';
import { AddEditAdminComponent } from './components/management/admin/admin-add-edit.component';
import { BingoBoardAddEditComponent } from './components/management/bingo/boards/bingo-board-add-edit.component';
import { BingoTileAddEditComponent } from './components/management/bingo/tiles/bingo-tile-add-edit.component';
import { LiveControlsComponent } from './components/management/live-controls/live-controls.component';
import { AdminAuthGuard } from './shared/middleware/authguards/admin.authguard';
import { CommonAuthGuard } from './shared/middleware/authguards/common.authguard';
import {LeaderboardStandingsComponent} from './components/leaderboard/standings/leaderboard-standings.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
  { path: 'login', component: LoginComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
  { path: 'admin/mods/add-edit', pathMatch: 'full', component: AddEditAdminComponent, canActivate: [AdminAuthGuard] },
  { path: 'admin/bingo/add-edit', pathMatch: 'full', component: BingoBoardAddEditComponent, canActivate: [AdminAuthGuard] },
  { path: 'admin/bingo/board/:boardID/tiles/add-edit', pathMatch: 'full', component: BingoTileAddEditComponent, canActivate: [AdminAuthGuard] },
  { path: 'admin/bingo/live-controls', pathMatch: 'full', component: LiveControlsComponent, canActivate: [AdminAuthGuard] },
  { path: 'game/bingo/play', pathMatch: 'full', component: BingoGameComponent, canActivate: [CommonAuthGuard] },
  { path: 'leaderboards', pathMatch: 'full', component: LeaderboardStandingsComponent, canActivate: [CommonAuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
