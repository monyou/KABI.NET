import { Component, OnInit, Renderer2 } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SwPush, SwUpdate } from '@angular/service-worker';
import { environment } from 'src/environments/environment';
import { PushNotificationsService } from './@shared/services/push-notifications.service';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'KABINET-WEB';

  constructor(
    private swPush: SwPush,
    private swUpdate: SwUpdate,
    private pushNotificationService: PushNotificationsService,
    private translate: TranslateService,
    private router: Router,
    private renderer: Renderer2
  ) { }

  ngOnInit() {
    // Language
    this.translate.addLangs(['bg', 'en']);
    this.translate.setDefaultLang('bg')
    this.translate.use('bg');

    // Push Notifications
    this.swPush.requestSubscription({
      serverPublicKey: environment.vapidKeys.publicKey
    }).then(
      sub => {
        this.pushNotificationService.addSub(sub).subscribe(
          () => {
            console.log("Subscription saved to database!");
          },
          err => {
            console.error("Could not save subscription to database!", err);
          }
        );
      }
    ).catch(
      err => console.error("Could not subscribe to notifications!", err)
    );

    this.router.events.subscribe(
      event => {
        if (event instanceof NavigationStart) {
          if (event.url !== "/login") {
            this.renderer.setStyle(document.body, "background", "url('../assets/backgrounds/body-liquid-background-pages.svg') no-repeat center bottom/cover");
          } else {
            this.renderer.setStyle(document.body, "background", "url('../assets/backgrounds/body-liquid-background-main.svg') no-repeat center bottom/cover");
          }
        }

        if (event instanceof NavigationEnd) {
          // Check if new version is available
          if (this.swUpdate.isEnabled) {
            this.swUpdate.checkForUpdate().finally();
          }
        }
      }
    );

    // Update Service worker when new version is available
    if (this.swUpdate.isEnabled) {
      this.swUpdate.available.subscribe(
        () => {
          this.swUpdate.activateUpdate().finally(
            () => {
              window.location.reload();
            }
          );
        }
      );
    }
  }
}
