   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:laundryApp.View.reports.accountsReports'laundryApp.View.reports.accountsReports  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5      M    GlaundryApp.View.reports.kitchenReports.uc_preparingOrdersLaundryReports   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Glocal5clr-namespace:laundryApp.View.reports.accountsReports     5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
      -   5        
  � Tag$#  preparingOrdersKitchenReports��5       .��5      � -   5         �Name$ 	grid_main��5      $��20 10q�6    � ColumnDefinitions 5   
   �� 5      $��2* �6   6-   �� 5      $
��auto �6   6/   �� 5      $��1* �6   6-   .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $
��auto �6    60   �� 5      $��* �6    6-   .+�5      �� $��	10,10,5,5q�6    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5          Opacity$	 0.3Q�5!      	   
BlurRadius$	 10Q�5"      ._�5$      �  5%      �� 5&      $��* �6(   �� 5'      $	��130 �6(   .+�5)      �� 
 �� CornerRadius$
 7,7,0,0��6   $��52��65   $��0��5*         SecondColor#��[  5,      ��5+      Z� ._�5-      � -   5        $ grid_tabControl��5-       5.      �� 5/   "   $
��auto �63   �� 52   "   $��* �63   �� 53   "   $
��auto �63   .+�57      �� -   5         ��Name$ bdr_preparingOrders��57   %   $��0��6B   
�������$��0q�6e   $��40��6z   $��60��6�   $��Bottom=�6�   $
 7,7,0,0��6�   
������ $��	10,5,0,-1q�6�   ._�58   "   �� -   5         ��Name$ btn_preparingOrders��58   )   
������ $  preparingOrders��6`   +  %MaterialDesignThemes.Wpf.RippleAssist   Feedback
 ���� 
������ $
��auto��59   v    � Triggers 5:   &   :� 5;   *   -   5        trig�� :� RoutedEvent$ Button.Click��5;   E   .[�5<   .   �� .��5=   2   �� .��5>   6   W�  �� 
TargetName$ bdrMain��5?       �� TargetProperty$ Opacity�5@       W� From$ 0X�5A      	 W� To$ 1X�6"    g� Duration$ 0:0:1D�5B      5C   3   5D   /   5E   +   .��5G   &   F� -   5        $��path_preparingOrder��5G   +   #��[  6F    �� Stretch$
 Fill=�6j   $	��Top=�6y   $��Center=�6�   $��0,-1,0,0q�6�   $��25��5H   (   $��25��63   $��LeftToRight=�6A     cooking#��[ 5I   *    � ToolTip 5J   *   _� 5K   .   -   5         _�Name$ tt_preparingOrder��5K   6   $��Preparing Order��6Q    _� 	Placement$
 left=�5L   /    _� HorizontalOffset$ 35��5M   /    _� PlacementRectangle$ 50,0,0,0�5N   /    _� VerticalOffset$	 -10��5O   /   5Q   '   5R   #   5S      �� 5U      $��10��6%   $
 7��66   ._�5V   "   �� -	   5         ��Name$ txt_tabTitle��5V   -   
�������$��Center=�5W      $��16(�6?   $��5 5 10 5q�6Q   5X      5Z      5[      �� 5]      $
 0,0,7,7��6   $��1��5^      
�������$
��auto��5`   ,   ��5_      Z� ._�5a      �� -
   5        $ bdrMain��5a   =   $��	15,0,15,0q�6!   $
��auto��6N   
������ ._�5b      �� $��Disabled=�6+   $
��Auto=�6R   .��5c   "   � -   5        $ grid_father��5c   '   .+�5d   &   � -   5        $ grid_invoice��5d   8   $��1��6+   $��
10,0,10,10q�6N   $y�Visible=�6c    5e   *   �� 5f   .   $
��auto �6<   6L   �� 5g   .   $
��auto �6<   6L   �� 5h   .   $
��auto �6<   6L    5k   *   �� 5l   .   $��* �6?   6K   �� 5m   .   $��* �6?   6K   �� 5n   .   $
��1.5* �6?   $	��200��6L   6]   .+�5r   *   ]� $��0��64   $��2��6A   ��.  .+�5s   .   �� $��0��65   $
 10��6B   $��40��6T   $��
