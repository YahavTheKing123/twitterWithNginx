import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { TokenStorageService } from '../_services/token-storage.service';
import { UtilitiesService } from '../_services/utilities.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: any = {
    username: null,
    password: null
  };
  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';

  constructor(private authService: AuthService, private tokenStorage: TokenStorageService, private utilitiesService: UtilitiesService, private router: Router) { }

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.router.navigate(['/home']);
    }
  }

  onSubmit(): void {
    const { username, password } = this.form;

    this.authService.login(username, password).subscribe(
      data => {
        console.log(data);
        if (data && data.success && data.message)
        {
          this.tokenStorage.saveToken(data.message);
          this.tokenStorage.saveUser(data);
          this.isLoginFailed = false;
          this.isLoggedIn = true;
          window.location.reload();
        }
        else
        {
          this.errorMessage = 'Failed to log in';
          this.isLoginFailed = false;
        }

      },
      err => {
        this.errorMessage = this.utilitiesService.getErrorMessage(err);
        this.isLoginFailed = true;
      }
    );
  }

}