using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamWebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalFloors = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elevators",
                columns: table => new
                {
                    SerialNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinFloor = table.Column<int>(type: "int", nullable: false),
                    MaxFloor = table.Column<int>(type: "int", nullable: false),
                    MoveSpeed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    BuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elevators", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_Elevators_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FloorCalls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentFloor = table.Column<int>(type: "int", nullable: false),
                    Course = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElevatorSerialNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloorCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FloorCalls_Elevators_ElevatorSerialNumber",
                        column: x => x.ElevatorSerialNumber,
                        principalTable: "Elevators",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DistanceTraveled = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalSeconds = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElevatorSerialNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripLogs_Elevators_ElevatorSerialNumber",
                        column: x => x.ElevatorSerialNumber,
                        principalTable: "Elevators",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_TotalFloors",
                table: "Buildings",
                column: "TotalFloors");

            migrationBuilder.CreateIndex(
                name: "IX_Elevators_BuildingId",
                table: "Elevators",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Elevators_ModelID",
                table: "Elevators",
                column: "ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_FloorCalls_ElevatorSerialNumber",
                table: "FloorCalls",
                column: "ElevatorSerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_FloorCalls_Timestamp",
                table: "FloorCalls",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_TripLogs_Description",
                table: "TripLogs",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_TripLogs_ElevatorSerialNumber",
                table: "TripLogs",
                column: "ElevatorSerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TripLogs_Timestamp",
                table: "TripLogs",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloorCalls");

            migrationBuilder.DropTable(
                name: "TripLogs");

            migrationBuilder.DropTable(
                name: "Elevators");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
