import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { CommonAuthGuard } from './shared/middleware/common.authguard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: "dashboard" },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
  { path: 'login', component: LoginComponent, pathMatch: 'full', canActivate: [CommonAuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
