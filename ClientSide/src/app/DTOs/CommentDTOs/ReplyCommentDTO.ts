export class ReplyCommentDTO{
    constructor(
        public text : string,
        public postId : number,
        public DestinationUserId : number,
        public parentId : number
    ){ }
}