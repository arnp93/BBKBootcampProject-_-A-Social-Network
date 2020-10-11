import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { PostDTO } from '../DTOs/Post/PostDTO';
import { PostResultResponse } from '../DTOs/Post/PostResultResponse';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) { }

  addNewPost(post: FormData):Observable<PostDTO>{
    return this.http.post<PostDTO>("/api/post/new-post",post);
  }

  getPostsByUserId() : Observable<PostResultResponse>{
    return this.http.get<PostResultResponse>("/api/post/user-posts"); 
  }
}
