using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamWebApi.Migrations
{
    /// <inheritdoc />
    public partial class EditTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorCalls_Elevators_ElevatorSerialNumber",
                table: "FloorCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_TripLogs_Elevators_ElevatorSerialNumber",
                table: "TripLogs");

            migrationBuilder.DropIndex(
                name: "IX_TripLogs_ElevatorSerialNumber",
                table: "TripLogs");

            migrationBuilder.DropIndex(
                name: "IX_FloorCalls_ElevatorSerialNumber",
                table: "FloorCalls");

            migrationBuilder.DropColumn(
                name: "ElevatorSerialNumber",
                table: "TripLogs");

            migrationBuilder.DropColumn(
                name: "ElevatorSerialNumber",
                table: "FloorCalls");

            migrationBuilder.AddColumn<Guid>(
                name: "ElevatorId",
                table: "TripLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ElevatorId",
                table: "FloorCalls",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TripLogs_ElevatorId",
                table: "TripLogs",
                column: "ElevatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FloorCalls_ElevatorId",
                table: "FloorCalls",
                column: "ElevatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorCalls_Elevators_ElevatorId",
                table: "FloorCalls",
                column: "ElevatorId",
                principalTable: "Elevators",
                principalColumn: "SerialNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripLogs_Elevators_ElevatorId",
                table: "TripLogs",
                column: "ElevatorId",
                principalTable: "Elevators",
                principalColumn: "SerialNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorCalls_Elevators_ElevatorId",
                table: "FloorCalls");

            migrationBuilder.DropForeignKey(
                name: "FK_TripLogs_Elevators_ElevatorId",
                table: "TripLogs");

            migrationBuilder.DropIndex(
                name: "IX_TripLogs_ElevatorId",
                table: "TripLogs");

            migrationBuilder.DropIndex(
                name: "IX_FloorCalls_ElevatorId",
                table: "FloorCalls");

            migrationBuilder.DropColumn(
                name: "ElevatorId",
                table: "TripLogs");

            migrationBuilder.DropColumn(
                name: "ElevatorId",
                table: "FloorCalls");

            migrationBuilder.AddColumn<Guid>(
                name: "ElevatorSerialNumber",
                table: "TripLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ElevatorSerialNumber",
                table: "FloorCalls",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripLogs_ElevatorSerialNumber",
                table: "TripLogs",
                column: "ElevatorSerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_FloorCalls_ElevatorSerialNumber",
                table: "FloorCalls",
                column: "ElevatorSerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorCalls_Elevators_ElevatorSerialNumber",
                table: "FloorCalls",
                column: "ElevatorSerialNumber",
                principalTable: "Elevators",
                principalColumn: "SerialNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripLogs_Elevators_ElevatorSerialNumber",
                table: "TripLogs",
                column: "ElevatorSerialNumber",
                principalTable: "Elevators",
                principalColumn: "SerialNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
