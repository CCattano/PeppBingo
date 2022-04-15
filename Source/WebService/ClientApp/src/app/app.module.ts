import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LayoutComponent } from './shared/components/layout/layout.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';


@NgModule({
  declarations: [
    //#region PAGE COMPONENTS
    DashboardComponent,
    //#endregion

    //#region SHARED COMPONENTS
    LayoutComponent,
    NavMenuComponent,
    //#endregion
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [LayoutComponent]
})
export class AppModule { }
