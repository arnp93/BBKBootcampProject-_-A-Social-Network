export class RegisterUserDTO{
    constructor(
        public firstName : string,
        public lastName : string,
        public email : string,
        public password : string,
        public rePassword: string
    ){}
}