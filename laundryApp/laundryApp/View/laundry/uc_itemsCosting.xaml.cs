using netoaster;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
//using System.IO;
using laundryApp.View.windows;
using Microsoft.Reporting.WinForms;
using laundryApp.View.sales;

namespace laundryApp.View.kitchen
{
    /// <summary>
    /// Interaction logic for uc_itemsCosting.xaml
    /// </summary>
    public partial class uc_itemsCosting : UserControl
    {
        private static uc_itemsCosting _instance;
        public static uc_itemsCosting Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_itemsCosting();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        string updatePermission = "itemsCosting_update";
        List<Item> itemsQuery = new List<Item>();
        string searchText = "";
        int categoryId = 0;
        public uc_itemsCosting()
        {
            InitializeComponent();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                catalogMenuList = new List<string> { "allMenu", "appetizers", "beverages", "fastFood", "mainCourses", "desserts" };
                categoryBtns = new List<Button> { btn_appetizers, btn_beverages, btn_fastFood, btn_mainCourses, btn_desserts };
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_RefrishItems", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefrishCategories", value = false });

                loading_RefrishItems();
                loading_RefrishCategories();
                do
                {
                    isDone = true;
                    foreach (var item in loadingList)
                    {
                        if (item.value == false)
                        {
                            isDone = false;
                            break;
                        }
                    }
                    if (!isDone)
                    {
                        await Task.Delay(0500);
                    }
                }
                while (!isDone);
                #endregion
                //enable categories buttons
                if (FillCombo.salesItems is null)
                    await FillCombo.RefreshSalesItems();
                HelpClass.activateCategoriesButtons(FillCombo.salesItems, FillCombo.categoriesList, categoryBtns);
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

            //txt_title.Text = AppSettings.resourcemanager.GetString("trItemsCosting");
            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            btn_refresh.ToolTip = AppSettings.resourcemanager.GetString("trRefresh");
            btn_pdf.ToolTip = AppSettings.resourcemanager.GetString("trPdf");
            btn_print.ToolTip = AppSettings.resourcemanager.GetString("trPrint");
            btn_pieChart.ToolTip = AppSettings.resourcemanager.GetString("trPieChart");
            btn_exportToExcel.ToolTip = AppSettings.resourcemanager.GetString("trExcel");
            btn_preview.ToolTip = AppSettings.resourcemanager.GetString("trPreview");
            txt_count.ToolTip = AppSettings.resourcemanager.GetString("trCount");

            dg_items.Columns[0].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_items.Columns[1].Header = AppSettings.resourcemanager.GetString("trPrimeCost");
            dg_items.Columns[2].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_items.Columns[3].Header = AppSettings.resourcemanager.GetString("trPriceWithService");

