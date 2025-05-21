using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Expense_Tracker.Models;

namespace Expense_Tracker.Views
{
    public partial class SalesPage : Page
    {
        private List<SaleItem> _sales;

        public SalesPage()
        {
            InitializeComponent();
            _sales = new List<SaleItem>();
            SalesGrid.ItemsSource = _sales;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text) || !int.TryParse(QuantityTextBox.Text, out int qty) || !double.TryParse(PriceTextBox.Text, out double price))
            {
                MessageBox.Show("Please enter valid product name, quantity and price.");
                return;
            }

            _sales.Add(new SaleItem
            {
                ProductName = ProductNameTextBox.Text,
                Quantity = qty,
                Price = price,
                Date = DateTime.Today
            });

            SalesGrid.Items.Refresh();

            // Clear inputs
            ProductNameTextBox.Text = "";
            QuantityTextBox.Text = "";
            PriceTextBox.Text = "";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SalesGrid.SelectedItem is SaleItem selectedItem)
            {
                _sales.Remove(selectedItem);
                SalesGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a sale to delete.");
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SalesGrid.SelectedItem is SaleItem selectedItem)
            {
                ProductNameTextBox.Text = selectedItem.ProductName;
                QuantityTextBox.Text = selectedItem.Quantity.ToString();
                PriceTextBox.Text = selectedItem.Price.ToString();

                _sales.Remove(selectedItem);
                SalesGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a sale to edit.");
            }
        }
    }
}
