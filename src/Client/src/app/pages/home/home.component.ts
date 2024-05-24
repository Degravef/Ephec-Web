import {ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild} from '@angular/core';
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
              protected loginService: LoginService,
              private cdr: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.loginService.role.subscribe(role => {
      switch (role) {
        case 'Instructor':
        case 'Student':
          this.courseService.getEnrolledCourses()
            .subscribe(data =>
            {
              console.log(data);
              this.courses = data
            });
          break;
        default:
          this.courseService.getAllCourses()
            .subscribe(data => this.courses = data);
          break;
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
    this.loginService.role.subscribe(role => {
      switch (role) {
        case 'Student':
          this.courseService.enroll(course.id).subscribe({
            next: (): void => {
              course.isEnrolled = true;
            }
          });
          break;
        case 'Instructor':
          this.courseService.assign(course.id).subscribe({
            next: (): void => {
              course.isEnrolled = true;
              course.hasInstructor = true;
            }
          });
          break;
      }
    });
  }

  quit(course: any): void {
    this.loginService.role.subscribe(role => {
      switch (role) {
        case 'Student':
          this.courseService.quitEnroll(course.id).subscribe({
            next: (): void => {
              course.isEnrolled = false;
            }
          });
          break;
        case 'Instructor':
          this.courseService.quitAssign(course.id).subscribe({
            next: (): void => {
              course.isEnrolled = false;
              course.hasInstructor = false;
            }
          });
          break;
      }
    });
  }
}
