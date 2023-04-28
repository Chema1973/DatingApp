import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  // @Input() usersFromHomeComponent: any;
  // Una propiedad se transmite del PADRE al HIJO
  @Output() cancelRegister = new EventEmitter();
  // Del HIJO llamaremos al PADRE

  model: any = {};

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    
  }

  register() {
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: () => {
      // next: response => {
        // console.log(response);
        // --> Si "register" de "accountService" devuelve el "user"
        //     lo que pintaría "response" sería un "UserDTO"
        this.cancel();
      }, error: error => console.log(error)
    })
  }

  cancel(){
    console.log('cancelled');
    this.cancelRegister.emit(false);
    // --> Enviamos el evento al PADRE
    //     En este caso, en el "home.component.html" -> (cancelRegister)="cancelRegisterMode($event)"
    //     home.component.ts : cancelRegisterMode
  }

}
