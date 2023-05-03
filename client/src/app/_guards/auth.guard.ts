import { Injectable } from '@angular/core';
// import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
// Protector de rutas
export class AuthGuard { // implements CanActivate {

  constructor(private accountService: AccountService, private toastr: ToastrService) {}

  canActivate() : Observable<boolean> {
    // Se suscribe al observable "currentUser$"
    return this.accountService.currentUser$.pipe(
      map((user) => {
        if (user) return true;
        else {
          this.toastr.error('You shall not pass!!');
          return false;
        }
      })
    )
  }
  /*
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }
  */
}
