import { Component, OnInit } from '@angular/core';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { AuthServiceService } from 'src/app/Services/auth-service.service';
import { DomainName } from 'src/app/Utilities/PathTools';
import { Router } from '@angular/router';
import { NotificationDTO } from '../../../DTOs/Notification/NotificationDTO';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  public URL: string = DomainName;
  public thisUser: UserDTO;
  public date: number = parseInt(Date.now.toString());
  public unreadNotifications: NotificationDTO[] = [];
  public lastNotifications: NotificationDTO[] = [];
  public accepted: boolean = false;

  constructor(private authService: AuthServiceService, private router: Router, private cookieService: CookieService) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
      
    });

    if (this.thisUser !== undefined && this.thisUser !== null && this.thisUser.notifications !== undefined && this.thisUser.notifications !== null)
      this.unreadNotifications = this.thisUser.notifications.filter(n => !n.isRead && !n.isDelete && n.userDestinationId === this.thisUser.userId && n.userOriginId !== this.thisUser.userId)
    if (this.thisUser !== undefined && this.thisUser !== null)
      this.lastNotifications = this.thisUser.notifications.filter(n => n.isRead);

  }

  goToProfile() {
    this.router.navigate(["/index"]);
  }

  viewProfile(userId: number) {
    this.router.navigate(['/view-profile', userId])
  }

  friendAcc(userOriginId: number) {
    this.authService.acceptFriendRequest(userOriginId).subscribe(res => {
      if (res.status === "Success") {
        this.accepted = true;
      }

    });
  }

  logout() {
    this.cookieService.delete("BBKBootcampCookie");
    this.authService.setCurrentUser(null);
    this.authService.logOut();
    this.router.navigate([""]);
  }

  removeNotification(notificationId: number) {
    this.authService.deleteNotification(notificationId).subscribe(res => {
      if (res.status === "Success"){
        this.thisUser.notifications = this.unreadNotifications.filter(n => n.id !== notificationId);
        this.unreadNotifications = this.unreadNotifications.filter(n => n.id !== notificationId);
      }
    });
  }
}
