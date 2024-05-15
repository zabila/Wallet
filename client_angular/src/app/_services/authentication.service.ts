import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, map } from 'rxjs';
import { Token } from '../_models/token';
import { UserService } from './user.service';

@Injectable({
    providedIn: 'root',
})
export class AuthenticationService {
    baseUrl = 'http://localhost:5021/api/authentication/';

    private currentTokenSource = new BehaviorSubject<Token | null>(null);
    currentToken$ = this.currentTokenSource.asObservable();

    constructor(
        private http: HttpClient,
        private userService: UserService,
    ) {}

    login(model: any) {
        return this.http.post<Token>(this.baseUrl + 'login', model).pipe(
            map((response: Token) => {
                const token = response;
                if (token) {
                    let jwt_token = response.accessToken;
                    localStorage.setItem('accessToken', jwt_token);
                    this.currentTokenSource.next(token);

                    const decodedToken: any = jwtDecode(jwt_token);
                    const userName = decodedToken.user_name;
                    this.userService.getCurrentUser(userName).subscribe();
                }
            }),
        );
    }

    setCurrentToken(token: Token) {
        this.currentTokenSource.next(token);
    }

    logout() {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('currentUser');
        this.currentTokenSource.next(null);
    }

    getAuthHeader() {
        const token = localStorage.getItem('accessToken');
        return `Bearer ${token}`;
    }
}
