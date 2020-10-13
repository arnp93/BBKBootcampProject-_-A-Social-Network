import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentDTO } from '../DTOs/CommentDTOs/CommentDTO';
import { SendCommentDTO } from '../DTOs/CommentDTOs/SendCommentDTO';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http : HttpClient) { }

  postComment(comment : SendCommentDTO):Observable<CommentDTO>{
    return this.http.post<CommentDTO>("/api/post/new-comment",comment);
  }
}
