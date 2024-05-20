import {Component, ViewChild, ElementRef} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {LoginService} from "../../services/login.service";
import * as bootstrap from 'bootstrap';
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [
    FormsModule,
    NgIf
  ],
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  @ViewChild('loginModal') loginModal!: ElementRef;

  protected formUsername: string = "";
  protected formPassword: string = "";
  protected isLoggedIn: boolean = false;
  protected username: string = "";

  constructor(private loginService: LoginService) {}

  onSubmit(): void {
    this.loginService.login(this.formUsername, this.formPassword).subscribe({
      next: (response) => {
        this.isLoggedIn = true;
        this.username = response.username;
        const modalElement = this.loginModal.nativeElement;
        const modalInstance = bootstrap.Modal.getInstance(modalElement);
        if (modalInstance) {
          modalInstance.hide();
        }
      },
      error: (err) => {
        console.log(err);
      }
    });
  }
}
