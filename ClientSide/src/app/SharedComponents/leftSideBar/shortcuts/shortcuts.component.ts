import { CookieService } from 'ngx-cookie-service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthServiceService } from '../../../Services/auth-service.service';
declare function loadPage();

@Component({
  selector: 'app-shortcuts',
  templateUrl: './shortcuts.component.html',
  styleUrls: ['./shortcuts.component.css']
})
export class ShortcutsComponent implements OnInit {

  constructor(private cookieService: CookieService, private router: Router,private authService: AuthServiceService) { }

  ngOnInit(): void {
    loadPage();
  }
  
  logout() {
    this.cookieService.delete("BBKBootcampCookie");
    this.authService.setCurrentUser(null);
    this.authService.logOut();
  
    
    // this.router.navigate([""]);
  }

  goToNewsFeed(){
    this.router.navigate(["/news-feed"]);
  }
}
