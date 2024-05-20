import {Component, OnInit} from "@angular/core";
import {HttpClient} from '@angular/common/http';
import {NgForOf} from "@angular/common";

@Component({
  selector: 'app-courses',
  standalone: true,
  templateUrl: './courses.component.html',
  imports: [
    NgForOf
  ],
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {

  protected courses: any;

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.http.get<any>("https://localhost:7167/api/Admin/users")
      .subscribe(data => this.courses = data);
  }
}
