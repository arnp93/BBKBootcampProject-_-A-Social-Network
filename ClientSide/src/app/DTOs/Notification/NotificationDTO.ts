import { UserDTO } from '../Account/UserDTO';
import { TypeOfNotification } from './TypeOfNotification';
export class NotificationDTO{
   constructor(
    public userOriginId : number,
    public isRead : boolean,
    public createDate : string,
    public typeOfNotification : TypeOfNotification,
    public user : UserDTO
   ){}
}