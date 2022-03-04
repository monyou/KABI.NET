import { Component, OnInit } from '@angular/core';
import { TavernAppointmentService } from '../../services/tavern-appointment.service';
import { GlobalConstants } from 'src/app/@shared/helpers/global-consts';
import { TavernAppointment } from '../../models/appointment.model';

@Component({
  selector: 'app-tavern',
  templateUrl: './tavern.component.html',
  styleUrls: ['./tavern.component.scss']
})
export class TavernComponent implements OnInit {

  allAppointments: TavernAppointment[] = [];

  constructor(
    private tavernAppointmentService: TavernAppointmentService
  ) { }

  ngOnInit() {
    this.getAllAppointments();
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
