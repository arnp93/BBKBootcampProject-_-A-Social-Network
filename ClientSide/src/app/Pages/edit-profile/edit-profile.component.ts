import { Component, OnInit, ViewChild } from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { AuthServiceService } from '../../Services/auth-service.service';
import { Router } from '@angular/router';
import { PostService } from '../../Services/post.service';
import { DomainName } from 'src/app/Utilities/PathTools';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  public URL: string = DomainName;
  public thisUser: UserDTO;
  public selectedCoverPicture: File = null;
  public editForm: FormGroup;
  public selectProfilePic: File = null;
  public newProfilePic: string = null;
  public isLoading = false;
  
  @ViewChild('profilePicError') private emailExist: SwalComponent;
  @ViewChild('success') private success: SwalComponent;
  @ViewChild('error') private error: SwalComponent;

  constructor(private authService: AuthServiceService, private route: Router, private postService: PostService) { }

  ngOnInit(): void {

    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });

    this.editForm = new FormGroup({
      firstName: new FormControl(this.thisUser.firstName),
      lastName: new FormControl(this.thisUser.lastName),
      phoneNumber: new FormControl(this.thisUser.phoneNumber),
      birthdayDay: new FormControl(null),
      birthdayMonth: new FormControl(),
      birthdayYear: new FormControl(),
      address: new FormControl(),
      city: new FormControl(this.thisUser.address),
      country: new FormControl(this.thisUser.address),
      about: new FormControl(this.thisUser.about),
      gender: new FormControl(this.thisUser.gender),
      isPrivate: new FormControl(this.thisUser.isPrivate)
    });
  }



  addPic(event) {
    this.isLoading = true;
    this.selectProfilePic = <File>event.target.files[0];
    this.uploadPic();
    this.isLoading = false;
  }
  addCoverPic(event) {
    this.isLoading = true;
    this.selectedCoverPicture = <File>event.target.files[0];
    this.uploadNewCoverPic();
    this.isLoading = false;
  }

  uploadPic() {
    const formData: FormData = new FormData();
    if (this.selectProfilePic != null)
      formData.append("pic", this.selectProfilePic, this.selectProfilePic.name);

    this.postService.newProfilePicture(formData).subscribe(res => {
      if (res.status === "Success") {
        this.newProfilePic = res.data;
        this.thisUser.profilePic = res.data;
      } else {
        this.emailExist.text = "Error! Please try again later";
        this.emailExist.fire();
      }
    });
  }

  uploadNewCoverPic() {
    const formData = new FormData();
    if (this.selectedCoverPicture !== null)
      formData.append("pic", this.selectedCoverPicture, this.selectedCoverPicture.name);

    this.postService.newCoverPicture(formData).subscribe(res => {
      if (res.status === "Success") {
        this.thisUser.coverPic = res.data;
      } else {
        this.emailExist.text = "Error! Please try again later";
        this.emailExist.fire();

      }
    });
  }

  editSubmit() {
    console.log(this.editForm);
    const editedUserInfo = new UserDTO(
      null, 0, this.editForm.controls.firstName.value, this.editForm.controls.lastName.value, this.editForm.controls.phoneNumber.value,
      this.editForm.controls.birthdayDay.value + " " + this.editForm.controls.birthdayMonth.value + " " + this.editForm.controls.birthdayYear.value,
      this.editForm.controls.address.value,
      this.editForm.controls.about.value,
      this.editForm.controls.isPrivate.value,
      this.editForm.controls.gender.value,
      null, null, 0, null, null, null
    );
    this.authService.editUser(editedUserInfo).subscribe(res => {
      if(res.status === "Success"){
        this.success.fire();
      }else{
        this.error.fire();
      }
    })
  }


}
