import {ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild} from "@angular/core";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {LoginService} from "../../services/login.service";
import {CourseService} from "../../services/course.service";
import {Modal} from "bootstrap";

@Component({
  selector: 'app-courses',
  standalone: true,
  templateUrl: './courses.component.html',
  imports: [
    NgForOf,
    AsyncPipe,
    NgIf
  ],
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {

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
          this.courseService.getUserCourses()
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
              this.cdr.detectChanges()
            }
          });
          break;
      }
    });
  }

  isEnrollButtonVisible(item: any): boolean {
    let isVisible = false;
    this.loginService.isLoggedIn.subscribe(isLoggedIn => {
      this.loginService.role.subscribe(role => {
        if (!isLoggedIn) {
          isVisible = false;
          return;
        }
        if (item.isEnrolled) {
          isVisible = false;
          return;
        }
        isVisible = !(role === 'Instructor' && item.hasInstructor);
      });
    });
    return isVisible;
  }
}

