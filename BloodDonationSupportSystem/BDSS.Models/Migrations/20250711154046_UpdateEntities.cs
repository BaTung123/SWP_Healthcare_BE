using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDSS.Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BloodRequestDate",
                table: "BloodRequestApplications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "BloodRequestApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BloodTransferType",
                table: "BloodDonationApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DonationEndDate",
                table: "BloodDonationApplications",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DonationStartDate",
                table: "BloodDonationApplications",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "BloodDonationApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodRequestDate",
                table: "BloodRequestApplications");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "BloodRequestApplications");

            migrationBuilder.DropColumn(
                name: "BloodTransferType",
                table: "BloodDonationApplications");

            migrationBuilder.DropColumn(
                name: "DonationEndDate",
                table: "BloodDonationApplications");

            migrationBuilder.DropColumn(
                name: "DonationStartDate",
                table: "BloodDonationApplications");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "BloodDonationApplications");
        }
    }
}
