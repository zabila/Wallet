import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { AuthenticationService } from '../_services/authentication.service';
import { UserService } from './../_services/user.service';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard {
    constructor(
        private authenticationService: AuthenticationService,
        private userService: UserService,
        private router: Router
    ) {}

    canActivate = () => {
        return this.isTokenValid();
    };

    isTokenValid() {
        return this.authenticationService.currentToken$.pipe(
            map((token) => {
                if (token) {
                    return true;
                }
                console.log('Not authenticated, redirecting to login page');
                this.router.navigate(['/login']);
                return false;
            })
        );
    }
}
