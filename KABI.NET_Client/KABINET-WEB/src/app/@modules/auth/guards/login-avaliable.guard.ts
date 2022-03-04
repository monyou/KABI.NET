import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class LoginAvaliableGuard implements CanActivate {

    constructor(
        private authService: AuthService,
        private router: Router
    ) { }

    canActivate() {

        if (this.authService.isLoggedIn()) {
            if (this.authService.isAdmin())
                this.router.navigate(['/admin-dashboard']);
            else
                this.router.navigate(['/laundry']);

            return false;
        }

        return true;
    }
}