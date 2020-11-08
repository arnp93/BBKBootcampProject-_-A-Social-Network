export class SignalRFriendRequestResponse{
    constructor(
        public id : number,
        public userOriginId: number,
        public firstName : string,
        public lastName: string,
        public message: string,
        public imageName: string,
        public isRead : boolean,
        public createDate : Date,
        public isAccepted :boolean
    ){}
}