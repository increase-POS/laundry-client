using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace laundryApp.Classes
{
    class ReportsHelp
    {
        public static void showSelectedStack(Grid grid, StackPanel stack)
        {
            foreach (StackPanel stackPanel in grid.Children)
            {
                stackPanel.Visibility = Visibility.Collapsed;
            }
            stack.Visibility = Visibility.Visible;
        }

        public static void hideAllColumns(DataGrid dg)
        {
            foreach (var item in dg.Columns)
            {
                item.Visibility = Visibility.Collapsed;
            }
        }

        public static void isEnabledButtons(Grid grid, Button button)
        {
            foreach (Border bdr in grid.Children)
            {
                bdr.Child.IsEnabled = true;
            }
            button.IsEnabled = false;
            button.Opacity = 1;
        }

        public static void paintTabControlBorder(Grid grid, Border border)
        {
            foreach (Border bdr in grid.Children)
            {
                bdr.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            }
            border.Background = Brushes.White;
        }

        public static void showTabControlGrid(Grid grid, Grid grd)
        {
            foreach (Grid item in grid.Children)
            {
                item.Visibility = Visibility.Collapsed;
            }
            grd.Visibility = Visibility.Visible;
        }

    }
}
