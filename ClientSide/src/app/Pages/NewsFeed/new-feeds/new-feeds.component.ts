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

  // public posts:ShowPostDTO[] = [];
  // public URL: string = DomainName;
  constructor(private postService : PostService) { }

  ngOnInit(): void {
  }

  loadMorePosts(){

  }
}
