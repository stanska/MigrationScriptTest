using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationScripTest.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Blog_AddColumn_NewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewColumn",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewColumn",
                table: "Blog");
        }
    }
}
