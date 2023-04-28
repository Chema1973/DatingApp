import { useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {}
  // currentUser$: Observable<User | null> = of(null)

  constructor(public accountService: AccountService) {}

  ngOnInit(): void {
    // this.currentUser$ = this.accountService.currentUser$;
    // this.getCurrentUser();
  }
/*
  getCurrentUser() {
    this.accountService.currentUser$.subscribe({
      next: user => this.loggedIn = !!user,
      error : error => console.log(error)
    })
  }
*/
  login() {
    console.log(this.accountService.currentUser$);
    // --> 
    console.log(this.model);
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(this.accountService.currentUser$);
        console.log('nav-login');
        console.log(this.accountService.currentUser$);
        // console.log(response);
      },
      error: error => console.log(error)
    })
  }

  logout() {
    this.accountService.logout();
    this.model.username = '';
    this.model.password = '';
  }

}
