import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/@modules/auth/services/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { GlobalConstants } from 'src/app/@shared/helpers/global-consts';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router,
    private snackbar: MatSnackBar,
    private translate: TranslateService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      EmailAddress: ['', [Validators.required, Validators.email]],
      Password: ['', [Validators.required]]
    });
  }

  login() {
    GlobalConstants.enableLoading();
    this.authService.login(this.loginForm.value).subscribe(
      () => {
        if (this.authService.isAdmin())
          this.router.navigate(['/admin-dashboard']);
        else
          this.router.navigate(['/laundry']);

        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('LoginPage.SnackbarSuccessfulMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'successSnackbar'
        });
      },
      error => {
        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('LoginPage.SnackbarErrorMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'dangerSnackbar'
        });
      }
    );
  }

  forgotPassword(forgotPasswordDialogRef: TemplateRef<any>) {
    const $dialogRef = this.dialog.open(forgotPasswordDialogRef);
  }
}
