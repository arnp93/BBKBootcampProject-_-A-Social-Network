import { Component, OnInit } from '@angular/core';
import { AuthServiceService } from '../../../Services/auth-service.service';
import { UserDTO } from '../../../DTOs/Account/UserDTO';
import { DomainName } from '../../../Utilities/PathTools';
import { Router } from '@angular/router';

@Component({
  selector: 'app-whos-following',
  templateUrl: './whos-following.component.html',
  styleUrls: ['./whos-following.component.css']
})
export class WhosFollowingComponent implements OnInit {

  public URL = DomainName;
  public newUsers : UserDTO[];
  constructor(private authService : AuthServiceService,private router:Router) { }

  ngOnInit(): void {
    this.authService.getLatestusers().subscribe(res => {
      if(res.status === "Success")
      this.newUsers = res.data;
    });
  }

  viewProfile(userId : number) {
    // refresh component
    this.router.navigateByUrl('/view-profile', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/view-profile',userId]);
  });
  }

}
