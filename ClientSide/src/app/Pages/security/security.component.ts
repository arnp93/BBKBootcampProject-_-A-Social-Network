import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { AuthServiceService } from '../../Services/auth-service.service';
import { ChangePasswordDTO } from '../../DTOs/Account/ChangePasswordDTO';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-security',
  templateUrl: './security.component.html',
  styleUrls: ['./security.component.css']
})
export class SecurityComponent implements OnInit, AfterViewInit {
  public securityForm: FormGroup;
  public thisUser: UserDTO;

  @ViewChild('error') private error: SwalComponent;
  @ViewChild('success') private success: SwalComponent;

  constructor(private authService: AuthServiceService) { }

  ngOnInit(): void {

    window.scroll(0,0);

    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });

    this.securityForm = new FormGroup({
      password: new FormControl(null, [
        Validators.minLength(9)
      ]),
      rePassword: new FormControl(null, [
        Validators.minLength(9)
      ]),
      isPrivate: new FormControl(this.thisUser.isPrivate)
    });

  }

  ngAfterViewInit(): void {
    if (this.thisUser.isPrivate) {
      var isPrivateElement = document.getElementById("isPrivate");
      isPrivateElement.setAttribute("value", "false")
    } else {
      var isPrivateElement = document.getElementById("isPrivate");
      isPrivateElement.setAttribute("value", "true")
    }

  }
  changeSecutiryDetails() {
    console.log(this.securityForm);

    const changePassword = new ChangePasswordDTO(
      this.securityForm.controls.password.value,
      this.securityForm.controls.rePassword.value,
      this.securityForm.controls.isPrivate.value
    );
    if (changePassword.password !== changePassword.rePassword) {
      this.error.text = "Passwords are not the same. Please try again";
      this.error.fire();
    } else {
      this.authService.changeSecurityDetails(changePassword).subscribe(res => {
        if (res.status === "Success") {
          this.success.fire();
          this.securityForm.reset();
          this.thisUser.isPrivate = changePassword.isPrivate;
          this.securityForm.controls.isPrivate.setValue(changePassword.isPrivate);
        } else {
          this.error.text = "Error! Please try again later!";
          this.error.fire();
        }
      });
    }
  }

}
