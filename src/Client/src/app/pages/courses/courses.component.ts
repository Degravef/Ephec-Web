import {Component, ElementRef, OnInit, ViewChild} from "@angular/core";
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
              protected loginService: LoginService) {
  }

  ngOnInit(): void {
    this.loginService.isLoggedIn.subscribe(isLoggedIn => {
      if (!isLoggedIn) {
        this.courseService.getAllCourses()
          .subscribe(data => this.courses = data);
      } else {
        this.courseService.getUserCourses()
          .subscribe(data => this.courses = data);
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
