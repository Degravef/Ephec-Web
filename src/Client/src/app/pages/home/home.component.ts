import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {CourseService} from "../../services/course.service";
import {LoginService} from "../../services/login.service";
import {Modal} from "bootstrap";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    AsyncPipe,
    NgForOf,
    NgIf
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent  implements OnInit {

  @ViewChild('courseDetailsModal') detailsModal!: ElementRef;

  protected courses: any;
  selectedCourse: any;

  constructor(private courseService: CourseService,
              protected loginService: LoginService) {
  }

  ngOnInit(): void {
    console.log("HomeComponent");
    this.loginService.isLoggedIn.subscribe(isLoggedIn => {
      console.log("isLoggedIn : " + isLoggedIn);
      if (!isLoggedIn) {
        return;
      } else {
        this.courseService.getEnrolledCourses()
          .subscribe(enrolledCourses => {
            console.log(enrolledCourses);
            this.courses = enrolledCourses
          });
      }
    });
  }

  openDetailsModal(course: any): void {
    this.courseService.getCourse(course.id).subscribe({
      next: (data): void => {
        const modalElement = this.detailsModal.nativeElement;
        let modalInstance: Modal | null = Modal.getInstance(modalElement);
        if (!modalInstance) {
          modalInstance = new Modal(modalElement);
        }
        if (modalInstance) {
          this.selectedCourse = data;
          modalInstance.show();
        }
      },
    });
  }

  enroll(course: any): void {
    this.courseService.enroll(course.id).subscribe({
      next: (): void => {
        course.isEnrolled = true;
      }
    });
  }

  quit(course: any): void {
    this.courseService.quit(course.id).subscribe({
      next: (): void => {
        course.isEnrolled = false;
      }
    });
  }
}
