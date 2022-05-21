import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {BingoGameComponent} from './components/game/bingo-game.component';
import {LoginComponent} from './components/login/login.component';
import {CommonAuthGuard} from './shared/middleware/authguards/common.authguard';
import {LeaderboardStandingsComponent} from './components/leaderboard/standings/leaderboard-standings.component';
import {AboutComponent} from './components/about/about.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'play' },
  { path: 'login', component: LoginComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
  { path: 'about', component: AboutComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
  { path: 'admin', loadChildren: () => import('./components/management/admin.module').then(m => m.AdminModule) },
  { path: 'play', pathMatch: 'full', component: BingoGameComponent, canActivate: [CommonAuthGuard] },
  { path: 'leaderboards', pathMatch: 'full', component: LeaderboardStandingsComponent, canActivate: [CommonAuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
