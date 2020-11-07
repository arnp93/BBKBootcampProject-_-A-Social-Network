import { Component, OnInit } from '@angular/core';
import { ShowPostDTO } from 'src/app/DTOs/Post/ShowPostDTO';
import { PostService } from 'src/app/Services/post.service';
import { DomainName } from 'src/app/Utilities/PathTools';

@Component({
  selector: 'app-new-feeds',
  templateUrl: './new-feeds.component.html',
  styleUrls: ['./new-feeds.component.css']
})
export class NewFeedsComponent implements OnInit {

  public URL: string = DomainName;
  public lastPostId: number = 0;
  private currentPage: number = 1;
  public morePostsError = false;
  public posts: ShowPostDTO[] = [];

  constructor(private postService: PostService) { }

  ngOnInit(): void {
    window.scrollTo(0, 0);
  }

  //Find Element in the page
  findPos(obj) {
    var curtop = 0;
    if (obj.offsetParent) {
      do {
        curtop += obj.offsetTop;
      } while (obj === obj.offsetParent);
      return [curtop];
    }
  }

  loadMorePosts() {
    var num = this.findPos(document.getElementById(this.lastPostId.toString()));
   
    this.postService.getMoreNewsfeedPosts(this.currentPage).subscribe(res => {
      if (res.status === "Success") {
        if (res.data.length !== 0) {
          res.data.forEach(post => {
            this.posts.push(post);
          });
          if (this.currentPage === 1) {
            window.scroll(0, num[0] + 500);
          } else {
            var num2 = this.findPos(document.getElementById(this.posts[this.posts.length - (res.data.length + 1)].id.toString()));
            window.scroll(0, num2[0] + 150);
          }
          this.currentPage = this.currentPage + 1;
        } else {
          this.morePostsError = true;
        }
      }
    });
  }
}
