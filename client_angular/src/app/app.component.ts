import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './_services/authentication.service';
import { UserService } from './_services/user.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
    title = 'Wallet';

    constructor(
        private accountService: AuthenticationService,
        private userService: UserService
    ) {}

    ngOnInit(): void {
        this.setCurrentToken();
        this.setCurrentUser();
    }

    setCurrentToken() {
        const token: string = localStorage.getItem('accessToken') || '';
        if (!token) return;
        this.accountService.setCurrentToken({
            accessToken: token,
            refreshToken: '',
        });
    }

    setCurrentUser() {
        const username: string = localStorage.getItem('currentUser') || '';
        if (!username) return;
        this.userService.setCurrentUser(username);
    }
}
