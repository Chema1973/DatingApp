import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  // baseUrl = 'https://localhost:5001/api/' ;
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  // --> Para evitar el error de "null"
  currentUser$ = this.currentUserSource.asObservable();
  // --> Establecemos un "Observable" que puede tener los valores de "User" o "null"

  // --> Añadimos SignalR (PresenceService)

  constructor(private http: HttpClient, private presenceService: PresenceService) { }

  login(model: any) {
    // Este método se llamacuando hace el login
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          // localStorage.setItem('user', JSON.stringify(user));
          // this.currentUserSource.next(user);
          this.setCurrentUser(user);
          // --> Es un eventEmitter. Dispara un evento a todos los suscriptores que están escuchando
          //     Es decir, está llamando a todos los suscriptores de "currentUser$"
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(
          user => {
            if (user){
              // localStorage.setItem('user', JSON.stringify(user));
              // this.currentUserSource.next(user);
              this.setCurrentUser(user);
            }
            // return user;
            // --> Si devolvemos al "user" en "register.component", en el método de registro
            //     podemos obtener los datos 
        }
      ))
  }

  setCurrentUser(user: User) {
    // Se le llama al ""arrancar" la aplicación

    user.roles = [];
    const roles = this.getDecoratedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);

    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
    console.log("SignalR::4");
    this.presenceService.createHubConnection(user);
    console.log("SignalR::5");
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    console.log("SignalR::6");
    this.presenceService.stopHubConnection();
    console.log("SignalR::7");
  }

  getDecoratedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
