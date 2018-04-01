import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthenticationService } from '../../../services/authentication-service.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IUser } from '../../../models/user/user';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.scss']
})
export class UserMenuComponent implements OnInit, OnDestroy  {

  currentUser: IUser = null;

  currentUserSubscription: Subscription;

  constructor(
    private _authenticationService: AuthenticationService,
    private _userService: UserService,
    private _router: Router) { }

  logout() {
    this._authenticationService.logout();
  }

  ngOnInit() {
    this.currentUserSubscription = this._authenticationService.CurrentUser.subscribe(
      result => this.currentUser = result
    );
  }

  ngOnDestroy(): void {
    this.currentUserSubscription.unsubscribe();
  }
}
