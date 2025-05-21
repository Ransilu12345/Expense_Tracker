using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Series;
using Expense_Tracker.Models;

namespace Expense_Tracker.Views
{
    public partial class DashboardPage : Page
    {
        public PlotModel PieChartModel { get; set; }

        private User _currentUser;
        private double _totalSales;
        private double _totalExpenses;

        public DashboardPage(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // Default data
            _totalSales = 100000;
            _totalExpenses = 20000;

            // Initialize pie chart
            PieChartModel = CreatePieChart(_totalSales, _totalExpenses);

            // Bind to UI
            DataContext = this;
        }

        private PlotModel CreatePieChart(double sales, double expenses)
        {
            var model = new PlotModel { Title = "Expense Breakdown" };

            var pieSeries = new PieSeries
            {
                StrokeThickness = 1,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            pieSeries.Slices.Add(new PieSlice("Expenses", expenses) { Fill = OxyColors.Salmon });
            pieSeries.Slices.Add(new PieSlice("Profit", sales - expenses) { Fill = OxyColors.LightGreen });

            model.Series.Add(pieSeries);

            return model;
        }
    }
}
