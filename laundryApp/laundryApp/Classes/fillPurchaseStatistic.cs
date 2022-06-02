using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace POS.Classes
{
    class fillPurchaseStatistic
    {
        public static IEnumerable<Invoice> fillDgBranches(IEnumerable<Invoice> Invoices, ObservableCollection<int> stackedButton, DataGridColumn col, CheckBox chkInvoice, CheckBox chkReturn, CheckBox chkDraft, ComboBox comboBox, CheckBox all, DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime, TextBox search)
        {
            fillDatesBranches(startDate, endDate, startTime, endTime);
            IEnumerable<Invoice> dgData = Invoices.Where(x =>
               ((chkDraft.IsChecked == true ? (x.invType == "pd" || x.invType == "pbd") : false) || (chkReturn.IsChecked == true ? (x.invType == "pb") : false) || (chkInvoice.IsChecked == true ? (x.invType == "p") : false))
               && (comboBox.SelectedItem != null ? stackedButton.Contains((int)x.branchId) : true)
               && (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
               && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)
               && (startTime.SelectedTime != null ? x.invDate >= startTime.SelectedTime : true)
               && (endTime.SelectedTime != null ? x.invDate <= endTime.SelectedTime : true)
               && (search.Text != null ? (x.invNumber.Contains(search.Text) || x.invType.Contains(search.Text) || x.discountType.Contains(search.Text) || x.branchName.Contains(search.Text)) : true));
            return dgData;
        }

        public static IEnumerable<Invoice> fillDgPos(IEnumerable<Invoice> Invoices, ObservableCollection<int> stackedButton, DataGridColumn col, CheckBox chkInvoice, CheckBox chkReturn, CheckBox chkDraft, ComboBox comboBox, CheckBox all, DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime, TextBox search)
        {
            fillDatesBranches(startDate, endDate, startTime, endTime);
            IEnumerable<Invoice> dgData = Invoices.Where(x =>
                ((chkDraft.IsChecked == true ? (x.invType == "pd" || x.invType == "pbd") : false) || (chkReturn.IsChecked == true ? (x.invType == "pb") : false) || (chkInvoice.IsChecked == true ? (x.invType == "p") : false))
                && (comboBox.SelectedItem != null ? stackedButton.Contains((int)x.posId) : true)
                && (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
                && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)
                && (startTime.SelectedTime != null ? x.invDate >= startTime.SelectedTime : true)
                && (endTime.SelectedTime != null ? x.invDate <= endTime.SelectedTime : true)
                && (search.Text != null ? (x.invNumber.Contains(search.Text) || x.invType.Contains(search.Text) || x.discountType.Contains(search.Text) || x.posName.Contains(search.Text)) : true));
            return dgData;
        }

        public static IEnumerable<Invoice> fillDgVendors(IEnumerable<Invoice> Invoices, ObservableCollection<int> stackedButton, DataGridColumn col, CheckBox chkInvoice, CheckBox chkReturn, CheckBox chkDraft, ComboBox comboBox, CheckBox all, DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime, TextBox search)
        {
            fillDatesBranches(startDate, endDate, startTime, endTime);
            IEnumerable<Invoice> dgData = Invoices.Where(x =>
               ((chkDraft.IsChecked == true ? (x.invType == "pd" || x.invType == "pbd") : false) || (chkReturn.IsChecked == true ? (x.invType == "pb") : false) || (chkInvoice.IsChecked == true ? (x.invType == "p") : false))
               && (comboBox.SelectedItem != null ? stackedButton.Contains((int)x.agentId) : true)
               && (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
               && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)
               && (startTime.SelectedTime != null ? x.invDate >= startTime.SelectedTime : true)
               && (endTime.SelectedTime != null ? x.invDate <= endTime.SelectedTime : true)
               && (search.Text != null ? (x.invNumber.Contains(search.Text) || x.invType.Contains(search.Text) || x.discountType.Contains(search.Text) || x.agentName.Contains(search.Text)) : true));
            return dgData;
        }

        public static IEnumerable<Invoice> fillDgUsers(IEnumerable<Invoice> Invoices, ObservableCollection<int> stackedButton, DataGridColumn col, CheckBox chkInvoice, CheckBox chkReturn, CheckBox chkDraft, ComboBox comboBox, CheckBox all, DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime, TextBox search)
        {
            fillDatesBranches(startDate, endDate, startTime, endTime);
            IEnumerable<Invoice> dgData = Invoices.Where(x =>
               ((chkDraft.IsChecked == true ? (x.invType == "pd" || x.invType == "pbd") : false) || (chkReturn.IsChecked == true ? (x.invType == "pb") : false) || (chkInvoice.IsChecked == true ? (x.invType == "p") : false))
               && (comboBox.SelectedItem != null ? stackedButton.Contains((int)x.createUserId) : true)
               && (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
               && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)
               && (startTime.SelectedTime != null ? x.invDate >= startTime.SelectedTime : true)
               && (endTime.SelectedTime != null ? x.invDate <= endTime.SelectedTime : true)
               && (search.Text != null ? (x.invNumber.Contains(search.Text) || x.invType.Contains(search.Text) || x.discountType.Contains(search.Text) || x.cUserAccName.Contains(search.Text)) : true));
            return dgData;
        }

        public static IEnumerable<Invoice> fillDgItems(IEnumerable<Invoice> Invoices, ObservableCollection<int> stackedButton, DataGridColumn col, CheckBox chkInvoice, CheckBox chkReturn, CheckBox chkDraft, ComboBox comboBox, CheckBox all, DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime, TextBox search)
        {
            fillDatesBranches(startDate, endDate, startTime, endTime);
            IEnumerable<Invoice> dgData = Invoices.Where(x =>
               ((chkDraft.IsChecked == true ? (x.invType == "pd" || x.invType == "pbd") : false) || (chkReturn.IsChecked == true ? (x.invType == "pb") : false) || (chkInvoice.IsChecked == true ? (x.invType == "p") : false))
               && (comboBox.SelectedItem != null ? stackedButton.Contains((int)x.branchId) : true)
               && (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
               && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)
               && (startTime.SelectedTime != null ? x.invDate >= startTime.SelectedTime : true)
               && (endTime.SelectedTime != null ? x.invDate <= endTime.SelectedTime : true)
               && (search.Text != null ? (x.invNumber.Contains(search.Text) || x.invType.Contains(search.Text) || x.discountType.Contains(search.Text) || x.posName.Contains(search.Text)) : true));
            return dgData;
        }

        private static void fillDatesBranches(DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime)
        {
            if (startDate.SelectedDate != null && startTime.SelectedTime != null)
            {
                string x = startDate.SelectedDate.Value.Date.ToShortDateString();
                string y = startTime.SelectedTime.Value.ToShortTimeString();
                string resultStartTime = x + " " + y;
                startTime.SelectedTime = DateTime.Parse(resultStartTime);
                startDate.SelectedDate = DateTime.Parse(resultStartTime);
            }
            if (endDate.SelectedDate != null && endTime.SelectedTime != null)
            {
                string x = endDate.SelectedDate.Value.Date.ToShortDateString();
                string y = endTime.SelectedTime.Value.ToShortTimeString();
                string resultEndTime = x + " " + y;
                endTime.SelectedTime = DateTime.Parse(resultEndTime);
                endDate.SelectedDate = DateTime.Parse(resultEndTime);
            }
        }

    }
}
