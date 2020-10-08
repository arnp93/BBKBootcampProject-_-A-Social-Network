import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
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
  setAlertOfNewRegister(){
    this.IsRegisteredNow = true;
  }
  getAlertOfNewRegister():boolean{
    return this.IsRegisteredNow;
  }
  LoginUser(login :UserLoginDTO): Observable<any>{
    return this.http.post<any>('/api/account/login',login);
  }
}
