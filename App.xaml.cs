using System.Windows;
using Expense_Tracker.Views;

namespace Expense_Tracker
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Expense_Tracker.DataAccess.DatabaseHelper.InitializeDatabase();

            var signUpWindow = new SignUpWindow();
            signUpWindow.Show();
        }


    }
}
