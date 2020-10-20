import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CommentDTO } from 'src/app/DTOs/CommentDTOs/CommentDTO';
import { SendCommentDTO } from 'src/app/DTOs/CommentDTOs/SendCommentDTO';
import { ShowPostDTO } from 'src/app/DTOs/Post/ShowPostDTO';
import { PostService } from 'src/app/Services/post.service';
import { DomainName } from 'src/app/Utilities/PathTools';
import { CommentService } from '../../../Services/comment.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-all-users-posts',
  templateUrl: './all-users-posts.component.html',
  styleUrls: ['./all-users-posts.component.css']
})
export class AllUsersPostsComponent implements OnInit {

  public posts: ShowPostDTO[] = [];
  public URL: string = DomainName;
  public newComments: CommentDTO[] = [];
  public commentForm: FormGroup;
  public userId: number;
  constructor(private postService: PostService, private commentService: CommentService, private router: Router) { }

  ngOnInit(): void {
    this.postService.getAllPosts().subscribe(res => {
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
  viewProfile(event) {
    const userId = event.target.parentNode.parentNode.childNodes[0].value;
    this.router.navigate(['/view-profile',userId])
  }

 
}
