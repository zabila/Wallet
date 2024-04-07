import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { AuthenticationService } from '../_services/authentication.service';
import { UserService } from './../_services/user.service';

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrl: './nav.component.css',
})
export class NavComponent implements OnInit {
    currentUser$: Observable<User | null> = of(null);

    constructor(
        private authenticationService: AuthenticationService,
        private userService: UserService,
        private router: Router
    ) {}

    ngOnInit(): void {
        this.currentUser$ = this.userService.currentUser$;
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}
