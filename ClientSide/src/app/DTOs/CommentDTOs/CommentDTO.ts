export class CommentDTO {
    constructor(
        public id : number,
        public firstName: string,
        public lastName: string,
        public profilePic: string,
        public userId: number,
        public text: string,
        public likeCount: number,
        public postId: number,
        public parentId: number,
        public replies: CommentDTO[]
    ) { }
}