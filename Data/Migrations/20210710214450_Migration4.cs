using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EVA3_MVC_AGENCIA.Data.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebt",
                table: "TReports_clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePayment",
                table: "TReports_clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "TReports_clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Credit",
                table: "TClients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "TClients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDebt",
                table: "TReports_clients");

            migrationBuilder.DropColumn(
                name: "DatePayment",
                table: "TReports_clients");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "TReports_clients");

            migrationBuilder.DropColumn(
                name: "Credit",
                table: "TClients");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "TClients");
        }
    }
}
