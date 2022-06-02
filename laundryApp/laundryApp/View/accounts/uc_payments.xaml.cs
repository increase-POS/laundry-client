using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.IO;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;

using System.Windows.Resources;

namespace laundryApp.View.accounts
{
    /// <summary>
    /// Interaction logic for uc_payments.xaml
    /// </summary>
    public partial class uc_payments : UserControl
    {
        public uc_payments()
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
        private static uc_payments _instance;
        public static uc_payments Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                    _instance = new uc_payments();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        Agent agentModel = new Agent();
        User userModel = new User();
        ShippingCompanies shCompanyModel = new ShippingCompanies();
        Pos posModel = new Pos();
        CashTransfer cashModel = new CashTransfer();
        CashTransfer cashtrans = new CashTransfer();

        IEnumerable<Agent> agents;
        IEnumerable<User> users;
        IEnumerable<ShippingCompanies> shCompanies;
        IEnumerable<CashTransfer> cashesQuery;
        IEnumerable<CashTransfer> cashesQueryExcel;

        IEnumerable<CashTransfer> cashes;

        string searchText = "";
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public List<Invoice> invoicesLst = new List<Invoice>();

        string createPermission = "payments_create";
        string reportsPermission = "payments_reports";

        public static List<string> requiredControlList;

        #region card
        List<Button> cardBtnList = new List<Button>();
        List<Ellipse> cardEllipseList = new List<Ellipse>();
        IEnumerable<Card> cards;
        Card cardModel = new Card();
        bool hasProcessNum = false;
        static private int _SelectedCard = -1;

        void InitializeCardsPic(IEnumerable<Card> cards)
        {
            #region cardImageLoad
            dkp_cards.Children.Clear();
            int userCount = 0;
            foreach (var item in cards)
            {
                #region Button
                Button button = new Button();
                button.DataContext = item;
                button.Tag = item.cardId;
                button.Padding = new Thickness(0, 0, 0, 0);
                button.Margin = new Thickness(2.5, 5, 2.5, 5);
                button.Background = null;
                button.BorderBrush = null;
                button.Height = 35;
                button.Width = 35;
                button.Click += card_Click;

                #region grid
                Grid grid = new Grid();
                #region 
                Ellipse ellipse = new Ellipse();
                //ellipse.Margin = new Thickness(-5, 0, -5, 0);
                ellipse.StrokeThickness = 1;
                ellipse.Stroke = Application.Current.Resources["SecondColor"] as SolidColorBrush;
                ellipse.Height = 35;
                ellipse.Width = 35;
                ellipse.FlowDirection = FlowDirection.LeftToRight;
                ellipse.ToolTip = item.name;
                ellipse.Tag = item.cardId;
                userImageLoad(ellipse, item.image);
                Grid.SetColumn(ellipse, userCount);
                grid.Children.Add(ellipse);
                cardEllipseList.Add(ellipse);
                #endregion
                #endregion

                button.Content = grid;
                #endregion
                dkp_cards.Children.Add(button);
                cardBtnList.Add(button);

            }
            #endregion
        }

        void card_Click(object sender, RoutedEventArgs e)
        {
            HelpClass.clearValidate(requiredControlList, this);
            var button = sender as Button;
            _SelectedCard = int.Parse(button.Tag.ToString());

            Card card = button.DataContext as Card;

            txt_card.Text = card.name;

            if (card.hasProcessNum)
            {
                tb_docNumCard.Visibility = Visibility.Visible;
                brd_docNumCard.Visibility = Visibility.Visible;
                hasProcessNum = true;
                if (!requiredControlList.Contains("processNum"))
                    requiredControlList.Add("processNum");
                if (!requiredControlList.Contains("card"))
                    requiredControlList.Add("card");
            }
            else
            {
                tb_docNumCard.Visibility = Visibility.Collapsed;
                brd_docNumCard.Visibility = Visibility.Collapsed;
                hasProcessNum = false;
                if (requiredControlList.Contains("processNum"))
                    requiredControlList.Remove("processNum");
                if (!requiredControlList.Contains("card"))
                    requiredControlList.Add("card");
            }
            //set border color
            foreach (var el in cardEllipseList)
            {
                if ((int)el.Tag == (int)button.Tag)
                    el.Stroke = Application.Current.Resources["MainColor"] as SolidColorBrush;
                else
                    el.Stroke = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            }
            HelpClass.validate(requiredControlList, this);
        }

