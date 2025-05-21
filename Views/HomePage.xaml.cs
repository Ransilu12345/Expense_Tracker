using System.Windows.Controls;
using Expense_Tracker.Models;

namespace Expense_Tracker.Views
{
    public partial class HomePage : Page
    {
        private User _currentUser;

        public HomePage(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }
    }
}
