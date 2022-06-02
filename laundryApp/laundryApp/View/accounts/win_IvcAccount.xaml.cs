using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace laundryApp.View.accounts
{
    /// <summary>
    /// Interaction logic for win_IvcAccount.xaml
    /// </summary>
    public partial class win_IvcAccount : Window
    {
      
        int selectedChart = 1;
        IEnumerable<CashTransfer> cashQuery;
        IEnumerable<Invoice> invoiceQuery;
        IEnumerable<AgentMembershipCash> subscriptionQuery;
        List<double> chartList;
        List<double> PiechartList;
        List<double> ColumnchartList;

        bool isSameList;
        int account;
        string label;

        public SeriesCollection SeriesCollection { get; set; }

        public win_IvcAccount(IEnumerable<CashTransfer> _cashQuery, int _account)
        {
            try
            {
                InitializeComponent();
                cashQuery = _cashQuery;
                account = _account;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public win_IvcAccount(IEnumerable<Invoice> _invoiceQuery, int _account)
        {
            try
            {
                InitializeComponent();
                invoiceQuery = _invoiceQuery;
                account = _account;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public win_IvcAccount(IEnumerable<AgentMembershipCash> _subscriptionQuery, int _account)
        {
            try
            {
                InitializeComponent();
                subscriptionQuery = _subscriptionQuery;
                account = _account;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                #region translate
                if (AppSettings.lang.Equals("en"))
            {

                grid_main.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                grid_main.FlowDirection = FlowDirection.RightToLeft;
            }
            translate();
            #endregion

            chartList = new List<double>();
            PiechartList = new List<double>();
            ColumnchartList = new List<double>();
            fillDates();
            fillSelectedChart();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trReports");
            rdoMonth.Content = AppSettings.resourcemanager.GetString("trMonth");
            rdoYear.Content = AppSettings.resourcemanager.GetString("trYear");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dpStrtDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dpEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
        }

        public void fillDates()
        {
            dpEndDate.SelectedDate = DateTime.Now;
            dpStrtDate.SelectedDate = dpEndDate.SelectedDate.Value.AddYears(-1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        public void fillChart()
        {
            chartList.Clear();
            MyAxis.Labels = new List<string>();
            int startYear = dpStrtDate.SelectedDate.Value.Year;
            int endYear = dpEndDate.SelectedDate.Value.Year;
            int startMonth = dpStrtDate.SelectedDate.Value.Month;
            int endMonth = dpEndDate.SelectedDate.Value.Month;
            if (rdoMonth.IsChecked == true)
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    for (int month = startMonth; month <= 12; month++)
                    {
                        DateTime firstOfThisMonth = DateTime.Now;
                        try
                        {
                            firstOfThisMonth = new DateTime(year, month, dpStrtDate.SelectedDate.Value.Day);
                        }
                        catch
                        {
                            try
                            {
                                firstOfThisMonth = new DateTime(year, month, 29);
                            }
                            catch
                            {
                                firstOfThisMonth = new DateTime(year, month, 28);
                            }
                        }
                        var firstOfNextMonth = firstOfThisMonth.AddMonths(1);

                        if (account == 1)
                        {
                            var Draw = cashQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            chartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trCashtransfers");
                        }
                        else if (account == 2)
                        {
                            var Draw = invoiceQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            chartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trOrdersCount");
                        }
                        else if (account == 3)
                        {
                            var Draw = subscriptionQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            chartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trSubscriptions");
                        }
                        MyAxis.Separator.Step = 2;
                        MyAxis.Labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + "/" + year);
                        if (year == dpEndDate.SelectedDate.Value.Year && month == dpEndDate.SelectedDate.Value.Month)
                        {
                            break;
                        }
                        if (month == 12)
                        {
                            startMonth = 1;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    var firstOfThisYear = new DateTime(year, 1, dpStrtDate.SelectedDate.Value.Month);
                    var firstOfNextMYear = firstOfThisYear.AddYears(1);
                    if (account == 1)
                    {
                        var Draw = cashQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        chartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trCashtransfers");
                    }
                    if (account == 2)
                    {
                        var Draw = invoiceQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        chartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trOrdersCount");
                    }
                    if (account == 3)
                    {
                        var Draw = subscriptionQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        chartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trSubscriptions");
                    }

                    MyAxis.Separator.Step = 1;
                    MyAxis.Labels.Add(year.ToString());
                }
            }
            SeriesCollection = new SeriesCollection
           {
                 new LineSeries
               {
                        Title = label,
                   Values = chartList.AsChartValues()
               },
           };
            grid1.Children.Clear();
            grid1.Children.Add(charts);
            DataContext = this;

        }

        public void fillPieChart()
        {

            PiechartList.Clear();
            SeriesCollection piechartData = new SeriesCollection();
            List<string> titles = new List<string>();
            int startYear = dpStrtDate.SelectedDate.Value.Year;
            int endYear = dpEndDate.SelectedDate.Value.Year;
            int startMonth = dpStrtDate.SelectedDate.Value.Month;
            int endMonth = dpEndDate.SelectedDate.Value.Month;
            if (rdoMonth.IsChecked == true)
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    for (int month = startMonth; month <= 12; month++)
                    {
                        DateTime firstOfThisMonth = DateTime.Now;
                        try
                        {
                            firstOfThisMonth = new DateTime(year, month, dpStrtDate.SelectedDate.Value.Day);
                        }
                        catch
                        {
                            try
                            {
                                firstOfThisMonth = new DateTime(year, month, 29);
                            }
                            catch
                            {
                                firstOfThisMonth = new DateTime(year, month, 28);
                            }
                        }
                        var firstOfNextMonth = firstOfThisMonth.AddMonths(1);
                        if (account == 1)
                        {
                            var Draw = cashQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            PiechartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trCashtransfers");
                        }
                        if (account == 2)
                        {
                            var Draw = invoiceQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            PiechartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trOrdersCount");
                        }
                        if (account == 3)
                        {
                            var Draw = subscriptionQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            PiechartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trSubscriptions");
                        }
                        titles.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + "/" + year);
                        if (year == dpEndDate.SelectedDate.Value.Year && month == dpEndDate.SelectedDate.Value.Month)
                        {
                            break;
                        }
                        if (month == 12)
                        {
                            startMonth = 1;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    var firstOfThisYear = new DateTime(year, 1, dpStrtDate.SelectedDate.Value.Month);
                    var firstOfNextMYear = firstOfThisYear.AddYears(1);
                    if (account == 1)
                    {
                        var Draw = cashQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        PiechartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trCashtransfers");
                    }
                    else if (account == 2)
                    {
                        var Draw = invoiceQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        PiechartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trOrdersCount");
                    }
                    else if (account == 3)
                    {
                        var Draw = subscriptionQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        PiechartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trSubscriptions");
                    }
                    titles.Add(year.ToString());
                }
            }
            for (int i = 0; i < PiechartList.Count(); i++)
            {
                List<double> final = new List<double>();
                List<string> lable = new List<string>();
                final.Add(PiechartList.ToList().Skip(i).FirstOrDefault());
                piechartData.Add(
                  new PieSeries
                  {
                      Values = final.AsChartValues(),
                      Title = titles.ToList().Skip(i).FirstOrDefault().ToString(),
                      DataLabels = true,
                  }
              );
            }
            pieChart.Series = piechartData;
        }

        public void fillColumnChart()
        {
            columnAxis.Labels = new List<string>();
            ColumnchartList.Clear();
            SeriesCollection columnchartData = new SeriesCollection();
            List<string> titles = new List<string>();
            int startYear = dpStrtDate.SelectedDate.Value.Year;
            int endYear = dpEndDate.SelectedDate.Value.Year;
            int startMonth = dpStrtDate.SelectedDate.Value.Month;
            int endMonth = dpEndDate.SelectedDate.Value.Month;
            if (rdoMonth.IsChecked == true)
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    for (int month = startMonth; month <= 12; month++)
                    {
                        DateTime firstOfThisMonth = DateTime.Now;
                        try
                        {
                            firstOfThisMonth = new DateTime(year, month, dpStrtDate.SelectedDate.Value.Day);
                        }
                        catch
                        {
                            try
                            {
                                firstOfThisMonth = new DateTime(year, month, 29);
                            }
                            catch
                            {
                                firstOfThisMonth = new DateTime(year, month, 28);
                            }
                        }
                        var firstOfNextMonth = firstOfThisMonth.AddMonths(1);
                        if (account == 1)
                        {
                            var Draw = cashQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            ColumnchartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trCashtransfers");
                        }
                        else if (account == 2)
                        {
                            var Draw = invoiceQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            ColumnchartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trOrdersCount");
                        }
                        else if (account == 3)
                        {
                            var Draw = subscriptionQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            ColumnchartList.Add(Draw);
                            label = AppSettings.resourcemanager.GetString("trSubscriptions");
                        }
                        columnAxis.Separator.Step = 2;
                        columnAxis.Labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + "/" + year);
                        if (year == dpEndDate.SelectedDate.Value.Year && month == dpEndDate.SelectedDate.Value.Month)
                        {
                            break;
                        }
                        if (month == 12)
                        {
                            startMonth = 1;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    var firstOfThisYear = new DateTime(year, 1, dpStrtDate.SelectedDate.Value.Month);
                    var firstOfNextMYear = firstOfThisYear.AddYears(1);
                    if (account == 1)
                    {
                        var Draw = cashQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        ColumnchartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trCashtransfers");
                    }
                    else if (account == 2)
                    {
                        var Draw = invoiceQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        ColumnchartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trOrdersCount");
                    }
                    else if (account == 3)
                    {
                        var Draw = subscriptionQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        ColumnchartList.Add(Draw);
                        label = AppSettings.resourcemanager.GetString("trSubscriptions");
                    }
                    columnAxis.Separator.Step = 1;
                    columnAxis.Labels.Add(year.ToString());
                }
            }
            columnchartData.Add(
                 new ColumnSeries
                 {
                     Title = label,
                     Values = ColumnchartList.AsChartValues(),
                 }
             );
            columnChart.Series = columnchartData;
        }

        private void dpStrtDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (dpEndDate.SelectedDate.Value.Year - dpStrtDate.SelectedDate.Value.Year > 1)
                {
                    rdoYear.IsChecked = true;
                    fillSelectedChart();
                }
                else
                {
                    fillSelectedChart();
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void dpEndDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (dpEndDate.SelectedDate.Value.Year - dpStrtDate.SelectedDate.Value.Year > 1)
                {
                    rdoYear.IsChecked = true;
                    fillSelectedChart();
                }
                else
                {
                    fillSelectedChart();
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                rdoMonth.IsChecked = true;
                fillDates();
                selectedChart = 1;
                fillSelectedChart();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void rdoYear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (dpEndDate.SelectedDate.Value.Year - dpStrtDate.SelectedDate.Value.Year > 1)
                {
                    rdoYear.IsChecked = true;
                    fillSelectedChart();
                }
                else
                {
                    fillSelectedChart();
                }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void rdoMonth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (dpEndDate.SelectedDate.Value.Year - dpStrtDate.SelectedDate.Value.Year > 1)
                {
                    rdoYear.IsChecked = true;
                    fillSelectedChart();
                }
                else
                {
                    fillSelectedChart();
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void fillSelectedChart()
        {
            grid1.Visibility = Visibility.Hidden;
            grd_pieChart.Visibility = Visibility.Hidden;
            grd_columnChart.Visibility = Visibility.Hidden;

            icon_rowChar.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DFDFDF"));
            icon_columnChar.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DFDFDF"));
            icon_pieChar.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DFDFDF"));

            if (selectedChart == 1)
            {
                grid1.Visibility = Visibility.Visible;
                icon_rowChar.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                fillChart();
            }
            else if (selectedChart == 2)
            {
                grd_pieChart.Visibility = Visibility.Visible;
                icon_pieChar.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                fillPieChart();
            }
            else if (selectedChart == 3)
            {
                grd_columnChart.Visibility = Visibility.Visible;
                icon_columnChar.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                fillColumnChart();
            }
        }

        private void btn_rowChart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                selectedChart = 1;
                fillSelectedChart();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void btn_pieChart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                selectedChart = 2;
                fillSelectedChart();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void btn_columnChart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                selectedChart = 3;
                fillSelectedChart();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }
    }
}
