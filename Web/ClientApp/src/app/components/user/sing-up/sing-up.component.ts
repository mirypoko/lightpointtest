import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { AuthenticationService } from '../../../services/authentication-service.service';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { IPostUser } from '../../../models/user/postUser';
import { HttpErrorResponse } from '@angular/common/http';
import { ISingIn } from '../../../models/authorization/singIn';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';


@Component({
  selector: 'app-sing-up',
  templateUrl: './sing-up.component.html',
  styleUrls: ['./sing-up.component.scss']
})
export class SingUpComponent implements OnInit {

  constructor(private _userService: UserService,
    private _authenticationService: AuthenticationService,
    private _router: Router,
    private _snackBarsService: SnackBarsServiceService) { }

  ngOnInit() {
  }

  errors: string[] = [];

  form = new FormGroup({
    "username": new FormControl("", [Validators.required, Validators.minLength(4), Validators.pattern("^[0-9]*[A-Za-z][A-Za-z0-9]*$")]),
    "email": new FormControl("", [Validators.required, Validators.email]),
    "password": new FormControl("", [Validators.required, Validators.minLength(8)]),
    "confirmPassword": new FormControl("", [Validators.required])
  });


  submit() {
    this.clearFormErrors();

    if (!this.form.valid) {
      this.showFormErrors(['Fill all fields']);
      return;
    }

    let formData = this.form.value;
    let postUser: IPostUser = {
      username: formData.username,
      email: formData.email,
      password: formData.password
    };

    this._userService.postUser(postUser).subscribe(
      result => {
        var singInObj: ISingIn = {
          userName: postUser.username,
          password: postUser.password 
        }
        this.login(singInObj);
      },
      (error: HttpErrorResponse) => {
        if (error.status == 400) {
          this.errors = <Array<string>>error.error;
        } else {
          this.showServerErrorSnackBar(error);
        }
      }
    );
  }

  protected login(singInObject: ISingIn) {
    this._authenticationService.login(singInObject).subscribe(
      result => {
        this._router.navigate(['/home']);
      },
      (error: HttpErrorResponse) => {
        if (error.status == 400) {
          this.errors.push("Invalid password.");
          return;
        }
        if (error.status == 404) {
          this.errors.push("User with received email or name not found.");
          return;
        }
        this.showServerErrorSnackBar(error);
      }
    );
  }

  protected showFormErrors(errors: Array<string>) {
    this.clearFormErrors();
    errors.forEach(element => {
      this.errors.push(element);
    });
  }

  protected clearFormErrors() {
    this.errors.splice(0, this.errors.length);
  }

  showServerErrorSnackBar(error: HttpErrorResponse) {
    let errorString = 'Server error ' + error.status + ': ' + error.statusText;
    this._snackBarsService.showError(errorString)
  }
}
