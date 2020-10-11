import { Component, OnInit } from '@angular/core';
import { PostService } from '../../../Services/post.service';
import { ShowPostDTO } from '../../../DTOs/Post/ShowPostDTO';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { UserDTO } from '../../../DTOs/Account/UserDTO';
import { DomainName } from '../../../Utilities/PathTools';

@Component({
  selector: 'app-user-posts',
  templateUrl: './user-posts.component.html',
  styleUrls: ['./user-posts.component.css']
})
export class UserPostsComponent implements OnInit {

  public URL : string = DomainName;
  public posts: ShowPostDTO[];
  public thisUser: UserDTO;
  constructor(private postService: PostService, private authService: AuthServiceService) { }

  ngOnInit(): void {
    this.postService.getPostsByUserId().subscribe(res => {
      if(res.status === "Success"){
        this.posts = res.data;
      }
      console.log(this.posts);
      
    });
    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });
  }

}
