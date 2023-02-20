namespace MigrationScripTest.view
{
    public class ExpenseHistory
    {
        public int Id { get; set; }
        public int ExpenseItemId { get; set; }
        public ExpenseItem ExpenseItem { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
    }
}
