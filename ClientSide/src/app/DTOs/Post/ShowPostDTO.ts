import { CommentDTO } from '../CommentDTOs/CommentDTO';
export interface ShowPostDTO {
    postText: string;
    fileName: string;
    userId: number;
    canalId: number;
    dateTime : Date;
    comments: CommentDTO[]
}