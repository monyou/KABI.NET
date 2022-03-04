import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/@modules/admin/services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MustMatch } from 'src/app/@shared/helpers/must-match.validator';
import { TranslateService } from '@ngx-translate/core';
import { GlobalConstants } from 'src/app/@shared/helpers/global-consts';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  changePasswordForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private snackbar: MatSnackBar,
    private translate: TranslateService
  ) { }

  ngOnInit() {
    this.changePasswordForm = this.formBuilder.group({
      currentPassword: ['', Validators.required],
      password: ['', [Validators.required]],
      confirmPassword: ['']
    }, { validator: MustMatch('password', 'confirmPassword') });
  }

  changePassword() {
    GlobalConstants.enableLoading();
    let newPassRequest = {
      "OldPassword": this.changePasswordForm.get('currentPassword').value,
      "NewPassword": this.changePasswordForm.get('password').value
    }
    this.userService.changePassword(newPassRequest).subscribe(
      () => {
        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('ChangePasswordPage.SnackbarSuccessfulMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'successSnackbar'
        });
        window.history.back();
      },
      error => {
        GlobalConstants.disableLoading();
        this.snackbar.open(this.translate.instant('ChangePasswordPage.SnackbarErrorMsg'), 'OK', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
          panelClass: 'dangerSnackbar'
        });
      }
    );
  }

}
