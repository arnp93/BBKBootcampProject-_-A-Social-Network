import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PostService } from '../../../Services/post.service';
import { Router } from '@angular/router';
import { ShowPostDTO } from '../../../DTOs/Post/ShowPostDTO';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { DomainName } from 'src/app/Utilities/PathTools';

declare function addEventListenertoHashtagListFromAngular();

@Component({
  selector: 'app-add-new-post',
  templateUrl: './add-new-post.component.html',
  styleUrls: ['./add-new-post.component.css']
})
export class AddNewPostComponent implements OnInit {

  //new post will add to index.component
  @Output() newPost: EventEmitter<ShowPostDTO> = new EventEmitter<ShowPostDTO>();
  public selectedFile: File = null;
  public URL: string = DomainName;
  public PostForm: FormGroup;
  public thisUser: UserDTO;
  constructor(private postService: PostService, private authService: AuthServiceService, private router: Router) { }

  ngOnInit(): void {
    this.PostForm = new FormGroup({
      postText: new FormControl(null, [
        Validators.minLength(1),
        Validators.required
      ])
    });
    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });
 
  }

  addPhoto(event) {
    this.selectedFile = <File>event.target.files[0];
  }

  postSubmit(): void {
    const formData: FormData = new FormData();
    if (this.selectedFile != null)
      formData.append("FileName", this.selectedFile, this.selectedFile.name);
      let postText = this.PostForm.controls.postText.value;
      let postWordsArray = postText.split(" ");
      postText = "";
      if(postWordsArray.length > 0){
        for (let i = 0; i < postWordsArray.length; i++) {
          if(postWordsArray[i].startsWith("#")){
            postWordsArray[i] = `<a class="link-hashtag"> ${postWordsArray[i]} </a>`
          }
        
          postText += " " + postWordsArray[i]; 
        }
      }
    formData.append("PostText", postText);

    this.postService.addNewPost(formData).subscribe(res => {
      if (res.status === "Success") {
        this.newPost.emit(res.data);
        this.PostForm.reset();
        addEventListenertoHashtagListFromAngular();
      } else {
        console.error("Error with Add new Post!!!");
        
      }
    });
  }

}
