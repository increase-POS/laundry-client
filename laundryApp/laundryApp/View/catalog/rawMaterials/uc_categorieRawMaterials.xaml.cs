using netoaster;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;
using System.IO;
using laundryApp.View.windows;

using Microsoft.Reporting.WinForms;
namespace laundryApp.View.catalog.rawMaterials
{
    /// <summary>
    /// Interaction logic for uc_categorieRawMaterials.xaml
    /// </summary>
    public partial class uc_categorieRawMaterials : UserControl
    {
        public uc_categorieRawMaterials()
        {
            try
            {
                InitializeComponent();
                if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1440)
                {
                    txt_deleteButton.Visibility = Visibility.Visible;
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_updateButton.Visibility = Visibility.Visible;
                    txt_add_Icon.Visibility = Visibility.Visible;
                    txt_update_Icon.Visibility = Visibility.Visible;
                    txt_delete_Icon.Visibility = Visibility.Visible;
                }
                else if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1360)
                {
                    txt_add_Icon.Visibility = Visibility.Collapsed;
                    txt_update_Icon.Visibility = Visibility.Collapsed;
                    txt_delete_Icon.Visibility = Visibility.Collapsed;
                    txt_deleteButton.Visibility = Visibility.Visible;
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_updateButton.Visibility = Visibility.Visible;

                }
                else
                {
                    txt_deleteButton.Visibility = Visibility.Collapsed;
                    txt_addButton.Visibility = Visibility.Collapsed;
                    txt_updateButton.Visibility = Visibility.Collapsed;
                    txt_add_Icon.Visibility = Visibility.Visible;
                    txt_update_Icon.Visibility = Visibility.Visible;
                    txt_delete_Icon.Visibility = Visibility.Visible;

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private static uc_categorieRawMaterials _instance;
        public static uc_categorieRawMaterials Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                    _instance = new uc_categorieRawMaterials();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        string basicsPermission = "categorie_basics";
        byte tgl_categorieState;
        string searchText = "";
        public static List<string> requiredControlList;
        //List<Unit> units;
        Unit unit = new Unit();
        #region for barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        #endregion
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.KeyDown -= HandleKeyPress;
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                MainWindow.mainWindow.KeyDown += HandleKeyPress;
                // for pagination onTop Always
                btns = new Button[] { btn_firstPage, btn_prevPage, btn_activePage, btn_nextPage, btn_lastPage };
                catigoriesAndItemsView.uccategorieRawMaterials = this;

                requiredControlList = new List<string> { "code", "name" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                Keyboard.Focus(tb_categoryCode);
                await RefreshCategorysList();
                await Search();
                Clear();
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

            //txt_title.Text = AppSettings.resourcemanager.GetString("trCategorie");
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_contentInformatin.Text = AppSettings.resourcemanager.GetString("trMoreInformation");
            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_details, AppSettings.resourcemanager.GetString("trDetailsHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_categoryCode, AppSettings.resourcemanager.GetString("trCode") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_taxes, AppSettings.resourcemanager.GetString("trTaxesHint") );
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint") );
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNote") + "...");

            btn_add.Content = AppSettings.resourcemanager.GetString("trAdd");
            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");
            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    categorie = new Category();
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        Boolean codeAvailable = checkCodeAvailabiltiy();
                        if (codeAvailable)
                        {                                                
                            decimal taxes = 0;                        
                            if (tb_taxes.Text != "")
                                taxes = decimal.Parse(tb_taxes.Text);                        

                            categorie = new Category();
                            categorie.categoryCode = tb_categoryCode.Text;
                            categorie.name = tb_name.Text;
                            categorie.details = tb_details.Text;
                            categorie.notes = tb_notes.Text;
                            categorie.image = "";
                            categorie.taxes = taxes;
                            categorie.type = "p";
                            categorie.isActive = 1;
                            categorie.createUserId = MainWindow.userLogin.userId;
                            categorie.updateUserId = MainWindow.userLogin.userId;
                            int res = await categorie.save(categorie);
                            if (res == -1)// إظهار رسالة الترقية
                                Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpgrade"), animation: ToasterAnimation.FadeIn);

                            else if (res == 0) // an error occure
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                categorie.categoryId = res;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                int categorieId = res;

                                if (openFileDialog.FileName != "")
                                    await categorie.uploadImage(openFileDialog.FileName, Md5Encription.MD5Hash("Inc-m" + categorieId.ToString()), categorieId);
                                Clear();
                                await RefreshCategorysList();
                                await Search();
                            }
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                {
                    HelpClass.StartAwait(grid_main);
                    if (categorie.categoryId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this))
                    {
                        Boolean codeAvailable = checkCodeAvailabiltiy(categorie.categoryCode);
                        if (codeAvailable)
                        {

                            decimal taxes = 0;
                            if (tb_taxes.Text != "")
                                taxes = decimal.Parse(tb_taxes.Text);

                       
                            categorie.categoryCode = tb_categoryCode.Text;
                            categorie.name = tb_name.Text;
                            categorie.details = tb_details.Text;
                            categorie.notes = tb_notes.Text;
                            categorie.taxes = taxes;
                            categorie.updateUserId = MainWindow.userLogin.userId;

                            int res = await categorie.save(categorie);
                            if (res == -1)// إظهار رسالة الترقية
                                Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpgrade"), animation: ToasterAnimation.FadeIn);

                            else if (res == 0) // an error occure
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                categorie.categoryId = res;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                if (openFileDialog.FileName != "")
                                    await categorie.uploadImage(openFileDialog.FileName, Md5Encription.MD5Hash("Inc-m" + categorie.categoryId.ToString()), categorie.categoryId);

                                Clear();
                                await RefreshCategorysList();
                                await Search();
                            }
                        }
                    }

                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {//delete
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (categorie.categoryId != 0)
                    {
                        if ((!categorie.canDelete) && (categorie.isActive == 0))
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxActivate");
                    // w.ShowInTaskbar = false;
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                await activate();
                                Clear();
                            }
                        }
                        else
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            if (categorie.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!categorie.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                    // w.ShowInTaskbar = false;
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (categorie.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!categorie.canDelete) && (categorie.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await categorie.delete(categorie.categoryId, MainWindow.userLogin.userId, categorie.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    categorie.categoryId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshCategorysList();
                                    await Search();
                                    Clear();
                                }
                            }
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async Task activate()
        {//activate
            categorie.isActive = 1;
            int s = await categorie.save(categorie);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshCategorysList();
                await Search();
            }
        }
        #endregion
        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
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
        private async void Tgl_isActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (categories is null)
                    await RefreshCategorysList();
                tgl_categorieState = 1;
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Tgl_isActive_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (categories is null)
                    await RefreshCategorysList();
                tgl_categorieState = 0;
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
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
        private async void HandleKeyPress(object sender, KeyEventArgs e)
        {
            /*
            try
            {
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 150)
                {
                    _BarcodeStr = "";
                }

                string digit = "";
                // record keystroke & timestamp 
                if (e.Key >= Key.D0 && e.Key <= Key.D9)
                {
                    //digit pressed!
                    digit = e.Key.ToString().Substring(1);
                    // = "1" when D1 is pressed
                }
                else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    digit = e.Key.ToString().Substring(6); // = "1" when NumPad1 is pressed

                }
                _BarcodeStr += digit;
                _lastKeystroke = DateTime.Now;

                // process barcode 
                if (e.Key.ToString() == "Return" && _BarcodeStr != "")
                {
                    // get categorie matches barcode
                    if (FillCombo.categorieUnitList != null)
                    {
                        var ob = FillCombo.categorieUnitList.ToList().Find(c => c.barcode == _BarcodeStr && FillCombo.purchaseTypes.Contains(c.type));
                        if (ob != null)
                        {
                            int categorieId = (int)ob.categorieId;
                            ChangeCategoryIdEvent(categorieId);
                        }
                        else
                        {
                            Clear();
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorCategoryNotFoundToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    _BarcodeStr = "";
                }
            }
            catch (Exception ex)
            {
                _BarcodeStr = "";
                HelpClass.ExceptionMessage(ex, this);
            }
            */
        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await RefreshCategorysList();
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region Refresh & Search

      
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            categorie = new Category();
            categorie.taxes = 0;
            this.DataContext = categorie;
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            #region image
            openFileDialog.FileName = "";
            HelpClass.clearImg(btn_image);
            #endregion
            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {


                //only  digits
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
        {
            try
            {
                //only english and digits
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
        private void ValidateEmpty_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        #region Image
        string imgFileName = "pic/no-image-icon-125x125.png";
        bool isImgPressed = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void Btn_image_Click(object sender, RoutedEventArgs e)
        {
            //select image
            try
            {
                HelpClass.StartAwait(grid_main);
                isImgPressed = true;
                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    HelpClass.imageBrush = new ImageBrush();
                    HelpClass.imageBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    btn_image.Background = HelpClass.imageBrush;
                    imgFileName = openFileDialog.FileName;
                }
                else
                { }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async Task getImg()
        {
            if (string.IsNullOrEmpty(categorie.image))
            {
                HelpClass.clearImg(btn_image);
            }
            else
            {
                byte[] imageBuffer = await categorie.downloadImage(categorie.image); // read this as BLOB from your DB

                var bitmapImage = new BitmapImage();
                if (imageBuffer != null)
                {
                    using (var memoryStream = new MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }

                    btn_image.Background = new ImageBrush(bitmapImage);
                    // configure trmporary path
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = System.IO.Path.Combine(dir, Global.TMPFolder, categorie.image);
                    openFileDialog.FileName = tmpPath;
                }
                else
                    HelpClass.clearImg(btn_image);
            }
        }
        #endregion

        #region report
       
        //report  parameters
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
     //   SaveFileDialog saveFileDialog = new SaveFileDialog();

        // end report parameters
        public void BuildReport()
        {
         
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Catalog\Ar\ArCategory.rdlc";
            }
            else
            {
                addpath = @"\Reports\Catalog\En\EnCategory.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            clsReports.categoryReport(categoriesQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {

            //pdf
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
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

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;

                    string pdfpath = "";
                    //
                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                    BuildReport();

                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                    wd_previewPdf w = new wd_previewPdf();
                    w.pdfPath = pdfpath;
                    if (!string.IsNullOrEmpty(w.pdfPath))
                    {
                        w.ShowDialog();
                        w.wb_pdfWebViewer.Dispose();


                    }
                    Window.GetWindow(this).Opacity = 1;
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {

                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
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

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    //Thread t1 = new Thread(() =>
                    //{
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

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    //Window.GetWindow(this).Opacity = 0.2;
                    //win_lvc win = new win_lvc(banksQuery, 5);
                    //win.ShowDialog();
                    //Window.GetWindow(this).Opacity = 1;
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



        #endregion

        #region  Cards
        CatigoriesAndItemsView catigoriesAndItemsView = new CatigoriesAndItemsView();
        #region Refrish Y
        Category categorie = new Category();
        IEnumerable<Category> categories;
        IEnumerable<Category> categoriesQuery;
        async Task<IEnumerable<Category>> RefreshCategorysList()
        {
            categories = await categorie.Get("p");
            return categories;
        }
        void RefrishCategorysCard(IEnumerable<Category> _categories)
        {
            grid_itemContainerCard.Children.Clear();
            catigoriesAndItemsView.gridCatigories = grid_itemContainerCard;
            catigoriesAndItemsView.FN_refrishCatalogItem_category(_categories.ToList(), "Category",5);
        }
        #endregion
        #region Get Id By Click  Y

        public async void ChangeCategoryIdEvent(int categorieId)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                categorie = categories.Where(x => x.categoryId == categorieId).FirstOrDefault();
                this.DataContext = categorie;
                await getImg();
                 #region delete
                if (categorie.canDelete)
                    btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                else
                {
                    if (categorie.isActive == 0)
                        btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                    else
                        btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                }
                #endregion
                HelpClass.clearValidate(requiredControlList, this);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region Search Y


        async Task Search()
        {
            //search
            try
            {
                HelpClass.StartAwait(grid_main);

                if (categories is null)
                    await RefreshCategorysList();
                searchText = tb_search.Text;
                categoriesQuery = categories.
                Where(x => (x.categoryCode.ToLower().Contains(searchText) ||
                    x.name.ToLower().Contains(searchText) ||
                    x.details.ToLower().Contains(searchText)
                    // || x.categoryName.ToLower().Contains(searchText)
                    ) && x.isActive == tgl_categorieState);
                pageIndex = 1;
                #region


                if (btns is null)
                    btns = new Button[] { btn_firstPage, btn_prevPage, btn_activePage, btn_nextPage, btn_lastPage };
                RefrishCategorysCard(pagination.refrishPagination(categoriesQuery, pageIndex, btns, 15));
                #endregion

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region Pagination Y
        Pagination pagination = new Pagination();
        Button[] btns;
        public int pageIndex = 1;

        private void Tb_pageNumberSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                categoriesQuery = categories.ToList();

                if (tb_pageNumberSearch.Text.Equals(""))
                {
                    pageIndex = 1;
                }
                else if (((categoriesQuery.Count() - 1) / 9) + 1 < int.Parse(tb_pageNumberSearch.Text))
                {
                    pageIndex = ((categoriesQuery.Count() - 1) / 9) + 1;
                }
                else
                {
                    pageIndex = int.Parse(tb_pageNumberSearch.Text);
                }

                #region

                RefrishCategorysCard(pagination.refrishPagination(categoriesQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_firstPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = 1;
                #region
                categoriesQuery = categories.ToList();
                RefrishCategorysCard(pagination.refrishPagination(categoriesQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_prevPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_prevPage.Content.ToString());
                #region
                categoriesQuery = categories.ToList();

                RefrishCategorysCard(pagination.refrishPagination(categoriesQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_activePage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_activePage.Content.ToString());
                #region
                categoriesQuery = categories.ToList();
                RefrishCategorysCard(pagination.refrishPagination(categoriesQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_nextPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_nextPage.Content.ToString());
                #region
                categoriesQuery = categories.ToList();
                RefrishCategorysCard(pagination.refrishPagination(categoriesQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_lastPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                categoriesQuery = categories.ToList();
                pageIndex = ((categoriesQuery.Count() - 1) / 9) + 1;
                #region
                categoriesQuery = categories.ToList();
                RefrishCategorysCard(pagination.refrishPagination(categoriesQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #endregion
        private Boolean checkCodeAvailabiltiy(string oldCode = "")
        {
            string code = tb_categoryCode.Text;

            if (code != oldCode && categories.ToList().Count > 0)
            {
                var match = categories.Where(x => x.categoryCode.Contains(code)).FirstOrDefault();

                if (match != null)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trDuplicateCodeToolTip"), animation: ToasterAnimation.FadeIn);
                    #region Tooltip_code
                    p_error_categoryCode.Visibility = Visibility.Visible;
                    ToolTip toolTip_code = new ToolTip();
                    toolTip_code.Content = AppSettings.resourcemanager.GetString("trDuplicateCodeToolTip");
                    toolTip_code.Style = Application.Current.Resources["ToolTipError"] as Style;
                    p_error_categoryCode.ToolTip = toolTip_code;
                    #endregion
                    return false;
                }
                else
                {
                    HelpClass.clearValidate(p_error_categoryCode);
                    return true;
                }
            }
            else
            {
                HelpClass.clearValidate(p_error_categoryCode);
                return true;
            }
        }

    
    }
}
