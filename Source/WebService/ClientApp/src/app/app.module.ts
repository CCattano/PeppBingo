import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule, Provider } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { AddEditAdminComponent } from './components/management/admin/admin-add-edit.component';
import { LayoutComponent } from './shared/components/layout/layout.component';
import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';
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
    AddEditAdminComponent,
    //#endregion

    //#region SHARED COMPONENTS
    LayoutComponent,
    NavMenuComponent,
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
    NgbModule
  ],
  providers: [
    //#region Interceptors
    httpInterceptors,
    //#endregion

    //#region Initializers
    appInitializers
    //#endregion
  ],
  bootstrap: [LayoutComponent]
})
export class AppModule { }
