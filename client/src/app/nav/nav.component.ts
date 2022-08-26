import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};
  // loggedIn: boolean;
  constructor(public accountService: AccountService) {}

  ngOnInit(): void {
    // this.getCurrentUser();
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  logout() {
    this.accountService.logout();
  }

  // /*
  // its not a good practice to subscribe to observable like this
  // and afterwards not unsubscribe from it, [could potentially cause memory leaks]
  // */
  // getCurrentUser() {
  //   this.accountService.currentUser$.subscribe({
  //     next: (user) => {
  //       this.loggedIn = !!user; // [!!] == turns object into boolean, if user is null => false : true
  //     },
  //     error: (err) => {
  //       console.log(err);
  //     },
  //   });
  // }
}
