import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { NewPostResponseDTO } from '../DTOs/Post/NewPostResponseDTO';
import { PostResultResponse } from '../DTOs/Post/PostResultResponse';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private posts: BehaviorSubject<PostResultResponse> = new BehaviorSubject<PostResultResponse>(null);
  constructor(private http: HttpClient) { }

  addNewPost(post: FormData): Observable<NewPostResponseDTO> {
    return this.http.post<NewPostResponseDTO>("/api/post/new-post", post);
  }

  getPostsByUserId(): Observable<PostResultResponse> {
    return this.http.get<PostResultResponse>("/api/post/user-posts");
  }

  setPosts(postResult: PostResultResponse) {
    this.posts.next(postResult);
  }

  getMorePosts(currentPage : number): Observable<PostResultResponse> {
    let params: HttpParams = new HttpParams().set("CurrentPage", currentPage.toString());

    return this.http.post<PostResultResponse>("/api/post/load-posts", params);
  }
}
