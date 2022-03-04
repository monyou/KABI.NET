using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KABINET_Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Room = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Laundries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    TotalToPay = table.Column<double>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    TotalLaundryTime = table.Column<TimeSpan>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laundries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Laundries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room" },
                values: new object[] { new Guid("f59f26f6-b173-4232-88f0-06899894c868"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$xaGyolwKkEnMTPcQVcOQs+8XBuVXwtsaBK8Djr1rXYxIS6uJ", 1, "999" });

            migrationBuilder.CreateIndex(
                name: "IX_Laundries_UserId",
                table: "Laundries",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Laundries");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
