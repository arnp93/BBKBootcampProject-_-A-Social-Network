import { ShowPostDTO } from './ShowPostDTO';

export class NewPostResponseDTO{
    constructor(
        public status: string,
        public data : ShowPostDTO
    ){}
}