import { Component, OnInit } from '@angular/core';
import { AuthServiceService } from '../../../Services/auth-service.service';

@Component({
  selector: 'app-whos-following',
  templateUrl: './whos-following.component.html',
  styleUrls: ['./whos-following.component.css']
})
export class WhosFollowingComponent implements OnInit {

  constructor(private authService : AuthServiceService) { }

  ngOnInit(): void {
  }

}
