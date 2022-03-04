import { Injectable } from '@angular/core';
import { BackendService } from '../../../@shared/services/backend.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: "root"
})
export class LaundryService {
    constructor(
        private backendService: BackendService
    ) { }

    public checkStatus(): Observable<any> {
        return this.backendService.backendRequest('get', `Laundry/GetLastRecord`);
    }

    public pay(data: any): Observable<any> {
        return this.backendService.backendRequest('post', `Laundry/Pay`, data);
    }

    public start(data: any): Observable<any> {
        return this.backendService.backendRequest('post', `Laundry/Start`, data);
    }

    public stop(): Observable<any> {
        return this.backendService.backendRequest('post', `Laundry/Stop`);
    }
}