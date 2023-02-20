using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationScripTest.view
{
    public class ExpenseByTotal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        [Column("totalamount")]
        public double TotalAmount { get; set; }
        public string? StaticColumn { get; set; }
    }
}
