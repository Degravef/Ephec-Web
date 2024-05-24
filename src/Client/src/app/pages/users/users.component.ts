import {Component, OnInit} from '@angular/core';
import {UsersService} from "../../services/users.service";
import {NgForOf, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {CourseService} from "../../services/course.service";

@Component({
  selector: 'app-users',
  standalone: true,
  templateUrl: './users.component.html',
  imports: [
    NgForOf,
    FormsModule,
    NgIf
  ],
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  roles: string[] = ['Student', 'Instructor', 'Admin'];
  allCourses:any[] = [];
  constructor(private usersService: UsersService,
              private courseService: CourseService) {}

  ngOnInit(): void {
    this.usersService.getAllUsers().subscribe((users: any[]) => {
      this.users = users.map(user => ({
        ...user,
        role: this.roles[user.role]
      }));
    });
    this.courseService.getAllCourses().subscribe({
      next: (courses: any) => {
        this.allCourses = courses;
      },
      error: (error: any): void => {
        console.log(error);
      }
    });
  }

  changeRole(userId: number, newRole: string): void {
    const roleIndex = this.roles.indexOf(newRole);
    this.usersService.promoteUser(userId, roleIndex).subscribe({
      next: (): void => {
        this.users = this.users.map(user => ({
          ...user,
          role: this.roles[user.role]
        }));
      },
      error: (error: any): void => {
        console.log(error);
      }
    });
  }

  toggleCoursesDropdown(user: any): void {
    user.showCoursesDropdown = !user.showCoursesDropdown;
    if (user.showCoursesDropdown) {
      switch (user.role) {
        case 'Student':
          this.courseService.getEnrolledCoursesForUser(user.id).subscribe({
            next: (courses: any) => {
              user.courses = courses;
              console.log(user);
              console.log(this.allCourses);
              },
            error: (error: any): void => {
              console.log(error);
            }
          });
          break;
        case 'Instructor':
          this.courseService.getAssignedCoursesForUser(user.id).subscribe({
            next: (courses: any) => {
              user.courses = courses;
            },
            error: (error: any): void => {
              console.log(error);
            }
          });
          break;
      }
    }
  }

  toggleCourseSelection(user: any, course: any, event: any) {
    if (event.target.checked) {
      switch (user.role) {
        case 'Student':
          this.courseService.enrollStudent(course.id, user.id).subscribe({
            next: (): void => {
              user.courses.push(course);
            },
            error: (error: any): void => {
              console.log(error);
            }
          });
          break;
        case 'Instructor':
          this.courseService.assignInstructor(course.id, user.id).subscribe({
            next: (): void => {
              user.courses.push(course);
            },
            error: (error: any): void => {
              console.log(error);
            }
          });
          break;
      }
    } else {
      switch (user.role) {
        case 'Student':
          this.courseService.quitEnrollStudent(course.id, user.id).subscribe({
            next: (): void => {
              user.courses = user.courses.filter(
                (c: { id: number; }) => c.id !== course.id);
            },
            error: (error: any): void => {
              console.log(error);
            }
          });
          break;
        case 'Instructor':
          this.courseService.quitAssignInstructor(course.id, user.id).subscribe({
            next: (): void => {
              user.courses = user.courses.filter(
                (c: { id: number; }) => c.id !== course.id);
            },
            error: (error: any): void => {
              console.log(error);
            }
          });
          break;
      }
    }
  }

  isCourseSelected(user: any, course: any):boolean {
    return user.courses?.some((c:any) => c.id === course.id)
  }
}
