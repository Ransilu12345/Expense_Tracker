using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using Expense_Tracker.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace Expense_Tracker.Views
{
    public partial class ReportsPage : Page
    {
        private User _currentUser;
        private double _totalSales;
        private double _totalExpenses;
        private double _netProfit;

        public ReportsPage(User user)
        {
            InitializeComponent();
            _currentUser = user;

            // Simulated data
            _totalSales = 100000.00;
            _totalExpenses = 20000.00;
            _netProfit = _totalSales - _totalExpenses;

            TotalSalesText.Text = $"Rs. {_totalSales:F2}";
            TotalExpensesText.Text = $"Rs. {_totalExpenses:F2}";
            NetProfitText.Text = $"Rs. {_netProfit:F2}";

            QuestPDF.Settings.License = LicenseType.Community;
        }

        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = "ExpenseReport.pdf";

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(50);
                        page.DefaultTextStyle(x => x.FontSize(14));

                        page.Header().Text("Monthly Expense Report")
                            .FontSize(20)
                            .Bold()
                            .AlignCenter();

                        page.Content().Column(col =>
                        {
                            col.Spacing(15);

                            col.Item().Text($"User: {_currentUser.Username}");

                            col.Item().Text(text =>
                            {
                                text.Span("Total Sales: Rs. ");
                                text.Span($"{_totalSales:F2}").FontColor(Colors.Green.Medium);
                            });

                            col.Item().Text(text =>
                            {
                                text.Span("Total Expenses: Rs. ");
                                text.Span($"{_totalExpenses:F2}").FontColor(Colors.Red.Medium);
                            });

                            col.Item().Text($"Net Profit: Rs. {_netProfit:F2}")
                                .FontSize(14)
                                .Bold()
                                .FontColor(Colors.Black);
                        });

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span("Generated on ");
                            text.Span(DateTime.Now.ToString("dd MMM yyyy HH:mm")).SemiBold();
                        });
                    });
                });

                document.GeneratePdf(fileName);

                MessageBox.Show($"PDF exported successfully to {fileName}");
                Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to export PDF: " + ex.Message);
            }
        }

        private void Button_IsMouseCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
