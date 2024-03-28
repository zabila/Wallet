import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
    title = 'Wallet';
    transactions: any;

    constructor(private http: HttpClient) {}

    ngOnInit(): void {
        this.http
            .get(
                'http://localhost:5032/api/account/fe7d0297-946a-4a7e-8d03-64dee035babf/transactions'
            )
            .subscribe({
                next: (response) => (this.transactions = response),
                error: (error) => console.log(error),
                complete: () => console.log('completed'),
            });
    }
}
