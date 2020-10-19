export class UserDTO{
    constructor(
        public token : string,
        public expireTime : number,
        public firstName : string,
        public lastName : string,
        public profilePic : string,
        public userId: number
    ){}
}