        ImageBrush brush = new ImageBrush();
        async void userImageLoad(Ellipse ellipse, string image)
        {
            try
            {
                if (!string.IsNullOrEmpty(image))
                {
                    clearImg(ellipse);

                    byte[] imageBuffer = await cardModel.downloadImage(image); // read this as BLOB from your DB
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    ellipse.Fill = new ImageBrush(bitmapImage);
                }
                else
                {
                    clearImg(ellipse);
                }
            }
            catch
            {
                clearImg(ellipse);
            }
        }
        private void clearImg(Ellipse ellipse)
        {
            Uri resourceUri = new Uri("pic/no-image-icon-90x90.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            brush.ImageSource = temp;
            ellipse.Fill = brush;
        }
        #endregion

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                requiredControlList = new List<string> { "cash", "depositTo", "paymentProcessType"};

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

                dp_searchEndDate.SelectedDate = DateTime.Now;
                dp_searchStartDate.SelectedDate = DateTime.Now;

                #region fill deposit to combo
                var depositlist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("trVendor")     , Value = "v" },
                new { Text = AppSettings.resourcemanager.GetString("trCustomer")   , Value = "c" },
                new { Text = AppSettings.resourcemanager.GetString("trUser")       , Value = "u" },
                new { Text = AppSettings.resourcemanager.GetString("trSalary")     , Value = "s" },
                new { Text = AppSettings.resourcemanager.GetString("trGeneralExpenses")     , Value = "e" },
                new { Text = AppSettings.resourcemanager.GetString("trAdministrativePull")  , Value = "m" },
                new { Text = AppSettings.resourcemanager.GetString("trShippingCompanies")  , Value = "sh" }
                 };
                cb_depositTo.DisplayMemberPath = "Text";
                cb_depositTo.SelectedValuePath = "Value";
                cb_depositTo.ItemsSource = depositlist;
                #endregion

                await fillVendors();

                await fillCustomers();

                await fillUsers();

                await fillShippingCompanies();

