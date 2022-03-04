import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../@modules/auth/services/auth.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ReportService } from 'src/app/@shared/services/report.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GlobalConstants } from '../../helpers/global-consts';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  addToHomescreenEvent: any = null;
  addToHome: boolean = false;

  constructor(
    public authService: AuthService,
    private router: Router,
    private translate: TranslateService,
    private reportService: ReportService,
    private snackbar: MatSnackBar
  ) { }

  ngOnInit() {
  }

  changeLang(langCode: string) {
    this.translate.use(langCode);
  }

  sendLaundryReport() {
    GlobalConstants.enableLoading();
    this.reportService.sendLaundryFullReport().subscribe(
      () => {
        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('Navbar.SnackbarSendFullReportSuccessfulMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'successSnackbar'
        });
      },
      err => {
        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('Navbar.SnackbarSendFullReportErrorMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'dangerSnackbar'
        });
      }
    );
  }

  sendTavernAppointmentReport() {
    GlobalConstants.enableLoading();
    this.reportService.sendTavernAppointmentFullReport().subscribe(
      () => {
        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('Navbar.SnackbarSendFullReportSuccessfulMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'successSnackbar'
        });
      },
      err => {
        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('Navbar.SnackbarSendFullReportErrorMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'dangerSnackbar'
        });
      }
    );
  }

  logout() {
    if (this.authService.logout()) {
      this.router.navigate(['/login']);
    }
  }
}
