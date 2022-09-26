using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetingSchedule.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[] { 1, "#00ff99", "Sala 1" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[] { 2, "#ff9933", "Sala 2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserName", "Password", "Role" },
                values: new object[] { "GustavoDBP", "123456", "admin" });

            migrationBuilder.InsertData(
                table: "Meetings",
                columns: new[] { "Id", "Description", "End", "Name", "RoomId", "Start" },
                values: new object[,]
                {
                    { 1, "Descrição da reunião 1", new DateTime(2022, 9, 20, 11, 0, 0, 0, DateTimeKind.Unspecified), "Reunião 1", 1, new DateTime(2022, 9, 20, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Descrição da reunião 3", new DateTime(2022, 9, 20, 15, 0, 0, 0, DateTimeKind.Unspecified), "Reunião 3", 1, new DateTime(2022, 9, 20, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Descrição da reunião 4", new DateTime(2022, 9, 21, 11, 0, 0, 0, DateTimeKind.Unspecified), "Reunião 4", 1, new DateTime(2022, 9, 21, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Descrição da reunião 2", new DateTime(2022, 9, 20, 11, 0, 0, 0, DateTimeKind.Unspecified), "Reunião 2", 2, new DateTime(2022, 9, 20, 10, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_RoomId",
                table: "Meetings",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
