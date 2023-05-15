import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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

  // model: any = {};

  // Formularios Reactivos
  // registerForm: FormGroup | undefined;
  registerForm: FormGroup = new FormGroup({});

  maxDate: Date = new Date();
  validationErrors: string[] | undefined;


  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) {}

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm()
  {
    // Reemplazamos "new FormGroup" por "this.fb.group" y simplificamos un poco más el código
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }
/*
  initializeForm2()
  {
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
    });
    // Esto se hace para que cuando cambie el valor de "password" (nos suscribimos a su evento "valueChange"),
    // automaticamente se vuelva a validar el campo "confirmPassword"
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }
*/
  matchValues(matchTo: string) : ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
      // "notMatching" la usaremos en el front para los mensajes de error
    };
  }

  register() {
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const values = {...this.registerForm.value, dateOfBirth: dob};

    // let numeros = [10, 4, 7, 15, 3, 25];
    // console.log(numeros);     // Imprime un array.
    // console.log(...numeros);  // Imprime una lista de argumentos.
    // Al anteponer los tres puntos que representan al spread operator transformamos la variable numeros
    // (que en el ejemplo representa un array con números) en una lista de argumentos, y es por ello que
    // podemos acceder al número mayor del array numeros. Es como si le quitáramos los corchetes
    // ( “[]” ) al array.


    // this.accountService.register(this.model).subscribe({
      this.accountService.register(values).subscribe({
      next: () => {
      // next: response => {
        // --> Si "register" de "accountService" devuelve el "user"
        //     lo que pintaría "response" sería un "UserDTO"
        // this.cancel();
        this.router.navigateByUrl('/members')
      }, error: error => {
        // this.toastr.error(error.error)
        this.validationErrors = error
      }
    })
    
  }

  cancel(){
    this.cancelRegister.emit(false);
    // --> Enviamos el evento al PADRE
    //     En este caso, en el "home.component.html" -> (cancelRegister)="cancelRegisterMode($event)"
    //     home.component.ts : cancelRegisterMode
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;

    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())).toISOString().slice(0,10);
  }

}
