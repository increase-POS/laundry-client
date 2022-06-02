using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace laundryApp.Classes
{
    public class Pagination
    {
        //int[] countCategories;
        //int[] countItemss;
        void pageNumberActive(Button btn, int indexContent)
        {
            btn.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush; 
            btn.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
            btn.Content = indexContent.ToString();
        }
        void pageNumberDisActive(Button btn, int indexContent)
        {
            btn.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DFDFDF"));
            btn.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#686868"));
            btn.Content = indexContent.ToString();

        }
        public IEnumerable<Category> refrishPagination(IEnumerable<Category> _items, int pageIndex, Button[] btns, int countItems)
        {
            try
            {
                //countItemss = CatigoriesAndItemsView.itemsRowColumnCount(3, 3);
                if (_items is null)
                {
                    return new List<Category>();
                }

                #region < 3 Page
                if (2 >= ((_items.Count() - 1) / countItems))
                {
                    if (pageIndex == 1)
                    {
                        pageNumberActive(btns[1], 1);
                        pageNumberDisActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / countItems) == 0)
                        {
                            btns[2].IsEnabled = false;
                            btns[3].IsEnabled = false;
                        }
                        else if (((_items.Count() - 1) / countItems) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else if (pageIndex == 2)
                    {
                        pageNumberDisActive(btns[1], 1);
                        pageNumberActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / countItems) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else
                    {

                        pageNumberDisActive(btns[1], 1);
                        pageNumberDisActive(btns[2], 2);
                        pageNumberActive(btns[3], 3);
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;
                    }

                }

                #endregion

                #region > 3 Page
                if (2 < ((_items.Count() - 1) / countItems))
                {
                    if (pageIndex == 1)
                    {
                        pageNumberActive(btns[1], pageIndex);
                        pageNumberDisActive(btns[2], pageIndex + 1);
                        pageNumberDisActive(btns[3], pageIndex + 2);
                        if (((_items.Count() - 1) / countItems) == 0)
                        {
                            btns[2].IsEnabled = false;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else if (pageIndex == 2)
                    {
                        pageNumberDisActive(btns[1], 1);
                        pageNumberActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / countItems) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    ///// last
                    else if ((pageIndex - 1) >= ((_items.Count() - 1) / countItems))
                    {

                        pageNumberDisActive(btns[1], pageIndex - 2);
                        pageNumberDisActive(btns[2], pageIndex - 1);
                        pageNumberActive(btns[3], pageIndex);
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;
                    }
                    else
                    {
                        pageNumberDisActive(btns[1], pageIndex - 1);
                        pageNumberActive(btns[2], pageIndex);
                        pageNumberDisActive(btns[3], pageIndex + 1);

                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;

                    }
                }

                #endregion
                #region 
                if (2 >= ((_items.Count() - 1) / countItems))
                {
                    if (1 == (pageIndex))
                    {
                    }
                    else if (1 >= ((_items.Count() - 1) / countItems))
                    {
                    }
                    btns[0].IsEnabled = false;
                    btns[4].IsEnabled = false;
                }
                else if (pageIndex == 1)
                {
                    btns[0].IsEnabled = false;
                    btns[4].IsEnabled = true;
                }
                else if (pageIndex == 2)
                {
                    btns[4].IsEnabled = true;
                    btns[0].IsEnabled = true;
                }
                ///// last
                else if ((pageIndex - 1) >= ((_items.Count() - 1) / countItems))
                {
                    btns[4].IsEnabled = false;
                    btns[0].IsEnabled = true;


                }
                else
                {
                    btns[4].IsEnabled = true;
                    btns[0].IsEnabled = true;

                }


                #endregion


                _items = _items.Skip((pageIndex - 1) * countItems).Take(countItems);
                return _items;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return _items;
            }

        }

        public IEnumerable<Item> refrishPagination(IEnumerable<Item> _items, int pageIndex, Button[] btns, int countItems)
        {
            try
            {
                //countItemss = CatigoriesAndItemsView.itemsRowColumnCount(3, 3);
                if (_items is null)
            {
                return new List<Item>();
            }

            #region < 3 Page
            if (2 >= ((_items.Count() - 1) / countItems))
            {
                if (pageIndex == 1)
                {
                    pageNumberActive(btns[1], 1);
                    pageNumberDisActive(btns[2], 2);
                    pageNumberDisActive(btns[3], 3);
                    if (((_items.Count() - 1) / countItems) == 0)
                    {
                        btns[2].IsEnabled = false;
                        btns[3].IsEnabled = false;
                    }
                    else if (((_items.Count() - 1) / countItems) <= 1)
                    {
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = false;
                    }
                }
                else if (pageIndex == 2)
                {
                    pageNumberDisActive(btns[1], 1);
                    pageNumberActive(btns[2], 2);
                    pageNumberDisActive(btns[3], 3);
                    if (((_items.Count() - 1) / countItems) <= 1)
                    {
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = false;
                    }
                }
                else
                {

                    pageNumberDisActive(btns[1], 1);
                    pageNumberDisActive(btns[2], 2);
                    pageNumberActive(btns[3], 3);
                    btns[2].IsEnabled = true;
                    btns[3].IsEnabled = true;
                }

            }

            #endregion

            #region > 3 Page
            if (2 < ((_items.Count() - 1) / countItems))
            {
                if (pageIndex == 1)
                {
                    pageNumberActive(btns[1], pageIndex);
                    pageNumberDisActive(btns[2], pageIndex + 1);
                    pageNumberDisActive(btns[3], pageIndex + 2);
                    if (((_items.Count() - 1) / countItems) == 0)
                    {
                        btns[2].IsEnabled = false;
                        btns[3].IsEnabled = false;
                    }
                }
                else if (pageIndex == 2)
                {
                    pageNumberDisActive(btns[1], 1);
                    pageNumberActive(btns[2], 2);
                    pageNumberDisActive(btns[3], 3);
                    if (((_items.Count() - 1) / countItems) <= 1)
                    {
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = false;
                    }
                }
                ///// last
                else if ((pageIndex - 1) >= ((_items.Count() - 1) / countItems))
                {

                    pageNumberDisActive(btns[1], pageIndex - 2);
                    pageNumberDisActive(btns[2], pageIndex - 1);
                    pageNumberActive(btns[3], pageIndex);
                    btns[2].IsEnabled = true;
                    btns[3].IsEnabled = true;
                }
                else
                {
                    pageNumberDisActive(btns[1], pageIndex - 1);
                    pageNumberActive(btns[2], pageIndex);
                    pageNumberDisActive(btns[3], pageIndex + 1);

                    btns[2].IsEnabled = true;
                    btns[3].IsEnabled = true;

                }
            }

            #endregion
            #region 
            if (2 >= ((_items.Count() - 1) / countItems))
            {
                if (1 == (pageIndex))
                {
                }
                else if (1 >= ((_items.Count() - 1) / countItems))
                {
                }
                btns[0].IsEnabled = false;
                btns[4].IsEnabled = false;
            }
            else if (pageIndex == 1)
            {
                btns[0].IsEnabled = false;
                btns[4].IsEnabled = true;
            }
            else if (pageIndex == 2)
            {
                btns[4].IsEnabled = true;
                btns[0].IsEnabled = true;
            }
            ///// last
            else if ((pageIndex - 1) >= ((_items.Count() - 1) / countItems))
            {
                btns[4].IsEnabled = false;
                btns[0].IsEnabled = true;


            }
            else
            {
                btns[4].IsEnabled = true;
                btns[0].IsEnabled = true;

            }


            #endregion


            _items = _items.Skip((pageIndex - 1) * countItems).Take(countItems);
            return _items;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return _items;
            }

        }
        public IEnumerable<MenuSetting> refrishPagination(IEnumerable<MenuSetting> _items, int pageIndex, Button[] btns ,int countItems)
        {
            try
            {
                //countItemss = CatigoriesAndItemsView.itemsRowColumnCount(3, 3);
                if (_items is null)
                {
                    return new List<MenuSetting>();
                }

                #region < 3 Page
                if (2 >= ((_items.Count() - 1) / countItems))
                {
                    if (pageIndex == 1)
                    {
                        pageNumberActive(btns[1], 1);
                        pageNumberDisActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / countItems) == 0)
                        {
                            btns[2].IsEnabled = false;
                            btns[3].IsEnabled = false;
                        }
                        else if (((_items.Count() - 1) / countItems) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else if (pageIndex == 2)
                    {
                        pageNumberDisActive(btns[1], 1);
                        pageNumberActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / countItems) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else
                    {

                        pageNumberDisActive(btns[1], 1);
                        pageNumberDisActive(btns[2], 2);
                        pageNumberActive(btns[3], 3);
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;
                    }

                }

                #endregion

                #region > 3 Page
                if (2 < ((_items.Count() - 1) / countItems))
                {
                    if (pageIndex == 1)
                    {
                        pageNumberActive(btns[1], pageIndex);
                        pageNumberDisActive(btns[2], pageIndex + 1);
                        pageNumberDisActive(btns[3], pageIndex + 2);
                        if (((_items.Count() - 1) / countItems) == 0)
                        {
                            btns[2].IsEnabled = false;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else if (pageIndex == 2)
                    {
                        pageNumberDisActive(btns[1], 1);
                        pageNumberActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / countItems) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    ///// last
                    else if ((pageIndex - 1) >= ((_items.Count() - 1) / countItems))
                    {

                        pageNumberDisActive(btns[1], pageIndex - 2);
                        pageNumberDisActive(btns[2], pageIndex - 1);
                        pageNumberActive(btns[3], pageIndex);
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;
                    }
                    else
                    {
                        pageNumberDisActive(btns[1], pageIndex - 1);
                        pageNumberActive(btns[2], pageIndex);
                        pageNumberDisActive(btns[3], pageIndex + 1);

                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;

                    }
                }

                #endregion
                #region 
                if (2 >= ((_items.Count() - 1) / countItems))
                {
                    if (1 == (pageIndex))
                    {
                    }
                    else if (1 >= ((_items.Count() - 1) / countItems))
                    {
                    }
                    btns[0].IsEnabled = false;
                    btns[4].IsEnabled = false;
                }
                else if (pageIndex == 1)
                {
                    btns[0].IsEnabled = false;
                    btns[4].IsEnabled = true;
                }
                else if (pageIndex == 2)
                {
                    btns[4].IsEnabled = true;
                    btns[0].IsEnabled = true;
                }
                ///// last
                else if ((pageIndex - 1) >= ((_items.Count() - 1) / countItems))
                {
                    btns[4].IsEnabled = false;
                    btns[0].IsEnabled = true;


                }
                else
                {
                    btns[4].IsEnabled = true;
                    btns[0].IsEnabled = true;

                }


                #endregion


                _items = _items.Skip((pageIndex - 1) * countItems).Take(countItems);
                return _items;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return _items;
            }

        }
        public IEnumerable<User> refrishPagination(IEnumerable<User> _items, int pageIndex, Button[] btns  ) 
        {
            try
            {
                if (_items is null)
                {
                    return new List<User>();
                }

                #region < 3 Page
                if (2 >= ((_items.Count() - 1) / 12))
                {
                    if (pageIndex == 1)
                    {
                        pageNumberActive(btns[1], 1);
                        pageNumberDisActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / 12) == 0)
                        {
                            btns[2].IsEnabled = false;
                            btns[3].IsEnabled = false;
                        }
                        else if (((_items.Count() - 1) / 12) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else if (pageIndex == 2)
                    {
                        pageNumberDisActive(btns[1], 1);
                        pageNumberActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / 12) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else
                    {

                        pageNumberDisActive(btns[1], 1);
                        pageNumberDisActive(btns[2], 2);
                        pageNumberActive(btns[3], 3);
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;
                    }

                }

                #endregion

                #region > 3 Page
                if (2 < ((_items.Count() - 1) / 12))
                {
                    if (pageIndex == 1)
                    {
                        pageNumberActive(btns[1], pageIndex);
                        pageNumberDisActive(btns[2], pageIndex + 1);
                        pageNumberDisActive(btns[3], pageIndex + 2);
                        if (((_items.Count() - 1) / 12) == 0)
                        {
                            btns[2].IsEnabled = false;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else if (pageIndex == 2)
                    {
                        pageNumberDisActive(btns[1], 1);
                        pageNumberActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / 12) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    ///// last
                    else if ((pageIndex - 1) >= ((_items.Count() - 1) / 12))
                    {

                        pageNumberDisActive(btns[1], pageIndex - 2);
                        pageNumberDisActive(btns[2], pageIndex - 1);
                        pageNumberActive(btns[3], pageIndex);
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;
                    }
                    else
                    {
                        pageNumberDisActive(btns[1], pageIndex - 1);
                        pageNumberActive(btns[2], pageIndex);
                        pageNumberDisActive(btns[3], pageIndex + 1);

                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;

                    }
                }

                #endregion
                #region 
                if (2 >= ((_items.Count() - 1) / 12))
                {
                    if (1 == (pageIndex))
                    {
                    }
                    else if (1 >= ((_items.Count() - 1) / 12))
                    {
                    }
                    btns[0].IsEnabled = false;
                    btns[4].IsEnabled = false;
                }
                else if (pageIndex == 1)
                {
                    btns[0].IsEnabled = false;
                    btns[4].IsEnabled = true;
                }
                else if (pageIndex == 2)
                {
                    btns[4].IsEnabled = true;
                    btns[0].IsEnabled = true;
                }
                ///// last
                else if ((pageIndex - 1) >= ((_items.Count() - 1) / 12))
                {
                    btns[4].IsEnabled = false;
                    btns[0].IsEnabled = true;


                }
                else
                {
                    btns[4].IsEnabled = true;
                    btns[0].IsEnabled = true;

                }


                #endregion


                _items = _items.Skip((pageIndex - 1) * 12).Take(12);
                return _items;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return _items;
            }

        }
        public IEnumerable<Agent> refrishPagination(IEnumerable<Agent> _items, int pageIndex, Button[] btns)
        {
            try
            {
                if (_items is null)
                {
                    return new List<Agent>();
                }

                #region < 3 Page
                if (2 >= ((_items.Count() - 1) / 12))
                {
                    if (pageIndex == 1)
                    {
                        pageNumberActive(btns[1], 1);
                        pageNumberDisActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / 12) == 0)
                        {
                            btns[2].IsEnabled = false;
                            btns[3].IsEnabled = false;
                        }
                        else if (((_items.Count() - 1) / 12) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else if (pageIndex == 2)
                    {
                        pageNumberDisActive(btns[1], 1);
                        pageNumberActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / 12) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else
                    {

                        pageNumberDisActive(btns[1], 1);
                        pageNumberDisActive(btns[2], 2);
                        pageNumberActive(btns[3], 3);
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;
                    }

                }

                #endregion

                #region > 3 Page
                if (2 < ((_items.Count() - 1) / 12))
                {
                    if (pageIndex == 1)
                    {
                        pageNumberActive(btns[1], pageIndex);
                        pageNumberDisActive(btns[2], pageIndex + 1);
                        pageNumberDisActive(btns[3], pageIndex + 2);
                        if (((_items.Count() - 1) / 12) == 0)
                        {
                            btns[2].IsEnabled = false;
                            btns[3].IsEnabled = false;
                        }
                    }
                    else if (pageIndex == 2)
                    {
                        pageNumberDisActive(btns[1], 1);
                        pageNumberActive(btns[2], 2);
                        pageNumberDisActive(btns[3], 3);
                        if (((_items.Count() - 1) / 12) <= 1)
                        {
                            btns[2].IsEnabled = true;
                            btns[3].IsEnabled = false;
                        }
                    }
                    ///// last
                    else if ((pageIndex - 1) >= ((_items.Count() - 1) / 12))
                    {

                        pageNumberDisActive(btns[1], pageIndex - 2);
                        pageNumberDisActive(btns[2], pageIndex - 1);
                        pageNumberActive(btns[3], pageIndex);
                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;
                    }
                    else
                    {
                        pageNumberDisActive(btns[1], pageIndex - 1);
                        pageNumberActive(btns[2], pageIndex);
                        pageNumberDisActive(btns[3], pageIndex + 1);

                        btns[2].IsEnabled = true;
                        btns[3].IsEnabled = true;

                    }
                }

                #endregion
                #region 
                if (2 >= ((_items.Count() - 1) / 12))
                {
                    if (1 == (pageIndex))
                    {
                    }
                    else if (1 >= ((_items.Count() - 1) / 12))
                    {
                    }
                    btns[0].IsEnabled = false;
                    btns[4].IsEnabled = false;
                }
                else if (pageIndex == 1)
                {
                    btns[0].IsEnabled = false;
                    btns[4].IsEnabled = true;
                }
                else if (pageIndex == 2)
                {
                    btns[4].IsEnabled = true;
                    btns[0].IsEnabled = true;
                }
                ///// last
                else if ((pageIndex - 1) >= ((_items.Count() - 1) / 12))
                {
                    btns[4].IsEnabled = false;
                    btns[0].IsEnabled = true;


                }
                else
                {
                    btns[4].IsEnabled = true;
                    btns[0].IsEnabled = true;

                }


                #endregion


                _items = _items.Skip((pageIndex - 1) * 12).Take(12);
                return _items;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return _items;
            }

        }
    }
}
