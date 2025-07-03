using System;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Series;
using Expense_Tracker.DataAccess;
using Expense_Tracker.Models;

namespace Expense_Tracker.Views
{
    public partial class DashboardPage : Page
    {
        public PlotModel PieChartModel { get; private set; }

        private User _currentUser;
        private double _totalSales;
        private double _totalExpenses;
        private double _netProfit;

        public DashboardPage(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            
            _totalSales = DatabaseHelper.GetTotalSales();
            _totalExpenses = DatabaseHelper.GetTotalExpenses();
            _netProfit = _totalSales - _totalExpenses;


            PieChartModel = new PlotModel { Title = "" };


            
            var pieSeries = new PieSeries
            {
                StrokeThickness = 0.25,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };

            pieSeries.Slices.Add(new PieSlice("Expenses", _totalExpenses) { IsExploded = true, Fill = OxyColors.IndianRed });
            pieSeries.Slices.Add(new PieSlice("Sales", _totalSales) { IsExploded = true, Fill = OxyColors.BlueViolet });
            pieSeries.Slices.Add(new PieSlice("Profit", _netProfit) { IsExploded = true, Fill = OxyColors.GreenYellow });

            PieChartModel.Series.Add(pieSeries);

            
            DataContext = this;

            
            TotalSalesText.Text = $"Total Sales: Rs. {_totalSales:F2}";
            TotalExpensesText.Text = $"Total Expenses: Rs. {_totalExpenses:F2}";
            NetProfitText.Text = $"Net Profit: Rs. {_netProfit:F2}";
        }
    }
}
