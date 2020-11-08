import { UserDTO } from '../Account/UserDTO';
import { TypeOfNotification } from './TypeOfNotification';
export class NotificationDTO {
   constructor(
      public id: number,
      public userOriginId: number,
      public userDestinationId : number,
      public postId : number,
      public message : string,
      public isRead: boolean,
      public isAccepted: boolean,
      public isDelete : boolean,
      public createDate: string,
      public typeOfNotification: TypeOfNotification,
      public user: UserDTO
   ) { }
}