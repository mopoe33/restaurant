using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restaurant.Migrations
{
    /// <inheritdoc />
    public partial class addTablePeopleNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeopleNumber",
                table: "Tables",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeopleNumber",
                table: "Tables");
        }
    }
}
