import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { CommentDTO } from '../DTOs/CommentDTOs/CommentDTO';
import { SendCommentDTO } from '../DTOs/CommentDTOs/SendCommentDTO';
import { IResponseDTO } from '../DTOs/Common/IResponseDTO';
import { ReplyCommentDTO } from '../DTOs/CommentDTOs/ReplyCommentDTO';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http : HttpClient) { }

  postComment(comment : SendCommentDTO):Observable<IResponseDTO<CommentDTO>>{
    return this.http.post<IResponseDTO<CommentDTO>>("/api/post/new-comment",comment);
  }
  replyComment(comment : ReplyCommentDTO):Observable<IResponseDTO<CommentDTO>>{
    return this.http.post<IResponseDTO<CommentDTO>>("/api/post/reply-comment",comment);
  }

}
