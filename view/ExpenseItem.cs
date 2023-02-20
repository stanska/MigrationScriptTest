namespace MigrationScripTest.view
{
    public class ExpenseItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<ExpenseHistory> History { get; set; } = new List<ExpenseHistory>();
    }
}
