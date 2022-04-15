import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule, Provider } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { LayoutComponent } from './shared/components/layout/layout.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';
import { AppInitializer } from './shared/middleware/app.initializer';


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
    //#endregion

    //#region SHARED COMPONENTS
    LayoutComponent,
    NavMenuComponent,
    //#endregion
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    //#region Initializers
    AppInitializer,
    appInitializers
    //#endregion
  ],
  bootstrap: [LayoutComponent]
})
export class AppModule { }
