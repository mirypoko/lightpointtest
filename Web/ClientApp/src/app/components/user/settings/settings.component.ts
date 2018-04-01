import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IUser } from '../../../models/user/user';
import { Subscription } from 'rxjs';
import { UserService } from '../../../services/user.service';
import { AuthenticationService } from '../../../services/authentication-service.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { ISingIn } from '../../../models/authorization/singIn';
import { IPutUser } from '../../../models/user/putUser';
import { IPutUserPassword } from '../../../models/user/putUserPassword';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  errors: string[] = [];

  currentUser: IUser = null;

  currentUserSubscription: Subscription;

  form = new FormGroup({
    "userName": new FormControl("", [Validators.required, Validators.minLength(4), Validators.pattern("^[0-9]*[A-Za-z][A-Za-z0-9]*$")]),
    "email": new FormControl("", [Validators.required, Validators.email]),
    "currentPassword": new FormControl("", [Validators.required, Validators.minLength(8)]),
    "password": new FormControl("", [Validators.minLength(8)]),
    "confirmPassword": new FormControl("", [Validators.minLength(8)])
  });


  constructor(private _userService: UserService,
    private _authenticationService: AuthenticationService,
    private _router: Router,
    private _snackBarsService: SnackBarsServiceService) { }

  ngOnInit() {
    this.currentUserSubscription = this._authenticationService.CurrentUser.subscribe(
      result => {
        if (result != null) {
          this.currentUser = result;
          this.form.controls['userName'].setValue(result.userName);
          this.form.controls['email'].setValue(result.email);
        }
      }
    );
  }

  submit() {

    this.clearFormErrors();

    if (!this.form.valid) {
      this.showFormErrors(['Fill in required fields']);
      return;
    }

    let formData = this.form.value;
    let putUser: IPutUser = {
      id: this.currentUser.id,
      userName: formData.userName,
      email: formData.email,
      currentPassword: formData.currentPassword
    };

    if (formData.password.length > 0) {
      if (formData.password != formData.confirmPassword) {
        this.showFormErrors(['The new password does not match confirmation']);
        return;
      }
    }

    this._userService.putUser(putUser).subscribe(
      result => {
        if (formData.password.length > 0) {
          this.putPasswordAndSingIn(formData.userName, formData.currentPassword, formData.password)
        } else {
          this.showSuccessSnackBar('Data updated');
        }
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

  protected putPasswordAndSingIn(userName: string, oldPassword: string, newPassword: string) {
    let putUserPassword: IPutUserPassword = {
      id: this.currentUser.id,
      newPassword: newPassword,
      currentPassword: oldPassword
    }

    this._userService.putPassword(putUserPassword).subscribe(
      result => {
        let singInObject: ISingIn = {
          userName: userName,
          password: newPassword
        };
        this.login(singInObject);
        this.showSuccessSnackBar('Data updated');
      },
      (error: HttpErrorResponse) => {
        if (error.status == 400) {
          this.errors = <Array<string>>error.error;
          return;
        } else {
          this.showServerErrorSnackBar(error);
        }
      }
    );
  }

  protected clearPasswordData() {
    this.form.controls['currentPassword'].reset();
    this.form.controls['currentPassword'].setValue('');
    this.form.controls['password'].reset();
    this.form.controls['password'].setValue('');
    this.form.controls['confirmPassword'].reset();
    this.form.controls['confirmPassword'].setValue('');
  }

  protected login(singInObject: ISingIn) {
    this._authenticationService.login(singInObject).subscribe(
      result => {

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

  ngOnDestroy(): void {
    this.currentUserSubscription.unsubscribe();
  }

  protected showServerErrorSnackBar(error: HttpErrorResponse) {
    let errorString = 'Server error ' + error.status + ': ' + error.statusText;
    this._snackBarsService.showError(errorString, true)
  }

  protected showSuccessSnackBar(message: string) {
    this._snackBarsService.showSuccess(message)
  }

}
