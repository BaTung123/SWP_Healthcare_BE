using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDSS.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationsEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodImports_Events_EventId",
                table: "BloodImports");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodImports_Events_EventId1",
                table: "BloodImports");

            migrationBuilder.DropIndex(
                name: "IX_BloodImports_EventId",
                table: "BloodImports");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BloodStorages");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "BloodImports");

            migrationBuilder.DropColumn(
                name: "Dob",
                table: "BloodImports");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "BloodImports");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "BloodImports");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "BloodImports");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BloodImports");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "BloodExports");

            migrationBuilder.DropColumn(
                name: "HospitalName",
                table: "BloodExports");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BloodExports");

            migrationBuilder.RenameColumn(
                name: "EventId1",
                table: "BloodImports",
                newName: "BloodDonationApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodImports_EventId1",
                table: "BloodImports",
                newName: "IX_BloodImports_BloodDonationApplicationId");

            migrationBuilder.AlterColumn<int>(
                name: "BloodType",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "UserEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "UserEvents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserEventsStatus",
                table: "UserEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BloodType",
                table: "BloodStorages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "BloodStorageId",
                table: "BloodImports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BloodStorageId",
                table: "BloodExports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BloodRequestApplicationId",
                table: "BloodExports",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BloodDonationApplications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    BloodStorageId = table.Column<long>(type: "bigint", nullable: true),
                    EventId = table.Column<long>(type: "bigint", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodDonationApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodDonationApplications_BloodStorages_BloodStorageId",
                        column: x => x.BloodStorageId,
                        principalTable: "BloodStorages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BloodDonationApplications_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BloodDonationApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BloodRequestApplications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodStorageId = table.Column<long>(type: "bigint", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodType = table.Column<int>(type: "int", nullable: false),
                    BloodTransferType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUrged = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodRequestApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodRequestApplications_BloodStorages_BloodStorageId",
                        column: x => x.BloodStorageId,
                        principalTable: "BloodStorages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodExports_BloodRequestApplicationId",
                table: "BloodExports",
                column: "BloodRequestApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonationApplications_BloodStorageId",
                table: "BloodDonationApplications",
                column: "BloodStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonationApplications_EventId",
                table: "BloodDonationApplications",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonationApplications_UserId",
                table: "BloodDonationApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequestApplications_BloodStorageId",
                table: "BloodRequestApplications",
                column: "BloodStorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodExports_BloodRequestApplications_BloodRequestApplicationId",
                table: "BloodExports",
                column: "BloodRequestApplicationId",
                principalTable: "BloodRequestApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodImports_BloodDonationApplications_BloodDonationApplicationId",
                table: "BloodImports",
                column: "BloodDonationApplicationId",
                principalTable: "BloodDonationApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodExports_BloodRequestApplications_BloodRequestApplicationId",
                table: "BloodExports");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodImports_BloodDonationApplications_BloodDonationApplicationId",
                table: "BloodImports");

            migrationBuilder.DropTable(
                name: "BloodDonationApplications");

            migrationBuilder.DropTable(
                name: "BloodRequestApplications");

            migrationBuilder.DropIndex(
                name: "IX_BloodExports_BloodRequestApplicationId",
                table: "BloodExports");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "UserEvents");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserEvents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserEvents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserEvents");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserEvents");

            migrationBuilder.DropColumn(
                name: "UserEventsStatus",
                table: "UserEvents");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "BloodStorages");

            migrationBuilder.DropColumn(
                name: "BloodRequestApplicationId",
                table: "BloodExports");

            migrationBuilder.RenameColumn(
                name: "BloodDonationApplicationId",
                table: "BloodImports",
                newName: "EventId1");

            migrationBuilder.RenameIndex(
                name: "IX_BloodImports_BloodDonationApplicationId",
                table: "BloodImports",
                newName: "IX_BloodImports_EventId1");

            migrationBuilder.AlterColumn<string>(
                name: "BloodType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BloodStorages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<long>(
                name: "BloodStorageId",
                table: "BloodImports",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "BloodImports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "BloodImports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EventId",
                table: "BloodImports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "BloodImports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "BloodImports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BloodImports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "BloodStorageId",
                table: "BloodExports",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "BloodExports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HospitalName",
                table: "BloodExports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BloodExports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BloodImports_EventId",
                table: "BloodImports",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodImports_Events_EventId",
                table: "BloodImports",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodImports_Events_EventId1",
                table: "BloodImports",
                column: "EventId1",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
