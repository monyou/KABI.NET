import { Injectable } from '@angular/core';
import { BackendService } from 'src/app/@shared/services/backend.service';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: "root"
})
export class ReportService {

    constructor(
        private backendService: BackendService
    ) { }

    public sendLaundryFullReport(): Observable<any> {
        return this.backendService.backendRequest('get', `Reports/FullLaundryReport`);
    }

    public sendTavernAppointmentFullReport(): Observable<any> {
        return this.backendService.backendRequest('get', `Reports/FullTavernAppointmentReport`);
    }
}