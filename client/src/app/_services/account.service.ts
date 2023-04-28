import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = 'https://localhost:5001/api/' ;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  // --> Para evitar el error de "null"
  currentUser$ = this.currentUserSource.asObservable();
  // --> Establecemos un "Observable" que puede tener los valores de "User" o "null"

  constructor(private http: HttpClient) { }

  login(model: any) {
    // Este método se llamacuando hace el login
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          console.log('Account-Login::1');
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
          // --> Es un eventEmitter. Dispara un evento a todos los suscriptores que están escuchando
          //     Es decir, está llamando a todos los suscriptores de "currentUser$"
          console.log('Account-Login::2');
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(
          user => {
            if (user){
              localStorage.setItem('user', JSON.stringify(user));
              this.currentUserSource.next(user);
            }
            // return user;
            // --> Si devolvemos al "user" en "register.component", en el método de registro
            //     podemos obtener los datos 
        }
      ))
  }

  setCurrentUser(user: User) {
    // Se le llama al ""arrancar" la aplicación
    console.log('setCurrentUser-Login::1');
    this.currentUserSource.next(user);
    console.log('setCurrentUser-Login::2');
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}