import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { ShowPostDTO } from 'src/app/DTOs/Post/ShowPostDTO';
import { PostService } from 'src/app/Services/post.service';
import { DomainName } from 'src/app/Utilities/PathTools';
import { AuthServiceService } from '../../../Services/auth-service.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})

export class IndexComponent implements OnInit {

  public lastPostId: number = 0;
  private currentPage: number = 1;
  public URL: string = DomainName;
  public thisUser: UserDTO;
  public newPosts: ShowPostDTO[] = [];
  public posts: ShowPostDTO[] = [];
  public selectProfilePic: File = null;
  public newProfilePic: string = null;
  public isLoading = false;

  @ViewChild('profilePicError') private emailExist: SwalComponent;

  constructor(private authService: AuthServiceService, private postService: PostService, private route: Router) { }

  ngOnInit(): void {

    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
      if(res === null){
        this.route.navigate([""]);
      }
    })
    
    this.authService.checkAuth().subscribe(res => {
      if (res.status === 'Success') {
        const currentUser = new UserDTO(res.data.token, res.data.expireTime, res.data.firstName, res.data.lastName, res.data.profilePic, res.data.userId,null,res.data.notifications,res.data.friends);
        this.authService.setCurrentUser(currentUser);
        this.thisUser = currentUser;
      }
      if (res.status === "Error") {
        this.authService.setCurrentUser(null);
        this.route.navigate(["login-error"]);
      }
    });
  }

  getNewPost(event: ShowPostDTO) {
    this.newPosts.push(event);
  }

  //Find Element in the page
  findPos(obj) {
    var curtop = 0;
    if (obj.offsetParent) {
      do {
        curtop += obj.offsetTop;
      } while (obj === obj.offsetParent);
      return [curtop];
    }
  }

  loadMorePosts() {
    var num = this.findPos(document.getElementById(this.lastPostId.toString()));

    this.postService.getMorePosts(this.currentPage).subscribe(res => {
      if (res.status === "Success") {

        res.data.forEach(post => {
          this.posts.push(post);
        });
        if (this.currentPage === 1) {
          window.scroll(0, num[0]);
        } else {
          var num2 = this.findPos(document.getElementById(this.posts[this.posts.length - (res.data.length + 1)].id.toString()));
          window.scroll(0, num2[0] + 120);
        }
        this.currentPage = this.currentPage + 1;
      }
    });
  }

  addPic(event) {
    this.isLoading = true;
    this.selectProfilePic = <File>event.target.files[0];
    this.uploadPic();
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
        this.emailExist.text = "Ha surgido algun problema! Porfavor Intentalo más tarde";
        this.emailExist.fire();
      }
    });
  }
}
