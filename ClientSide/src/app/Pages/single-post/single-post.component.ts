import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CommentDTO } from 'src/app/DTOs/CommentDTOs/CommentDTO';
import { SendCommentDTO } from 'src/app/DTOs/CommentDTOs/SendCommentDTO';
import { ShowPostDTO } from 'src/app/DTOs/Post/ShowPostDTO';
import { PostService } from 'src/app/Services/post.service';
import { DomainName } from 'src/app/Utilities/PathTools';
import { Router, ActivatedRoute } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { EditPostDTO } from 'src/app/DTOs/Post/EditPostDTO';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { AuthServiceService } from 'src/app/Services/auth-service.service';
import { CommentService } from 'src/app/Services/comment.service';

declare function addEventListenertoHashtagListFromAngular();

@Component({
  selector: 'app-single-post',
  templateUrl: './single-post.component.html',
  styleUrls: ['./single-post.component.css']
})
export class SinglePostComponent implements OnInit {


  @Output() public lastPostId = new EventEmitter();
  public post: ShowPostDTO;
  public URL: string = DomainName;
  public newComments: CommentDTO[] = [];
  public commentForm: FormGroup;
  public editPostForm: FormGroup;
  public userId: number;
  public postId: number;

  @ViewChild('editProfileError') private editError: SwalComponent;

  //for check if user is in friends list (manage Add Friend and Remove Friend in posts menu)
  public friendsIds: number[] = [];
  
  public activeNotificationsIds: number[] = [];
  public notificationIdsForSendedFriendRequests: number[] = [];

  public thisUser: UserDTO;

  @ViewChild('commentError') private commentError: SwalComponent;

  constructor(private activatedRoute: ActivatedRoute, private postService: PostService, private authService: AuthServiceService, private commentService: CommentService, private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.postId = parseInt(param.postId);
    });
 

    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
      if (res !== null && res.friends !== null) {
        for (let i = 0; i < res.friends.length; i++) {
          this.friendsIds.push(res.friends[i].userId);
        }
      }
      if (res !== null && res.notifications !== null) {
        let sendRequests = res.notifications.filter(n => n.userOriginId == res.userId && !n.isAccepted && !n.isDelete);
        for (let i = 0; i < sendRequests.length; i++) {
          this.notificationIdsForSendedFriendRequests.push(sendRequests[i].userDestinationId);
        }
      }
    });

    this.postService.getSinglePost(this.postId).subscribe(res => {
      if (res.status === "Success"){
        this.post = res.data;
      }
    })

    this.commentForm = new FormGroup({
      text: new FormControl(null, [
        Validators.minLength(1)
      ]),
      postId: new FormControl
    });

    this.editPostForm = new FormGroup({
      postText: new FormControl()
    });

    addEventListenertoHashtagListFromAngular();

  }

  newCommentSubmit(postId) {
    const newComment = new SendCommentDTO(
      this.commentForm.controls.text.value,
      parseInt(postId),
      0
    );

    this.commentService.postComment(newComment).subscribe(res => {
      if (res.status === "Success") {
        res.data.profilePic = this.thisUser.profilePic;
        console.log(res.data.profilePic);

        this.newComments.push(res.data)
        this.commentForm.reset();
      } else {
        this.commentError.text = 'Failed! Please try again later!';
        this.commentError.fire();
      }
    });
  }

  viewProfile(userId: number) {
    this.router.navigate(['/view-profile', userId])
  }

  friendRequest(event, userId: number) {
    this.authService.friendRequest(userId).subscribe(res => {
      if (res.status === "Success") {
        this.notificationIdsForSendedFriendRequests.push(userId);
      }
    });
  }

  like(postId: number) {
    this.postService.addOrRemoveLike(postId).subscribe(res => {

      if (res.status === "Success") {
        if (res.data !== null && !res.data.isDelete) {
          if (this.post.likes.find(l => l.id === res.data.id) === undefined) {
            this.post.likes.push(res.data);
          } else
            this.post.likes = this.post.likes.filter(l => l.id !== res.data.id);
        }
      }
    });
  }

  postSubmit(event, postId: number): void {

    const newPostText = event.target.firstChild.value;

    this.postService.editPost(new EditPostDTO(newPostText, postId)).subscribe(res => {
      if (res.status === "Success") {
        event.target.parentNode.style.display = "none";
        event.target.parentNode.parentNode.firstChild.innerHTML = newPostText;
      } else {
        this.editError.fire();
      }
    });
  }

  deletePost(event, postId: number) {
    this.postService.deletePost(postId).subscribe(res => {
      if (res.status === "Success")
        event.target.parentNode.parentNode.parentNode.parentNode.parentNode.innerHTML = "<p class='alert alert-info'> Deleted! </p>";
    });
  }

  cancelFriendRequest(event, userId: number) {
    this.authService.deleteFriendRequest(userId).subscribe(res => {
      if (res.status === "Success") {
        if (this.notificationIdsForSendedFriendRequests.includes(userId)) {
          this.notificationIdsForSendedFriendRequests = this.notificationIdsForSendedFriendRequests.filter(id => id !== userId);
        }
      }
    })
  }

  removeFriend(event, userId: number) {




  }

}
