import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthServiceService } from '../../Services/auth-service.service';
import { UserDTO } from '../../DTOs/Account/UserDTO';
import { DomainName } from '../../Utilities/PathTools';

@Component({
  selector: 'app-right-side-bar',
  templateUrl: './right-side-bar.component.html',
  styleUrls: ['./right-side-bar.component.css']
})
export class RightSideBarComponent implements OnInit {

  public thisUser: UserDTO;
  public URL = DomainName;
  constructor(private router: Router, private authService: AuthServiceService) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(res => {
      this.thisUser = res;
    });
  }

  goToProfile() {
    this.router.navigate(["/index"]);
  }
}
