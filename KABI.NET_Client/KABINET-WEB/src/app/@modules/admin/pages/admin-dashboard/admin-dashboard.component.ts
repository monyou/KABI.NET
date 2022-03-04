import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { User } from 'src/app/@modules/admin/models/user.model';
import { UserService } from 'src/app/@modules/admin/services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { UserRoles } from 'src/app/@shared/enums/user-role.enum';
import { TranslateService } from '@ngx-translate/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LaundryService } from 'src/app/@modules/laundry/services/laundry.service';
import { padNumber } from 'src/app/@shared/helpers/functions';
import { PushNotificationsService } from 'src/app/@shared/services/push-notifications.service';
import { PushNotificationPayload } from 'src/app/@shared/models/push-notification.model';
import { GlobalConstants } from 'src/app/@shared/helpers/global-consts';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  laundryStatus: boolean = false;
  laundryTimerText: string = null;
  laundryTimer: any = null;
  addUserForm: FormGroup = null;
  editUserForm: FormGroup = null;
  allUsers: User[] = [];
  userTableDisplayedColumns: string[] = ['Id', 'FirstName', 'LastName', 'Room', 'Actions'];
  userTableDataSource: MatTableDataSource<User>;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private userService: UserService,
    private dialog: MatDialog,
    private snackbar: MatSnackBar,
    private translate: TranslateService,
    private formBuilder: FormBuilder,
    private laundryService: LaundryService,
    private pushNotificationsService: PushNotificationsService,
    private router: Router
  ) { }

  ngOnInit() {
    this.checkLaundryWorking();
    this.getUsers();
  }

  getUsers() {
    GlobalConstants.enableLoading();
    this.userService.getAll().subscribe(
      success => {
        this.allUsers = success.body as Array<User>;
        this.allUsers = this.allUsers.filter(u => u.role !== UserRoles.Admin);
        this.userTableDataSource = new MatTableDataSource(this.allUsers);
        this.userTableDataSource.paginator = this.paginator;
        this.userTableDataSource.sort = this.sort;

        GlobalConstants.disableLoading();
      },
      err => {
        GlobalConstants.disableLoading();
      }
    );
  }

  addUser(addUserDialog: TemplateRef<any>) {
    this.addUserForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      room: ['', [Validators.required, Validators.pattern('^[1-9]{1}[0-9]{0,2}$')]]
    });

    const $dialogRef = this.dialog.open(addUserDialog, {
      width: "300px"
    });
    $dialogRef.afterClosed().subscribe(
      response => {
        if (response) {
          GlobalConstants.enableLoading();
          let newUserRequest = {
            "Email": response.email,
            "FirstName": response.firstName,
            "LastName": response.lastName,
            "Room": response.room
          };
          this.userService.create(newUserRequest).subscribe(
            success => {
              response.id = success.body;
              response.role = UserRoles.Member;
              response.warnings = 0;
              this.allUsers.push(response as User);
              this.userTableDataSource.data = this.allUsers;

              GlobalConstants.disableLoading();
              this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarAddSuccessfulMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'successSnackbar'
              });
            },
            error => {
              GlobalConstants.disableLoading();
              this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarAddErrorMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'dangerSnackbar'
              });
            }
          );
        }
      }
    );
  }

  editUser(userEmail: string, editUserDialog: TemplateRef<any>) {
    this.editUserForm = this.formBuilder.group({
      NewPassword: ['', Validators.required],
    });

    const $dialogRef = this.dialog.open(editUserDialog, {
      width: "300px"
    });
    $dialogRef.afterClosed().subscribe(
      response => {
        if (response) {
          GlobalConstants.enableLoading();
          let editUserRequest = {
            "Email": userEmail,
            "NewPassword": response.NewPassword
          };
          this.userService.changePasswordByAdmin(editUserRequest).subscribe(
            () => {
              GlobalConstants.disableLoading();
              this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarEditSuccessfulMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'successSnackbar'
              });
            },
            error => {
              GlobalConstants.disableLoading();
              this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarEditErrorMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'dangerSnackbar'
              });
            }
          );
        }
      }
    );
  }

  deleteUser(userEmail: string, deleteDialog: TemplateRef<any>) {
    const $dialogRef = this.dialog.open(deleteDialog, {
      width: "300px"
    });
    $dialogRef.afterClosed().subscribe(
      response => {
        if (response) {
          GlobalConstants.enableLoading();
          let deleteUserRequest = {
            "Email": userEmail
          };
          this.userService.delete(deleteUserRequest).subscribe(
            () => {
              this.allUsers.splice(this.allUsers.findIndex(u => u.email === userEmail), 1);
              this.userTableDataSource.data = this.allUsers;

              GlobalConstants.disableLoading();
              this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarDeleteSuccessfulMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'successSnackbar'
              });
            },
            error => {
              GlobalConstants.disableLoading();
              this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarDeleteErrorMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'dangerSnackbar'
              });
            }
          );
        }
      }
    );
  }

  applyFilterToUsersTable(filterValue: string) {
    this.userTableDataSource.filter = filterValue.trim().toLowerCase();

    if (this.userTableDataSource.paginator) {
      this.userTableDataSource.paginator.firstPage();
    }
  }

  startLaundry(userEmail: string) {
    let laundryRequest = {
      "Email": userEmail
    };
    GlobalConstants.enableLoading();
    this.laundryService.start(laundryRequest).subscribe(
      () => {
        this.laundryTimerText = '00:00:00';
        this.laundryStatus = true;
        this.laundryTimer = this.initTimer(new Date());
        GlobalConstants.disableLoading();
      },
      err => {
        GlobalConstants.disableLoading();
      }
    );
  }

  stopLaundry(payDialog: TemplateRef<any>) {
    GlobalConstants.enableLoading();
    this.laundryService.stop().subscribe(
      result => {
        this.laundryTimerText = null;
        clearInterval(this.laundryTimer);
        this.laundryStatus = false;
        GlobalConstants.disableLoading();
        let lTime = result.body.totalLaundryTime;
        let lHours = lTime.hours;
        let lMinutes = lTime.minutes;

        // Send push notification that laundry is free (for all subscribed users)
        let notificationPayload = <PushNotificationPayload>{
          notification: {
            title: "KABI.NET Laundry Status",
            body: "The laundry is available now!",
            icon: "../../../../../assets/logo/main-logo.png",
            vibrate: [100, 50, 100],
            data: {
              dateOfArrival: Date.now(),
              primaryKey: 1
            },
            actions: [{
              action: "explore",
              title: "Check it out"
            }]
          }
        };
        this.pushNotificationsService.sendPushNotifications(notificationPayload).subscribe();

        const $dialogRef = this.dialog.open(payDialog, {
          width: "300px",
          disableClose: true,
          data: {
            model: result.body,
            laundryHours: lHours,
            laundryMinutes: lMinutes
          }
        });
        $dialogRef.afterClosed().subscribe(
          response => {
            if (response) {
              GlobalConstants.enableLoading();
              let payRequest = {
                "LaundryId": response
              };
              this.laundryService.pay(payRequest).subscribe(
                () => {
                  GlobalConstants.disableLoading();
                  this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarPayLaundrySuccessfulMsg'), 'OK', {
                    duration: 3000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                    panelClass: 'successSnackbar'
                  });
                },
                error => {
                  GlobalConstants.disableLoading();
                  this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarPayLaundryErrorMsg'), 'OK', {
                    duration: 3000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                    panelClass: 'dangerSnackbar'
                  });
                }
              );
            } else {
              this.snackbar.open(this.translate.instant('AdminDashboardPage.SnackbarPayLaundryInfoMsg'), 'OK', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
                panelClass: 'warningSnackbar'
              });
            }
          }
        );
      },
      err => {
        GlobalConstants.disableLoading();
      }
    );
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

  warnUserForBlacklist(user: User) {
    GlobalConstants.enableLoading();
    this.userService.warnUser({ "Email": user.email }).subscribe(
      () => {
        let newUser = user;
        let message = null;
        newUser.warnings++;
        if (newUser.warnings === 3) {
          newUser.role = UserRoles.Blocked;
          message = this.translate.instant('AdminDashboardPage.WarnUserForBlackListSuccessfulMsg2');
        } else {
          message = this.translate.instant('AdminDashboardPage.WarnUserForBlackListSuccessfulMsg', { warnings: (3 - newUser.warnings) });
        }
        this.allUsers.splice(this.allUsers.indexOf(user), 1, newUser);
        this.userTableDataSource.data = this.allUsers;
        GlobalConstants.disableLoading();

        this.snackbar.open(message, 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'successSnackbar'
        });
      },
      err => {
        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('AdminDashboardPage.WarnUserForBlackListErrorMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'dangerSnackbar'
        });
      }
    );
  }

  openTavernScheduler(userEmail: string, firstName: string, lastName: string) {
    this.router.navigate(["/tavern-scheduler", { email: userEmail, names: `${firstName} ${lastName}` }]);
  }

  private initTimer(oldDate: Date) {
    return setInterval(() => {
      let newDate = new Date();
      let startPointForTimer = new Date(oldDate.getFullYear(), oldDate.getMonth(), oldDate.getDate(), newDate.getHours() - oldDate.getHours(), newDate.getMinutes() - oldDate.getMinutes(), newDate.getSeconds() - oldDate.getSeconds());
      this.laundryTimerText = `${padNumber(startPointForTimer.getHours(), 2)}:${padNumber(startPointForTimer.getMinutes(), 2)}:${padNumber(startPointForTimer.getSeconds(), 2)}`;
    }, 1000);
  }
}
