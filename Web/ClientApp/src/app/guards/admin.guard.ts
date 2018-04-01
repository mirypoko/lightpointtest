import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthenticationService } from '../services/authentication-service.service';

@Injectable()
export class AdminGuard implements CanActivate {
  constructor(
    private _router: Router,
    private _authService: AuthenticationService
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return new Observable<boolean>(
      sub => {
        this._authService.currentUserIsAdmin().subscribe(
          isAdmin => {
            if (isAdmin) {
              sub.next(isAdmin);
            } else {
              this._router.navigate(['/404']);
              sub.next(false);
            }
          }
        );
      }
    );
  }
}
