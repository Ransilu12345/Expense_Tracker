using Expense_Tracker.Models;
using Expense_Tracker.Views;
using System.Windows;

namespace Expense_Tracker
{
    public partial class MainWindow : Window
    {
        private User _user;

        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;
            MainFrame.Navigate(new HomePage(_user));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage(_user));
        }
        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage(_user));
        }

        private void Sales_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SalesPage());
        }


        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InventoryPage(_user));
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportsPage(_user));
        }

    }
}
