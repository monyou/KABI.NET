import { Injectable } from '@angular/core';
import { BackendService } from '../../../@shared/services/backend.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: "root"
})
export class TavernAppointmentService {
    constructor(
        private backendService: BackendService
    ) { }

    public addAppointment(data: any): Observable<any> {
        return this.backendService.backendRequest('post', `TavernAppointment/AddAppointment`, data);
    }

    public getAllAppointments(): Observable<any> {
        return this.backendService.backendRequest('get', `TavernAppointment/AllAppointments`);
    }

    public getAppointmentById(data: any): Observable<any> {
        return this.backendService.backendRequest('post', `TavernAppointment/AppointmentById`, data);
    }
}