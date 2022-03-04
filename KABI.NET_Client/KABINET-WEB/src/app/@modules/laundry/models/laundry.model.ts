import { User } from '../../admin/models/user.model';

export class Laundry {
    id: string;
    startTime: Date;
    endTime: Date;
    user: User;
    totalToPay: number;
    totalLaundryTime: string;
    isPaid: boolean;
}