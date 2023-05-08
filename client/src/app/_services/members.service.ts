import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers() {
    // return this.http.get<Member[]>(this.baseUrl + 'users', this.getHttpOptions());
    // Nuestro JwtInterceptor se encargará de añadir en el header del Token del Bearer

    if (this.members.length > 0) return of(this.members);
    // --> Con el "of" devolvemos el array como si fuera un "observable"

    // return this.http.get<Member[]>(this.baseUrl + 'users');

    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map( members => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(username: string) {
    // return this.http.get<Member>(this.baseUrl + 'users/' + username, this.getHttpOptions());
    // Nuestro JwtInterceptor se encargará de añadir en el header del Token del Bearer

    const member = this.members.find(x => x.userName === username);

    if (member) return of(member);

    return this.http.get<Member>(this.baseUrl + 'users/' + username);

  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member};
      })
    );
  }
/*
  getHttpOptions() {
    const userString = localStorage.getItem('user');
    if (!userString) return;

    const user = JSON.parse(userString);

    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + user.token
      })
    }
  }
*/
}
