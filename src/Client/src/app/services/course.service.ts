import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../environment";
import {LoginService} from "./login.service";
import {first, Observable, switchMap} from "rxjs";

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

  getEnrolledCoursesForUser(userId: number) {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        console.log("getEnrolledCourses : token : " + token);
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.get(`${environment.baseApiUrl}/Admin/enrolledCourses/${userId}`, { headers });
      })
    );
  }

  getAssignedCoursesForUser(userId: number) {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        console.log("getEnrolledCourses : token : " + token);
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.get(`${environment.baseApiUrl}/Admin/assignedCourses/${userId}`, { headers });
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

  assign(courseId: number) : Observable<object> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.post(`${environment.baseApiUrl}/course/assign/${courseId}`, {}, { headers });
      })
    );
  }

  quitEnroll(courseId: number) : Observable<object> {
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

  quitAssign(courseId: number) : Observable<object> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.delete(`${environment.baseApiUrl}/course/Assign/${courseId}`, { headers });
      })
    );
  }

  createCourse(newCourse: any) {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.post(`${environment.baseApiUrl}/Course`, newCourse, { headers });
      })
    )
  }

  updateCourse(course: any) {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.put(`${environment.baseApiUrl}/Course`, course, { headers });
      })
    )
  }

  deleteCourse(courseId: number) {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.delete(`${environment.baseApiUrl}/Course/${courseId}`, { headers });
      })
    )
  }

  enrollStudent(courseId: number, userId : number) : Observable<object> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.post(`${environment.baseApiUrl}/admin/Enroll/`, {courseId, userId}, { headers });
      })
    );
  }

  assignInstructor(courseId: number, userId : number) : Observable<object> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        return this.httpClient.post(`${environment.baseApiUrl}/admin/assign/`, {courseId, userId}, { headers });
      })
    );
  }

  quitEnrollStudent(courseId: number, userId : number) : Observable<object> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        const body = {courseId, userId};
        return this.httpClient.request('DELETE',`${environment.baseApiUrl}/admin/Enroll/`, { headers, body });
      })
    );
  }

  quitAssignInstructor(courseId: number, userId : number) : Observable<object> {
    return this.loginService.token.pipe(
      first(),
      switchMap(token => {
        const headers = new HttpHeaders()
          .set('Content-Type', 'application/json')
          .set('Authorization', `Bearer ${token}`);
        const body = {courseId, userId};
        return this.httpClient.request('DELETE',`${environment.baseApiUrl}/admin/assign/`, { headers, body });
      })
    );
  }
}
