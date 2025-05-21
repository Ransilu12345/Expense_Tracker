using Expense_Tracker.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Expense_Tracker.Views
{
    public partial class InventoryPage : Page
    {
        private ObservableCollection<InventoryItem> _inventoryItems;

        public InventoryPage(User _user)
        {
            InitializeComponent();
            _inventoryItems = new ObservableCollection<InventoryItem>();
            InventoryGrid.ItemsSource = _inventoryItems;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ItemNameTextBox.Text) ||
                !int.TryParse(QuantityTextBox.Text, out int quantity) ||
                !double.TryParse(PriceTextBox.Text, out double price))
            {
                MessageBox.Show("Please enter valid item details.");
                return;
            }

            _inventoryItems.Add(new InventoryItem
            {
                ItemName = ItemNameTextBox.Text,
                Quantity = quantity,
                Price = price
            });

            ClearFields();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryGrid.SelectedItem is InventoryItem selectedItem)
            {
                ItemNameTextBox.Text = selectedItem.ItemName;
                QuantityTextBox.Text = selectedItem.Quantity.ToString();
                PriceTextBox.Text = selectedItem.Price.ToString();
                _inventoryItems.Remove(selectedItem);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryGrid.SelectedItem is InventoryItem selectedItem)
            {
                _inventoryItems.Remove(selectedItem);
            }
        }

        private void ClearFields()
        {
            ItemNameTextBox.Text = "";
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
        }
    }
}
