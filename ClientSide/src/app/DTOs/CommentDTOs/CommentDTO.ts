export class CommentDTO{
    constructor(
        
        public id : number,
        public text : string,
        public likeCount : number,
        public PostId :number,
        public parentId : number,
        public replies : CommentDTO[]
    ){}
}