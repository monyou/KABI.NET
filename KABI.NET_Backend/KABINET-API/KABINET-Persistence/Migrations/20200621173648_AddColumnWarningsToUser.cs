using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KABINET_Persistence.Migrations
{
    public partial class AddColumnWarningsToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8edb41e8-3617-4155-be15-c1a2784a1e45"));

            migrationBuilder.AddColumn<int>(
                name: "Warnings",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room", "Warnings" },
                values: new object[] { new Guid("893742d2-d200-46d4-8503-d1f1c13784b4"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$f3rupWWpzw9z+3BtQf79pOsEJrIUDgYlsXErVPzBkt28Mw2p", 1, "999", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("893742d2-d200-46d4-8503-d1f1c13784b4"));

            migrationBuilder.DropColumn(
                name: "Warnings",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Role", "Room" },
                values: new object[] { new Guid("8edb41e8-3617-4155-be15-c1a2784a1e45"), "admin@kabi.net", "Admin", "Admin", "$MYHASH$V1$10000$nwo+pSwcbGNR13fd+9CXE8CTO3uo08J+LBiyNW+5Xb/PUjQi", 1, "999" });
        }
    }
}
