import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-right-side-bar',
  templateUrl: './right-side-bar.component.html',
  styleUrls: ['./right-side-bar.component.css']
})
export class RightSideBarComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  goToProfile() {
    this.router.navigate(["/index"]);
  }
}