            txt_details.Text = AppSettings.resourcemanager.GetString("trDetails");
            txt_code.Text = AppSettings.resourcemanager.GetString("trCode");
            txt_name.Text = AppSettings.resourcemanager.GetString("trName");
            txt_purchasePrice.Text = AppSettings.resourcemanager.GetString("trPrimeCost");
            txt_price.Text = AppSettings.resourcemanager.GetString("trPrice");
            txt_priceWithService.Text = AppSettings.resourcemanager.GetString("trPriceWithService");
            txt_allMenu.Text = AppSettings.resourcemanager.GetString("trAll");
            txt_appetizers.Text = AppSettings.resourcemanager.GetString("trAppetizers");
            txt_beverages.Text = AppSettings.resourcemanager.GetString("trBeverages");
            txt_fastFood.Text = AppSettings.resourcemanager.GetString("trFastFood");
            txt_mainCourses.Text = AppSettings.resourcemanager.GetString("trMainCourses");
            txt_desserts.Text = AppSettings.resourcemanager.GetString("trDesserts");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_searchTags, AppSettings.resourcemanager.GetString("trTagsHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
        }
        #region loading
        List<keyValueBool> loadingList;
        async void loading_RefrishItems()
        {
            try
            {
                if (FillCombo.salesItems == null)
                    await FillCombo.RefreshSalesItems();
               
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefrishItems"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_RefrishCategories()
        {
            try
            {
                if (FillCombo.categoriesList == null)
                    await FillCombo.RefreshCategory();

            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefrishCategories"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        #endregion
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        #region catalogMenu
        public static List<string> catalogMenuList;
        public static List<Button> categoryBtns;
        private async void catalogMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                string senderTag = (sender as Button).Tag.ToString();
                if (senderTag != "allMenu")
                    categoryId = FillCombo.categoriesList.Where(x => x.categoryCode == senderTag).FirstOrDefault().categoryId;
                else
                    categoryId = -1;
                await refreshCategoryTag();


                #region refresh colors
                foreach (var control in catalogMenuList)
                {
                    Border border = FindControls.FindVisualChildren<Border>(this).Where(x => x.Tag != null && x.Name == "bdr_" + control)
                        .FirstOrDefault();
                    if (border.Tag.ToString() == senderTag)
                        border.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    else
                        border.Background = Application.Current.Resources["White"] as SolidColorBrush;
                }
                foreach (var control in catalogMenuList)
                {

                    Path path = FindControls.FindVisualChildren<Path>(this).Where(x => x.Tag != null && x.Name == "path_" + control)
                        .FirstOrDefault();
                    if (path.Tag.ToString() == senderTag)
                        path.Fill = Application.Current.Resources["White"] as SolidColorBrush;
                    else
                        path.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
                }
                foreach (var control in catalogMenuList)
                {
                    TextBlock textBlock = FindControls.FindVisualChildren<TextBlock>(this).Where(x => x.Tag != null && x.Name == "txt_" + control)
                        .FirstOrDefault();
                    if (textBlock.Tag.ToString() == senderTag)
                        textBlock.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    else
                        textBlock.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                }
                #endregion
               await Search();
            }
            catch { }
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
        private async void Cb_searchTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
      
        private async void Dg_items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (dg_items.SelectedIndex != -1)
                {
                    FillCombo.item = dg_items.SelectedItem as Item;
                    this.DataContext = FillCombo.item;
                }

                //HelpClass.clearValidate(requiredControlList, this);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);

                await RefreshItemsList();
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(updatePermission, FillCombo.groupObjects, "one"))
                {
                    //save
                    int res = await FillCombo.item.saveItemsCosting(itemsQuery);
                    if (res > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                        await FillCombo.RefreshSalesItems();
                    }
                    else
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

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

        #region Refresh & Search & clear

        async Task Search()
        {
            //search
            try
            {
                searchText = tb_search.Text.ToLower();

                itemsQuery = FillCombo.salesItems.Where(s => s.name.ToLower().Contains(searchText)
                                                        || s.code.ToLower().Contains(searchText)
                                                        || s.details.ToLower().Contains(searchText))
                    .Select(x => new Item()
                    {
                        itemId = x.itemId,
                        name = x.name,
                        code = x.code,
                        itemUnitId = x.itemUnitId,
                        avgPurchasePrice = x.avgPurchasePrice,
                        price = x.price,
                        priceWithService = x.priceWithService,
                        isActive = x.isActive,
                        categoryId = x.categoryId,
                        tagId = x.tagId,
                        createDate = x.createDate
                    }).ToList();

                if (categoryId > 0)
                    itemsQuery = itemsQuery.Where(x => x.categoryId == categoryId).ToList();

                if (cb_searchTags.SelectedIndex > 0)
                    itemsQuery = itemsQuery.Where(x => x.tagId == (int)cb_searchTags.SelectedValue).ToList();

                RefreshItemsView();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        async Task RefreshItemsList()
        {
            await FillCombo.RefreshSalesItems();
            HelpClass.activateCategoriesButtons(FillCombo.salesItems, FillCombo.categoriesList, categoryBtns);
        }
        void RefreshItemsView()
        {
            dg_items.ItemsSource = itemsQuery;
            txt_count.Text = itemsQuery.Count().ToString();
        }

        private void Clear()
        {
            try
            {
                dg_items.SelectedIndex = -1;
                FillCombo.item = new Item();
                this.DataContext = FillCombo.item;
            }
            catch { }
        }
        private async Task refreshCategoryTag()
        {
            await FillCombo.fillTagsWithDefault(cb_searchTags,categoryId);
            //cb_searchTags.SelectedIndex = 0;
        }

        #endregion

        #region report

        //report  parameters
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        // end report parameters
        public void BuildReport()
        {

            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Kitchen\Ar\ArItemsCosting.rdlc";
            }
            else
            {
                addpath = @"\Reports\Kitchen\En\EnItemsCosting.rdlc";
            }
            string reppath = reportclass.PathUp(System.IO.Directory.GetCurrentDirectory(), 2, addpath);
            clsReports.itemCosting(itemsQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {

                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
                    #region
                    BuildReport();

                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
                    #endregion
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

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
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
                    #region
                    Window.GetWindow(this).Opacity = 0.2;

                    string pdfpath = "";
                    //
                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(System.IO.Directory.GetCurrentDirectory(), 2, pdfpath);

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
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


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
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{

                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
                    #endregion
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


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

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
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
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


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

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
                #region
                Window.GetWindow(this).Opacity = 0.2;
                win_lvcSales win = new win_lvcSales(itemsQuery, 3);
                win.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                #endregion
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Tt_pieChart_Closed(object sender, RoutedEventArgs e)
        {

        }


        #endregion

     
    }
}
