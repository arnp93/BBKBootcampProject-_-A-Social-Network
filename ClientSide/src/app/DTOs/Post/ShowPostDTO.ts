import { CommentDTO } from '../CommentDTOs/CommentDTO';
import { UserDTO } from '../Account/UserDTO';
import { LikeDTO } from './LikeDTO';
export interface ShowPostDTO {
    id:number,
    postText: string;
    fileName: string;
    userId: number;
    canalId: number;
    dateTime : Date;
    comments: CommentDTO[],
    likes : LikeDTO[],
    user:UserDTO
}