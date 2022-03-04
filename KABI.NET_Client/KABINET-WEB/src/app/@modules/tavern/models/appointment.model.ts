import { User } from '../../admin/models/user.model';

export class TavernAppointment {
    id: string;
    startTime: Date;
    endTime: Date;
    user: User;
    title: string;
}