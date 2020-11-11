import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './Pages/Register/register.component';
import { LoginComponent } from './Pages/Login/login/login.component';
import { IndexComponent } from './Pages/Index/index/index.component';
import { LoginErrorComponent } from './Pages/login-error/login-error.component';
import { NotFoundComponent } from './Pages/not-found/not-found.component';
import { NewFeedsComponent } from './Pages/NewsFeed/new-feeds/new-feeds.component';
import { ViewProfileComponent } from './Pages/view-profile/view-profile.component';
import { FriendsPostsComponent } from './Pages/friends-posts/friends-posts.component';
import { EditProfileComponent } from './Pages/edit-profile/edit-profile.component';
import { UserAuthGuard } from './Utilities/UserAuthGuard';
import { SecurityComponent } from './Pages/security/security.component';
import { HashtagPostsMainPageComponent } from './Pages/hashtag-posts/hashtag-posts-main-page/hashtag-posts-main-page.component';

const appRoutes:Routes = [
    {path: 'register', component: RegisterComponent},
    {path: '', component: LoginComponent},
    {path: 'index', component: IndexComponent, canActivate: [UserAuthGuard]},
    {path: 'login-error', component: LoginErrorComponent},
    {path: 'active-error', component: NotFoundComponent},
    {path: 'news-feed', component: NewFeedsComponent, canActivate: [UserAuthGuard]},
    {path: 'view-profile/:userId', component: ViewProfileComponent, canActivate: [UserAuthGuard]},
    {path: 'friends-posts', component: FriendsPostsComponent, canActivate: [UserAuthGuard]},
    {path: 'edit-profile', component: EditProfileComponent, canActivate: [UserAuthGuard]},
    {path: 'security', component: SecurityComponent, canActivate: [UserAuthGuard]},
    {path: 'hashtag-posts', component: HashtagPostsMainPageComponent, canActivate: [UserAuthGuard]},
    {path: '**', component: NotFoundComponent}
]

@NgModule({
    imports: [
      RouterModule.forRoot(appRoutes)
    ],
    exports: [RouterModule]
  })
export class AppRoutingModule{

}