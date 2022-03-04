export class PushNotificationSub {
    endpoint: string;
    expirationTime: number;
    keys: {
        p256dh: string;
        auth: string;
    }
}

export class PushNotificationPayload {
    notification: {
        title: string;
        body: string;
        icon: string;
        vibrate: number[],
        data: {
            dateOfArrival: number,
            primaryKey: number
        },
        actions: {
            action: string;
            title: string;
        }[]
    }
}

export class WebPushVapidDetails {
    email: string;
    publicVapidKey: string;
    privateVapidKey: string;
}