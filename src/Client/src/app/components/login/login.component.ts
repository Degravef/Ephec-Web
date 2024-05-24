import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {AsyncPipe, NgIf} from "@angular/common";
import {Modal} from 'bootstrap';
import {LoginService} from "../../services/login.service";

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [
    FormsModule,
    NgIf,
    AsyncPipe
  ],
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  @ViewChild('loginModal') loginModal!: ElementRef;

  protected logUsername: string = "";
  protected logPassword: string = "";
  protected regUsername: string = "";
  protected regPassword: string = "";
  protected verifyPassword: string = "";
  protected loginError: string = "";
  protected registerError: string = "";

  isLoggedIn: boolean = false;
  username: string = "";

  constructor(protected loginService: LoginService) {
  }

  onLogin(): void {
    this.loginService.login(this.logUsername, this.logPassword).subscribe({
      next: (): void => {
        const modalElement = this.loginModal.nativeElement;
        const modalInstance: Modal | null = Modal.getInstance(modalElement);
        if (modalInstance) {
          this.logUsername = "";
          this.logPassword = "";
          modalInstance.hide();
          document.querySelector('.modal-backdrop')?.remove();
        }
      },
      error: (err): void => {
        this.loginError = 'Login failed. Please check your username and password.';
        console.log(err);
      }
    });
  }

  onRegister(): void {
    if (this.regPassword !== this.verifyPassword) {
      this.registerError = 'Passwords do not match.';
      return;
    }
    this.loginService.register(this.regUsername, this.regPassword).subscribe({
      next: () => {
        const modalElement = this.loginModal.nativeElement;
        const modalInstance: Modal | null = Modal.getInstance(modalElement);
        if (modalInstance) {
          this.regUsername = "";
          this.regPassword = "";
          modalInstance.hide();
          document.querySelector('.modal-backdrop')?.remove();
        }
      },
      error: (err) => {
        this.registerError = 'Registration failed. Please try again or try a different username.';
        console.log(err);
      }
    });
  }

  ngOnInit(): void {
    this.loginService.isLoggedIn.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
    });

    this.loginService.username.subscribe(username => {
      this.username = username;
    });
  }

  logout() {
    this.loginService.logout();
  }
}
