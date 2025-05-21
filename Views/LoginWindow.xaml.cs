using Expense_Tracker.DataAccess;
using Expense_Tracker.Models;
using System.Windows;
using System.Windows.Threading;

namespace Expense_Tracker.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorText.Text = "Please fill in all fields.";
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            var isValid = DatabaseHelper.ValidateUser(username, password);
            if (isValid)
            {
                SuccessText.Text = "Signing in...";
                SuccessText.Visibility = Visibility.Visible;

                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    SuccessText.Visibility = Visibility.Collapsed;

                    
                    var user = new User { Username = username, Password = password };

                    var mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    this.Close();
                };
                timer.Start();
            }
            else
            {
                ErrorText.Text = "Invalid username or password.";
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void SignUpLink_Click(object sender, RoutedEventArgs e)
        {
            var signUpWindow = new SignUpWindow();
            signUpWindow.Show();
            this.Close();
        }
    }
}
