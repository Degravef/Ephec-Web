import {Routes} from '@angular/router';
import {CoursesComponent} from "./pages/courses/courses.component";
import {HomeComponent} from "./pages/home/home.component";
import {UsersComponent} from "./pages/users/users.component";

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'courses', component: CoursesComponent },
  { path: 'users', component: UsersComponent },
];
