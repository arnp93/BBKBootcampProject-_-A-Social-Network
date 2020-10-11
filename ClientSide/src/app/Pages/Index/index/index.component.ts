import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { AuthServiceService } from '../../../Services/auth-service.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  constructor(private authService: AuthServiceService, private route: Router) { }

  ngOnInit(): void {
    this.authService.checkAuth().subscribe(res => {
       if (res.status === 'Success') {
         const currentUser = new UserDTO(res.data.token,res.data.expireTime,res.data.firstName,res.data.lastName,res.data.userId);
         this.authService.setCurrentUser(currentUser);
      }
      if (res.status === "Error") {
        this.route.navigate(["login-error"]);
      }
    });

  }
}
