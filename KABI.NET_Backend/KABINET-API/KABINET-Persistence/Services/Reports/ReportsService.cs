using KABINET_Application.Boundaries.Notification;
using KABINET_Application.Boundaries.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KABINET_Persistance.Services.Reports
{
    public class ReportsService : IReportsService
    {
        private readonly KabinetDbContext kabinetDbContext;
        private readonly INotificationService nothificationService;
        private readonly IConfiguration configuration;
        private readonly double tavernAppointmentFee;

        public ReportsService(
            KabinetDbContext kabinetDbContext,
            INotificationService notificationService,
            IConfiguration configuration)
        {
            this.kabinetDbContext = kabinetDbContext;
            this.nothificationService = notificationService;
            this.configuration = configuration;
            this.tavernAppointmentFee = double.Parse(this.configuration.GetSection("TavernAppointmentFee").Value);
        }

        public async Task SendLaundryFullReportAsync()
        {
            var reportData = await kabinetDbContext.LaundryFullReports.FromSqlRaw("sp_LaundryFullReport").ToListAsync();
            var totalToPay = 0.0;
            var totalLaundryTime = new TimeSpan();
            foreach (var item in reportData)
            {
                totalToPay += item.ToPay;
                totalLaundryTime.Add(item.LaundryTime);
            }
            var subject = $"Отчет пране за месец {this.BGMonthNameConverter(DateTime.UtcNow.Month)}";
            var reciever = this.configuration.GetSection("ReportsRecieverEmail").Value;
            var message = new StringBuilder();
            message.AppendLine($@"
<div style='background-color: white; width: 100%; padding: 10px; box-shadow: 0 0 10px gray; border: double 10px #0b7a75;'>
  <div style='width: 100%; margin-bottom: 20px;'>
    <div style='width: 50px; height: 50px;'>
      <img style='width: 100%; height: 100%;' src='https://i.imgur.com/m0HqXL5.png' alt='logo'>
    </div>
  </div>
  <div style='width: 100%; margin-right: auto; margin-left: auto;'>
    <div style='display: flex; flex-wrap: wrap; justify-content: center!important;'>
      <div style='flex: 0 0 100%; max-width: 100%; position: relative; width: 100%;'>
        <h1 style='text-align: center; margin-bottom: 1.5rem;'>{subject}</h1>
      </div>
    </div>
    <div style='display: flex; flex-wrap: wrap;'>
      <div style='flex: 0 0 100%; max-width: 100%; position: relative; width: 100%;'>
        <div style='width:100%; overflow: auto;'>
          <table style='width: 100%; margin-bottom: 1rem; color: #212529; border-collapse: collapse;'>
            <thead>
              <tr>
                <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Потребител </th>
                <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Стая </th>
                <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Дата </th>
                <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Времетраене </th>
                <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Дължи </th>
                <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Платено? </th>
              </tr>
            </thead>
            <tbody>
");

            foreach (var item in reportData)
            {
                message.AppendLine($@"
            <tr style='text-align: center;'>
                <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>{item.User}</td>
                <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>{item.Room}</td>
                <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>
                    {item.StartTime.Day}-{item.StartTime.Month}-{item.StartTime.Year}
                </td>
                <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>
	                {string.Format("{0:00}", item.LaundryTime.Hours)}ч. и {string.Format("{0:00}", item.LaundryTime.Minutes)}м.
                </td>
                <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>{item.ToPay} лв.</td>
                <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>{(item.IsPaid ? "Да" : "Не")}</td>
            </tr>
");
            }

            message.AppendLine($@"</tbody>
            <tfoot>
              <tr>
                <td style='text-align: right; padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'> Общо времетраене: </td>
                <td colspan='5' style='padding-left: 10px; padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>
                  {string.Format("{0:00}", totalLaundryTime.Hours)}ч. и {string.Format("{0:00}", totalLaundryTime.Minutes)}м.
                </td>
              </tr>
              <tr style='border: 0 !important;'>
                <td style='text-align: right; border: 0 !important'>
                  <b> Общо платени: </b></td>
                <td colspan='5' style='border: 0 !important; padding-left: 10px;'><b style='font-size: 2em;'>{totalToPay} лв.</b></td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>
    </div>
  </div>
</div>
");

            await this.nothificationService.SendEmailAsync(reciever, subject, message.ToString());
        }

        public async Task SendTavernAppointmentFullReportAsync()
        {
            var reportData = await kabinetDbContext.TavernAppointmentFullReports.FromSqlRaw("sp_TavernAppointmentFullReport").ToListAsync();
            var subject = $"Отчет механа за месец {this.BGMonthNameConverter(DateTime.UtcNow.Month)}";
            var reciever = this.configuration.GetSection("ReportsRecieverEmail").Value;
            var message = new StringBuilder();
            message.AppendLine($@"
            <div style='background-color: white; width: 100%; padding: 10px; box-shadow: 0 0 10px gray; border: double 10px #0b7a75;'>
              <div style='width: 100%; margin-bottom: 20px;'>
                <div style='width: 50px; height: 50px;'>
                  <img style='width: 100%; height: 100%;' src='https://i.imgur.com/m0HqXL5.png' alt='logo'>
                </div>
              </div>
              <div style='width: 100%; margin-right: auto; margin-left: auto;'>
                <div style='display: flex; flex-wrap: wrap; justify-content: center!important;'>
                  <div style='flex: 0 0 100%; max-width: 100%; position: relative; width: 100%;'>
                    <h1 style='text-align: center; margin-bottom: 1.5rem;'>{subject}</h1>
                  </div>
                </div>
                <div style='display: flex; flex-wrap: wrap;'>
                  <div style='flex: 0 0 100%; max-width: 100%; position: relative; width: 100%;'>
                    <div style='width:100%; overflow: auto;'>
                      <table style='width: 100%; margin-bottom: 1rem; color: #212529; border-collapse: collapse;'>
                        <thead>
                          <tr>
                            <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Потребител </th>
                            <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Стая </th>
                            <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Начало </th>
                            <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Времетраене </th>
                            <th scope='col' style='color: #fff; background-color: #343a40; border-color: #454d55; vertical-align: bottom; border-bottom: 2px solid #dee2e6; padding: .75rem; border-top: 1px solid #dee2e6;'> Мероприятие </th>
                          </tr>
                        </thead>
                        <tbody>
            ");

            foreach (var item in reportData)
            {
                message.AppendLine($@"
                        <tr style='text-align: center;'>
                            <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>{item.UserNames}</td>
                            <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>{item.UserRoom}</td>
                            <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>
                                {item.StartTime.ToString("dd-MM-yyyy HH:mm")}
                            </td>
                            <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>
            	                {string.Format("{0:00}", item.DurationInMinutes / 60)}ч. и {string.Format("{0:00}", item.DurationInMinutes % 60)}м.
                            </td>
                            <td scope='row' style='padding: .75rem; vertical-align: top; border-top: 1px solid #dee2e6;'>{item.EventDescription}</td>
                        </tr>
            ");
            }

            message.AppendLine($@"</tbody>
                        <tfoot>
                          <tr style='border: 0 !important;'>
                            <td style='text-align: right; border: 0 !important'>
                              <b> Общо платени: </b></td>
                            <td colspan='5' style='border: 0 !important; padding-left: 10px;'><b style='font-size: 2em;'>{reportData.Count * this.tavernAppointmentFee} лв.</b></td>
                          </tr>
                        </tfoot>
                      </table>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            ");

            await this.nothificationService.SendEmailAsync(reciever, subject, message.ToString());
        }

        private string BGMonthNameConverter(int monthNumber)
        {
            switch (monthNumber)
            {
                case 1:
                    return "Януари";
                case 2:
                    return "Февруари";
                case 3:
                    return "Март";
                case 4:
                    return "Април";
                case 5:
                    return "Май";
                case 6:
                    return "Юни";
                case 7:
                    return "Юли";
                case 8:
                    return "Август";
                case 9:
                    return "Септември";
                case 10:
                    return "Октомври";
                case 11:
                    return "Ноември";
                case 12:
                    return "Декември";
                default:
                    return "";
            }
        }
    }
}
