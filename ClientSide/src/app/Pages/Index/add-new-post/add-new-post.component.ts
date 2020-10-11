import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PostService } from '../../../Services/post.service';

@Component({
  selector: 'app-add-new-post',
  templateUrl: './add-new-post.component.html',
  styleUrls: ['./add-new-post.component.css']
})
export class AddNewPostComponent implements OnInit {
  
  public selectedFile : File = null;
  public PostForm: FormGroup;
  constructor(private postService: PostService) { }

  ngOnInit(): void {
    this.PostForm = new FormGroup({
      postText: new FormControl(null, [
        Validators.minLength(1),
        Validators.required
      ])
    });
  }

  addPhoto(event) {
     this.selectedFile = <File>event.target.files[0];
  }
  postSubmit(): void {
    const formData : FormData = new FormData();
    formData.append("FileName",this.selectedFile,this.selectedFile.name);
    formData.append("PostText",this.PostForm.controls.postText.value);

    this.postService.addNewPost(formData).subscribe(res => {
      console.log(res);

    })
  }

}