using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KABINET_Persistence.Migrations
{
    public partial class AddTavernAppointmentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f59f26f6-b173-4232-88f0-06899894c868"));

            migrationBuilder.CreateTable(
                name: "TavernAppointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    AllDay = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TavernAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TavernAppointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room" },
                values: new object[] { new Guid("8edb41e8-3617-4155-be15-c1a2784a1e45"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$nwo+pSwcbGNR13fd+9CXE8CTO3uo08J+LBiyNW+5Xb/PUjQi", 1, "999" });

            migrationBuilder.CreateIndex(
                name: "IX_TavernAppointments_UserId",
                table: "TavernAppointments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TavernAppointments");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8edb41e8-3617-4155-be15-c1a2784a1e45"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room" },
                values: new object[] { new Guid("f59f26f6-b173-4232-88f0-06899894c868"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$xaGyolwKkEnMTPcQVcOQs+8XBuVXwtsaBK8Djr1rXYxIS6uJ", 1, "999" });
        }
    }
}
