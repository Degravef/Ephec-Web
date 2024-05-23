import {Injectable} from '@angular/core';
import {BehaviorSubject, catchError, Observable, of, tap} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {jwtDecode}  from 'jwt-decode';
import {environment} from "../environment";

@Injectable({
  providedIn: 'root',
})
export class LoginService {

  private _token: BehaviorSubject<string> = new BehaviorSubject<string>(this.getInitialToken());
  public readonly token: Observable<string> = this._token.asObservable();

  private _isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.getInitialLoginState());
  public readonly isLoggedIn: Observable<boolean> = this._isLoggedIn.asObservable();

  private _username: BehaviorSubject<string> = new BehaviorSubject<string>(this.getInitialUsername());
  public readonly username: Observable<string> = this._username.asObservable();

  private _role: BehaviorSubject<string> = new BehaviorSubject<string>(this.getInitialRole());
  public readonly role: Observable<string> = this._role.asObservable();

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
    localStorage.setItem('token', token);
    this.refresh();
  }

  private refresh() {
    this._token.next(this.getInitialToken());
    if (this._token.getValue() === '') {
      this._isLoggedIn.next(false);
      this._username.next('');
      this._role.next('');
    } else {
      this._isLoggedIn.next(this.getInitialLoginState());
      this._username.next(this.getInitialUsername());
      this._role.next(this.getInitialRole());
    }
  }

  private getInitialLoginState(): boolean {
    return this._token.getValue() !== '';
  }

  private getInitialToken(): string {
    return localStorage.getItem('token')?? "";
  }

  private getInitialUsername(): string {
    try {
      const token: string = this._token.getValue();
      if (token === null) return "";
      let decoded = jwtDecode(token) as any;
      return decoded.name ?? "";
    } catch (e) {
      return "";
    }
  }

  private getInitialRole() {
    try {
      const token: string = this._token.getValue();
      if (token === null) return "";
      let decoded = jwtDecode(token) as any;
      return decoded.role ?? "";
    } catch (e) {
      return "";
    }
  }
}
