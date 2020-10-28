import { Component, OnInit } from '@angular/core';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { UserDTO } from '../../../DTOs/Account/UserDTO';
import { DomainName } from '../../../Utilities/PathTools';

@Component({
  selector: 'app-whos-following',
  templateUrl: './whos-following.component.html',
  styleUrls: ['./whos-following.component.css']
})
export class WhosFollowingComponent implements OnInit {

  public URL = DomainName;
  public newUsers : UserDTO[];
  constructor(private authService : AuthServiceService) { }

  ngOnInit(): void {
    this.authService.getLatestusers().subscribe(res => {
      if(res.status === "Success")
      console.log(res);
      
      this.newUsers = res.data;
    });
  }

}
