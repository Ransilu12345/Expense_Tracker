using Expense_Tracker.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Expense_Tracker.DataAccess;

namespace Expense_Tracker.Views
{
    public partial class InventoryPage : Page
    {
        private ObservableCollection<InventoryItem> _inventoryItems;
        private InventoryItem? _editingInventoryItem = null;


        public InventoryPage(User _user)
        {
            InitializeComponent();
            _inventoryItems = new ObservableCollection<InventoryItem>();
            InventoryGrid.ItemsSource = _inventoryItems;
            LoadInventory();
        }

        private void LoadInventory()
        {
            _inventoryItems.Clear();
            var items = DatabaseHelper.GetAllInventoryItems();
            foreach (var item in items)
            {
                _inventoryItems.Add(item);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = ItemNameTextBox.Text;
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) ||
                !double.TryParse(PriceTextBox.Text, out double price))
            {
                MessageBox.Show("Enter valid details.");
                return;
            }

            if (_editingInventoryItem != null)
            {
                _editingInventoryItem.ItemName = name;
                _editingInventoryItem.Quantity = quantity;
                _editingInventoryItem.Price = price;

                DatabaseHelper.UpdateInventoryItem(_editingInventoryItem.Id, name, quantity, price);
                InventoryGrid.Items.Refresh();
                _editingInventoryItem = null;
            }
            else
            {
                var newItem = new InventoryItem { ItemName = name, Quantity = quantity, Price = price };
                DatabaseHelper.AddInventoryItem(name, quantity, price);
                LoadInventory();
            }

            ClearFields();
        }


        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryGrid.SelectedItem is InventoryItem selectedItem)
            {
                ItemNameTextBox.Text = selectedItem.ItemName;
                QuantityTextBox.Text = selectedItem.Quantity.ToString();
                PriceTextBox.Text = selectedItem.Price.ToString();

                _editingInventoryItem = selectedItem;
            }
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryGrid.SelectedItem is InventoryItem selectedItem)
            {
                DatabaseHelper.DeleteInventoryItem(selectedItem.Id);
                _inventoryItems.Remove(selectedItem);
                InventoryGrid.Items.Refresh();
            }
            InventoryGrid.Items.Refresh();
            
        }

        private void ClearFields()
        {
            ItemNameTextBox.Text = "";
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
        }
    }
}
