import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserLoginDTO } from '../../../DTOs/Account/UserLoginDTO';
import { Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @ViewChild('error') private error: SwalComponent;
  public loading = false;
  public IsRegisteredNow: boolean = false;
  public LoginForm: FormGroup;

  constructor(private authService: AuthServiceService, private route: Router, private CookieService: CookieService) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(res => {
      if (res !== null) {
        this.route.navigate(["index"]);
      }
    });


    //delay
    setInterval(function () {
      if (document.getElementById("loginForm") !== null) {
        document.getElementById("loginForm").style.display = "block";
      }
    }, 2500);

    this.IsRegisteredNow = this.authService.getAlertOfNewRegister();

    this.LoginForm = new FormGroup({
      email: new FormControl,
      password: new FormControl
    });
  }


  loginSubmit() {
    this.loading = true;
    const userLoginData = new UserLoginDTO(
      this.LoginForm.controls.email.value,
      this.LoginForm.controls.password.value
    );

    this.authService.LoginUser(userLoginData).subscribe(res => {
      if (res.status === "Success") {
        this.CookieService.set("BBKBootcampCookie", res.data.token, res.data.expireTime * 60);
        this.loading = false;
        
        if (res.data.notifications === null || res.data.notifications === undefined)
          res.data.notifications = []

        this.authService.setCurrentUser(res.data);  
        
        this.route.navigate(["/index"]);
      } else if (res.status === "NotFound") {
        this.loading = false;
        this.error.title = "la informacion no es valido";
        this.error.fire();
      } else if (res.status === "Error") {
        this.loading = false;
        this.error.title = "Activa tu cuenta!";
        this.error.fire();
      }
    });
  }


}
