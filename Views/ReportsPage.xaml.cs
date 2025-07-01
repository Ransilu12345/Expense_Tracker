using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Expense_Tracker.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Expense_Tracker.DataAccess;

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

            // Fetch real data from the database
            _totalSales = DatabaseHelper.GetTotalSales();
            _totalExpenses = DatabaseHelper.GetTotalExpenses();
            _netProfit = _totalSales - _totalExpenses;

            // Update UI
            TotalSalesText.Text = $"Rs. {_totalSales:F2}";
            TotalExpensesText.Text = $"Rs. {_totalExpenses:F2}";
            NetProfitText.Text = $"Rs. {_netProfit:F2}";

            // Required for QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = "Portfolio Report.pdf";

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

                            col.Item().Text(text =>
                            {
                                text.Span("Net Profit: Rs. ");
                                text.Span($"{_netProfit:F2}").Bold();
                            });
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
    }
}
