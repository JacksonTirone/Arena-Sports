import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../service/api.service';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(private authService: AuthService, private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            if (err.status === 401 || err.status === 403) {
                // auto logout if 401 response returned from api
                this.authService.logout();
                //  location.reload(true);
                //this.router.navigate(["/login"]);

                return throwError("Não autorizado");
            } else {
                const error = err.error.message || err.statusText;

                if (error === 'OK') {
                    let token = err.error.text;
                    localStorage.setItem('token', '' + token);
                    this.router.navigate(["/dashboard"]);
                }

                return throwError(error);
            }
        }))
    }
}