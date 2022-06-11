using Microsoft.Win32;
using netoaster;
using Newtonsoft.Json;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace laundryApp.View.settings
{
    /// <summary>
    /// Interaction logic for uc_packageBookSetting.xaml
    /// </summary>
    public partial class uc_packageBookSetting : UserControl
    {
        private static uc_packageBookSetting _instance;
        public static uc_packageBookSetting Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_packageBookSetting();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_packageBookSetting()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        ProgramDetails progDetailsModel = new ProgramDetails();
        ProgramDetails progDetails = new ProgramDetails();

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
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

                progDetails = await progDetailsModel.getCurrentInfo();

                #region unlimited
                if (progDetails.branchCount == -1)
                { dpnl_branch.Visibility = Visibility.Collapsed; txt_branchUnlimited.Visibility = Visibility.Visible; }
                else
                { dpnl_branch.Visibility = Visibility.Visible; txt_branchUnlimited.Visibility = Visibility.Collapsed; }

                if (progDetails.storeCount == -1)
                { dpnl_store.Visibility = Visibility.Collapsed; txt_storeUnlimited.Visibility = Visibility.Visible; }
                else
                { dpnl_store.Visibility = Visibility.Visible; txt_storeUnlimited.Visibility = Visibility.Collapsed; }

                if (progDetails.vendorCount == -1)
                { dpnl_vendor.Visibility = Visibility.Collapsed; txt_vendorUnlimited.Visibility = Visibility.Visible; }
                else
                { dpnl_vendor.Visibility = Visibility.Visible; txt_vendorUnlimited.Visibility = Visibility.Collapsed; }

                if (progDetails.userCount == -1)
                { dpnl_user.Visibility = Visibility.Collapsed; txt_userUnlimited.Visibility = Visibility.Visible; }
                else
                { dpnl_user.Visibility = Visibility.Visible; txt_userUnlimited.Visibility = Visibility.Collapsed; }

                if (progDetails.customerCount == -1)
                { dpnl_customer.Visibility = Visibility.Collapsed; txt_customerUnlimited.Visibility = Visibility.Visible; }
                else
                { dpnl_customer.Visibility = Visibility.Visible; txt_customerUnlimited.Visibility = Visibility.Collapsed; }

                if (progDetails.posCount == -1)
                { dpnl_pos.Visibility = Visibility.Collapsed; txt_posUnlimited.Visibility = Visibility.Visible; }
                else
                { dpnl_pos.Visibility = Visibility.Visible; txt_posUnlimited.Visibility = Visibility.Collapsed; }

                if (progDetails.saleinvCount == -1)
                { dpnl_salesInv.Visibility = Visibility.Collapsed; txt_salesInvUnlimited.Visibility = Visibility.Visible; }
                else
                { dpnl_salesInv.Visibility = Visibility.Visible; txt_salesInvUnlimited.Visibility = Visibility.Collapsed; }

                if (progDetails.itemCount == -1)
                { dpnl_item.Visibility = Visibility.Collapsed; txt_itemUnlimited.Visibility = Visibility.Visible; }
                else
                { dpnl_item.Visibility = Visibility.Visible; txt_itemUnlimited.Visibility = Visibility.Collapsed; }
                #endregion

                if (HelpClass.isSupportPermision())
                {
                    col_upgrade.Width = col_extend.Width;
                }
                else
                {
                    col_upgrade.Width = new GridLength(0);
                }

                this.DataContext = progDetails;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void translate()
        {
            txt_packageDetails.Text = AppSettings.resourcemanager.GetString("trPackageDetails");
            txt_packageCodeTitle.Text = AppSettings.resourcemanager.GetString("trCode");
            txt_packageNameTitle.Text = AppSettings.resourcemanager.GetString("trName");
            txt_agentTitle.Text = AppSettings.resourcemanager.GetString("trAgent");
            txt_customerNameTitle.Text = AppSettings.resourcemanager.GetString("trRegisteredFor");
            txt_expiredTitle.Text = AppSettings.resourcemanager.GetString("trExpiredDate");
            txt_statusTitle.Text = AppSettings.resourcemanager.GetString("trServerStatus");
            txt_serverStatusTitle.Text = AppSettings.resourcemanager.GetString("trServerType");

            txt_programDetails.Text = AppSettings.resourcemanager.GetString("trProgramDetails");
            txt_programTitle.Text = AppSettings.resourcemanager.GetString("trProgram");
            txt_versionTitle.Text = AppSettings.resourcemanager.GetString("trVersion");

            txt_activationDetails.Text = AppSettings.resourcemanager.GetString("trActivationDetails");
            txt_serialsTitle.Text = AppSettings.resourcemanager.GetString("trSerial");
            txt_activationCodeTitle.Text = AppSettings.resourcemanager.GetString("trActivationCode");

            txt_packageLimits.Text = AppSettings.resourcemanager.GetString("trPackageLimits");
            txt_branchCountTitle.Text = AppSettings.resourcemanager.GetString("trBranches");
            txt_userCountTitle.Text = AppSettings.resourcemanager.GetString("trUsers");
            txt_customerCountTitle.Text = AppSettings.resourcemanager.GetString("trCustomers");
            txt_salesInvCountTitle.Text = AppSettings.resourcemanager.GetString("trInvoicesPerMonth");
            txt_storeCountNameTitle.Text = AppSettings.resourcemanager.GetString("trStores");
            txt_posCountNameTitle.Text = AppSettings.resourcemanager.GetString("trPOSs");
            txt_vendorCountNameTitle.Text = AppSettings.resourcemanager.GetString("trVendors");
            txt_itemCountNameTitle.Text = AppSettings.resourcemanager.GetString("trItems");

            txt_branchUnlimited.Text = AppSettings.resourcemanager.GetString("trUnlimited");
            txt_userUnlimited.Text = AppSettings.resourcemanager.GetString("trUnlimited");
            txt_customerUnlimited.Text = AppSettings.resourcemanager.GetString("trUnlimited");
            txt_salesInvUnlimited.Text = AppSettings.resourcemanager.GetString("trUnlimited");
            txt_storeUnlimited.Text = AppSettings.resourcemanager.GetString("trUnlimited");
            txt_posUnlimited.Text = AppSettings.resourcemanager.GetString("trUnlimited");
            txt_vendorUnlimited.Text = AppSettings.resourcemanager.GetString("trUnlimited");
            txt_itemUnlimited.Text = AppSettings.resourcemanager.GetString("trUnlimited");

            btn_extend.Content = AppSettings.resourcemanager.GetString("trExtend");
            btn_upgrade.Content = AppSettings.resourcemanager.GetString("trUpgrade");

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        private async void Btn_upgrade_Click(object sender, RoutedEventArgs e)
        {//upgrade
            int chk = 0;
            string activationkey = progDetails.packageSaleCode;//get from info 

            if (progDetails.isOnlineServer.Value)//online
            {
                try
                {
                    if (activationkey.Trim() != "".Trim())
                    {
                        AvtivateServer ac = new AvtivateServer();

                        chk = await ac.checkconn();

                        chk = await ac.StatSendserverkey(activationkey, "up");
                        // //change      chk = 3;
                        //nochange     chk = 2;

                        if (chk <= 0)
                        {
                            string message = "inc(" + chk + ")";

                            string messagecode = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));

                            string msg = AppSettings.resourcemanager.GetString("trUpgradeNotCompleted") + "(" +
                                        AppSettings.resourcemanager.GetString("trErrorCode") + ":" + messagecode + ")";

                            Toaster.ShowWarning(Window.GetWindow(this), message: msg, animation: ToasterAnimation.FadeIn);
                        }

                        else
                        {
                            if (chk == 3)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trUpgradeCompleted"), animation: ToasterAnimation.FadeIn);
                                progDetails = await progDetailsModel.getCurrentInfo();
                                this.DataContext = progDetails;
                            }
                            else if (chk == 2)
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChanges"), animation: ToasterAnimation.FadeIn);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trServerNotFount"), animation: ToasterAnimation.FadeIn);
                }
            }
            else//offline
            {
                // start activate

                chk = 0;
                string message = "";
                try
                {

                    bool isServerActivated = true;
                    AvtivateServer ac = new AvtivateServer();
                    string activeState = "up";
                    int activematch = 0;

                    string filepath = "";
                    openFileDialog.Filter = "INC|*.ac; ";
                    SendDetail customerdata = new SendDetail();
                    SendDetail dc = new SendDetail();
                    if (openFileDialog.ShowDialog() == true)
                    {
                        filepath = openFileDialog.FileName;

                        string objectstr = "";

                        objectstr = ReportCls.decodetoString(filepath);

                        dc = JsonConvert.DeserializeObject<SendDetail>(objectstr, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
                        packagesSend pss = new packagesSend();

                        pss = dc.packageSend;
                        isServerActivated = dc.packageSend.isServerActivated;
                        pss.activeApp = "-";

                        dc.packageSend = pss;

                        // string activeState = "";
                        if (dc.packageSend.activeState == activeState)
                        {
                            activematch = 1;
                            customerdata = await ac.OfflineActivate(dc, activeState);
                        }
                        else
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: "The file isn't upgrade file", animation: ToasterAnimation.FadeIn);
                        }

                    }
                    // upload
                    if (activematch == 1)
                    {
                        if (customerdata.packageSend.result > 0)
                        {
                            // if first activate OR upgrade  show save dialoge to save customer data in file 
                            saveFileDialog.Filter = "File|*.ac;";
                            if (saveFileDialog.ShowDialog() == true)
                            {
                                string DestPath = saveFileDialog.FileName;

                                string myContent = JsonConvert.SerializeObject(customerdata);

                                bool res = false;

                                res = ReportCls.encodestring(myContent, DestPath);

                                if (res)
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: "Success", animation: ToasterAnimation.FadeIn);
                                }
                                else
                                {
                                    Toaster.ShowWarning(Window.GetWindow(this), message: "Error", animation: ToasterAnimation.FadeIn);
                                }

                            }
                        }
                        else
                        {
                            string msg = "NOT complete - " + customerdata.packageSend.result.ToString();
                            Toaster.ShowWarning(Window.GetWindow(this), msg, animation: ToasterAnimation.FadeIn);
                        }
                    }
                    else
                    {
                        //close
                    }
                    //end uploaa 
                }
                catch (Exception ex)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: "The server Not Found", animation: ToasterAnimation.FadeIn);
                }
                //end activate
            }
        }

        private async void Btn_extend_Click(object sender, RoutedEventArgs e)
        {//extend
            int chk = 0;
            string activationkey = progDetails.packageSaleCode;//get from info 

            if (progDetails.isOnlineServer.Value)//online
            {
                #region 
                try
                {
                    if (activationkey.Trim() != "".Trim())
                    {
                        AvtivateServer ac = new AvtivateServer();

                        chk = await ac.checkconn();

                        chk = await ac.StatSendserverkey(activationkey, "rn");
                        // //change      chk = 3;
                        //nochange     chk = 2;

                        if (chk <= 0)
                        {
                            string message = "inc(" + chk + ")";

                            string messagecode = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));

                            string msg = AppSettings.resourcemanager.GetString("trActivationNotCompleted") + "(" +
                                         AppSettings.resourcemanager.GetString("trErrorCode") + ":" + messagecode + ")";

                            Toaster.ShowWarning(Window.GetWindow(this), message: msg, animation: ToasterAnimation.FadeIn);
                        }

                        else
                        {
                            if (chk == 3)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trActivationCompleted"), animation: ToasterAnimation.FadeIn);
                                progDetails = await progDetailsModel.getCurrentInfo();
                                this.DataContext = progDetails;
                            }
                            else if (chk == 2)
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChanges"), animation: ToasterAnimation.FadeIn);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trServerNotFount"), animation: ToasterAnimation.FadeIn);
                }
                #endregion
            }
            else//offline
            {
                // start activate
                chk = 0;
                string message = "";
                try
                {
                    bool isServerActivated = true;
                    AvtivateServer ac = new AvtivateServer();
                    string activeState = "rn";
                    int activematch = 0;

                    string filepath = "";
                    openFileDialog.Filter = "INC|*.ac; ";
                    SendDetail customerdata = new SendDetail();
                    SendDetail dc = new SendDetail();
                    if (openFileDialog.ShowDialog() == true)
                    {
                        filepath = openFileDialog.FileName;

                        string objectstr = "";

                        objectstr = ReportCls.decodetoString(filepath);

                        dc = JsonConvert.DeserializeObject<SendDetail>(objectstr, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
                        packagesSend pss = new packagesSend();

                        pss = dc.packageSend;
                        isServerActivated = dc.packageSend.isServerActivated;
                        pss.activeApp = "-";

                        dc.packageSend = pss;

                        // string activeState = "";
                        if (dc.packageSend.activeState == activeState)
                        {
                            activematch = 1;
                            customerdata = await ac.OfflineActivate(dc, activeState);
                        }
                        else
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: "The file isn't Extend file", animation: ToasterAnimation.FadeIn);
                        }
                    }
                    // upload
                    if (activematch == 1)
                    {
                        if (customerdata.packageSend.result > 0)
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: "Success", animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {
                            string msg = "NOT complete - " + customerdata.packageSend.result.ToString();
                            Toaster.ShowWarning(Window.GetWindow(this), msg, animation: ToasterAnimation.FadeIn);
                        }
                    }
                    else
                    {
                        //close
                    }
                    //end uploaa 
                }
                catch (Exception ex)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: "The server Not Found", animation: ToasterAnimation.FadeIn);
                }

                //end activate

            }

        }

        private async void Btn_serials_Click(object sender, RoutedEventArgs e)
        {//serials
            try
            {
                HelpClass.StartAwait(grid_main);

                Window.GetWindow(this).Opacity = 0.2;
                wd_serials w = new wd_serials();
                w.activationCode = progDetails.packageSaleCode;
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;

                progDetails = await progDetailsModel.getCurrentInfo();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
    }
}
