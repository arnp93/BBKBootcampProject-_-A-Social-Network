import { UserDTO } from './../../DTOs/Account/UserDTO';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommentDTO } from 'src/app/DTOs/CommentDTOs/CommentDTO';
import { SendCommentDTO } from 'src/app/DTOs/CommentDTOs/SendCommentDTO';
import { AuthServiceService } from '../../Services/auth-service.service';
import { DomainName } from '../../Utilities/PathTools';
import { CommentService } from '../../Services/comment.service';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { ReplyCommentDTO } from 'src/app/DTOs/CommentDTOs/ReplyCommentDTO';

@Component({
  selector: 'app-view-profile',
  templateUrl: './view-profile.component.html',
  styleUrls: ['./view-profile.component.css']
})
export class ViewProfileComponent implements OnInit {
  //#region public variable for using in view
  public URL = DomainName;
  private userId: number;
  private thisUserId: number;
  public user: UserDTO = new UserDTO(
    null, null, null, null, null, null, null, null, null, null, null, null, null, [], [], []
  );
  public thisUser : UserDTO;
  public newComments: CommentDTO[] = [];
  public commentForm: FormGroup;
  @ViewChild('errorPrivate') private errorPrivate: SwalComponent;

  //for check if user is in friends list (manage Add Friend and Remove Friend in posts menu)
  public friendsIds: number[] = [];

  public notificationIdsForSendedFriendRequests: number[] = [];

  constructor(private activatedRoute: ActivatedRoute, private authService: AuthServiceService, private commentService: CommentService, private router: Router) { }

  ngOnInit(): void {

    this.authService.getCurrentUser().subscribe(res => {
      if (res !== null){
        this.thisUserId = res.userId;
        this.thisUser = res;
      }
        

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


    this.activatedRoute.params.subscribe(param => {
      this.userId = parseInt(param.userId);
    });

    if (this.userId === this.thisUserId) {
      this.router.navigate(["/index"]);
    };

    this.authService.userProfile(this.userId).subscribe(res => {
      if (res.status === "Success") {
        if (res.data.isPrivate && !this.friendsIds.includes(this.userId)) {
          this.errorPrivate.fire();
          this.user.userId = res.data.userId;
          this.user.firstName = res.data.firstName;
          this.user.lastName = res.data.lastName;
          this.user.coverPic = res.data.coverPic;
          this.user.profilePic = res.data.profilePic;
          this.user.isPrivate = res.data.isPrivate;
          this.user.friends = res.data.friends;
          this.user.posts = null
        } else {
          this.user = res.data;
        }
      }
    });

    this.commentForm = new FormGroup({
      text: new FormControl(null, [
        Validators.minLength(1)
      ]),
      postId: new FormControl
    });

    window.scrollTo(0, 0);

    
    
  }

  newCommentSubmit(postId) {
    const newComment = new SendCommentDTO(
      this.commentForm.controls.text.value,
      parseInt(postId),
      0
    );

    this.commentService.postComment(newComment).subscribe(res => {
      if (res.status === "Success") {
        this.newComments.push(res.data)
        this.commentForm.reset();
      }
    });
  }


  viewProfile(userId: number) {
    // refresh component
    this.router.navigateByUrl('/view-profile', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/view-profile', userId]);
    });
  }

  friendRequest(event, userId: number) {
    this.authService.friendRequest(userId).subscribe(res => {
      if (res.status === "Success") {
        this.notificationIdsForSendedFriendRequests.push(userId);
      }
    });
  }

  cancelFriendRequest(event, userId: number) {
    this.authService.deleteFriendRequest(userId).subscribe(res => {
      if (res.status === "Success") {
        if (this.notificationIdsForSendedFriendRequests.includes(userId)) {
          this.notificationIdsForSendedFriendRequests = this.notificationIdsForSendedFriendRequests.filter(id => id !== userId);
        }
      }
    });
  }

  newCommentReplySubmit(postId,commentId,parentCommentUserId){
    const newComment = new ReplyCommentDTO(
      this.commentForm.controls.text.value,
      parseInt(postId),
      parseInt(parentCommentUserId),
      parseInt(commentId)
    );

    this.commentService.replyComment(newComment).subscribe(res => {
      if (res.status === "Success") {
        res.data.profilePic = this.thisUser.profilePic;
        let post = this.user.posts.filter(p => p.id === postId)[0];
        
        let comment = post.comments.filter(c => c.id === commentId)[0];
        console.log(res);
        
          comment.replies.unshift(res.data);
        
        this.commentForm.reset();
      }
    });
  }

}
