using System.Windows;
using System.Windows.Controls;
using Expense_Tracker.DataAccess;
using Expense_Tracker.Models;

namespace Expense_Tracker.Views
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.");
                return;
            }

            var success = DatabaseHelper.RegisterUser(username, password);
            if (success)
            {
                MessageBox.Show("Registration successful! You can now log in.");

                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username already exists. Please choose another.");
            }
        }


        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void FullNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            ErrorText.Visibility = Visibility.Collapsed;
        }

    }
}
