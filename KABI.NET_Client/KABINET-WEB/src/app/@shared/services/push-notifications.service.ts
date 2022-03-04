import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { WebPushVapidDetails, PushNotificationPayload } from '../models/push-notification.model';

@Injectable({
    providedIn: "root"
})
export class PushNotificationsService {
    constructor(
        private http: HttpClient
    ) { }

    public addSub(data: any): Observable<any> {
        return this.http.post(`${environment.pushNotificationServer}AddSubscription`, data);
    }

    public sendPushNotifications(notificationPayload: PushNotificationPayload) {
        let webPushConf = <WebPushVapidDetails>{
            email: 'monyou@abv.bg',
            publicVapidKey: environment.vapidKeys.publicKey,
            privateVapidKey: environment.vapidKeys.privateKey
        };

        return this.http.post(`${environment.pushNotificationServer}SendPushNotification`, { 'webPushConf': webPushConf, 'pushPayload': notificationPayload });
    }
}