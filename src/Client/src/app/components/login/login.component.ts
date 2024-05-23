import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {NgIf} from "@angular/common";
import {Modal} from 'bootstrap';
import {LoginService} from "../../services/login.service";

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
export class LoginComponent implements OnInit {

  @ViewChild('loginModal') loginModal!: ElementRef;

  protected logUsername: string = "";
  protected logPassword: string = "";
  protected regUsername: string = "";
  protected regPassword: string = "";
  protected verifyPassword: string = "";

  isLoggedIn: boolean = false;
  username: string = "";

  constructor(private loginService: LoginService) {
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
        console.log(err);
      }
    });
  }

  onRegister(): void {
    if (this.regPassword !== this.verifyPassword) return;
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
