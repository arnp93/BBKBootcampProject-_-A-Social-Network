import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthServiceService } from '../Services/auth-service.service';

@Injectable({
    providedIn: 'root'
  })
export class UserAuthGuard implements CanActivate {
    constructor(private authService: AuthServiceService, private router: Router) { }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        this.authService.isAuthenticated().then(res => {
            if (res) {
                return true;
            } else {
               this.router.navigate(['']);
            }
        });
        return true;
    }


}