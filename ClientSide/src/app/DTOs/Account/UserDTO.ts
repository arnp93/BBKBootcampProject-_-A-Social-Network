import { ShowPostDTO } from '../Post/ShowPostDTO';
import { NotificationDTO } from '../Notification/NotificationDTO';
import { FriendDTO } from './FriendDTO';
import { Gender } from './Gender';
export class UserDTO{
    constructor(
        public token : string,
        public expireTime : number,
        public firstName : string,
        public lastName : string,
        public phoneNumber : string,
        public birthDay : string,
        public address : string,
        public about : string,
        public isPrivate : boolean,
        public gender : Gender,
        public profilePic : string,
        public coverPic : string,
        public userId: number,
        public posts:ShowPostDTO[],
        public notifications : NotificationDTO[],
        public friends : FriendDTO[]
    ){}
}