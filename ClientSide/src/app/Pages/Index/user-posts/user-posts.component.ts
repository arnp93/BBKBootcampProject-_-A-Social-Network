import { Component, OnInit, Output, EventEmitter } from '@angular/core';
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

@Component({
  selector: 'app-user-posts',
  templateUrl: './user-posts.component.html',
  styleUrls: ['./user-posts.component.css']
})
export class UserPostsComponent implements OnInit {

  @Output() public lastPostId = new EventEmitter();
  public URL: string = DomainName;
  public posts: ShowPostDTO[];
  public thisUser: UserDTO;
  public commentForm: FormGroup;
  public newComments: CommentDTO[] = [];
  constructor(private postService: PostService, private commentService: CommentService, private authService: AuthServiceService, private router: Router) { }

  ngOnInit(): void {
    this.postService.getPostsByUserId().subscribe(res => {
      if (res.status === "Success") {
        this.posts = res.data;
        this.lastPostId.emit(res.data[res.data.length - 1].id);
      }
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
  }


  newCommentSubmit(postId) {
    const newComment = new SendCommentDTO(
      this.commentForm.controls.text.value,
      parseInt(postId),
      0
    );

    this.commentService.postComment(newComment).subscribe(res => {

      if (res.status === "Success") {
        this.newComments.push(res.data);
        this.commentForm.reset();
      }

    });
  }

  viewProfile(userId : number) {
    this.router.navigate(['/view-profile', userId])
  }

}