10,10,10,5q�6`   $��1q�6v     	LightGrey#��[ 6�   ._�5t   2   � .+�5v   6   (  "System.Windows.Controls.DatePicker  -   5        $��dp_startDate��5v   A      SelectedDateFormat$ Short=�6U   $	��100��6r   $��10,0,5,0q�5w   A   $��40��6T   $��Right=�6`   ���#737373$��0q�5x   Y   $	��4,0q�6m    ( $MaterialDesignFloatingHintDatePicker#��[ 5y   A   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Start Date...��5{   A   .  (MaterialDesignThemes.Wpf.TextFieldAssist   CharacterCounterStyle 5z   A   �� 5|   A   5}   3   5~   /   �� 5�   .   $��0��65   $��40��6B   $
 10��6N   $��
10,10,10,5q�6`   $��1q�6v   #��[ 6�   ._�5�   2   � .+�5�   6     -   5        $��
dp_endDate��5�   B   $ Short=�6T   $	��100��6p   $��10,0,5,0q�5�   A   $��40��6T   $��Right=�6`   ���#737373$��0q�5�   Y   $	��4,0q�6m   #��[ 5�   A   $ End Date...��5�   A    5�   A   �� 5�   y   5�   3   5�   /   5�   +   ]� 5�   *   $��0��64   ��.  .+�5�   .   �� $��0��65   $
 10��6B   $��40��6T   $��
10,10,10,5q�6`   $��1q�6v   #��[ 6�   ._�5�   2   � .+�5�   6   �� -   5        $��cb_branches��5�   ~   $	��150��6o    & "MaterialDesignFloatingHintComboBox#��[ 5�   ?   $ Branch ....��5�   ?   ���#737373$��0q�5�   W   $	��4,0q�6k   ���#4E4E4E$��40��5�   ?   $��10,0,5,0q�6K   $��1��6^   6m   5�   3   5�   /   �� 5�   .   -   5          ��Name$  chk_allBranches��5�   w   $y�	Collapsed=�67     	MainColor#��[ 6O   $��1��6�   $	��All��6�    2 .MaterialDesignFilterChipPrimaryOutlineCheckBox#��[ 6�   6>  5�   +   ]� 5�   *   -   5        ! ]�Name$! pnl_pos��5�   X   $��2��64   ��.  .+�5�   .   �� $��0��65   $��40��6B   $
 10��6O   $��	10,5,10,5q�6a   $��1q�6v   #��[ 6�   ._�5�   2   � .+�5�   6   �� -   5        $��cb_category��5�   N   $	��150��6?   #��[ 5�   ?   $ Category ....��5�   ?   ���#737373$��0q�5�   W   $	��4,0q�6k   ���#4E4E4E$��40��5�   ?   $��10,0,5,0q�6K   $��1��6^   ��5�   :   �� 5�   >   .R�5�   B   �� ��6L   �� *categoryName+ # translateCategoryNameForReports" �� 	Converter#" [ 5�   ?   5�   7   5�   3   5�   /   �� 5�   .   -   5        $  chk_allCategories��5�   `   #��[ 68   $��2��6{   $	��All��6�   #��[ 6�   6.  5�   +   5�   '   � 5�   &   -   5        $ 	grid_item��5�   8   $��1��6+   $��
10,0,10,10q�6K   $y�	Collapsed=�6_    5�   *   �� 5�   .   $
��auto �6<   6L   �� 5�   .   $
��auto �6<   6M    5�   *   �� 5�   .   $��* �6?   6K   �� 5�   .   $��* �6?   6K   �� 5�   .   $
��1.5* �6?   $	��200��6L   6]   5�   '   5�   #   5�      5�      5�      5�      5�      �� 5�      $��1��6   $��	10,5,5,10q�6#    5�        5�      $ 1Q�5�      $	 360Q�5�      $
 Gray��5�      $	 0.3Q�5�      $	 10Q�5�      ._�5�      � $��1��6    5�      �� 5�      $
