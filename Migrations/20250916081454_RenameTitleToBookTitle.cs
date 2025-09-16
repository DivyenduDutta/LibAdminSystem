using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibAdminSystem.Migrations
{
    /// <inheritdoc />
    public partial class RenameTitleToBookTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "BookTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookTitle",
                table: "Books",
                newName: "Title");
        }
    }
}
