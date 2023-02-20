namespace MigrationScripTest.table
{
    public class Blog
    {
        public int Id { get; set; }
        public string? BlogName { get; set; }
        public string? NewColumn { get; set; }
        public DateTime Timestamp { get; set; }
    }
}