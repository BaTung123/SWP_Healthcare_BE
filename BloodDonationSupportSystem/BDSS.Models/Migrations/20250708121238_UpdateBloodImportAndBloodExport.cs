using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDSS.Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBloodImportAndBloodExport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BloodStorageId",
                table: "BloodImports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BloodStorageId",
                table: "BloodExports",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodImports_BloodStorageId",
                table: "BloodImports",
                column: "BloodStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodExports_BloodStorageId",
                table: "BloodExports",
                column: "BloodStorageId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodExports_BloodStorages_BloodStorageId",
                table: "BloodExports");

            migrationBuilder.DropForeignKey(
                name: "FK_BloodImports_BloodStorages_BloodStorageId",
                table: "BloodImports");

            migrationBuilder.DropIndex(
                name: "IX_BloodImports_BloodStorageId",
                table: "BloodImports");

            migrationBuilder.DropIndex(
                name: "IX_BloodExports_BloodStorageId",
                table: "BloodExports");

            migrationBuilder.DropColumn(
                name: "BloodStorageId",
                table: "BloodImports");

            migrationBuilder.DropColumn(
                name: "BloodStorageId",
                table: "BloodExports");
        }
    }
}
