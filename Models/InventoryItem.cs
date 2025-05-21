namespace Expense_Tracker.Models
{
    public class InventoryItem
    {
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
