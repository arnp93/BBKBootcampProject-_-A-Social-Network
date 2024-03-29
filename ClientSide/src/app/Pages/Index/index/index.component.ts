import { AfterViewInit, Component,NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { ShowPostDTO } from 'src/app/DTOs/Post/ShowPostDTO';
import { PostService } from 'src/app/Services/post.service';
import { DomainName } from 'src/app/Utilities/PathTools';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { SignalrService } from '../../../Services/signalr.service';

declare function addEventListenertoHashtagList();

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})

export class IndexComponent implements OnInit, AfterViewInit {

  public lastPostId: number = 0;
  private currentPage: number = 1;
  public URL: string = DomainName;
  public thisUser: UserDTO;
  public newPosts: ShowPostDTO[] = [];
  public posts: ShowPostDTO[] = [];
  public selectProfilePic: File = null;
  public selectedCoverPicture: File = null;
  public newProfilePic: string = null;
  public isLoading = false;
  public morePostsError = false;

  @ViewChild('profilePicError') private emailExist: SwalComponent;

  constructor(private signalrService : SignalrService,private ngZone: NgZone,private router : Router,private authService: AuthServiceService, private postService: PostService, private route: Router) { }
  ngAfterViewInit(): void {
  }

  ngOnInit(): void {

    addEventListenertoHashtagList();

    window['hashtagClick'] = { component: this, zone: this.ngZone, hashtagClickedFunction: (hashtagText) => this.returnToHashtagPage(hashtagText) }; 
    window['viewPost'] = { component: this, zone: this.ngZone, viewPostFunction: (postId) => this.viewSinglePost(postId) }; 


    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
      this.signalrService.startConnection();
      setTimeout(() => {
        // this.signalrService.askServerListener();
        this.signalrService.askServer(res.userId);
      }, 1500);
      if (res === null) {
        this.route.navigate([""]);
      }
    });
    
    this.authService.checkAuth().subscribe(res => {
      if (res.status === 'Success') {
        const currentUser = new UserDTO(
          res.data.token,res.data.expireTime,res.data.firstName,res.data.lastName,res.data.phoneNumber
          ,res.data.birthDay,res.data.address,res.data.about,res.data.isPrivate,res.data.gender,res.data.profilePic,
          res.data.coverPic,res.data.userId,res.data.posts,res.data.notifications,res.data.friends
          );
        this.authService.setCurrentUser(currentUser);
        this.thisUser = currentUser;
      }
      if (res.status === "Error") {
        this.authService.setCurrentUser(null);
        this.route.navigate(["login-error"]);
      }
    });

    window.scrollTo(0, 0);
  }

  returnToHashtagPage(hashtagText : string){
    hashtagText = hashtagText.replace("#","").trim();
    this.postService.getHashtagPosts(hashtagText).subscribe(res => {
      if(res.status === "Success"){
        this.postService.setHashtagPosts(res.data);
        this.route.navigate(["/hashtag-posts"]);
      }
    });
  }

  getNewPost(event: ShowPostDTO) {
    this.newPosts.push(event);
  }

  //Find Element in the html page
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
        if (res.data.length !== 0) {
          res.data.forEach(post => {
            this.posts.push(post);
          });
          if (this.currentPage === 1) {
            window.scroll(0, num[0] + 500);
          } else {
            var num2 = this.findPos(document.getElementById(this.posts[this.posts.length - (res.data.length + 1)].id.toString()));
            window.scroll(0, num2[0] + 120);
          }
          this.currentPage = this.currentPage + 1;
        } else {
          this.morePostsError = true;
        }
      }
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

  viewSinglePost(postId: number) {
    this.router.navigate(['/view-post', postId])
  }
}
