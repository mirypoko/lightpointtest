import { Injectable } from '@angular/core';
import { IJwtToken } from '../models/authorization/jwtToken';
import { Observable } from 'rxjs/Observable';
import { ISingIn } from '../models/authorization/singIn';
import { HttpHeaders, HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { UserService } from './user.service';

import { Router } from '@angular/router';
import { IUser } from '../models/user/user';
import { BehaviorSubject } from 'rxjs';
import { ServiceResult } from '../models/serviceResult';

const apiUrl = '/api/authentication';

const AccessTokenLifeTime = 60000;

@Injectable()
export class AuthenticationService {

    public static get UseJwtBearer(): boolean {
        return this._useJwtBearer;
    }

    public get GetJwtToken(): IJwtToken {
        return this._jwtToken;
    }

    public get CurrentUser(): BehaviorSubject<IUser> {
        return this._currentUser;
    }

    private static _useJwtBearer: boolean = true;

    private _jwtToken: IJwtToken;

    private _refreshAccessTokenIntervalId: any;

    private _currentUser: BehaviorSubject<IUser> = new BehaviorSubject(null);

    constructor(private _httpClient: HttpClient,
        private _router: Router) { }

    public currentUserIsAdmin(): Observable<boolean> {
        return new Observable<boolean>(
            sub =>{
                if(this._currentUser.value == null){
                    this.autoLogin().subscribe(
                        isAuth =>{
                            if(isAuth){
                                sub.next(this._currentUser.value.roles.indexOf('ADMIN') != -1);
                            }else{
                                return false;
                            }
                        }
                    );
                }else{
                    sub.next(this._currentUser.value.roles.indexOf('ADMIN') != -1);
                }
            }
        );
    }

    public isAuthenticated(): Observable<boolean> {
        return this._httpClient.get<boolean>(apiUrl + "/IsAuthenticated");
    }

    public autoLogin(): Observable<boolean> {
        return new Observable<boolean>(
            sub => {
                if (AuthenticationService.UseJwtBearer) {
                    this.loadJwtTokenFromLocalStorage().subscribe(
                        jwtToken => {
                            if (jwtToken == null) {
                                sub.next(false);
                            } else {
                                this.getNewJwtByRefreshToken().subscribe(
                                    newJwt => {
                                        this.setJwtTokenInToLocalStorage(newJwt);
                                        this.loadCurrentUserFromServer().subscribe(
                                            result => sub.next(result),
                                            error => sub.error(error)
                                        );
                                    },
                                    error => {
                                        this.logoutJwt();
                                        sub.error(error)
                                    }
                                );
                            }
                        }
                    );
                } else {
                    this.loadCurrentUserFromServer().subscribe(
                        result => sub.next(result),
                        error => sub.error(error)
                    );
                }
            }
        );
    }

    public login(singIn: ISingIn): Observable<any> {
        if (AuthenticationService.UseJwtBearer) {
            return this.loginJwt(singIn);
        } else {
            return this.loginCookes(singIn);
        }
    }

    public logout(): void {
        if (AuthenticationService.UseJwtBearer) {
            this.logoutJwt();
            this._currentUser.next(null);
            this._router.navigate(['/home']);
        } else {
            this.logoutCookies()
                .subscribe(
                    result => {
                        this._currentUser.next(null);
                        this._router.navigate(['/home']);
                    },
                    error => {
                        console.error(error);
                    }
                );
        }
    }

    private loginCookes(singIn: ISingIn): Observable<any> {
        return new Observable<any>(
            sub => {
                let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
                this._httpClient.post(apiUrl + '/Cookies', JSON.stringify(singIn), { headers: httpHeaders })
                    .subscribe(
                        result => {
                            this.loadCurrentUserFromServer().subscribe(
                                r => sub.next(true),
                                error => sub.error(error)
                            );
                        },
                        error => sub.error(error)
                    );
            }
        );
    }

    private loginJwt(singIn: ISingIn): Observable<any> {
        return new Observable<any>(
            sub => {
                this.getJwtTokenFromServer(singIn).subscribe(
                    result => {
                        this._jwtToken = result;
                        this.setJwtTokenInToLocalStorage(result);
                        this.runGetJwtTokenSheduler();
                        this.loadCurrentUserFromServer().subscribe(
                            r => sub.next(true),
                            error => sub.error(error)
                        );
                    }, error => {
                        sub.error(error);
                    }
                )
            }
        )
    }

    private logoutCookies(): Observable<any> {
        let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
        return this._httpClient.post(apiUrl + '/logout', null);
    }

    private logoutJwt(): void {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userId');
        this._jwtToken = null;
        this.stopGetAccessjwtTokenSheduler();
    }

    private stopGetAccessjwtTokenSheduler() {
        clearTimeout(this._refreshAccessTokenIntervalId);
    }

    private runGetJwtTokenSheduler() {
        this._refreshAccessTokenIntervalId = setInterval(() => {
            this.refreshJwtTokenAction();
            console.log("Updated Jwt token");
        }, AccessTokenLifeTime - 10000);
    }

    private refreshJwtTokenAction() {
        if (this._jwtToken != null) {
            this.getNewJwtByRefreshToken().subscribe(
                result => {
                    this.setJwtTokenInToLocalStorage(result);
                    this._jwtToken = result;
                },
                (error: HttpErrorResponse) => {
                    if (error.status == 400) {
                        this.stopGetAccessjwtTokenSheduler();
                        this.logoutJwt();
                    }
                }
            )
        } else {
            this.stopGetAccessjwtTokenSheduler();
            this.logoutJwt();
        }
    }

    private setJwtTokenInToLocalStorage(jwtToken: IJwtToken): void {
        localStorage.setItem('refreshToken', jwtToken.refreshToken);
        localStorage.setItem('accessToken', jwtToken.accessToken);
        localStorage.setItem('userId', jwtToken.userId);
    }

    private loadCurrentUserFromServer(): Observable<any> {
        return new Observable<any>(
            sub => {
                this.getCurrentUserFromServer().subscribe(
                    result => {
                        this._currentUser.next(result);
                        sub.next(true);
                    },
                    error => {
                        sub.error();
                    })
            }
        );
    }

    private loadJwtTokenFromLocalStorage(): Observable<IJwtToken> {
        return new Observable<IJwtToken>(
            sub => {
                let jwtToken: IJwtToken = {
                    refreshToken: localStorage.getItem('refreshToken'),
                    accessToken: localStorage.getItem('accessToken'),
                    userId: localStorage.getItem('userId')
                }
                if (jwtToken.accessToken == null || jwtToken.refreshToken == null) {
                    this._jwtToken = null;
                    sub.next(null);
                } else {
                    this._jwtToken = jwtToken;
                    sub.next(jwtToken);
                }
            }
        );
    }

    private getNewJwtByRefreshToken(): Observable<IJwtToken> {
        let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
        let params = { refreshToken: this._jwtToken.refreshToken };
        return this._httpClient.post<IJwtToken>(apiUrl + '/UpdateJwtToken', JSON.stringify(params), { headers: httpHeaders });
    }

    private getJwtTokenFromServer(singIn: ISingIn): Observable<IJwtToken> {
        let httpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
        return this._httpClient.post<IJwtToken>(apiUrl, JSON.stringify(singIn), { headers: httpHeaders });
    }

    private getCurrentUserFromServer(): Observable<IUser> {
        return this._httpClient.get<IUser>(apiUrl + '/current');
    }
}
