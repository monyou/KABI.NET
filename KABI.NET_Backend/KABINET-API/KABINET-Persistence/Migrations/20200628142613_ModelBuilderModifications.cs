using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KABINET_Persistence.Migrations
{
    public partial class ModelBuilderModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("25ca76d0-ccc3-48eb-ac8e-cd40e08fb78a"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TavernAppointments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Laundries",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room", "Warnings" },
                values: new object[] { new Guid("a5c823fd-c669-45ed-aafb-1bd72ad1c9bf"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$yHmZJhIdo2WO7LGoSxO385KVvpAwF5VVCCiiojCOIBcBnjhL", 1, "999", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a5c823fd-c669-45ed-aafb-1bd72ad1c9bf"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TavernAppointments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Laundries",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room", "Warnings" },
                values: new object[] { new Guid("25ca76d0-ccc3-48eb-ac8e-cd40e08fb78a"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$fIvoKsNfjcekeJH/l2A3br1ycEq61HIVkBz57x0Is3GY8gfL", 1, "999", 0 });
        }
    }
}
