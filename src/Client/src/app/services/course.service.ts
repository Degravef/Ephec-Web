import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../environment";
import {LoginService} from "./login.service";
import {first, firstValueFrom, Observable, switchMap} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  constructor(private loginService : LoginService,
              private httpClient : HttpClient) { }

  getAllCourses() {
    return this.httpClient.get(`${environment.baseApiUrl}/Courses`);
  }

  getCourse(id: number) {
    return this.httpClient.get(`${environment.baseApiUrl}/Course/${id}`);
  }

  getUserCourses() {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        console.log("getUserCourses : token : " + token);
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.get(`${environment.baseApiUrl}/UserCourses`, { headers });
      })
    );
  }

  getEnrolledCourses() {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        console.log("getEnrolledCourses : token : " + token);
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.get(`${environment.baseApiUrl}/EnrolledCourses`, { headers });
      })
    );
  }

  enroll(courseId: number) : Observable<object> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.post(`${environment.baseApiUrl}/course/Enroll/${courseId}`, {}, { headers });
      })
    );
  }

  quit(courseId: number) : Observable<object> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.delete(`${environment.baseApiUrl}/course/Enroll/${courseId}`, { headers });
      })
    );
  }
}