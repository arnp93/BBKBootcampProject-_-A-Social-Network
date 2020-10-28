import { Component, OnDestroy } from '@angular/core';
import { AuthServiceService } from './Services/auth-service.service';
import { UserDTO } from './DTOs/Account/UserDTO';
declare function loadPage();

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent  {
  title = 'BBKBootcamp';

  constructor(private authService: AuthServiceService) { }


  ngOnInit(): void {
    // this.authService.checkAuth().subscribe(res => {
    //   if (res.status === 'Success'){
    //     console.log("from app compo");

    //     const currentUser = new UserDTO(res.data.token,res.data.expireTime,res.data.firstName,res.data.lastName,res.data.userId);
    //     this.authService.setCurrentUser(currentUser);
    //   }
    //   });
    loadPage();
    this.authService.checkAuth().subscribe(res => {
      if (res.status === 'Success') {
        const currentUser = new UserDTO(res.data.token, res.data.expireTime, res.data.firstName, res.data.lastName, res.data.profilePic, res.data.userId, null, res.data.notifications, res.data.friends);
        this.authService.setCurrentUser(currentUser);
      }
    });
  }
}
