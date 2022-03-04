import { Injectable } from '@angular/core';
import { BackendService } from '../../../@shared/services/backend.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: "root"
})
export class UserService {
    constructor(
        private backendService: BackendService
    ) { }

    public getAll(): Observable<any> {
        return this.backendService.backendRequest('get', `User/AllUsers`);
    }

    public create(data: any): Observable<any> {
        return this.backendService.backendRequest('post', `Account/Register`, data);
    }

    public delete(data: any): Observable<any> {
        return this.backendService.backendRequest('delete', `User/DeleteUser`, data);
    }

    public changePasswordByAdmin(data: any): Observable<any> {
        return this.backendService.backendRequest('put', `User/ChangePasswordByAdmin`, data);
    }

    public changePassword(data: any): Observable<any> {
        return this.backendService.backendRequest('put', `Account/ChangePassword`, data);
    }

    public warnUser(data: any): Observable<any> {
        return this.backendService.backendRequest('post', `User/AddWarning`, data);
    }
}