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
import { ResponsiveMenuComponent } from './SharedComponents/responsive-menu/responsive-menu.component';
import { FooterComponent } from './SharedComponents/footer/footer.component';
import { LoginErrorComponent } from './Pages/login-error/login-error.component';
import { NotFoundComponent } from './Pages/not-found/not-found.component';
import { CookieService } from 'ngx-cookie-service';
import { ShortcutsComponent } from './SharedComponents/leftSideBar/shortcuts/shortcuts.component';
import { RecentActivitiesComponent } from './SharedComponents/leftSideBar/recent-activities/recent-activities.component';
import { WhosFollowingComponent } from './SharedComponents/leftSideBar/whos-following/whos-following.component';
import { MenuComponent } from './SharedComponents/normalMenu/menu/menu.component';
import { AddNewPostComponent } from './Pages/Index/add-new-post/add-new-post.component';
import { UserPostsComponent } from './Pages/Index/user-posts/user-posts.component';
import { RightSideBarComponent } from './SharedComponents/right-side-bar/right-side-bar.component';
import { SettingsComponent } from './SharedComponents/settings/settings.component';
import { PostCommentComponent } from './Pages/Index/post-comment/post-comment.component';
import { NewFeedsComponent } from './Pages/NewsFeed/new-feeds/new-feeds.component';
import { AllUsersPostsComponent } from './Pages/NewsFeed/all-users-posts/all-users-posts.component';
import { IndexComponent } from './Pages/Index/index/index.component';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    IndexComponent,
    ResponsiveMenuComponent,
    FooterComponent,
    LoginErrorComponent,
    NotFoundComponent,
    ShortcutsComponent,
    RecentActivitiesComponent,
    WhosFollowingComponent,
    MenuComponent,
    AddNewPostComponent,
    UserPostsComponent,
    RightSideBarComponent,
    SettingsComponent,
    PostCommentComponent,
    NewFeedsComponent,
    AllUsersPostsComponent
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
