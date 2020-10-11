import { ShowPostDTO } from './ShowPostDTO';
export class PostResultResponse{
    constructor(
        public status: string,
        public data : ShowPostDTO[]
    ){}
}