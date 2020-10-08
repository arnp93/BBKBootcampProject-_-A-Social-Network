import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './Pages/Register/register.component';
import { LoginComponent } from './Pages/Login/login/login.component';

const appRoutes:Routes = [
    {path: 'register', component: RegisterComponent},
    {path: '', component: LoginComponent}

]

@NgModule({
    imports: [
      RouterModule.forRoot(appRoutes)
    ],
    exports: [RouterModule]
  })
export class AppRoutingModule{

}