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
  public editForm : FormGroup = null;
  public selectProfilePic: File = null;
  public newProfilePic: string = null;
  public isLoading = false;
  @ViewChild('profilePicError') private emailExist: SwalComponent;
  constructor(private authService : AuthServiceService, private route:Router,private postService : PostService) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });

    this.editForm = new FormGroup({
      firstName: new FormControl(this.thisUser.firstName, [
        Validators.minLength(1),
        Validators.required
      ]),
      lastName: new FormControl(this.thisUser.lastName, [
        Validators.minLength(1),
        Validators.required
      ]),
       phoneNumber: new FormControl(this.thisUser.phoneNumber, [
        Validators.minLength(1),
        Validators.required
      ]),  
      birthDay: new FormControl(this.thisUser.birthDay, [
        Validators.minLength(1),
        Validators.required
      ]),
      address: new FormControl(this.thisUser.address, [
        Validators.minLength(1),
        Validators.required
      ]),
      about: new FormControl(this.thisUser.about, [
        Validators.minLength(1),
        Validators.required
      ]),
      gender: new FormControl(this.thisUser.gender, [
        Validators.minLength(1),
        Validators.required
      ]),
      isPrivate: new FormControl(this.thisUser.isPrivate, [
        Validators.minLength(1),
        Validators.required
      ])
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

}
