import { Component, OnInit } from '@angular/core';
import { TavernAppointmentService } from '../../services/tavern-appointment.service';
import { GlobalConstants } from 'src/app/@shared/helpers/global-consts';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { TavernAppointment } from '../../models/appointment.model';
import { User } from 'src/app/@modules/admin/models/user.model';

@Component({
  selector: 'app-tavern-scheduler',
  templateUrl: './tavern-scheduler.component.html',
  styleUrls: ['./tavern-scheduler.component.scss']
})
export class TavernSchedulerComponent implements OnInit {

  allAppointments: TavernAppointment[] = [];
  userEmail: string = null;
  userNames: string = null;
  enterAppointmentTitleForm: FormGroup;
  now: Date = new Date();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private tavernAppointmentService: TavernAppointmentService,
    private dialog: MatDialog,
    private snackbar: MatSnackBar,
    private translate: TranslateService
  ) { }

  ngOnInit() {
    this.getAllAppointments();
    this.enterAppointmentTitleForm = this.formBuilder.group({
      Title: ['', Validators.required]
    });
    this.route.params.subscribe(
      params => {
        this.userEmail = params.email;
        this.userNames = params.names;
      }
    );
  }

  initCalendarScheduler() {
    let calendarScheduler = document.querySelector(".my-calendar-scheduler") as HTMLElement;
    calendarScheduler.setAttribute("style", "top: 70px !important;left: calc(50% - 148px) !important;");
  }

  createTavernAppointment(e, enterAppointmentTitleDialog) {
    let pickedDates = e.value as Date[];
    const $dialogRef = this.dialog.open(enterAppointmentTitleDialog, {
      width: "300px"
    });
    $dialogRef.afterClosed().subscribe(
      response => {
        if (response === true) {
          let title = this.enterAppointmentTitleForm.get("Title").value;
          GlobalConstants.enableLoading();
          let addAppointmentRequest = {
            UserEmail: this.userEmail,
            StartTime: pickedDates[0],
            EndTime: pickedDates[1],
            Title: title
          };
          this.tavernAppointmentService.addAppointment(addAppointmentRequest).subscribe(
            success => {
              let appointmentId = success.body;
              this.allAppointments.push({
                id: appointmentId,
                startTime: pickedDates[0],
                endTime: pickedDates[1],
                title: title,
                user: {
                  email: this.userEmail,
                  firstName: this.userNames.split(' ')[0],
                  lastName: this.userNames.split(' ')[1]
                } as User
              } as TavernAppointment);

              GlobalConstants.disableLoading();
              this.snackbar.open(this.translate.instant('TavernSchedulerPage.SnackbarAddAppointmentSuccessfulMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'successSnackbar'
              });
            },
            err => {
              GlobalConstants.disableLoading();
              this.snackbar.open(this.translate.instant('TavernSchedulerPage.SnackbarAddAppointmentErrorMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'dangerSnackbar'
              });
            }
          );
        }

        this.enterAppointmentTitleForm.reset();
      }
    );
  }

  private getAllAppointments() {
    GlobalConstants.enableLoading();
    this.tavernAppointmentService.getAllAppointments().subscribe(
      success => {
        this.allAppointments = success.body as Array<TavernAppointment>;
        GlobalConstants.disableLoading();
      },
      err => {
        GlobalConstants.disableLoading();
      }
    );
  }

  trackByAppointmentId(index: number, appointment: TavernAppointment): string {
    return appointment.id;
  }
}
