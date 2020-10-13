import { Component, OnInit } from '@angular/core';
import { PostService } from '../../../Services/post.service';
import { ShowPostDTO } from '../../../DTOs/Post/ShowPostDTO';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { UserDTO } from '../../../DTOs/Account/UserDTO';
import { DomainName } from '../../../Utilities/PathTools';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-posts',
  templateUrl: './user-posts.component.html',
  styleUrls: ['./user-posts.component.css']
})
export class UserPostsComponent implements OnInit {


  public URL: string = DomainName;
  public posts: ShowPostDTO[];
  public thisUser: UserDTO;
  public newPosts: ShowPostDTO[] = [];

  constructor(private postService: PostService, private authService: AuthServiceService, private router: Router) { }

  ngOnInit(): void {
    this.postService.getPostsByUserId().subscribe(res => {
      if (res.status === "Success") {
        this.posts = res.data.reverse();
      }
    });
    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });
  }

  getNewPost(event: ShowPostDTO) {
    this.newPosts.push(event);
    console.log(this.newPosts);
  }

}
