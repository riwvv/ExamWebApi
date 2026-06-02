using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMileage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Mileage",
                table: "Elevators",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Elevators");
        }
    }
}
