import { Observable } from 'rxjs/Observable';
import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';

import { HttpResponse } from '@angular/common/http';

import 'rxjs/add/operator/do';
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/catch';
import { AuthenticationService } from '../services/authentication-service.service';

export class JwtInterceptor implements HttpInterceptor {

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        if(AuthenticationService.UseJwtBearer){
            let token = localStorage.getItem('accessToken');
            if (token != null) {
                request = request.clone({
                    setHeaders: {
                        Authorization: `Bearer `+ token
                    }
                });
            } else {
                request = request.clone();
            }
        } else {
            request = request.clone();
        }

        return next.handle(request);
    }
}