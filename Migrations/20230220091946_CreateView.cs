using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationScripTest.Migrations
{
    /// <inheritdoc />
    public partial class CreateView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW ExpenseByTotalView " +
                "AS SELECT p.Id, p.Name, p.Category, sum(h.Amount) " +
                "AS TotalAmount FROM ExpenseItems p " +
                "JOIN ExpenseHistory h ON p.Id = h.ExpenseItemId " +
                "GROUP BY p.Id, p.Name, p.Category ORDER BY (TotalAmount) DESC;");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW public.ExpenseByTotalView;");
        }
    }
}
