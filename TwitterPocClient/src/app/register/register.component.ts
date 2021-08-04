import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form: any = {
    username: null,
    email: null,
    password: null
  };
  isSuccessful = false;
  isSignUpFailed = false;
  errorMessage = '';

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    const { username, password } = this.form;

    this.authService.register(username, password).subscribe(
      data => {
        console.log(data);
        this.isSuccessful = true;
        this.isSignUpFailed = false;
      },
      err => {
        console.log(err);
        if (err.status == 400 && err.error && err.error.errors)
        {
           var modelErrors = err.error.errors;
           var key = Object.keys(modelErrors)[0];
           var value = modelErrors[key];
           this.errorMessage = key + ': ' + value;
        }
        else if (err.error.message)
        {
           this.errorMessage = err.error.message;
        }
        else
        {
          this.errorMessage = err.error.Message;
        }

        this.isSignUpFailed = true;
      }
    );
  }
}