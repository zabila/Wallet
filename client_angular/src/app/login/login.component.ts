import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { Token } from '../_models/token';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.css',
})
export class LoginComponent {
    model: any = {};
    currentToken$: Observable<Token | null> = of(null);

    constructor(
        private authenticationService: AuthenticationService,
        private route: Router,
        private toastr: ToastrService,
    ) {}

    ngOnInit(): void {
        this.currentToken$ = this.authenticationService.currentToken$;
    }

    login() {
        this.authenticationService.login(this.model).subscribe({
            next: () => {
                this.route.navigate(['/home']);
            },
            error: (error) => {
                console.log(error);
                this.toastr.error('Invalid username or password');
            },
        });
    }

    register() {
        this.route.navigate(['/register']);
    }
}
