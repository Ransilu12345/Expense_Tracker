using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Expense_Tracker.Models;
using Expense_Tracker.DataAccess;

namespace Expense_Tracker.Views
{
    public partial class SalesPage : Page
    {
        private ObservableCollection<SaleItem> _sales;
        private SaleItem? _editingSaleItem = null;

        public SalesPage()
        {
            InitializeComponent();
            _sales = new ObservableCollection<SaleItem>();
            SalesGrid.ItemsSource = _sales;
            LoadSales();
        }

        private void LoadSales()
        {
            _sales.Clear();
            var allSales = DatabaseHelper.GetAllSales();
            foreach (var sale in allSales)
            {
                _sales.Add(sale);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = ProductNameTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) ||
                !int.TryParse(QuantityTextBox.Text, out int quantity) ||
                !double.TryParse(PriceTextBox.Text, out double price))
            {
                MessageBox.Show("Please enter valid product name, quantity, and price.");
                return;
            }

            if (_editingSaleItem != null)
            {
                
                _editingSaleItem.ProductName = name;
                _editingSaleItem.Quantity = quantity;
                _editingSaleItem.Price = price;

                DatabaseHelper.UpdateSalesItem(_editingSaleItem.Id, name, quantity, price);
                SalesGrid.Items.Refresh();
                _editingSaleItem = null;
            }
            else
            {
                
                DatabaseHelper.AddSaleItem(name, quantity, price);
                LoadSales();
            }

            ClearFields();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SalesGrid.SelectedItem is SaleItem selectedItem)
            {
                ProductNameTextBox.Text = selectedItem.ProductName;
                QuantityTextBox.Text = selectedItem.Quantity.ToString();
                PriceTextBox.Text = selectedItem.Price.ToString();

                _editingSaleItem = selectedItem;
            }
            else
            {
                MessageBox.Show("Please select a sale item to edit.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SalesGrid.SelectedItem is SaleItem selectedItem)
            {
                DatabaseHelper.DeleteSalesItem(selectedItem.Id);
                _sales.Remove(selectedItem);
                SalesGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a sale to delete.");
            }
        }

        private void ClearFields()
        {
            ProductNameTextBox.Text = "";
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
        }
    }
}