                #region fill process type
                var typelist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("trCash")       , Value = "cash" },
                //new { Text = AppSettings.resourcemanager.GetString("trDocument")   , Value = "doc" },
                new { Text = AppSettings.resourcemanager.GetString("trCheque")     , Value = "cheque" },
                new { Text = AppSettings.resourcemanager.GetString("trAnotherPaymentMethods") , Value = "card" },
                 };
                cb_paymentProcessType.DisplayMemberPath = "Text";
                cb_paymentProcessType.SelectedValuePath = "Value";
                cb_paymentProcessType.ItemsSource = typelist;
                #endregion

                #region fill card combo
                try
                {
                    cards = await cardModel.GetAll();
                    InitializeCardsPic(cards);
                }
                catch { }
                #endregion

                dp_searchStartDate.SelectedDateChanged += this.dp_SelectedStartDateChanged;
                dp_searchEndDate.SelectedDateChanged += this.dp_SelectedEndDateChanged;

                btn_image.IsEnabled = false;

                await Search();

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
            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );

            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trTransaferDetails");
            //txt_title.Text = AppSettings.resourcemanager.GetString("trPayments");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            chb_all.Content = AppSettings.resourcemanager.GetString("trAll");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_searchStartDate, AppSettings.resourcemanager.GetString("trStartDate")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_searchEndDate, AppSettings.resourcemanager.GetString("trEndDate")+"...");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_transNum, AppSettings.resourcemanager.GetString("trNo."));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_depositTo, AppSettings.resourcemanager.GetString("trDepositToHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_recipientV, AppSettings.resourcemanager.GetString("trRecipientHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_recipientC, AppSettings.resourcemanager.GetString("trRecipientHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_recipientU, AppSettings.resourcemanager.GetString("trRecipientHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_recipientSh, AppSettings.resourcemanager.GetString("trRecipientHint"));

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_paymentProcessType, AppSettings.resourcemanager.GetString("trPaymentTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_docNumCheque, AppSettings.resourcemanager.GetString("trDocNumHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_docNumCard, AppSettings.resourcemanager.GetString("trProcessNumHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_cash, AppSettings.resourcemanager.GetString("trCashHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            dg_paymentsAccounts.Columns[0].Header = AppSettings.resourcemanager.GetString("trTransferNumberTooltip");
            dg_paymentsAccounts.Columns[1].Header = AppSettings.resourcemanager.GetString("trRecepient");
            dg_paymentsAccounts.Columns[2].Header = AppSettings.resourcemanager.GetString("trPaymentTypeTooltip");
            dg_paymentsAccounts.Columns[3].Header = AppSettings.resourcemanager.GetString("trDate");
            dg_paymentsAccounts.Columns[4].Header = AppSettings.resourcemanager.GetString("trCashTooltip");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            btn_add.Content = AppSettings.resourcemanager.GetString("trSave");
            txt_image.Text = AppSettings.resourcemanager.GetString("trImage");
            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            txt_print_pay.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_pdf.Text = AppSettings.resourcemanager.GetString("trPdfBtn");

        }

        private void Btn_confirm_Click(object sender, RoutedEventArgs e)
        {//confirm

        }

        private async void dp_SelectedStartDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                await RefreshCashesList();
                await Search();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void dp_SelectedEndDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                await RefreshCashesList();
                await Search();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Dg_paymentsAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//selection
            try
            {

                HelpClass.StartAwait(grid_main);

                if (dg_paymentsAccounts.SelectedIndex != -1)
                {
                    cashtrans = dg_paymentsAccounts.SelectedItem as CashTransfer;
                    this.DataContext = cashtrans;

                    if (cashtrans != null)
                    {
                        cb_paymentProcessType.SelectedIndex = -1;
                        btn_image.IsEnabled = true;

                        cb_depositTo.SelectedValue = cashtrans.side;
                        ///////////////////////////
                        btn_add.IsEnabled = false;
                        cb_depositTo.IsEnabled = false;
                        cb_recipientV.IsEnabled = false;
                        cb_recipientC.IsEnabled = false;
                        cb_recipientU.IsEnabled = false;
                        cb_recipientSh.IsEnabled = false;
                        cb_paymentProcessType.IsEnabled = false;
                        tb_docNumCheque.IsEnabled = false;
                        tb_docNumCard.IsEnabled = false;
                        gd_card.IsEnabled = false;
                        tb_cash.IsEnabled = false;
                        tb_notes.IsEnabled = false;
                        /////////////////////////
                        #region depositor
                        switch (cb_depositTo.SelectedValue.ToString())
                        {
                            case "v":
                                cb_recipientV.SelectedIndex = -1;
                                try
                                { cb_recipientV.SelectedValue = cashtrans.agentId.Value; }
                                catch { }
                                bdr_recipientC.Visibility = Visibility.Collapsed; 
                                bdr_recipientU.Visibility = Visibility.Collapsed; 
                                bdr_recipientSh.Visibility = Visibility.Collapsed;
                                break;
                            case "c":
                                cb_recipientC.SelectedIndex = -1;
                                try
                                { cb_recipientC.SelectedValue = cashtrans.agentId.Value; }
                                catch { }
                                bdr_recipientV.Visibility = Visibility.Collapsed; 
                                bdr_recipientU.Visibility = Visibility.Collapsed; 
                                bdr_recipientSh.Visibility = Visibility.Collapsed;
                                break;
                            case "u":
                            case "s":
                                cb_recipientU.SelectedIndex = -1;
                                try
                                { cb_recipientU.SelectedValue = cashtrans.userId.Value; }
                                catch { }
                                bdr_recipientV.Visibility = Visibility.Collapsed; 
                                bdr_recipientC.Visibility = Visibility.Collapsed; 
                                bdr_recipientSh.Visibility = Visibility.Collapsed;
                                break;
                            case "sh":
                                cb_recipientSh.SelectedIndex = -1;
                                try
                                { cb_recipientSh.SelectedValue = cashtrans.shippingCompanyId.Value; }
                                catch { }
                                bdr_recipientC.Visibility = Visibility.Collapsed; 
                                bdr_recipientV.Visibility = Visibility.Collapsed; 
                                bdr_recipientU.Visibility = Visibility.Collapsed; 
                                break;
                            case "e":
                            case "m":
                                bdr_recipientV.Visibility = Visibility.Collapsed; 
                                bdr_recipientC.Visibility = Visibility.Collapsed; 
                                bdr_recipientU.Visibility = Visibility.Collapsed; 
                                bdr_recipientSh.Visibility = Visibility.Collapsed;
                                break;
                        }
                        #endregion
                        cb_paymentProcessType.SelectedValue = cashtrans.processType;
                   
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                
                HelpClass.StartAwait(grid_main);

                await Search();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        int s = 0;
        int s1 = 0;
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                HelpClass.StartAwait(grid_main);
                
                s = 0; s1 = 0;
                if (FillCombo.groupObject.HasPermissionAction(createPermission, FillCombo.groupObjects, "one") )
                {
                    if (MainWindow.posLogin.boxState == "o") // box is open
                    {
                        #region validate

                        bool enoughMoney = true;
                        if ((!cb_paymentProcessType.Text.Equals("")) && (cb_paymentProcessType.SelectedValue.ToString().Equals("cash")) &&
                            (!await chkEnoughBalance(decimal.Parse(tb_cash.Text))))
                        {
                            enoughMoney = false;

                            HelpClass.SetValidate(p_error_cash, "trPopNotEnoughBalance");
                        }
                        #endregion

                        #region save

                        if ( enoughMoney && HelpClass.validate(requiredControlList, this) )
                        {
                            string recipient = cb_depositTo.SelectedValue.ToString();
                            int agentid = 0;

                            CashTransfer cash = new CashTransfer();

                            cash.transType = "p";
                            cash.posId = MainWindow.posLogin.posId;
                            cash.transNum = await cashModel.generateCashNumber(cash.transType + cb_depositTo.SelectedValue.ToString());
                            cash.cash = decimal.Parse(tb_cash.Text);
                            cash.notes = tb_notes.Text;
                            cash.createUserId = MainWindow.userLogin.userId;
                            cash.side = cb_depositTo.SelectedValue.ToString();
                            cash.processType = cb_paymentProcessType.SelectedValue.ToString();

                            if (bdr_recipientV.IsVisible)
                            { cash.agentId = Convert.ToInt32(cb_recipientV.SelectedValue); agentid = Convert.ToInt32(cb_recipientV.SelectedValue); }

                            if (bdr_recipientC.IsVisible)
                            { cash.agentId = Convert.ToInt32(cb_recipientC.SelectedValue); agentid = Convert.ToInt32(cb_recipientC.SelectedValue); }

                            if (bdr_recipientU.IsVisible)
                                cash.userId = Convert.ToInt32(cb_recipientU.SelectedValue);

                            if (bdr_recipientSh.IsVisible)
                                cash.shippingCompanyId = Convert.ToInt32(cb_recipientSh.SelectedValue);

                            if (cb_paymentProcessType.SelectedValue.ToString().Equals("card"))
                            {
                                cash.cardId = _SelectedCard;
                                cash.docNum = tb_docNumCard.Text;
                            }

                            if (cb_paymentProcessType.SelectedValue.ToString().Equals("doc"))
                                cash.docNum = await cashModel.generateDocNumber("pbnd");

                            if (cb_paymentProcessType.SelectedValue.ToString().Equals("cheque"))
                                cash.docNum = tb_docNumCheque.Text;

                            if (bdr_recipientV.IsVisible || bdr_recipientC.IsVisible)
                            {
                                if (tb_cash.IsReadOnly)
                                    s1 = await cashModel.PayListOfInvoices(cash.agentId.Value, invoicesLst, "pay", cash);
                                else
                                    s1 = await cashModel.PayByAmmount(cash.agentId.Value, decimal.Parse(tb_cash.Text), "pay", cash);
                            }
                            else
                                s = await cashModel.Save(cash);

                            if ((!s.Equals("0")) || (!s1.Equals("")) || (s1.Equals("-1")))
                            {
                                if (cb_paymentProcessType.SelectedValue.ToString().Equals("cash"))
                                    await calcBalance(cash.cash, recipient, agentid);

                                if ((bdr_recipientU.IsVisible) && (cash.side == "u"))
                                    await calcUserBalance(cash.cash, cash.userId.Value);

                                if (bdr_recipientSh.IsVisible)
                                    await calcShippingComBalance(cash.cash, cash.shippingCompanyId.Value);

                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                Clear();
                                await RefreshCashesList();
                                await Search();
                                await MainWindow.refreshBalance();
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {
                            // validate card
                            if (p_error_card.Visibility == Visibility.Visible)
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectCreditCard"), animation: ToasterAnimation.FadeIn);
                            }
                        }
                        #endregion
                    }
                    else //box is closed
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBoxIsClosed"), animation: ToasterAnimation.FadeIn);
                    }
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async Task<bool> chkEnoughBalance(decimal ammount)
        {
            Pos pos = await posModel.getById(MainWindow.posLogin.posId);
            if (pos.balance >= ammount)
            { return true; }
            else { return false; }
        }
       
        
        private async Task calcBalance(decimal ammount, string recipient, int agentid)
        {//balance for pos
            int s = 0;
            //increase pos balance
            Pos pos = await posModel.getById(MainWindow.posLogin.posId);

            pos.balance -= ammount;

            s = await pos.save(pos);
        }

        private async Task calcUserBalance(decimal value, int userId)
        {//balance for user
            User user = await userModel.getUserById(userId);

            if (user.balanceType == 0)
            {
                if (value > user.balance)
                {
                    value -= user.balance;
                    user.balance = value;
                    user.balanceType = 1;
                }
                else
                    user.balance -= value;
            }
            else
            {
                user.balance += value;
            }

            await userModel.save(user);
        }

        private async Task calcShippingComBalance(decimal value, int shippingcompanyId)
        {//balance for shipping company
            ShippingCompanies shCom = await shCompanyModel.GetByID(shippingcompanyId);

            if (shCom.balanceType == 0)
            {
                if (value > shCom.balance)
                {
                    value -= shCom.balance;
                    shCom.balance = value;
                    shCom.balanceType = 1;
                }
                else
                    shCom.balance -= value;
            }
            else
            {
                shCom.balance += value;
            }

            await shCompanyModel.save(shCom);
        }

        

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {//clear
            try
            {
                HelpClass.StartAwait(grid_main);

                Clear();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        async Task<IEnumerable<CashTransfer>> RefreshCashesList()
        {
            cashes = await cashModel.GetCashBond("p", "all");
            cashes = cashes.Where(x => (x.processType != "balance")).GroupBy(x => x.transNum).Select(x => new CashTransfer
            {
                cashTransId = x.FirstOrDefault().cashTransId,
                transType = x.FirstOrDefault().transType,
                posId = x.FirstOrDefault().posId,
                userId = x.FirstOrDefault().userId,
                agentId = x.FirstOrDefault().agentId,
                invId = x.FirstOrDefault().invId,
                transNum = x.FirstOrDefault().transNum,
                createDate = x.FirstOrDefault().createDate,
                updateDate = x.FirstOrDefault().updateDate,
                cash = x.Sum(g => g.cash),
                updateUserId = x.FirstOrDefault().updateUserId,
                createUserId = x.FirstOrDefault().createUserId,
                notes = x.FirstOrDefault().notes,
                posIdCreator = x.FirstOrDefault().posIdCreator,
                isConfirm = x.FirstOrDefault().isConfirm,
                cashTransIdSource = x.FirstOrDefault().cashTransIdSource,
                side = x.FirstOrDefault().side,
                docName = x.FirstOrDefault().docName,
                docNum = x.FirstOrDefault().docNum,
                docImage = x.FirstOrDefault().docImage,
                bankId = x.FirstOrDefault().bankId,
                bankName = x.FirstOrDefault().bankName,
                agentName = x.FirstOrDefault().agentName,
                usersName = x.FirstOrDefault().usersName,// side =u
                posName = x.FirstOrDefault().posName,
                posCreatorName = x.FirstOrDefault().posCreatorName,
                processType = x.FirstOrDefault().processType,
                cardId = x.FirstOrDefault().cardId,
                bondId = x.FirstOrDefault().bondId,
                usersLName = x.FirstOrDefault().usersLName,// side =u
                createUserName = x.FirstOrDefault().createUserName,
                createUserLName = x.FirstOrDefault().createUserLName,
                createUserJob = x.FirstOrDefault().createUserJob,
                cardName = x.FirstOrDefault().cardName,
                bondDeserveDate = x.FirstOrDefault().bondDeserveDate,
                bondIsRecieved = x.FirstOrDefault().bondIsRecieved,
                shippingCompanyId = x.FirstOrDefault().shippingCompanyId,
                shippingCompanyName = x.FirstOrDefault().shippingCompanyName

            });

            return cashes;

        }

        void RefreshCashView()
        {
            dg_paymentsAccounts.ItemsSource = cashesQuery;
            cashes = cashes.Where(x => x.processType != "balance");
            txt_count.Text = cashesQuery.Count().ToString();
        }
        async Task Search()
        {
            try
            {
                if (cashes is null)
                    await RefreshCashesList();

                if (chb_all.IsChecked == false)
                {
                    searchText = tb_search.Text.ToLower();
                    cashesQuery = cashes.Where(s => (s.transNum.ToLower().Contains(searchText)
                    || s.cash.ToString().ToLower().Contains(searchText)
                    )
                    && (s.side == "v" || s.side == "c" || s.side == "u" || s.side == "s" || s.side == "e" || s.side == "m" || s.side == "sh")
                    && s.transType == "p"
                    && s.processType != "inv"
                    && s.updateDate.Value.Date >= dp_searchStartDate.SelectedDate.Value.Date
                    && s.updateDate.Value.Date <= dp_searchEndDate.SelectedDate.Value.Date
                    );
                }
                else
                {
                    searchText = tb_search.Text.ToLower();
                    cashesQuery = cashes.Where(s => (s.transNum.ToLower().Contains(searchText)
                    || s.cash.ToString().ToLower().Contains(searchText)
                    )
                    && (s.side == "v" || s.side == "c" || s.side == "u" || s.side == "s" || s.side == "e" || s.side == "m" || s.side == "sh")
                    && s.transType == "p"
                    && s.processType != "inv"
                    );
                }
                cashesQueryExcel = cashesQuery.ToList();
                RefreshCashView();
            }
            catch { }
        }

      

        private async Task<string> SimLongRunningProcessAsync()
        {
            await Task.Delay(2000);
            return "Success";
        }

        void FN_ExportToExcel()
        {
            var QueryExcel = cashesQueryExcel.AsEnumerable().Select(x => new
            {
                TransNum = x.transNum,
                DepositTo = x.side,
                Recipient = x.agentName,
                OpperationType = x.processType,
                Cash = x.cash
            });
            var DTForExcel = QueryExcel.ToDataTable();
            DTForExcel.Columns[0].Caption = AppSettings.resourcemanager.GetString("trTransferNumberTooltip");
            DTForExcel.Columns[1].Caption = AppSettings.resourcemanager.GetString("trDepositTo");
            DTForExcel.Columns[2].Caption = AppSettings.resourcemanager.GetString("trRecepient");
            DTForExcel.Columns[3].Caption = AppSettings.resourcemanager.GetString("trPaymentTypeTooltip");
            DTForExcel.Columns[4].Caption = AppSettings.resourcemanager.GetString("trCashTooltip");

            ExportToExcel.Export(DTForExcel);

        }

        private void Btn_image_Click(object sender, RoutedEventArgs e)
        {//image
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(createPermission, FillCombo.groupObjects, "one") )
                {
                    if (cashtrans != null || cashtrans.cashTransId != 0)
                    {
                        wd_uploadImage w = new wd_uploadImage();

                        w.tableName = "cashTransfer";
                        w.tableId = cashtrans.cashTransId;
                        w.docNum = cashtrans.docNum;
                    // w.ShowInTaskbar = false;
                        w.ShowDialog();
                    }
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                searchText = "";
                tb_search.Text = "";
                await RefreshCashesList();
                await Search();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_paymentProcessType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//type selection
            try
            {
                HelpClass.StartAwait(grid_main);

                if (requiredControlList.Contains("docNumCheque"))
                    requiredControlList.Remove("docNumCheque");
                if (requiredControlList.Contains("processNum"))
                    requiredControlList.Remove("processNum");
                if (requiredControlList.Contains("card"))
                    requiredControlList.Remove("card");

                HelpClass.clearValidate(requiredControlList, this);
                switch (cb_paymentProcessType.SelectedIndex)
                {
                    case 0://cash
                        bdr_cheque.Visibility = Visibility.Collapsed;
                        tb_docNumCheque.Clear();
                        bdr_card.Visibility = Visibility.Collapsed;
                        _SelectedCard = 0;
                        tb_docNumCard.Clear();
                        txt_card.Text = "";
                        break;

                    case 1://cheque
                        bdr_cheque.Visibility = Visibility.Visible;
                        bdr_card.Visibility = Visibility.Collapsed;
                        _SelectedCard = -1;
                        tb_docNumCard.Clear();
                        txt_card.Text = "";
                        if (!requiredControlList.Contains("docNumCheque"))
                            requiredControlList.Add("docNumCheque");
                        break;

                    case 2://card
                        bdr_cheque.Visibility = Visibility.Collapsed;
                        bdr_card.Visibility = Visibility.Visible;
                        //if (!requiredControlList.Contains("processNum"))
                        //    requiredControlList.Add("processNum");
                        if (!requiredControlList.Contains("card"))
                            requiredControlList.Add("card");
                        try
                        {
                            if (cashtrans.cardId != null)
                            {
                                Button btn = cardBtnList.Where(c => (int)c.Tag == cashtrans.cardId.Value).FirstOrDefault();
                                card_Click(btn, null);
                            }
                        }
                        catch { }
                        break;
                    case -1:
                        bdr_cheque.Visibility = Visibility.Collapsed;
                        bdr_card.Visibility = Visibility.Collapsed;
                        _SelectedCard = 0;
                        break;
                }

                HelpClass.validate(requiredControlList, this);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_depositTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//deposit selection
            try
            {
                HelpClass.StartAwait(grid_main);
                
                switch (cb_depositTo.SelectedIndex)
                {
                    case 0://vendor
                        cb_recipientV.SelectedIndex = -1;
                        bdr_recipientV.Visibility = Visibility.Visible;
                        btn_invoices.Visibility = Visibility.Visible;
                        btn_invoices.IsEnabled = false;
                        bdr_recipientC.Visibility = Visibility.Collapsed;
                        bdr_recipientU.Visibility = Visibility.Collapsed;
                        bdr_recipientSh.Visibility = Visibility.Collapsed;
                        requiredControlList = new List<string> { "cash", "depositTo", "paymentProcessType" , "recipientV" };
                        break;
                    case 1://customer
                        cb_recipientC.SelectedIndex = -1;
                        bdr_recipientV.Visibility = Visibility.Collapsed;
                        btn_invoices.Visibility = Visibility.Visible;
                        btn_invoices.IsEnabled = false;
                        bdr_recipientC.Visibility = Visibility.Visible;
                        bdr_recipientU.Visibility = Visibility.Collapsed;
                        bdr_recipientSh.Visibility = Visibility.Collapsed;
                        requiredControlList = new List<string> { "cash", "depositTo", "paymentProcessType", "recipientC" };
                        break;
                    case 2://user
                        cb_recipientU.SelectedIndex = -1;
                        bdr_recipientV.Visibility = Visibility.Collapsed;
                        btn_invoices.Visibility = Visibility.Collapsed;
                        btn_invoices.IsEnabled = false;
                        bdr_recipientC.Visibility = Visibility.Collapsed;
                        bdr_recipientU.Visibility = Visibility.Visible;
                        bdr_recipientSh.Visibility = Visibility.Collapsed;
                        requiredControlList = new List<string> { "cash", "depositTo", "paymentProcessType", "recipientU" };
                        break;
                    case 3://salary
                        cb_recipientU.SelectedIndex = -1;
                        bdr_recipientV.Visibility = Visibility.Collapsed;
                        btn_invoices.Visibility = Visibility.Collapsed;
                        btn_invoices.IsEnabled = false;
                        bdr_recipientC.Visibility = Visibility.Collapsed;
                        bdr_recipientU.Visibility = Visibility.Visible;
                        bdr_recipientSh.Visibility = Visibility.Collapsed;
                        requiredControlList = new List<string> { "cash", "depositTo", "paymentProcessType", "recipientU" };
                        break;
                    case 4: //general expenses
                    case 5://administrative pull
                        bdr_recipientV.Visibility = Visibility.Collapsed;
                        btn_invoices.Visibility = Visibility.Collapsed;
                        btn_invoices.IsEnabled = false;
                        bdr_recipientC.Visibility = Visibility.Collapsed;
                        bdr_recipientC.Visibility = Visibility.Collapsed;
                        bdr_recipientSh.Visibility = Visibility.Collapsed;
                        cb_recipientV.Text = ""; cb_recipientC.Text = ""; cb_recipientU.Text = ""; cb_recipientSh.Text = "";
                        requiredControlList = new List<string> { "cash", "depositTo", "paymentProcessType"};
                        break;
                    case 6://shipping company
                        cb_recipientSh.SelectedIndex = -1;
                        bdr_recipientV.Visibility = Visibility.Collapsed;
                        btn_invoices.Visibility = Visibility.Collapsed;
                        btn_invoices.IsEnabled = false;
                        bdr_recipientC.Visibility = Visibility.Collapsed;
                        bdr_recipientU.Visibility = Visibility.Collapsed;
                        bdr_recipientSh.Visibility = Visibility.Visible;
                        requiredControlList = new List<string> { "cash", "depositTo", "paymentProcessType", "recipientSh" };
                        break;
                }
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async Task fillVendors()
        {
            try
            {
                agents = await agentModel.GetActiveForAccount("v", "p");

                cb_recipientV.ItemsSource = agents;
                cb_recipientV.DisplayMemberPath = "name";
                cb_recipientV.SelectedValuePath = "agentId";
            }
            catch { }
        }

        private async Task fillCustomers()
        {
            try
            {
                agents = await agentModel.GetActiveForAccount("c", "p");

                cb_recipientC.ItemsSource = agents;
                cb_recipientC.DisplayMemberPath = "name";
                cb_recipientC.SelectedValuePath = "agentId";
            }
            catch { }
        }

        private async Task fillUsers()
        {
            try
            {
                users = await userModel.GetActiveForAccount("p");

                cb_recipientU.ItemsSource = users;
                cb_recipientU.DisplayMemberPath = "username";
                cb_recipientU.SelectedValuePath = "userId";
            }
            catch { }
        }

        private async Task fillShippingCompanies()
        {
            try
            {//
                shCompanies = await shCompanyModel.GetForAccount("p");
                shCompanies = shCompanies.Where(sh => sh.deliveryType != "local");
                cb_recipientSh.ItemsSource = shCompanies;
                cb_recipientSh.DisplayMemberPath = "name";
                cb_recipientSh.SelectedValuePath = "shippingCompanyId";
            }
            catch { }
        }

        private void Tb_validateEmptyTextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                validateEmpty(name, sender);
                var txb = sender as TextBox;
                if ((sender as TextBox).Name == "tb_cash")
                    HelpClass.InputJustNumber(ref txb);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                validateEmpty(name, sender);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void validateEmpty(string name, object sender)
        {
        }

        private void PreventSpaces(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void Tb_cash_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only decimal
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void Tb_docNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only int
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    
        private void Btn_invoices_Click(object sender, RoutedEventArgs e)
        {//invoices
            try
            {
                invoicesLst.Clear();
                Window.GetWindow(this).Opacity = 0.2;
                wd_invoicesList w = new wd_invoicesList();

                if (cb_depositTo.SelectedValue.ToString() == "v")
                    w.agentId = Convert.ToInt32(cb_recipientV.SelectedValue);
                else if (cb_depositTo.SelectedValue.ToString() == "c")
                    w.agentId = Convert.ToInt32(cb_recipientC.SelectedValue);

                w.invType = "pay";

                    // w.ShowInTaskbar = false;
                w.ShowDialog();
                if (w.isActive)
                {
                    tb_cash.Text = HelpClass.DecTostring(w.sum);
                    tb_cash.IsReadOnly = true;
                    invoicesLst.AddRange(w.selectedInvoices);
                }
                Window.GetWindow(this).Opacity = 1;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_salaries_Click(object sender, RoutedEventArgs e)
        {//salaries

        }

        private void Cb_recipientV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((cb_recipientV.SelectedIndex != -1) && (cb_recipientV.IsEnabled))
                    btn_invoices.IsEnabled = true;
                else
                    btn_invoices.IsEnabled = false;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Tb_EnglishDigit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only english and digits
            Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
        }

        private void Cb_recipientC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((cb_recipientC.SelectedIndex != -1) && (cb_recipientC.IsEnabled))
                    btn_invoices.IsEnabled = true;
                else
                    btn_invoices.IsEnabled = false;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

      


        private async void Chb_all_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                dp_searchStartDate.IsEnabled =
            dp_searchEndDate.IsEnabled = false;

                Btn_refresh_Click(btn_refresh, null);


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Chb_all_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                dp_searchStartDate.IsEnabled =
                dp_searchEndDate.IsEnabled = true;

                Btn_refresh_Click(btn_refresh, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            cashtrans = new CashTransfer();

            foreach (var el in cardEllipseList)
            {
                el.Stroke = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            }

            tb_transNum.Text = "";
            this.DataContext = cashtrans;
            ///////////////////////////////
            btn_add.IsEnabled = true;
            btn_invoices.Visibility = Visibility.Collapsed;
            btn_invoices.IsEnabled = false;
            tb_cash.IsReadOnly = false;
            cb_depositTo.SelectedIndex = -1;
            cb_paymentProcessType.SelectedIndex = -1;
            _SelectedCard = -1;
            bdr_card.Visibility = Visibility.Collapsed;
            bdr_recipientV.Visibility = Visibility.Collapsed;
            bdr_recipientC.Visibility = Visibility.Collapsed;
            bdr_recipientU.Visibility = Visibility.Collapsed;
            bdr_recipientSh.Visibility = Visibility.Collapsed;
            bdr_cheque.Visibility = Visibility.Collapsed;
            ///////////////////////////
            btn_add.IsEnabled = true;
            cb_depositTo.IsEnabled = true;
            cb_recipientV.IsEnabled = true;
            cb_recipientC.IsEnabled = true;
            cb_recipientU.IsEnabled = true;
            cb_recipientSh.IsEnabled = true;
            cb_paymentProcessType.IsEnabled = true;
            tb_docNumCheque.IsEnabled = true;
            tb_docNumCard.IsEnabled = true;
            //dp_docDateCheque.IsEnabled = true;
            gd_card.IsEnabled = true;
            tb_cash.IsEnabled = true;
            tb_notes.IsEnabled = true;
            /////////////////////////

            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only  digits
            try
            {
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                if (textBox.Tag.ToString() == "int")
                {
                    Regex regex = new Regex("[^0-9]");
                    e.Handled = regex.IsMatch(e.Text);
                }
                else if (textBox.Tag.ToString() == "decimal")
                {
                    input = e.Text;
                    e.Handled = !decimal.TryParse(textBox.Text + input, out _decimal);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only english and digits
            try
            {
                Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = e.Key == Key.Space;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        #region Doc report


        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        { //doc
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(createPermission, FillCombo.groupObjects, "one"))
                {
                    if (cashtrans.cashTransId > 0)
                    {
                        BuildVoucherReport();

                        saveFileDialog.Filter = "PDF|*.pdf;";

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string filepath = saveFileDialog.FileName;
                            try { LocalReportExtensions.ExportToPDF(rep, filepath); }
                            catch { }
                        }
                    }
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_print_pay_Click(object sender, RoutedEventArgs e)
        {//doc
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(createPermission, FillCombo.groupObjects, "one"))
                {
                    if (cashtrans.cashTransId > 0)
                    {
                        BuildVoucherReport();

                        LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
                    }
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        public void BuildVoucherReport()
        {
            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                if (AppSettings.docPapersize == "A4")
                {
                    addpath = @"\Reports\Account\Doc\Ar\ArPayReportA4.rdlc";
                }
                else //A5
                {
                    addpath = @"\Reports\Account\Doc\Ar\ArPayReport.rdlc";
                }

            }
            else
            {
                if (AppSettings.docPapersize == "A4")
                {
                    addpath = @"\Reports\Account\Doc\En\PayReportA4.rdlc";
                }
                else //A5
                {
                    addpath = @"\Reports\Account\Doc\En\PayReport.rdlc";
                }

            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            rep.ReportPath = reppath;
            rep.DataSources.Clear();
            rep.EnableExternalImages = true;
            rep.SetParameters(reportclass.fillPayReport(cashtrans));

            rep.Refresh();
        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//doc
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(createPermission, FillCombo.groupObjects, "one"))
                {

                    Window.GetWindow(this).Opacity = 0.2;

                    string pdfpath;
                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                    if (cashtrans.cashTransId > 0)
                    {

                        BuildVoucherReport();
                        LocalReportExtensions.ExportToPDF(rep, pdfpath);
                        wd_previewPdf w = new wd_previewPdf();
                        w.pdfPath = pdfpath;
                        if (!string.IsNullOrEmpty(w.pdfPath))
                        {
                            // w.ShowInTaskbar = false;
                            w.ShowDialog();

                            w.wb_pdfWebViewer.Dispose();
                        }
                        else
                            Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                        Window.GetWindow(this).Opacity = 1;
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        #region  reports
        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Account\report\Ar\ArPayAcc.rdlc";
            }
            else
            {
                addpath = @"\Reports\Account\report\En\EnPayAcc.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //cashesQueryExcel = cashesQuery.ToList();
            clsReports.paymentAccReport(cashesQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);
            rep.Refresh();
        }
       

      
 
       
        private void Btn_pdf1_Click_1(object sender, RoutedEventArgs e)
        {
            //pdf
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    #region
                    BuildReport();

                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_preview1_Click_1(object sender, RoutedEventArgs e)
        {
            //preview
            try
            {

                HelpClass.StartAwait(grid_main);
                /////////////////////
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    string pdfpath = "";


                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                    BuildReport();
                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                    wd_previewPdf w = new wd_previewPdf();
                    w.pdfPath = pdfpath;
                    if (!string.IsNullOrEmpty(w.pdfPath))
                    {
                        // w.ShowInTaskbar = false;
                        w.ShowDialog();
                        w.wb_pdfWebViewer.Dispose();
                    }
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                /////////////////////

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_print_Click_1(object sender, RoutedEventArgs e)
        {
            //print
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    #region
                    BuildReport();

                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_exportToExcel_Click_1(object sender, RoutedEventArgs e)
        {
            //excel
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    #region
                    //Thread t1 = new Thread(() =>
                    //{
                    #region
                    BuildReport();
                    this.Dispatcher.Invoke(() =>
                    {
                        saveFileDialog.Filter = "EXCEL|*.xls;";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string filepath = saveFileDialog.FileName;
                            LocalReportExtensions.ExportToExcel(rep, filepath);
                        }


                    });
                    #endregion
                    //});
                    //t1.Start();
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);
                /////////////////////
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") )
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    //cashesQueryExcel = cashesQuery.ToList();
                    win_IvcAccount win = new win_IvcAccount(cashes, 1);
                    // // w.ShowInTaskbar = false;
                    win.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                /////////////////////

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        #endregion
    }
}
