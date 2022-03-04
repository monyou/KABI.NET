import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/@modules/auth/services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class AccessTokenInterceptorService implements HttpInterceptor {

    constructor(
        private authService: AuthService
    ) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
        const token = this.authService.getAccessToken();
        let newHeaders = req.headers;
        if (token) {
            newHeaders = newHeaders.append('X-AccessToken', token);
        }
        const authReq = req.clone({ headers: newHeaders });
        return next.handle(authReq);
    }
}