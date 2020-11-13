import { Component, NgZone, OnInit } from '@angular/core';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { AuthServiceService } from 'src/app/Services/auth-service.service';
import { DomainName } from 'src/app/Utilities/PathTools';
import { Router } from '@angular/router';
import { NotificationDTO } from '../../../DTOs/Notification/NotificationDTO';
import { CookieService } from 'ngx-cookie-service';
// import { SignalRServiceService } from '../../../Services/signal-rservice.service';
import { FriendDTO } from '../../../DTOs/Account/FriendDTO';

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
  public newNotification: NotificationDTO = null;

  constructor(private ngZone: NgZone,private authService: AuthServiceService, private router: Router, private cookieService: CookieService) { }

  ngOnInit(): void {
    window['signalFriendRequestAccept'] = { component: this, zone: this.ngZone, acceptRequestFunction: (friendId) => this.friendAcc(friendId) }; 
    window['signalViewProfile'] = { component: this, zone: this.ngZone, viewProfileFunction: (userId) => this.viewProfile(userId) }; 
   
   
   
   
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
        this.authService.userProfile(userOriginId).subscribe(res => {
          this.thisUser.friends.push(new FriendDTO(res.data.userId,res.data.about + " " + res.data.lastName,res.data.profilePic));
        });
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
      if (res.status === "Success") {
        this.thisUser.notifications = this.unreadNotifications.filter(n => n.id !== notificationId);
        this.unreadNotifications = this.unreadNotifications.filter(n => n.id !== notificationId);
      }
    });
  }
}
