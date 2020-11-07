export class LikeDTO{
    constructor(
        public id:number,
        public userId : number,
        public firstName : string,
        public lastName : string,
        public profilePic : string,
        public isDelete : boolean
    ){}
}