using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Controls;

namespace StepAppAdmin.Views
{
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            InitializeComponent();
            LoadSalesData();
            LoadRevenueData();
            LoadUserData();
        }

        private void LoadSalesData()
        {
            SalesChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Satışlar",
                    Values = new ChartValues<int> { 10, 50, 39, 50 }
                }
            };

            SalesChart.AxisX.Add(new Axis
            {
                Title = "Aylar",
                Labels = new[] { "Yanvar", "Fevral", "Mart", "Aprel" }
            });

            SalesChart.AxisY.Add(new Axis
            {
                Title = "Satış Miqdarı"
            });
        }

        private void LoadRevenueData()
        {
            RevenueChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Gəlir",
                    Values = new ChartValues<double> { 1000, 1500, 1300, 1800 }
                }
            };

            RevenueChart.AxisX.Add(new Axis
            {
                Title = "Aylar",
                Labels = new[] { "Yanvar", "Fevral", "Mart", "Aprel" }
            });

            RevenueChart.AxisY.Add(new Axis
            {
                Title = "Gəlir (AZN)"
            });
        }

        private void LoadUserData()
        {
            UserChart.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Aktiv İstifadəçilər",
                    Values = new ChartValues<int> { 70 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Pasiv İstifadəçilər",
                    Values = new ChartValues<int> { 30 },
                    DataLabels = true
                }
            };
        }
    }
}
