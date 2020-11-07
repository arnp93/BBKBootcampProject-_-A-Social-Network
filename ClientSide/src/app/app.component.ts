import { Component, OnDestroy } from '@angular/core';
import { AuthServiceService } from './Services/auth-service.service';
import { UserDTO } from './DTOs/Account/UserDTO';
import { SignalRServiceService } from './Services/signal-rservice.service';


declare function loadPage();

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent implements OnDestroy {
  title = 'BBKBootcamp';

  constructor(private authService: AuthServiceService, private signalrService: SignalRServiceService) { }

  ngOnInit(): void {

    loadPage();
    this.authService.checkAuth().subscribe(res => {
      if (res.status === 'Success') {
        const currentUser = new UserDTO(res.data.token, res.data.expireTime, res.data.firstName, res.data.lastName, res.data.profilePic, res.data.coverPic, res.data.userId, null, res.data.notifications, res.data.friends);
        this.signalrService.startConnection();
        setTimeout(() => {
         
          // this.signalrService.askServerListener();
          this.signalrService.askServer(res.data.userId);
        }, 1500);
        this.authService.setCurrentUser(currentUser);
      }
    });


  }

  ngOnDestroy(): void {
    this.signalrService.hubConnection.off("askServerResponse");
  }


}
