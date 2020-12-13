import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { CommentDTO } from 'src/app/DTOs/CommentDTOs/CommentDTO';
import { ReplyCommentDTO } from 'src/app/DTOs/CommentDTOs/ReplyCommentDTO';
import { SendCommentDTO } from 'src/app/DTOs/CommentDTOs/SendCommentDTO';
import { ShowPostDTO } from 'src/app/DTOs/Post/ShowPostDTO';
import { AuthServiceService } from 'src/app/Services/auth-service.service';
import { CommentService } from 'src/app/Services/comment.service';
import { PostService } from 'src/app/Services/post.service';
import { DomainName } from 'src/app/Utilities/PathTools';

@Component({
  selector: 'app-all-friends-posts',
  templateUrl: './all-friends-posts.component.html',
  styleUrls: ['./all-friends-posts.component.css']
})
export class AllFriendsPostsComponent implements OnInit {

  public posts: ShowPostDTO[] = [];
  public URL: string = DomainName;
  public newComments: CommentDTO[] = [];
  public commentForm: FormGroup;
  public userId: number;


  //for check if user is in friends list (manage Add Friend and Remove Friend in posts menu)
  public friendsIds: number[] = [];
  public activeNotificationsIds: number[] = [];

  public thisUser: UserDTO;

  @ViewChild('commentError') private commentError: SwalComponent;

  constructor(private postService: PostService, private authService: AuthServiceService, private commentService: CommentService, private router: Router) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;

      if (res !== null && res.friends !== null) {
        for (let i = 0; i < res.friends.length; i++) {
          this.friendsIds.push(res.friends[i].userId);
        }
      }

      if (res !== null && res.notifications !== null) {
        for (let i = 0; i < res.notifications.length; i++) {
          if (!res.notifications[i].isAccepted)
            this.activeNotificationsIds.push(res.notifications[i].userDestinationId);
        }
      }
    });

    this.postService.getFriendsPosts().subscribe(res => {
      if (res.status === "Success") {
        this.posts = res.data;
      }
    });

    this.commentForm = new FormGroup({
      text: new FormControl(null, [
        Validators.minLength(1)
      ]),
      postId: new FormControl
    });

    window.scrollTo(0,0);
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
        if (event.target.innerText === "Add Friend")
          event.target.innerText = "Cancel Request";
        else
          event.target.innerText = "Add Friend";
      }
    });
  }

  like(postId: number) {
    this.postService.addOrRemoveLike(postId).subscribe(res => {

      if (res.status === "Success") {
        let post = this.posts.filter(p => p.id === postId)[0];
        if (res.data !== null && !res.data.isDelete) {
          if (post.likes.find(l => l.id === res.data.id) === undefined) {
            post.likes.push(res.data);
          } else
            post.likes = post.likes.filter(l => l.id !== res.data.id);
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
        let post = this.posts.filter(p => p.id === postId)[0];
        let comment = post.comments.filter(c => c.id === commentId)[0];
        comment.replies.unshift(res.data);
        this.commentForm.reset();
      }
    });
  }


  removeRequest(event, userId: number) {

  }

  removeFriend(event, userId: number) {

  }


}
