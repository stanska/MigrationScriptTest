using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationScripTest.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_RenameName_To_BlogName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Blogs",
                newName: "BlogName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BlogName",
                table: "Blogs",
                newName: "Name");
        }
    }
}
