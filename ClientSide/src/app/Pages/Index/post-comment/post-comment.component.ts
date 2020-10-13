import { Component, Input, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CommentService } from '../../../Services/comment.service';
import { SendCommentDTO } from '../../../DTOs/CommentDTOs/SendCommentDTO';
import { Router } from '@angular/router';
import { CommentDTO } from 'src/app/DTOs/CommentDTOs/CommentDTO';
import { ShowPostDTO } from '../../../DTOs/Post/ShowPostDTO';

@Component({
  selector: 'app-post-comment',
  templateUrl: './post-comment.component.html',
  styleUrls: ['./post-comment.component.css']
})
export class PostCommentComponent implements OnInit {

  
  @Input() postId: number;
  @Output() test : CommentDTO;
  public commentForm: FormGroup;
  constructor(private commentService: CommentService, private router : Router) { }

  ngOnInit(): void {
    this.commentForm = new FormGroup({
      text: new FormControl(null, [
        Validators.minLength(1)
      ])
    });
  }
  
  newCommentSubmit() {
    const newComment = new SendCommentDTO(
      this.commentForm.controls.text.value,
      this.postId,
      0
    );

    this.commentService.postComment(newComment).subscribe(res =>{
      console.log(res);
     this.test = res;
     this.router.navigateByUrl('/user-posts-component', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/index']);
    });
    });
  }

}
