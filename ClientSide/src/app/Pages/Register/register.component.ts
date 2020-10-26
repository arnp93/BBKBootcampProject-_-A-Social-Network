import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthServiceService } from '../../Services/auth-service.service';
import { RegisterUserDTO } from '../../DTOs/Account/RegisterUserDTO';
import { Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @ViewChild('emailExist') private emailExist: SwalComponent;

  public activeEmailMsg: string = null;
  public loading = false;
  public registerForm: FormGroup;
  constructor(private authService: AuthServiceService, private route: Router) { }

  ngOnInit(): void {
    var user = this.authService.getCurrentUser()
      if (user != null) {
        this.route.navigate(["index"]);
      }
  
    //delay
    setInterval(function () {
      document.getElementById("registerForm").style.display = "block";
    }, 1500);

    //Form Group
    this.registerForm = new FormGroup({
      firstName: new FormControl(null,
        [
          Validators.required,
          Validators.maxLength(50)
        ]),
      lastName: new FormControl(null,
        [
          Validators.required,
          Validators.maxLength(50)
        ]),
      userName: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(100)
        ]),
      email: new FormControl(null,
        [
          Validators.email,
          Validators.required
        ]),
      password: new FormControl(null, [
        Validators.required,
        Validators.minLength(9),
        Validators.maxLength(150)
      ]),
      rePassword: new FormControl(null, [
        Validators.required,
        Validators.minLength(9),
        Validators.maxLength(150)
      ])
    });
  }

  registerUser() {
    this.loading = true;
    const registerData = new RegisterUserDTO(
      this.registerForm.controls.firstName.value,
      this.registerForm.controls.lastName.value,
      this.registerForm.controls.email.value,
      this.registerForm.controls.password.value,
      this.registerForm.controls.rePassword.value
    );
    if (registerData.password === registerData.rePassword) {
      this.authService.RegisterUser(registerData).subscribe(res => {
        if (res.status === "Success") {
          this.authService.setAlertOfNewRegister();
          this.activeEmailMsg = "Te hemos enviado un email con el enlace de Activar tu cuenta"
          this.loading = false;
        } else {
          this.loading = false;
          if (res.data.error === "Email Exist") {
            this.emailExist.title = "Este email ya esta Disponible!";
            this.emailExist.fire();
          }
        }
      });
    } else {
      this.loading = false;
      this.emailExist.title = "Las contraseñas no son iguales";
      this.emailExist.fire();
    }
  }

}
