import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommentDTO } from 'src/app/DTOs/CommentDTOs/CommentDTO';
import { SendCommentDTO } from 'src/app/DTOs/CommentDTOs/SendCommentDTO';
import { UserDTO } from '../../DTOs/Account/UserDTO';
import { AuthServiceService } from '../../Services/auth-service.service';
import { DomainName } from '../../Utilities/PathTools';
import { CommentService } from '../../Services/comment.service';

@Component({
  selector: 'app-view-profile',
  templateUrl: './view-profile.component.html',
  styleUrls: ['./view-profile.component.css']
})
export class ViewProfileComponent implements OnInit {
  public URL = DomainName;
  private userId: number;
  private thisUserId: number;
  public user: UserDTO = new UserDTO(null, 0, "", "", "", 0, null,[],[]);
  public newComments: CommentDTO[] = [];
  public commentForm: FormGroup;
  constructor(private activatedRoute: ActivatedRoute, private authService: AuthServiceService,private commentService:CommentService, private router: Router) { }

  ngOnInit(): void {

    this.authService.getCurrentUser().subscribe(res => {
      if (res !== null)
      this.thisUserId = res.userId;
    });
    
    this.activatedRoute.params.subscribe(param => {
      this.userId = parseInt(param.userId);
    });

    if (this.userId === this.thisUserId) {
      this.router.navigate(["/index"])
    };

    this.authService.userProfile(this.userId).subscribe(res => {
      if(res.status === "Success"){
        this.user = res.data;
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
        this.newComments.push(res.data)
        this.commentForm.reset();
      }
    });
  }

  
  viewProfile(userId : number) {
    // refresh component
    this.router.navigateByUrl('/view-profile', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/view-profile',userId]);
  });
  }

}
