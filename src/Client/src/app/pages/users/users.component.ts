import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {UsersService} from "../../services/users.service";
import {NgForOf} from "@angular/common";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-users',
  standalone: true,
  templateUrl: './users.component.html',
  imports: [
    NgForOf,
    FormsModule
  ],
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  roles: string[] = ['Student', 'Instructor', 'Admin'];

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.usersService.getAllUsers().subscribe((users: any[]) => {
      this.users = users.map(user => ({
        ...user,
        role: this.roles[user.role]
      }));
    });
  }

  changeRole(userId: number, newRole: string): void {
    const roleIndex = this.roles.indexOf(newRole);
    this.usersService.promoteUser(userId, roleIndex).subscribe(() => {
      this.usersService.getAllUsers().subscribe((users: any[]) => {
        this.users = users.map(user => ({
          ...user,
          role: this.roles[user.role]
        }));
      });
    });
  }
}
