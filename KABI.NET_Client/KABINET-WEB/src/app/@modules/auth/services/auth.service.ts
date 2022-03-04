import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserRoles } from '../../../@shared/enums/user-role.enum';

@Injectable({
    providedIn: "root"
})
export class AuthService {
    private readonly AccessToken = 'AccessToken';

    constructor(
        private http: HttpClient
    ) { }

    login(data): Observable<any> {
        return this.http.post(environment.apiUrl + 'Account/Login', { "Email": data.EmailAddress, "Password": data.Password }).pipe(
            tap(
                result => this.setAccessToken(result)
            )
        );
    }

    logout(): boolean {
        this.clearTokens();
        return true;
    }

    isLoggedIn(): boolean {
        if (this.getAccessToken() === null || this.getAccessToken() === '')
            return false;
        else
            return true;
    }

    getAccessToken() {
        return localStorage.getItem(this.AccessToken);
    }

    isAdmin(): boolean {
        let role = this.decodeJWT(this.getAccessToken()).Role;

        return role === UserRoles.Admin ? true : false;
    }

    decodeJWT(token) {
        let base64Url = token.split('.')[1];
        let base64 = decodeURIComponent(atob(base64Url).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(base64);
    };

    private setAccessToken(token: string) {
        localStorage.setItem(this.AccessToken, token);
    }

    private clearTokens() {
        localStorage.removeItem(this.AccessToken);
    }
}