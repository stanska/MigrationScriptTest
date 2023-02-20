using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Migrations;
using MigrationScripTest.view;
using System.Diagnostics.Metrics;

#nullable disable

namespace MigrationScripTest.Migrations
{
    /// <inheritdoc />
    public partial class AlterView_ExpenseTotal_StaticColumn : Migration
    {

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW ExpenseByTotalView AS 
               SELECT p.Id, 
                      p.Name, 
                      p.Category, 
                      sum(h.Amount) AS TotalAmount, 
                      'a new column' as StaticColumn 
                 FROM ExpenseItems p
               JOIN ExpenseHistory h ON p.Id = h.ExpenseItemId
               GROUP BY p.Id, p.Name, p.Category");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW ExpenseByTotalView AS 
               SELECT p.Id, 
                      p.Name, 
                      p.Category, 
                      sum(h.Amount) AS TotalAmount
                 FROM ExpenseItems p
               JOIN ExpenseHistory h ON p.Id = h.ExpenseItemId
               GROUP BY p.Id, p.Name, p.Category");
        }
    }
}
