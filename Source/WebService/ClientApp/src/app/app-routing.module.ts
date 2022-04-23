import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { AddEditAdminComponent } from './components/management/admin/admin-add-edit.component';
import { BingoBoardAddEditComponent } from './components/management/bingo/boards/bingo-board-add-edit.component';
import { AdminAuthGuard } from './shared/middleware/authguards/admin.authguard';
import { CommonAuthGuard } from './shared/middleware/authguards/common.authguard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
  { path: 'login', component: LoginComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
  { path: 'admin/mods/add-edit', pathMatch: 'full', component: AddEditAdminComponent, canActivate: [AdminAuthGuard] },
  { path: 'admin/bingo/add-edit', pathMatch: 'full', component: BingoBoardAddEditComponent, canActivate: [AdminAuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