��auto �6(   68   �� 5�      $��45 �6(   66   �� 5�      $��* �6(   65   �� 5�      $��50 �6(   .+�5�      �� $��1��6   
�������$
 7,7,0,0��6=   ._�5�      �� $��0,0,0,1q�6!   ���	#FFC8C8C8._�5�      ]� ��.  .+�5�   "   �� $��0��6)   $	��200��66   $
 10��6C   $��	10,5,10,5q�6U   $��1q�6j   #��[ 6~   ._�5�   &   � .+�5�   *   �� -   5        # ��Name$# 
txt_search��5�   2   $��10,0,5,0q�5�   2   ���#737373$��0q�6]   $	��4,0q�6q    % !MaterialDesignFloatingHintTextBox#��[ 5�   2   $% Enter Here To Start Search ....��5�   2   ���#4e4e4e5�   2   5�   '   5�   #   �� 5�   "   $��0,5,0,0q�6)   
�������$��30��6N   $��30��6Y   ._�5�   &   F� ���#4e4e4e$
 Fill=�6:   $��0��5�      $
��17.5��5�      $
��17.5��6&   ����p��ffffff0@������lW@���fffff&P@��������LG@ffffff0@������`��`,a 0v�hx�`r��ō�������}��q����f�z��@) ����Z @ sӀ�  ���� `�Z  GS@�� �����y_@ $� ����. G��. G�. G�. G� B� �{�������F@��D�������F@��D� ���$7Ѡ'�@��S���43333U@`�������J@�A � �V `� ظ23333]@@�~�`���k�~�fffff&@@��ffffff0@Q��ffffff0@� K@zA�����LV@43333�T@@�`6 �����LL@@zA�����F@43333sM@�����F@gfffffC@�����LL@hfffff:@S�a     ,@�����LV@������+@ Khfffff:@S��i433333@@���    @D@���43333sH@� 3� Tj��i�����YP@ K@zA�`}�`F�Ӏ`F�gffff�S@@2Ygffff�S@`���gffff�S@�d�43333�S@�����=@  � ��� ����`�y �,�{s@y��`am �������'�Э �-��W`��3 �� ]����������H@S ��������G@@��������F@ M�������E@� B� �� ��`F�`}�`F�	5�      5�   #   �� 5�   "   -   5        $ btn_refresh��5�   s   $��LeftToRight=�6)   $��Right=�6E   $��40��6\   $��40��6g   +  %MaterialDesignThemes.Wpf.ButtonAssist$   CornerRadius$$ 15��5�   )   % � Cursor$
% Hand��6W    	 SmallButtonCircleRotate#��[	 5�   X   $	��5,1q�5�   )   & J� RenderTransformOrigin$& 0.5,0.5&�66   ' � DataContext$�
' �
M896,0L896,0C401.2,0,0,401.2,0,896v0c0,494.8,401.2,896,896,896h0c494.8,0,896-401.2,896-896v0C1792,401.2,1390.8,0,896,0z   M1451.3,1021c-31.9,133.7-98.8,242.1-200.6,325.2c-101.8,83.1-221,124.6-357.7,124.6c-72.8,0-143.3-13.7-211.4-41.2  s-128.8-66.6-182.2-117.5l-96.5,96.5c-9.5,9.5-20.7,14.2-33.7,14.2s-24.2-4.7-33.7-14.2c-9.5-9.5-14.2-20.7-14.2-33.7v-335.3  c0-13,4.7-24.2,14.2-33.7s20.7-14.2,33.7-14.2h335.3c13,0,24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7c0,13-4.7,24.2-14.2,33.7  l-102.5,102.5c35.4,32.9,75.6,58.4,120.5,76.3c44.9,18,91.5,26.9,139.9,26.9c66.9,0,129.2-16.2,187.1-48.6  c57.9-32.4,104.3-77.1,139.2-134c5.5-8.5,18.7-37.7,39.7-87.6c4-11.5,11.5-17.2,22.4-17.2h143.7c6.5,0,12.1,2.4,16.8,7.1  c4.7,4.7,7.1,10.4,7.1,16.8C1452,1018.2,1451.8,1020,1451.3,1021z M1470.7,752.3c0,13-4.7,24.2-14.2,33.7  c-9.5,9.5-20.7,14.2-33.7,14.2h-335.3c-13,0-24.2-4.7-33.7-14.2s-14.2-20.7-14.2-33.7c0-13,4.7-24.2,14.2-33.7l103.3-103.3  C1083.3,547,996.3,512.9,896,512.9c-66.9,0-129.2,16.2-187.1,48.6s-104.3,77.1-139.2,134c-5.5,8.5-18.7,37.7-39.7,87.6  c-4,11.5-11.5,17.2-22.5,17.2H358.7c-6.5,0-12.1-2.4-16.8-7.1c-4.7-4.7-7.1-10.4-7.1-16.8V771c32.4-133.7,99.8-242.1,202-325.2  c102.3-83.1,222-124.6,359.2-124.6c72.8,0,143.7,13.8,212.5,41.5s130,66.7,183.3,117.1l97.3-96.5c9.5-9.5,20.7-14.2,33.7-14.2  s24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7V752.3z��5�   )   ��5�   )   Z� ��6?   Z� z�5  &   V� 5  *   .~�5  .   �� �� 5  .   �� 5  .   ( �� Angle$( 0Q�6>   T� 5  .   5  +    5	  &   _� 5
  *   -   5        $ 
tt_refresh��5
  2   $��Refresh��6F   $
 left=�5  /   $ 15��5  /   $ 50,0,0,0�5  /   $ 25��5  /   5  #   5     5     5     �� 5     $��2��6   
�������._�5     � $��1��6    .+�5     �� -   5        $ brd_roundDataGrid��5  Z   $��
5,5,5,17.5q�6%   $��1��6;   $
 10��6H   $��0q�6u   
�������5     &   System.Windows.Controls.DataGrid  5     -   5        $��	dg_orders��5  (   $��
5,5,5,17.5q�5  (   )   
IsReadOnly) . 
�������*   AutoGenerateColumns* .  +   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$+ SizeToHeader 5  V   ,   AlternatingRowBackground, �#f3f3f3- �� AlternationCount$- 2��5  K   . J� OpacityMask. 5  "   !� 5  &   / !� Visual/ 62   �� 0 �� ElementName$0 brd_roundDataGrid��1 � 	Resources1 5   "   =	  7System.Windows.Controls.Primitives.DataGridColumnHeader2 �� BasedOn3 �� 
TargetType4 �� Property5 �� Value��%  (��       5!  6   * SystemColors.HighlightBrushKey+)'	       7	 7   �� 6&   6j   �� 5"  &   82   "3 	 6e   .�5#  *   �� 4 � ��5 �#F5D7C46G   �� 5$  *   4 � ��
5 �����6H   �� 5%  *   4 � ��$5 0 0 1 0q�6L   �� 5&  *   4 � ��85  6G   5'  '   5(  #   6   Columns6 5)  "   0
  *System.Windows.Controls.DataGridTextColumn
  5*  &   -   5        col_orderNum��,  &System.Windows.Controls.DataGridColumn7   Width$7 * 5*  T   8   Header$	8 No.��6^   1  +System.Windows.Controls.DataGridBoundColumn9   Binding9 6q   �� *
orderNum+
  5+  &   -   5        
col_invNum��$7 * 5+  T   $	8 No.��6^   9 6q   �� *	invNumber+
  5,  &   -   5        col_itemName��$7 * 5,  T   $
8 Item��6^   9 6q   �� *
itemName+
  5-  &   -   5        col_quantity��$7 * 5-  T   $	8 QTY��6^   9 6q   �� *
quantity+
  5.  &   -   5        
col_status��$7 * 5.  T   $8 Status��6^   9 6q   �� *status+ !
 preparingOrderStatusConverter#" [
 
  5/  &   -   5        col_date��$7 * 5/  T   $
8 Date��6^   9 6q   �� *
createDate+  dateFrameConverter#" [ 
  50  &   -    5        
col_branch��$7 * 50  T   $8 Branch��6^   9 6q   �� *
branchName+
  51  &   -!   5        col_category��$7 * 51  T   $8 Category��6^   9 6q   �� *categoryName+#" [ 
  52  &   -"   5        col_tag��$7 * 52  T   $	8 Tag��6^   9 6q   �� *	tagName+
  53  &   -#   5        col_duration��$7 * 53  T   $8 Duration��6^   9 6q   �� *realDuration+  decimalToTimeConverter#" [ 55     56     58     �� 59     $��3��6   
�������$
 0,0,7,7��6=   ._�5:     ]� ��. .+�5;     �� $��0,0,0,1q�6%   #��[ 6?   $��
10,0,10,10q�6h   $
��auto��6|   $
��Left=�6�   ._�5<  "   �� : �� Orientation$: 
Horizontal=�6.   .+�5>  &   �� -$   5        $ btn_pdf��5>  -   $��10,0,5,0q�6m   $��0q�5?     $��35��6'   ��5>  U   Z� ��5@     Z� .��5A  *   '  !MaterialDesignThemes.Wpf.PackIcon  ���#D20A0A;   Kind$; FilePdf=�6W   $��22��5C     $��22��6&    5D  .   _� 5E  2   -%   5        $ 	tt_report��5E  :   $��Report��6M   $
 left=�5F  /   $ 5��5G  /   $ 50,0,0,0�5H  /   $	 -10��5I  /   5K  +   5L  '   �� 5M  &   -&   5        $ 	btn_print��5N  4   $��5,0,5,0q�5M  H   $��0q�5N     $��35��6'   ��5M  /   Z� ��5O     Z� .��5P  *     ���#313131$; Printer=�6W   $��Center=�5Q     $��22��5R     $��22��6"    5S  .   _� 5T  2   -'   5        $ tt_print��5T  :   $��Print��6L   $
 left=�5U  /   $ 5��5V  /   $ 50,0,0,0�5W  /   $	 -10��5X  /   5Z  +   5[  '   �� 5\  &   -(   5        $ btn_exportToExcel��5]  7   $��3��5\  H   $��5,0,5,0q�6X   $��0q�5]     $��35��6)   ��5\  /   Z� ��5^  (   Z� .��5_  *   F� ���#378500$
 Fill=�6>   $��20��5`     $��20��6&   $��LeftToRight=�5a     ��������- � $� ��- � $�  O[ ��c" O[ Q�c"H�z� @Q $� H�z� @� $� ��� ���!��� ���! � � $�  � � $� ��� ���!��� ���! $� � $�  $� � $� �����!�����! -1� $�  -1� $� ��^����^������^���h�X���P�Qр��0; Ӏ���3 �����- ���- � 6n -1� -1 -1� -1��� 6n��� 6n -1� 6n $� � -1 $� � -1��� � 6n��� � 6n $� � 6n � � -1 � � -1��� � 6n��� � 6n � � 6n z � -1 z � -1��[ � 6n��[ � 6n z �    (, �    @�`���� �������     �    (, ��Ɛ  �E�����@Zd;ߏ-@�H$u Щ� U-��o@v��/�,@���x�&@��~j�4,@Q؛r ��~j�4,@��r P�� @�p 0)� �	o ��� ��QT  ����*  �Q8\ �x�&1*@�/ �Ry ��bY �Ry ��MbX9@੧ �x4q `S�  �r ��� p=
ףp@�v���'@Yd;�O�@�v���'@G�z��@|?5^�I'@MbX94@23333�&@k�����@�8� n���!#@������@���K7(@������@�0�Z$"@��� ��A`��b(@�рƐ  �рƐ  �	 5d  .   _� 5e  2   -)   5        $ tt_excel��5e  :   $��Export to excel��6L   $
 left=�5f  /   $ 35��5g  /   $ 50,0,0,0�5h  /   $	 -10��5i  /   5k  +   5l  '   �� 5m  &   -*   5        $ btn_preview��5n  7   $��3��5m  H   $��0,1,5,0q�6X   $��0q�5n     $��35��6)   ��5m  /   Z� ��5o  (   Z� .��5p  *   � $��LeftToRight=�6/   .+�5q  .   F� ���#4E4E4E$
 Fill=�6B   $��15��5r     $��18��6&   $��LeftToRight=�5s     ������T1 E� 1���A�������dx@�pi�Y:������$q@@��������n@ s� �M�����iu@������Ik@�� �����)k@������t@ ������Tt@S�>/�����'t@@�P������s@`�p������s@S@ 23333+s@�H�����̔r@�9������r@S�	�23333kq@������r@������p@@G.effff�p@S �ceffff�p@gffffft@effff�p@gffffft@����̴p@43333ct@�����om@`�r03333si@`�rcffffve@S`�rcffff�d@`�rcffff�d@��#cffff�d@gffffFo@cffff�d@gffff�n@cffff�d@    `n@������d@    `n@03333�c@�    `n@���     n@ ��    �m@`�����̼m@�m��@���m�Ӏ���m�����m� �{������T@S��������T@�/���Gਤ���ਤ����43333sS@ jM43333sS@�S�ਤ =Q�ਤ�bw�Э �  ����gffff�T@@� 2:�}.gfffffW@�}.��Q��}.�gfffft@�}.�3����gffff^t@�����ix@�`�Z��F�*E��F�T1 E��/��aS� f��T`#Y��T�����T�`�b��T���T��T��H oV43333S^@��T�gffff�]@@�Q     ]@�~<`~�@��43333�\@����� ���j<�����yp@S@��gffffvp@���ఴ@Oఴ� ��ఴ�������q@ �������	r@��������	r@ �� )2�I0 ��aS�/��aS�_ z"S� $gffff�]@�����Y^@�_�3M�_� ���_�43333�i@�_��V�]943333�i@@$gӠ%[�q�����̜i@ �������i@ ��hffffvg@fffff6`@������e@ ��hffff&d@ ���63333s^@ ���63333�]@ ��������\@fffff`@     ]@ �wӠ��@�Z��j<�_ z"��+���	�������^@ E�	 �|���	fffff`@���	������lj@���	�23333�j@���	�ͼ���	������j@@R�	Ӡ���l 
 � �
�����ik@ $H
������ik@@dt
 P��T�
������j@ ��
������j@�����9f@������j@�|�
�����yj@�|�
� E�	�|�
�@�	�|�
�dD�����9f@�����_@�|�
�������^@�|�
�����l^@�����9f@�+ ��
�`� �ۏ
�����u
�����I
�����j
`� �l 
�+���	���Q�D��@���D���s��D���@���d��D��@+��� )2`��gffffr@�&��/�VR    �q@�To    �q@�����9l@gffff�q@�����?l@43333�q@�����<l@������q@�����<l@�����9i@�����<l@63333f@�����<l@������b@�����<l@kffff�_@�����?l@����̬^@�����?l@kffff�]@�����<l@������\@�����	l@73333�\@������k@� q�`��`�D���Q�D��`�K�0�ӀOG@�gffff�i@`���{�����w@gffffFi@�����$w@�}������!w@`;������!w@43333�^@�����!w@43333s]@�����!w@gffff]@���43333�\@�#��    �\@����a�`�s43333s^@`�s43333�`@�����|v@��������|v@433333d@�����|v@gffff�e@�����|v@�W"23333{v@������h@�����|v@43333#i@�����|v@�����li@������v@43333�i@������v@Ӡ�B 5�@wP�M�`�K�0�� ,��/S�Ӵ�����!t@`�c�T1���T1�43333�^@�T1��D�T1�����^@�a443333�]@`�)����gffff]@@s�gffff]@���gffff]@gffff�s@gfffff]@gffff�s@    �]@43333�s@������]@gffffvs@43333s^@���43333�^@����gffff&d@����gffff�e@���gffffFg@���gffff�h@���gffff�h@��gffffi@��gffff6i@���gffff�i@gffff~s@    �i@`��gffff�i@���� U��  5@G. ,��/�`�YCS ������V@`�Q`�����yV@S`����̬[@ ������l`@��@c	����D	`O(����̌c@�����p@����̌c@S ׋������c@ �������c@ 1�������c@Q��=������c@� n����� x�%�`�YC	 5t  2   _� 5u  6   -+   5        $ 
tt_preview��5u  >   $��Preview��6R   $
 left=�5v  /   $ 35��5w  /   $ 50,0,0,0�5x  /   $	 -10��5y  /   5{  /   F� 5|  .     Blue#��[ 63   $
 Fill=�6P   $��13��5}     $��6��6&   $��	17,10,0,0q�61   $��LeftToRight=�5~     ������O|��}�`g� � e`���� �� Թ �V@� ���%����������s@@W��g,@��@ܳ S�V3�����Qr@`{��T�����iS �������t@ ���������#� %��^�fffffp@�����lv@`s ��S ��������w@���P����@#S������̬v@����k�@w�@����0�  ��#��Ҧ�O|��}� @�����'@����\�� \[��������1r@gffff�s@ M&��������t@��������u@������r@������u@ ��������u@�I��@���)�@k"@�� @���@?S �����T`g�~��i��%�i�������is@�i�������r@��e������r@�<��������r@�������ds@ *������t@�����������Q��@?S ��	5�  /   5�  +   5�  '   �� 5�  &   -,   5        $ btn_settings��5�  9   $��3��5�  H   $��
5,0,5,-0.6q�6X   $��0q�5�     $
��Auto��6)   $y�	Collapsed=�6O   ��5�  /   Z� ��5�  (   Z� .��5�  *     $��20��5�     $��20��6&   $��LeftToRight=�5�     $; Settings=�68   
�������5�  +   5�  '   �� 5�  &   --   5        $ 	txt_count��5�  H   $��4��6]   < �� Padding$< 10q�5�     $��0��6,   ���#378500$��12(�6J   ��5�  2   Z�  5�  *   _� 5�  .   -.   5        $ tt_count��5�  6   $��Customers Count��6H   $
 left=�5�  /   $ 35��5�  /   $ 	50,25,0,0�5�  /   $	 -20��5�  /   5�  '   5�  #   5�     5�     5�     5�     5�     5�     �� 5�  
   $��1��6   
������ $��5��6@   $��Stretch=�6J   � 5�  
   $��2��6    5�     �� 5�     $��* �6    6-   �� 5�     $
��auto �6    60   �� 5�     $��* �6    6-   �� 5�     $
��auto �6    60   �� 5�     $��* �6    6-   .+�5�     �� $��0��6   
�������$��	5,10,10,5q�66   $
 7��6I    5�       5�     $ 1Q�5�     $	 360Q�5�     $
 Gray��5�     $	 0.3Q�5�     $	 10Q�5�     ._�5�     � $��	5,10,5,10q�6   .+�5�     #  LiveCharts.Wpf.CartesianChart  -/   5        $��cartesianChart��5�  )   &   LiveCharts.Wpf.Charts.Base.Chart=   LegendLocation$= Bottom=�6?   >   AxisX> 5�       LiveCharts.Wpf.Axis  -0   5        ?  Name$? axcolumn��5�  (   @   	Separator@ 5�  "     LiveCharts.Wpf.Separator  5�  &   A   Step$A 1Q�64   5�     5�     5�     5�     �� 5�     $��1��6   
������ $��Stretch=�6A   $��5��6_   �� 5�     $��2��6   
�������$��5,5,10,5q�65   $
 7��6G    5�       5�     $ 1Q�5�     $	 360Q�5�     $
 Gray��5�     $	 0.3Q�5�     $	 10Q�5�     ._�5�     � -1   5        $ grid1��5�     $��	5,10,5,10q�6&    5�     �� 5�     $��* �6+   67   �� 5�     $
��auto �6+   6:   .+�5�       LiveCharts.Wpf.PieChart  -2   5        $��chart1��5�  #   $= Right=�61   5�     5�     5�     �� 5�     $��3��6   
������ $��Stretch=�6A   $��5��6_   �� 5�     $��4��6   
�������$��	5,5,10,10q�65   $
 7��6H    5�       5�     $ 1Q�5�     $	 360Q�5�     $
 Gray��5�     $	 0.3Q�5�     $	 10Q�5�     ._�5�       -3   5        $��rowChart��5�  %   $��	5,10,5,10q�65   $= Bottom=�6H   > 5�       -4   5        $? MyAxis��5�  $   @ 5�       5�  "   $A 1Q�60   5�     5�     5�     5�     5�     5�     