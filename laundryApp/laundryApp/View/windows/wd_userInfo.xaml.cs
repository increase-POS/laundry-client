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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_userInfo.xaml
    /// </summary>
    public partial class wd_userInfo : Window
    {
        public wd_userInfo()
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
        BrushConverter bc = new BrushConverter();
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                #region translate

                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;

                translate();
                #endregion

                txt_userName.Text = MainWindow.userLogin.fullName;
                txt_pos.Text = MainWindow.posLogin.name;
                txt_branch.Text = MainWindow.posLogin.branchName;
                userImageLoad(bdr_mainImage, MainWindow.userLogin.image);

                
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
            txt_userNameTitle.Text = AppSettings.resourcemanager.GetString("trUserName") + ":";
            txt_posTitle.Text = AppSettings.resourcemanager.GetString("trPOS") + ":";
            if (MainWindow.branchLogin.type == "b")
                txt_branchTitle.Text = AppSettings.resourcemanager.GetString("tr_Branch") + ":";
            else
                txt_branchTitle.Text = AppSettings.resourcemanager.GetString("tr_Store") + ":";
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    //Btn_save_Click(null, null);
                }
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }
        List<SettingCls> set = new List<SettingCls>();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
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
        ImageBrush brush = new ImageBrush();
        async void userImageLoad(Border border, string image)
        {
            try
            {
                if (!string.IsNullOrEmpty(image))
                {
                    clearImg(border);

                    byte[] imageBuffer = await MainWindow.userLogin.downloadImage(image); // read this as BLOB from your DB
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    border.Background = new ImageBrush(bitmapImage);
                }
                else
                {
                    clearImg(border);
                }
            }
            catch
            {
                clearImg(border);
            }
        }
        private void clearImg(Border border)
        {
            Uri resourceUri = new Uri("/pic/no-image-icon-125x125.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            brush.ImageSource = temp;
            border.Background = brush;
        }



    }
}
