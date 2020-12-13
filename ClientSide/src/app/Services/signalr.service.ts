import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { NotificationDTO } from '../DTOs/Notification/NotificationDTO';
import { SignalRFriendRequestResponse } from '../DTOs/SignalR/SignalRResponse';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  public hubConnection: signalR.HubConnection;
  ngZone: any;

  constructor() { }



  startConnection() {

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:44317/notificationHub', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      }).build();

    this.hubConnection.on("AddFriendRequest", this.AddFriendRequest);
    this.hubConnection.on("NewComment", this.NewComment);
    this.hubConnection.start().then(function () {
      console.log('Hub Connection Started : alert from signalR Hub Service (ClientSide)');
    }).catch(err => console.log('Error while starting connection: ' + err))
  }

  //save connection of user in database
  askServer(userId: number) {
    this.hubConnection.invoke('SaveConnection', userId).catch(err => console.log(err));
  }

  AddFriendRequest(notification: SignalRFriendRequestResponse) {
    let innerHtmlOfMenu = document.getElementById("notifications").innerHTML;
    var date = new Date();
    var newNotification = ` <li *ngFor="let notification of unreadNotifications">
    <a *ngIf="thisUser.notifications.length > 0">
        <img width="40" height="40" src="https://localhost:44317/ProfilePictures/${notification.imageName}"
            alt="">
        <div class="mesg-meta">
            <h6 onclick="viewProfile(${notification.userOriginId})" class="link">
                ${notification.firstName} ${notification.lastName}</h6>
            <span>${notification.message}</span>
            <i>${date.getFullYear().toString() + "-" + date.getMonth().toString() + "-" + date.getDate().toString() + "<br />" + date.getHours().toString() + ":" + date.getMinutes().toString()}</i>
            <div>
                <span>
                    <input class="btn-xs btn-success small btn-margin"
                        type="button" value="Accept" onclick="signalFriendAcc(event,${notification.userOriginId})">
                </span>
                <input class="btn-xs btn-warning small btn-margin" type="button" value="Remove"
                    onclick="removeNotification(${notification.id})">
            </div>
        </div>
    </a>
    <span class="tag green">New</span>
</li>`


    document.getElementById("notifications").innerHTML = innerHtmlOfMenu + newNotification;


    if(document.getElementById("notification-count") !== null){
      var count = parseInt(document.getElementById("notification-count").innerHTML);
      if (count !== null || count !== undefined || count !== NaN) {
        count = count + 1;
      } else {
        count = 1;
      }
      document.getElementById("notification-count").innerHTML = count.toString();
    }
    document.getElementById("notifyFriendRequest").innerHTML = notification.firstName + " " + notification.lastName + "-" + notification.message;
    document.getElementById("notifyFriendRequest").click();
  
  }

  NewComment(notification: NotificationDTO) {
    let innerHtmlOfMenu = document.getElementById("notifications").innerHTML;
    var newNotification = ` <li *ngFor="let notification of unreadNotifications">
    <a *ngIf="thisUser.notifications.length > 0">
        <img width="40" height="40" src="https://localhost:44317/ProfilePictures/${notification.user.profilePic}"
            alt="">
        <div class="mesg-meta">
            <h6 onclick="viewProfile(${notification.user.userId})" class="link">
                ${notification.user.firstName} ${notification.user.lastName}</h6>
            <span>${notification.message}</span>
            <i>${notification.createDate}</i>
            <div>
                <span>
                    <input class="btn-xs btn-success small btn-margin"
                        type="button" value="View" onclick="viewSinglePost(${notification.postId})">
                </span>
                <input class="btn-xs btn-warning small btn-margin" type="button" value="Remove"
                    onclick="removeNotification(${notification.id})">
            </div>
        </div>
    </a>
    <span class="tag green">New</span>
</li>`

    document.getElementById("notifications").innerHTML = newNotification + innerHtmlOfMenu;

    if(document.getElementById("notification-count") !== null){
      var count = parseInt(document.getElementById("notification-count").innerHTML);
      if (count !== null || count !== undefined || count !== NaN) {
        count = count + 1;
      } else {
        count = 1;
      }
      document.getElementById("notification-count").innerHTML = count.toString();
    }
    
    document.getElementById("notifyComment").innerHTML = notification.user.firstName + " " + notification.user.lastName + "-" + notification.message;
    document.getElementById("notifyComment").click();
  }
}
