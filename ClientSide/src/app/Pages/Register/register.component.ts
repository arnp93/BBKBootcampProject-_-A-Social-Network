import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { AuthServiceService } from '../../Services/auth-service.service';
import { RegisterUserDTO } from '../../DTOs/Account/RegisterUserDTO';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  public registerForm: FormGroup;
  constructor(private authService: AuthServiceService) { }

  ngOnInit(): void {
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
        password: new FormControl(null,[
          Validators.required,
          Validators.minLength(9),
          Validators.maxLength(150)
        ]),
        rePassword: new FormControl(null,[
          Validators.required,
          Validators.minLength(9),
          Validators.maxLength(150)
        ])
    });
  }

  registerUser(){
    const registerData = new RegisterUserDTO(
      this.registerForm.controls.firstName.value,
      this.registerForm.controls.lastName.value,
      this.registerForm.controls.userName.value,
      this.registerForm.controls.email.value,
      this.registerForm.controls.password.value,
      this.registerForm.controls.rePassword.value
    );
    this.authService.RegisterUser(registerData).subscribe(res =>{
      console.log(res);
    });
    
  }


}
