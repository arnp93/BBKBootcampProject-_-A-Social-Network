import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ILoginUserAccount } from '../DTOs/Account/ILoginUserAccount';
import { RegisterUserDTO } from '../DTOs/Account/RegisterUserDTO';
import { UserDTO } from '../DTOs/Account/UserDTO';
import { UserLoginDTO } from '../DTOs/Account/UserLoginDTO';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  private IsRegisteredNow : boolean;
  private currentUser: BehaviorSubject<UserDTO> = new BehaviorSubject<UserDTO>(null);
  constructor(private http: HttpClient) { }

  RegisterUser(user : RegisterUserDTO): Observable<any>{
    return this.http.post<RegisterUserDTO>('/api/account/register', user);
  }
  setAlertOfNewRegister():void{
    this.IsRegisteredNow = true;
  }
  getAlertOfNewRegister():boolean{
    return this.IsRegisteredNow;
  }
  LoginUser(login :UserLoginDTO): Observable<ILoginUserAccount>{
    return this.http.post<any>('/api/account/login',login);
  }

  setCurrentUser(user : UserDTO) : void{
    this.currentUser.next(user);
  }

  getCurrentUser():Observable<UserDTO> {
    return this.currentUser;
  }

  checkAuth():Observable<ILoginUserAccount>{
    return this.http.post<ILoginUserAccount>('/api/account/check-auth', null);
  }
}
