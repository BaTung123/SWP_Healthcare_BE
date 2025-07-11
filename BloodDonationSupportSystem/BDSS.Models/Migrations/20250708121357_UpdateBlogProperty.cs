using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDSS.Models.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Blogs",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Blogs",
                newName: "Image");
        }
    }
}
