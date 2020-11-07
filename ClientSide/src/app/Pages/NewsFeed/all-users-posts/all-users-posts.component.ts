import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CommentDTO } from 'src/app/DTOs/CommentDTOs/CommentDTO';
import { SendCommentDTO } from 'src/app/DTOs/CommentDTOs/SendCommentDTO';
import { ShowPostDTO } from 'src/app/DTOs/Post/ShowPostDTO';
import { PostService } from 'src/app/Services/post.service';
import { DomainName } from 'src/app/Utilities/PathTools';
import { CommentService } from '../../../Services/comment.service';
import { Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { UserDTO } from '../../../DTOs/Account/UserDTO';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { EditPostDTO } from 'src/app/DTOs/Post/EditPostDTO';

@Component({
  selector: 'app-all-users-posts',
  templateUrl: './all-users-posts.component.html',
  styleUrls: ['./all-users-posts.component.css']
})
export class AllUsersPostsComponent implements OnInit {

  @Output() public lastPostId = new EventEmitter();
  public posts: ShowPostDTO[] = [];
  public URL: string = DomainName;
  public newComments: CommentDTO[] = [];
  public commentForm: FormGroup;
  public editPostForm: FormGroup;
  public userId: number;

  @ViewChild('editProfileError') private editError: SwalComponent;
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

    this.postService.getAllPosts().subscribe(res => {
      if (res.status === "Success") {
        this.posts = res.data;
        if (res.data[res.data.length - 1] !== undefined) {
          this.lastPostId.emit(res.data[res.data.length - 1].id);
        }
      }
    });

    this.commentForm = new FormGroup({
      text: new FormControl(null, [
        Validators.minLength(1)
      ]),
      postId: new FormControl
    });

    this.editPostForm = new FormGroup({
      postText: new FormControl()
    });
    
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

  removeRequest(event, userId: number) {

  }

  removeFriend(event, userId: number) {

  }


}
