export class RegisterUserDTO{
    constructor(
        public username : string,
        public firstName : string,
        public lastName : string,
        public email : string,
        public password : string,
        public rePassword: string
    ){}
}