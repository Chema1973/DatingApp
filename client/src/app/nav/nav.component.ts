import { useAnimation } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of, take } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {}
  username: string = '';
  // currentUser$: Observable<User | null> = of(null)

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService
    ) {}
  // Con "private router: Router" permitiremos la navegación por código

  ngOnInit(): void {
    // this.currentUser$ = this.accountService.currentUser$;
    // this.getCurrentUser();
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: response => {
        if (!response) return;
        this.username = response.username;
      }
    })
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
    // --> 
    this.accountService.login(this.model).subscribe({
      // this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: response => {
        this.username = this.model.username;

        this.router.navigateByUrl('/members');
        this.model = {};
      }//,
      // next: _ => this.router.navigateByUrl('/members') //, // El '_' indica que no hay respuesta (al igual que '()')
      // error: error => this.toastr.error(error.error) // console.log(error) // 
    })
  }

  logout() {
    this.accountService.logout();
    this.model.username = '';
    this.model.password = '';
    // NAVEGACIÓN
    this.router.navigateByUrl('/');
  }

}
