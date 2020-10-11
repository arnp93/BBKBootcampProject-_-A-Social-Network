import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './Pages/Register/register.component';
import { LoginComponent } from './Pages/Login/login/login.component';
import { IndexComponent } from './Pages/Index/index/index.component';
import { LoginErrorComponent } from './Pages/login-error/login-error.component';
import { NotFoundComponent } from './Pages/not-found/not-found.component';

const appRoutes:Routes = [
    {path: 'register', component: RegisterComponent},
    {path: '', component: LoginComponent},
    {path: 'index', component: IndexComponent},
    {path: 'login-error', component: LoginErrorComponent},
    {path: 'active-error', component: NotFoundComponent},
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