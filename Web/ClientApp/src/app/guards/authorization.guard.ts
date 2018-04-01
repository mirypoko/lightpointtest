import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { retry } from 'rxjs/operator/retry';
import { AuthenticationService } from '../services/authentication-service.service';

@Injectable()
export class AuthorizationGuard implements CanActivate {

  public static BackRoutUrl: string = null;

  constructor(
    private _router: Router,
    private _authService: AuthenticationService
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

    if (this._authService.isAuthenticated()) {
      return true;
    } else {
      AuthorizationGuard.BackRoutUrl = this._router.url;
      this._router.navigate(['/singin']);
      return false;
    }
  }
}
