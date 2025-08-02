using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDSS.Models.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceBloodStorageWithBloodBag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodDonationApplications_BloodStorages_BloodStorageId",
                table: "BloodDonationApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodExports_BloodStorages_BloodStorageId",
                table: "BloodExports");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodImports_BloodStorages_BloodStorageId",
                table: "BloodImports");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodRequestApplications_BloodStorages_BloodStorageId",
                table: "BloodRequestApplications");

            migrationBuilder.DropTable(
                name: "BloodStorages");

            migrationBuilder.RenameColumn(
                name: "BloodStorageId",
                table: "BloodRequestApplications",
                newName: "BloodBagId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodRequestApplications_BloodStorageId",
                table: "BloodRequestApplications",
                newName: "IX_BloodRequestApplications_BloodBagId");

            migrationBuilder.RenameColumn(
                name: "BloodStorageId",
                table: "BloodImports",
                newName: "BloodBagId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodImports_BloodStorageId",
                table: "BloodImports",
                newName: "IX_BloodImports_BloodBagId");

            migrationBuilder.RenameColumn(
                name: "BloodStorageId",
                table: "BloodExports",
                newName: "BloodBagId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodExports_BloodStorageId",
                table: "BloodExports",
                newName: "IX_BloodExports_BloodBagId");

            migrationBuilder.RenameColumn(
                name: "BloodStorageId",
                table: "BloodDonationApplications",
                newName: "BloodBagId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodDonationApplications_BloodStorageId",
                table: "BloodDonationApplications",
                newName: "IX_BloodDonationApplications_BloodBagId");

            migrationBuilder.CreateTable(
                name: "BloodBags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CollectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodBags", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BloodDonationApplications_BloodBags_BloodBagId",
                table: "BloodDonationApplications",
                column: "BloodBagId",
                principalTable: "BloodBags",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodExports_BloodBags_BloodBagId",
                table: "BloodExports",
                column: "BloodBagId",
                principalTable: "BloodBags",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodImports_BloodBags_BloodBagId",
                table: "BloodImports",
                column: "BloodBagId",
                principalTable: "BloodBags",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodRequestApplications_BloodBags_BloodBagId",
                table: "BloodRequestApplications",
                column: "BloodBagId",
                principalTable: "BloodBags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodDonationApplications_BloodBags_BloodBagId",
                table: "BloodDonationApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodExports_BloodBags_BloodBagId",
                table: "BloodExports");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodImports_BloodBags_BloodBagId",
                table: "BloodImports");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodRequestApplications_BloodBags_BloodBagId",
                table: "BloodRequestApplications");

            migrationBuilder.DropTable(
                name: "BloodBags");

            migrationBuilder.RenameColumn(
                name: "BloodBagId",
                table: "BloodRequestApplications",
                newName: "BloodStorageId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodRequestApplications_BloodBagId",
                table: "BloodRequestApplications",
                newName: "IX_BloodRequestApplications_BloodStorageId");

            migrationBuilder.RenameColumn(
                name: "BloodBagId",
                table: "BloodImports",
                newName: "BloodStorageId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodImports_BloodBagId",
                table: "BloodImports",
                newName: "IX_BloodImports_BloodStorageId");

            migrationBuilder.RenameColumn(
                name: "BloodBagId",
                table: "BloodExports",
                newName: "BloodStorageId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodExports_BloodBagId",
                table: "BloodExports",
                newName: "IX_BloodExports_BloodStorageId");

            migrationBuilder.RenameColumn(
                name: "BloodBagId",
                table: "BloodDonationApplications",
                newName: "BloodStorageId");

            migrationBuilder.RenameIndex(
                name: "IX_BloodDonationApplications_BloodBagId",
                table: "BloodDonationApplications",
                newName: "IX_BloodDonationApplications_BloodStorageId");

            migrationBuilder.CreateTable(
                name: "BloodStorages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodStorages", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BloodDonationApplications_BloodStorages_BloodStorageId",
                table: "BloodDonationApplications",
                column: "BloodStorageId",
                principalTable: "BloodStorages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodExports_BloodStorages_BloodStorageId",
                table: "BloodExports",
                column: "BloodStorageId",
                principalTable: "BloodStorages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodImports_BloodStorages_BloodStorageId",
                table: "BloodImports",
                column: "BloodStorageId",
                principalTable: "BloodStorages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BloodRequestApplications_BloodStorages_BloodStorageId",
                table: "BloodRequestApplications",
                column: "BloodStorageId",
                principalTable: "BloodStorages",
                principalColumn: "Id");
        }
    }
}
