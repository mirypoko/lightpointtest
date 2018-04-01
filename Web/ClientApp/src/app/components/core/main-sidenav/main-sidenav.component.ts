import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { Route } from '@angular/compiler/src/core';
import { IUser } from '../../../models/user/user';
import { Subscription } from 'rxjs';
import { UserService } from '../../../services/user.service';
import { AuthenticationService } from '../../../services/authentication-service.service';

@Component({
  selector: 'app-main-sidenav',
  templateUrl: './main-sidenav.component.html',
  styleUrls: ['./main-sidenav.component.css']
})
export class MainSidenavComponent implements OnDestroy {

  mobileQuery: MediaQueryList;

  currentUser: IUser = null;

  currentUserIsAdmin: boolean = false;

  currentUserSubscription: Subscription;

  private _mobileQueryListener: () => void;

  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private media: MediaMatcher,
    private _userService: UserService,
    private _authService: AuthenticationService) {

    this.mobileQuery = media.matchMedia('(max-width: 650px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
    this.currentUserSubscription = this._authService.CurrentUser.subscribe(
      result => {
        this.currentUser = result;
        this._authService.currentUserIsAdmin().subscribe(
          sub => this.currentUserIsAdmin = sub
        )
      }
    );

  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
    this.currentUserSubscription.unsubscribe();
  }


}
