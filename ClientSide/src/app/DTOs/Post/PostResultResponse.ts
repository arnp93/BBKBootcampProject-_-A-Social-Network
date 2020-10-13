import { ShowPostDTO } from './ShowPostDTO';
import { BehaviorSubject } from 'rxjs';
export class PostResultResponse{
    constructor(
        public status: string,
        public data : ShowPostDTO[]
    ){}
}