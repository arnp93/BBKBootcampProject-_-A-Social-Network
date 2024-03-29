import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ILoginUserAccount } from '../DTOs/Account/ILoginUserAccount';
import { RegisterUserDTO } from '../DTOs/Account/RegisterUserDTO';
import { UserDTO } from '../DTOs/Account/UserDTO';
import { UserLoginDTO } from '../DTOs/Account/UserLoginDTO';
import { IResponseDTO } from '../DTOs/Common/IResponseDTO';
import { ChangePasswordDTO } from '../DTOs/Account/ChangePasswordDTO';

@Injectable({
  providedIn: 'root'
})

export class AuthServiceService {

  private isLogin = false;

  private IsRegisteredNow: boolean;
  private currentUser: BehaviorSubject<UserDTO> = new BehaviorSubject<UserDTO>(null);

  constructor(private http: HttpClient) { }

  RegisterUser(user: RegisterUserDTO): Observable<any> {
    return this.http.post<RegisterUserDTO>('/api/account/register', user);
  }
  setAlertOfNewRegister(): void {
    this.IsRegisteredNow = true;
  }
  getAlertOfNewRegister(): boolean {
    return this.IsRegisteredNow;
  }
  LoginUser(login: UserLoginDTO): Observable<ILoginUserAccount> {
    return this.http.post<ILoginUserAccount>('/api/account/login', login);
  }

  setCurrentUser(user: UserDTO): void {
    this.currentUser.next(user);
    if (user !== null) {
      this.isLogin = true;
    } else {
      this.isLogin = false;
    }
  }

  isAuthenticated() {
    const promis = new Promise((resolve, reject) => {
      resolve(this.isLogin);
    });
    return promis;
  }

  getCurrentUser(): BehaviorSubject<UserDTO> {
    return this.currentUser;
  }

  checkAuth(): Observable<ILoginUserAccount> {
    return this.http.post<ILoginUserAccount>('/api/account/check-auth', null);
  }

  userProfile(userId: number): Observable<IResponseDTO<UserDTO>> {
    return this.http.get<IResponseDTO<UserDTO>>('/api/account/view-profile/' + userId);
  }

  friendRequest(userId: number): Observable<IResponseDTO<any>> {
    return this.http.post<IResponseDTO<any>>("/api/account/friend-request", userId);
  }

  logOut() {
    this.http.get<IResponseDTO<UserDTO>>('/api/account/sign-out');
  }

  acceptFriendRequest(origionUserId: number): Observable<IResponseDTO<UserDTO>> {
    return this.http.post<IResponseDTO<UserDTO>>("/api/account/accept-friend", origionUserId);
  }

  deleteNotification(notificationId: number): Observable<IResponseDTO<any>> {
    return this.http.post<IResponseDTO<any>>("/api/account/delete-notification", notificationId);
  }

  getLatestusers(): Observable<IResponseDTO<UserDTO[]>> {
    return this.http.get<IResponseDTO<UserDTO[]>>("/api/account/latest-users");
  }

  editUser(user : UserDTO) : Observable<IResponseDTO<UserDTO>>{
    return this.http.post<IResponseDTO<UserDTO>>("/api/account/edit-user", user);
  }

  changeSecurityDetails(securityDetails : ChangePasswordDTO) : Observable<IResponseDTO<any>>{
    return this.http.post<IResponseDTO<any>>("/api/account/edit-security-details", securityDetails);
  }

  deleteFriendRequest(destinationUserId : number) : Observable<IResponseDTO<any>>{
    return this.http.post<IResponseDTO<any>>("/api/account/delete-request", destinationUserId);

  }

}
