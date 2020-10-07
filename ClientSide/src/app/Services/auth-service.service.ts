import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterUserDTO } from '../DTOs/Account/RegisterUserDTO';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {

  constructor(private http: HttpClient) { }

  RegisterUser(user : RegisterUserDTO): Observable<RegisterUserDTO>{
    return this.http.post<RegisterUserDTO>('/api/account', user);
  }
}
