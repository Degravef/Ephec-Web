import {ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild} from "@angular/core";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {LoginService} from "../../services/login.service";
import {CourseService} from "../../services/course.service";
import {Modal} from "bootstrap";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-courses',
  standalone: true,
  templateUrl: './courses.component.html',
  imports: [
    NgForOf,
    AsyncPipe,
    NgIf,
    FormsModule
  ],
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {

  @ViewChild('courseDetailsModal') detailsModal!: ElementRef;
  @ViewChild('createCourseModal') createModal!: ElementRef;
  @ViewChild('editCourseModal') editModal!: ElementRef;

  protected courses: any;
  selectedCourse: any = {};
  newCourse: any = {};

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
            .subscribe(data => {
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

  openCreateCourseModal() {
    const modalElement = this.createModal.nativeElement;
    let modalInstance: Modal | null = Modal.getInstance(modalElement);
    if (!modalInstance) {
      modalInstance = new Modal(modalElement);
    }
    if (modalInstance) {
      modalInstance.show();
    }
  }

  createCourse() {
    this.courseService.createCourse(this.newCourse).subscribe({
      next: (data): void => {
        this.courses.push(data);
        this.newCourse = {};
        const modalElement = this.createModal.nativeElement;
        const modalInstance: Modal | null = Modal.getInstance(modalElement);
        if (modalInstance) {
          modalInstance.hide();
        }
      },
      error: (err): void => {
        console.error('Error creating course:', err);
      }
    });
  }

  deleteCourse(courseId: number) {
    this.courseService.deleteCourse(courseId).subscribe({
      next: (): void => {
        this.courses = this.courses.filter(
          (course: { id: number; }) => course.id !== courseId);
        const modalElement = this.detailsModal.nativeElement;
        const modalInstance: Modal | null = Modal.getInstance(modalElement);
        if (modalInstance) {
          modalInstance.hide();
        }
      },
      error: (err): void => {
        console.error('Error deleting course:', err);
      }
    });
  }

  openEditCourseModal() {
    const detailsModalElement = this.detailsModal.nativeElement;
    const detailsModalInstance: Modal | null = Modal.getInstance(detailsModalElement);
    if (detailsModalInstance) {
      detailsModalInstance.hide();
    }
    const editModalElement = this.editModal.nativeElement;
    let editModalInstance: Modal | null = Modal.getInstance(editModalElement);
    if (!editModalInstance) {
      editModalInstance = new Modal(editModalElement);
    }
    if (editModalInstance) {
      editModalInstance.show();
    }
  }

  editCourse() {
    console.log(this.selectedCourse);
    this.courseService.updateCourse(this.selectedCourse).subscribe({
      next: (): void => {
        const modalElement = this.editModal.nativeElement;
        const modalInstance: Modal | null = Modal.getInstance(modalElement);
        if (modalInstance) {
          modalInstance.hide();
        }
        const index = this.courses.findIndex((course: any) => course.id === this.selectedCourse.id);
        if (index !== -1) {
          this.courses[index] = this.selectedCourse;
        }
      },
      error: (err): void => {
        console.error('Error deleting course:', err);
      }
    });
    const modalElement = this.createModal.nativeElement;
    const modalInstance: Modal | null = Modal.getInstance(modalElement);
    if (modalInstance) {
      modalInstance.hide();
    }
  }
}

