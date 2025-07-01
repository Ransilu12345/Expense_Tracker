namespace Expense_Tracker.Models
{
    public class SaleItem
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
} 
