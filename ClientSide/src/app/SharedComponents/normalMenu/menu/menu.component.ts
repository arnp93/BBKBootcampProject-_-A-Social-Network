import { Component, OnInit } from '@angular/core';
import { UserDTO } from 'src/app/DTOs/Account/UserDTO';
import { AuthServiceService } from 'src/app/Services/auth-service.service';
import { DomainName } from 'src/app/Utilities/PathTools';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  public URL: string = DomainName;
  public thisUser: UserDTO;
  constructor(private authService: AuthServiceService) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });
  }

}
