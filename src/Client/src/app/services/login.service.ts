import {Injectable} from '@angular/core';
import {BehaviorSubject, catchError, Observable, of, tap} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../environment";

@Injectable({
  providedIn: 'root',
})
export class LoginService {

  private _isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public readonly isLoggedIn: Observable<boolean> = this._isLoggedIn.asObservable();

  private _token: BehaviorSubject<string> = new BehaviorSubject<string>("");
  public readonly token: Observable<string> = this._token.asObservable();

  private _username: BehaviorSubject<string> = new BehaviorSubject<string>("");
  public readonly username: Observable<string> = this._username.asObservable();

  constructor(private http: HttpClient) {
  }

  login(username: string, password: string): Observable<any> {
    this.logout();
    return this.http.post<any>(environment.baseApiUrl + "/login", {username, password})
      .pipe(
        tap(data => {
          this._username.next(data.username);
          this._token.next(data.token);
          this._isLoggedIn.next(true);
        })
      );
  }

  register(username: string, password: string): Observable<any> {
    this.logout();
    return this.http.post<any>(environment.baseApiUrl + "/register", {username, password})
      .pipe(
        tap(data => this.saveLoginData(data)),
        catchError(() => {
          this.logout();
          return of(false);
        })
      );
  }

  logout(): void {
    this._isLoggedIn.next(false);
    this._token.next("");
    this._username.next("");
  }

  private saveLoginData(data: any): void {
    this._username.next(data.username);
    this._token.next(data.token);
    console.log(data.token);
    this._isLoggedIn.next(true);
    console.log(this._isLoggedIn.value);
  }
}
