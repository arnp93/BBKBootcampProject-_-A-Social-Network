export class ChangePasswordDTO {
    constructor(
        public password: string,
        public rePassword: string,
        public isPrivate: boolean
    ) { }
}