import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../_services/user.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
})
export class RegisterComponent {
    model: any = {};

    constructor(
        private router: Router,
        private userService: UserService,
        private toastr: ToastrService
    ) {}

    register() {
        this.validationModel();
        this.userService.register(this.model).subscribe({
            next: () => {
                this.toastr.success('Registration successful');
                this.router.navigate(['/login']);
            },
            error: (error) => {
                console.log(error);
                if (error.error && error.error.length > 0) {
                    error.error.forEach((err: any) => {
                        this.toastr.error(err.description);
                    });
                }
            },
        });
    }

    login() {
        this.router.navigate(['/login']);
    }

    validationModel() {
        if (!this.model.email) {
            this.toastr.error('Email address is required');
            return false;
        }
        if (!this.validateEmail(this.model.email)) {
            this.toastr.error('Invalid email address');
            return false;
        }
        if (!this.model.password) {
            this.toastr.error('Password is required');
            return false;
        }
        if (this.model.password !== this.model.confirmPassword) {
            this.toastr.error('Passwords do not match');
            return false;
        }
        if (!this.model.firstName) {
            this.toastr.error('First name is required');
            return false;
        }
        if (!this.model.lastName) {
            this.toastr.error('Last name is required');
            return false;
        }
        if (!this.model.telegramUserId) {
            this.toastr.error('Telegram ID is required');
            return false;
        }
        if (!this.model.telegramUsername) {
            this.toastr.error('Telegram username is required');
            return false;
        }
        return true;
    }

    private validateEmail(email: string) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }
}
