import {HttpClient} from '@angular/common/http';
import {Component, OnInit} from "@angular/core";
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

  private http: HttpClient;
  protected courses: any;

  constructor(_http: HttpClient) {
    this.http = _http;
  }

  ngOnInit(): void {
    this.http.get<any>("https://localhost:7167/api/Admin/users")
      .subscribe(data => this.courses = data);
  }
}
