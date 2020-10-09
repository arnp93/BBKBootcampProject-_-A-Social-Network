export class UserDTO{
    constructor(
        public Token : string,
        public ExpireTime : string,
        public FirstName : string,
        public LastName : string,
        public Id: number
    ){}
}