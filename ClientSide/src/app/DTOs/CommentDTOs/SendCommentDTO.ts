export class SendCommentDTO{
    constructor(
        public text : string,
        public postId : number,
        public userId : number
    ){}
}