import { Component, OnInit } from '@angular/core';
import { LaundryService } from 'src/app/@modules/laundry/services/laundry.service';
import { padNumber } from 'src/app/@shared/helpers/functions';
import { GlobalConstants } from 'src/app/@shared/helpers/global-consts';

@Component({
  selector: 'app-laundry',
  templateUrl: './laundry.component.html',
  styleUrls: ['./laundry.component.scss']
})
export class LaundryComponent implements OnInit {
  laundryTimerText: string = null;
  laundryTimer: any = null;
  laundryStatus: boolean = false;

  constructor(
    private laundryService: LaundryService
  ) { }

  ngOnInit() {
    this.checkLaundryWorking();
  }

  checkLaundryWorking() {
    GlobalConstants.enableLoading();
    this.laundryService.checkStatus().subscribe(
      result => {
        if (result.body && result.body.endTime === null) {
          this.laundryTimerText = '00:00:00';
          this.laundryTimer = this.initTimer(new Date(result.body.startTime));
          this.laundryStatus = true;
        }
        GlobalConstants.disableLoading();
      },
      err => {
        GlobalConstants.disableLoading();
      }
    );
  }

  private initTimer(oldDate: Date) {
    return setInterval(() => {
      let newDate = new Date();
      let startPointForTimer = new Date(oldDate.getFullYear(), oldDate.getMonth(), oldDate.getDate(), newDate.getHours() - oldDate.getHours(), newDate.getMinutes() - oldDate.getMinutes(), newDate.getSeconds() - oldDate.getSeconds());
      this.laundryTimerText = `${padNumber(startPointForTimer.getHours(), 2)}:${padNumber(startPointForTimer.getMinutes(), 2)}:${padNumber(startPointForTimer.getSeconds(), 2)}`;
    }, 1000);
  }

}
