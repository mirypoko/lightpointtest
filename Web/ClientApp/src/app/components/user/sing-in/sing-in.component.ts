import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../services/authentication-service.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { ISingIn } from '../../../models/authorization/singIn';
import { HttpErrorResponse } from '@angular/common/http';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';
import { AuthorizationGuard } from '../../../guards/authorization.guard';

@Component({
  selector: 'app-sing-in',
  templateUrl: './sing-in.component.html',
  styleUrls: ['./sing-in.component.scss']
})
export class SingInComponent implements OnInit {

  constructor(private _authenticationService: AuthenticationService,
    private _router: Router,
    private _userService: UserService,
    private _snackBarsService: SnackBarsServiceService) { }

  errors: string[] = [];

  form = new FormGroup({
    "userName": new FormControl("", [Validators.required, Validators.minLength(4), Validators.pattern("^[0-9]*[A-Za-z][A-Za-z0-9]*$")]),
    "password": new FormControl("", [Validators.required, Validators.minLength(8)]),
  });

  ngOnInit() {
  }

  submit() {
    this.clearFormErrors();

    if (!this.form.valid) {
      this.showFormErrors(['Fill all fields']);
      return;
    }

    let formData = this.form.value;

    let singInObject: ISingIn = {
      userName: formData.userName,
      password: formData.password
    };

    this.login(singInObject);
  }

  protected login(singInObject: ISingIn) {
    this._authenticationService.login(singInObject).subscribe(
      result => {
        if(AuthorizationGuard.BackRoutUrl != null){
          this._router.navigate([AuthorizationGuard.BackRoutUrl]);
        }else{
          this._router.navigate(['/home']);
        }
      },
      (error: HttpErrorResponse) => {
        if(error == undefined){
          this.errors.push("Unknown error.");
          return;
        }
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

  protected clearFormErrors() {
    this.errors.splice(0, this.errors.length);
  }

  protected showFormErrors(errors: Array<string>) {
    this.clearFormErrors();
    errors.forEach(element => {
      this.errors.push(element);
    });
  }

  showServerErrorSnackBar(error: HttpErrorResponse) {
    let errorString = 'Server error ' + error.status + ': ' + error.statusText;
    this._snackBarsService.showError(errorString)
  }
}
