import {
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable({
    providedIn: 'root',
})
export class AuthInterceptor implements HttpInterceptor {
    urlPattern: string;

    constructor(
        private authenticationService: AuthenticationService,
        private router: Router
    ) {
        this.urlPattern = `http://localhost:5021/api`;
    }

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        if (request.url.startsWith(this.urlPattern)) {
            let authHeader = this.authenticationService.getAuthHeader();
            request = request.clone({
                setHeaders: {
                    Authorization: authHeader,
                },
            });
        }
        return next.handle(request).pipe(
            tap(
                () => {},
                (error) => {
                    if (error.status === 401) {
                        this.authenticationService.logout();
                        this.router.navigate(['/login']);
                    }
                }
            )
        );
    }
}
