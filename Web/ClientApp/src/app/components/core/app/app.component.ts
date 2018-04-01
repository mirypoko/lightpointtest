import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MediaMatcher } from '@angular/cdk/layout';
import { AuthenticationService } from '../../../services/authentication-service.service';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    constructor(
        private _userService: UserService,
        private _authenticationService: AuthenticationService,
        private _snackBarsService: SnackBarsServiceService
    ) {
    }

    ngOnInit(): void {
        this._authenticationService.autoLogin().subscribe();
    }
}