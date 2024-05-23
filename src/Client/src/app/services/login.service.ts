import {Injectable} from '@angular/core';
import {BehaviorSubject, catchError, Observable, of, tap} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {jwtDecode}  from 'jwt-decode';
import {environment} from "../environment";

@Injectable({
  providedIn: 'root',
})
export class LoginService {

  private _isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.getInitialLoginState());
  public readonly isLoggedIn: Observable<boolean> = this._isLoggedIn.asObservable();

  private _username: BehaviorSubject<string> = new BehaviorSubject<string>(this.getInitialUsername());
  public readonly username: Observable<string> = this._username.asObservable();

  constructor(private http: HttpClient) {

  }

  login(username: string, password: string): Observable<any> {
    this.logout();
    return this.http.post<any>(environment.baseApiUrl + "/login", {username, password})
      .pipe(
        tap(data => this.saveLoginData(data.token))
      );
  }

  register(username: string, password: string): Observable<any> {
    this.logout();
    return this.http.post<any>(environment.baseApiUrl + "/register", {username, password})
      .pipe(
        tap(data => this.saveLoginData(data.token)),
        catchError(() => {
          this.logout();
          return of(false);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.refresh();
  }

  private saveLoginData(token: string): void {
    console.log(`token: ${token}`);
    localStorage.setItem('token', token);
    this.refresh();
  }

  private refresh() {
    this._isLoggedIn.next(this.getInitialLoginState());
    this._username.next(this.getInitialUsername());
  }

  private getInitialLoginState(): boolean {
    return localStorage.getItem('token') !== null;
  }

  private getInitialUsername(): string {
     const token: string | null = localStorage.getItem('token');
     if (token === null) return "";
     let decoded = jwtDecode(token) as any;
     return decoded.name ?? "";
  }
}
