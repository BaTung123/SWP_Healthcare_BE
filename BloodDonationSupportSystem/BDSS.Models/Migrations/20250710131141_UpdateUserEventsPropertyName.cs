using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDSS.Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEventsPropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "UserEvents",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "UserEvents",
                newName: "Amount");
        }
    }
}
