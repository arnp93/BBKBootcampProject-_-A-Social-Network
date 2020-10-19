import { CommentDTO } from '../CommentDTOs/CommentDTO';
import { UserDTO } from '../Account/UserDTO';
export interface ShowPostDTO {
    id:number,
    postText: string;
    fileName: string;
    userId: number;
    canalId: number;
    dateTime : Date;
    comments: CommentDTO[],
    user:UserDTO
}