import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { NewPostResponseDTO } from '../DTOs/Post/NewPostResponseDTO';
import { PostResultResponse } from '../DTOs/Post/PostResultResponse';
import { IResponseDTO } from '../DTOs/Common/IResponseDTO';
import { EditPostDTO } from '../DTOs/Post/EditPostDTO';
import { LikeDTO } from '../DTOs/Post/LikeDTO';
import { ShowPostDTO } from '../DTOs/Post/ShowPostDTO';
import { stringify } from 'querystring';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  private posts: BehaviorSubject<PostResultResponse> = new BehaviorSubject<PostResultResponse>(null);
  private hashtagPosts : BehaviorSubject<ShowPostDTO[]> = new BehaviorSubject<ShowPostDTO[]>(null);
  constructor(private http: HttpClient) { }

  addNewPost(post: FormData): Observable<NewPostResponseDTO> {
    return this.http.post<NewPostResponseDTO>("/api/post/new-post", post);
  }

  getPostsByUserId(): Observable<PostResultResponse> {
    return this.http.get<PostResultResponse>("/api/post/user-posts");
  }

  getAllPosts(): Observable<PostResultResponse> {
    return this.http.get<PostResultResponse>("/api/post/all-posts");
  }
  getFriendsPosts() : Observable<IResponseDTO<ShowPostDTO[]>>{
    return this.http.get<IResponseDTO<ShowPostDTO[]>>("/api/post/friends-posts");
  }

  setPosts(postResult: PostResultResponse) {
    this.posts.next(postResult);
  }

  getMorePosts(currentPage: number): Observable<PostResultResponse> {
    let params: HttpParams = new HttpParams().set("CurrentPage", currentPage.toString());

    return this.http.post<PostResultResponse>("/api/post/load-posts", params);
  }

  getMoreNewsfeedPosts(currentPage : number): Observable<PostResultResponse>{
    let params: HttpParams = new HttpParams().set("CurrentPage", currentPage.toString());

    return this.http.post<PostResultResponse>("/api/post/load-newsfeed-posts", params);
  }

  newProfilePicture(form: FormData): Observable<IResponseDTO<string>> {
    return this.http.post<IResponseDTO<string>>("/api/post/profile-pic", form);
  }

  newCoverPicture(form: FormData): Observable<IResponseDTO<string>> {
    return this.http.post<IResponseDTO<string>>("/api/post/cover-pic", form);
  }

  editPost(editPost: EditPostDTO): Observable<IResponseDTO<any>> {
    return this.http.post<IResponseDTO<any>>("/api/post/edit-post", editPost);
  }

  deletePost(postId:number) : Observable<IResponseDTO<any>>{
    return this.http.post<IResponseDTO<any>>("/api/post/delete-post", postId);
  }

  addOrRemoveLike(postId: number): Observable<IResponseDTO<LikeDTO>> {
    return this.http.post<IResponseDTO<LikeDTO>>("/api/post/like", postId);
  }

  getHashtagPosts(hashtagText : string) : Observable<IResponseDTO<ShowPostDTO[]>>{
    let data = new FormData;
    data.append("hashtagText", hashtagText);
    return this.http.post<IResponseDTO<ShowPostDTO[]>>("/api/post/hashtag-posts" , data);
  }

  setHashtagPosts(hashtagPosts : ShowPostDTO[]){
    this.hashtagPosts.next(hashtagPosts);
  }

  getCurrentHashtagPosts(){
    return this.hashtagPosts;
  }
}
