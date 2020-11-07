import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { NotificationDTO } from '../DTOs/Notification/NotificationDTO';

@Injectable({
  providedIn: 'root'
})
export class SignalRServiceService {

  constructor() { }

  hubConnection:signalR.HubConnection = new signalR.HubConnectionBuilder()
  .withUrl('https://localhost:44317/notificationHub' , {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  }).build();

  startConnection(){

    this.hubConnection.on("AddFriendRequest",this.AddFriendRequest);
    this.hubConnection.start().then(function(){
      console.log('Hub Connection Started');
      
    }).catch(err => console.log('Error while starting connection: ' + err))
  }

  askServer(userId : number){
    this.hubConnection.invoke('SaveConnection', userId).catch(err => console.log(err));
  }

  sendNotificationOfAddFriend(userId : number){
    this.hubConnection.invoke("AddFriendNotification", userId).catch(err => console.log(err));
  }

  AddFriendRequest(notification:NotificationDTO){
    console.log(notification);
    
  }

  // askServerListener(){
  //   this.hubConnection.on("askServerResponse", (someText) => {
  //     console.log(someText);
  //   });
  // }
}
