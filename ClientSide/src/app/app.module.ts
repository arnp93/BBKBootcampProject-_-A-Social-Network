import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { AppComponent } from './app.component';
import { RegisterComponent } from './Pages/Register/register.component';
import { AuthServiceService } from './Services/auth-service.service';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Interceptor } from './Utilities/Interceptor';
import { LoginComponent } from './Pages/Login/login/login.component';
import { NgxLoadingModule } from 'ngx-loading';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { IndexComponent } from './Pages/Index/index/index.component';
import { ResponsiveMenuComponent } from './SharedComponents/responsive-menu/responsive-menu.component';
import { FooterComponent } from './SharedComponents/footer/footer.component';
import { LoginErrorComponent } from './Pages/login-error/login-error.component';
import { NotFoundComponent } from './Pages/not-found/not-found.component';
import { CookieService } from 'ngx-cookie-service';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    IndexComponent,
    ResponsiveMenuComponent,
    FooterComponent,
    LoginErrorComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgxLoadingModule.forRoot({}),
    [SweetAlert2Module.forRoot()],
  ],
  providers: [
    AuthServiceService,
    CookieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: Interceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
