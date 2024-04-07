import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    baseUrl = 'http://localhost:5021/api/user/';

    private currentUserSource = new BehaviorSubject<User | null>(null);
    currentUser$ = this.currentUserSource.asObservable();

    constructor(private http: HttpClient) {}

    getCurrentUser(username: string) {
        return this.http
            .get<User>(this.baseUrl + 'GetCurrent', {
                params: { username },
            })
            .pipe(
                map((response: User) => {
                    const user = response;
                    if (user) {
                        this.currentUserSource.next(user);
                        localStorage.setItem('currentUser', user.userName!);
                    } else {
                        this.currentUserSource.next(null);
                        localStorage.removeItem('accessToken');
                        localStorage.removeItem('currentUser');
                    }
                })
            );
    }

    setCurrentUser(username: string) {
        this.getCurrentUser(username).subscribe();
    }

    register(user: User) {
        return this.http.post(this.baseUrl + 'register', user);
    }
}
