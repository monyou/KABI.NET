import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class BackendService {

    constructor(
        private http: HttpClient
    ) { }

    backendRequest(requestType, requestTarget, requestData?, responseType: any = 'json'): Observable<any> {
        return this.http.request(
            requestType,
            environment.apiUrl + requestTarget,
            {
                body: requestData,
                observe: "response",
                responseType: responseType
            }
        );
    }
}
