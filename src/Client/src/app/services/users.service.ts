import { Injectable } from '@angular/core';
import {first, Observable, switchMap} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../environment";
import {LoginService} from "./login.service";

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient,
              private loginService:LoginService) {}

  getAllUsers(): Observable<any> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.http.get(`${environment.baseApiUrl}/Admin/users`, { headers });
      })
    )
  }

  promoteUser(userId: number, role: number): Observable<any> {
    return this.http.put(`${environment.baseApiUrl}/Admin/promote`, { userId, role });
  }
}
