using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consulting.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoForBlogPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "BlogPosts",
                type: "BLOB",
                maxLength: 131072,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "BlogPosts");
        }
    }
}
