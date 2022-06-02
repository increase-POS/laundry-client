using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_selectInvType.xaml
    /// </summary>
    public partial class wd_selectInvType : Window
    {
        public wd_selectInvType()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            this.Close();
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_select_Click(btn_select, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        //public Tables table;
        //List<Tables> tables = new List<Tables>();
        public bool isOk { get; set; }


        public static List<string> requiredControlList = new List<string>();
        string notes = "";
        UserSetValues userSetValuesModel = new UserSetValues();
        SetValues invSet;
        UserSetValues defaultInvTypeSetValue;
        int defaulInvType;
        int settingId;
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "invType" };

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                FillCombo.FillInvoiceType(cb_invType);
                await getDefaultInvoiceType();

                cb_invType.SelectedValue = AppSettings.invType;
                if (cb_invType.SelectedIndex == -1)
                    cb_invType.SelectedIndex = 0;

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
            txt_title.Text = AppSettings.resourcemanager.GetString("invoiceType");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_invType, AppSettings.resourcemanager.GetString("invoiceType") + "...");
            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");
        }

        private async Task getDefaultInvoiceType()
        {
            List<UserSetValues> lst = await userSetValuesModel.GetAll();
            SetValues setValueModel = new SetValues();
            invSet = await setValueModel.GetBySetNameAndUserId("invType", MainWindow.userLogin.userId);
            if (invSet != null)
            {
                defaulInvType = invSet.valId;
                settingId = (int)invSet.settingId;
                notes = invSet.notes;
                try
                {
                    defaultInvTypeSetValue = lst.Where(u => u.valId == defaulInvType && u.userId == MainWindow.userLogin.userId).FirstOrDefault();
                    cb_invType.SelectedValue = invSet.value;
                }
                catch { }
            }
            else
            {
                SettingCls settingCls = new SettingCls();
                List<SettingCls> lstSettings;
                lstSettings = await settingCls.GetAll();
                var invTypeSet = lstSettings.Where(x => x.name == "invType").FirstOrDefault();
                settingId = (int)invTypeSet.settingId;
                notes = invTypeSet.notes;
            }
        }
        private async void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HelpClass.validate(requiredControlList, this))
                {
                    HelpClass.StartAwait(grid_main);
                    // if have id return true


                    isOk = true;

                    if (defaultInvTypeSetValue == null)
                        defaultInvTypeSetValue = new UserSetValues();

                    #region save set value
                    if (invSet == null)
                    {
                        invSet = new SetValues();
                        invSet.settingId = settingId;
                        invSet.isSystem = 0;
                        invSet.isDefault = 0;
                    }

                    invSet.value = cb_invType.SelectedValue.ToString();
                    invSet.notes = notes;

                    int res = await invSet.Save(invSet);
                    #endregion

                    #region save user setting value
                    if (res > 0)
                    {
                        defaultInvTypeSetValue.userId = MainWindow.userLogin.userId;
                        defaultInvTypeSetValue.valId = res;
                        defaultInvTypeSetValue.notes = notes;
                        defaultInvTypeSetValue.createUserId = MainWindow.userLogin.userId;
                        defaultInvTypeSetValue.updateUserId = MainWindow.userLogin.userId;
                        await userSetValuesModel.Save(defaultInvTypeSetValue);
                    }
                    #endregion

                    AppSettings.invType = cb_invType.SelectedValue.ToString();
                    this.Close();

                    HelpClass.EndAwait(grid_main);
                }
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
