import { Component, OnDestroy } from '@angular/core';
import { AuthServiceService } from './Services/auth-service.service';
import { UserDTO } from './DTOs/Account/UserDTO';
// import { SignalRServiceService } from './Services/signal-rservice.service';
import { SignalrService } from './Services/signalr.service';


declare function loadPage();
// declare function addEventListenertoHashtagList();

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent implements OnDestroy {
  title = 'BBKBootcamp';

  constructor(private signalrService: SignalrService, private authService: AuthServiceService) { }

  ngOnInit(): void {
    // addEventListenertoHashtagList();
    loadPage();
    this.authService.checkAuth().subscribe(res => {
      if (res.status === 'Success') {
        const currentUser = new UserDTO(
          res.data.token, res.data.expireTime, res.data.firstName, res.data.lastName, res.data.phoneNumber
          , res.data.birthDay, res.data.address, res.data.about, res.data.isPrivate, res.data.gender, res.data.profilePic,
          res.data.coverPic, res.data.userId, res.data.posts, res.data.notifications, res.data.friends
        );
        this.authService.setCurrentUser(currentUser);
        this.signalrService.startConnection();
        setTimeout(() => {
          // this.signalrService.askServerListener();
          this.signalrService.askServer(res.data.userId);
        }, 1500);
      }
    });
  }

  ngOnDestroy(): void {
    // this.signalrService.hubConnection.off("askServerResponse");
  }
}
