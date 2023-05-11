
import { inject, Injectable } from '@angular/core';
import {
  Router, 
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
  ResolveFn
} from '@angular/router';
import { mergeMap, Observable, of } from 'rxjs';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';

// ÑAPA
// TODO
// Mirar Resolve deprecated
// IMPORTANTE como ejemplo de Resolve en Angular 15
/*
@Injectable({
  providedIn: 'root'
})*/

export const MemberDetailedResolver: ResolveFn<Member> = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
      // return inject(MembersService).getMember(route.paramMap.get('username')!);

      var username = route.paramMap.get('username');
      console.log(route.paramMap);
      if(username)
        return inject(MembersService).getMember(username);
      else
        return of();
    };


/*

export const pageSettingsResolver: ResolveFn<Member> = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    // return inject(MembersService).getMember(route.paramMap.get('username')!);
    return inject(MembersService).getMember('todd');
  };
  */
/*
export const MemberDetailedResolver: ResolveFn<Member> = (route: ActivatedRouteSnapshot) => {
  const router = inject(Router);
  const cs = inject(MembersService);
  const id = route.paramMap.get('username')!;
  return cs.getMember(id).pipe(mergeMap(member => {
    return of(member);
  }));
  */
/*
  constructor(private memberService: MembersService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Member> {
    console.log('Traza::4');
    return this.memberService.getMember(route.paramMap.get('username')!);
  }*/
// }

