import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title: string = 'Dating App';
  
  constructor(
    private accountService: AccountService){}
  
  ngOnInit(): void {
    // Cuando entramos en la aplicación comprobamos si el usuario ha dejado se ha logado y ha salido sin hacer logout
    this.setCurrentUser();
  }



  setCurrentUser() {
    const userString = localStorage.getItem('user');
    console.log('setCurrentUser::1');
    console.log(userString);
    if (!userString) return;
    const user: User = JSON.parse(userString);
    console.log('setCurrentUser::2');
    this.accountService.setCurrentUser(user);
    // --> Se llama al "setCurrentUser" del "accountService"
    console.log('setCurrentUser::3');
  }

  
}
