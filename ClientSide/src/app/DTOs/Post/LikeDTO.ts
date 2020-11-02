export class LikeDTO{
    constructor(
        public userId : number,
        public firstName : string,
        public lastName : string,
        public profilePic : string,
        public isDelete : boolean
    ){}
}