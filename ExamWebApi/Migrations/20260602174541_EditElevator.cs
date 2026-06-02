using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamWebApi.Migrations
{
    /// <inheritdoc />
    public partial class EditElevator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentFloor",
                table: "Elevators",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MoveStatus",
                table: "Elevators",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentFloor",
                table: "Elevators");

            migrationBuilder.DropColumn(
                name: "MoveStatus",
                table: "Elevators");
        }
    }
}
