import { Component, OnInit, Output, EventEmitter, ViewChild, AfterViewInit } from '@angular/core';
import { PostService } from '../../../Services/post.service';
import { ShowPostDTO } from '../../../DTOs/Post/ShowPostDTO';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { UserDTO } from '../../../DTOs/Account/UserDTO';
import { DomainName } from '../../../Utilities/PathTools';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SendCommentDTO } from 'src/app/DTOs/CommentDTOs/SendCommentDTO';
import { CommentService } from '../../../Services/comment.service';
import { CommentDTO } from '../../../DTOs/CommentDTOs/CommentDTO';
import { EditPostDTO } from 'src/app/DTOs/Post/EditPostDTO';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { ReplyCommentDTO } from '../../../DTOs/CommentDTOs/ReplyCommentDTO';

declare function addEventListenertoHashtagListFromAngular();

@Component({
  selector: 'app-user-posts',
  templateUrl: './user-posts.component.html',
  styleUrls: ['./user-posts.component.css']
})
export class UserPostsComponent implements OnInit {

  @Output() public lastPostId = new EventEmitter();
  public editPostForm: FormGroup;
  public URL: string = DomainName;
  public posts: ShowPostDTO[];
  public thisUser: UserDTO;
  public commentForm: FormGroup;
  public newComments: CommentDTO[] = [];
  public commentReplyForm : FormGroup;

  @ViewChild('editProfileError') private editError: SwalComponent;

  constructor(private postService: PostService, private commentService: CommentService, private authService: AuthServiceService, private router: Router) { }
 

  ngOnInit(): void {
    addEventListenertoHashtagListFromAngular();
   
    this.postService.getPostsByUserId().subscribe(res => {
      if (res.status === "Success") {
        this.posts = res.data;
        if (res.data[res.data.length - 1] !== undefined) {
          this.lastPostId.emit(res.data[res.data.length - 1].id);
        }
      }
    });

    this.editPostForm = new FormGroup({
      postText: new FormControl()
    });

    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });


    this.commentForm = new FormGroup({
      text: new FormControl(null, [
        Validators.minLength(1)
      ]),
      postId: new FormControl
    });

    this.commentReplyForm = new FormGroup({
      text: new FormControl(null, [
        Validators.minLength(1)
      ]),
      postId: new FormControl
    });
  }


  newCommentSubmit(postId,commentId) {
    const newComment = new SendCommentDTO(
      this.commentForm.controls.text.value,
      parseInt(postId),
      0
    );

    this.commentService.postComment(newComment).subscribe(res => {
      if (res.status === "Success") {
        res.data.profilePic = this.thisUser.profilePic;
        this.newComments.push(res.data);
        this.commentForm.reset();
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

  viewProfile(userId: number) {
    this.router.navigate(['/view-profile', userId])
  }

  postSubmit(event, postId: number): void {

    let postText = event.target.firstChild.value;

      let postWordsArray = postText.split(" ");
      
      postText = "";

      if(postWordsArray.length > 0){
        for (let i = 0; i < postWordsArray.length; i++) {
          if(postWordsArray[i].startsWith("#")){
            postWordsArray[i] = `<a href="#" class="link-hashtag" > ${postWordsArray[i]} </a>`
          }
        
          postText += " " + postWordsArray[i]; 
        }
      }

    this.postService.editPost(new EditPostDTO(postText, postId)).subscribe(res => {
      if (res.status === "Success") {
        event.target.parentNode.style.display = "none";
        event.target.parentNode.parentNode.firstChild.innerHTML = postText;
      } else {
        this.editError.fire();
      }
    });
  }

  deletePost(event , postId : number){
    this.postService.deletePost(postId).subscribe(res => {
      if(res.status === "Success")
     event.target.parentNode.parentNode.parentNode.parentNode.parentNode.innerHTML = "<p class='alert alert-info'> Deleted! </p>";
    });
  }

}
