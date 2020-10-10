import { Component, OnInit } from '@angular/core';
import { AuthServiceService } from './Services/auth-service.service';
import { UserDTO } from './DTOs/Account/UserDTO';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
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
   }

}
