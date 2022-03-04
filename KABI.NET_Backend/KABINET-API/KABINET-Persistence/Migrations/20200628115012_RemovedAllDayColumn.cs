using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KABINET_Persistence.Migrations
{
    public partial class RemovedAllDayColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("893742d2-d200-46d4-8503-d1f1c13784b4"));

            migrationBuilder.DropColumn(
                name: "AllDay",
                table: "TavernAppointments");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room", "Warnings" },
                values: new object[] { new Guid("25ca76d0-ccc3-48eb-ac8e-cd40e08fb78a"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$fIvoKsNfjcekeJH/l2A3br1ycEq61HIVkBz57x0Is3GY8gfL", 1, "999", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("25ca76d0-ccc3-48eb-ac8e-cd40e08fb78a"));

            migrationBuilder.AddColumn<bool>(
                name: "AllDay",
                table: "TavernAppointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room", "Warnings" },
                values: new object[] { new Guid("893742d2-d200-46d4-8503-d1f1c13784b4"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$f3rupWWpzw9z+3BtQf79pOsEJrIUDgYlsXErVPzBkt28Mw2p", 1, "999", 0 });
        }
    }
}
