   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   ,    &laundryApp.View.windows.wd_ordersReady   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   -   5          � WindowStyle$
  None=�5	   	    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5
   	   $��0q�6   $	��400��5   	   $	��600��6   
������  � Title$ Increase POS��5   	    � AllowsTransparency . .��5      � -   5         �Name$ 	grid_main��5   *   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $��30��6   $��Center=�6'    � ColumnDefinitions 5      �� 5       $��* �6/   �� 5!      $
��1.2* �6/   .+�5#      F� ���#DFDFDF �� Stretch$
 Fill=�61   $��LeftToRight=�6@   $��Right=�5$      $��0��6;   $��Center=�5%      $��20��65   $��20��6@     
waiterHand#��[ 5&   #   �� 5'      -   5         ��Name$ 	txt_title��5'   2   $��orders��6$   
�������$
��Left=�5(   $   $��Center=�6?   $��1��5)      $	��5,0q�64   5*      5+      5-      5/      �� 50   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��51      ���#999999
�������._�52      � -   5        $ grid_mainGrid��52       53      �� 54      $��* �6$   �� 55      $��10 �6$   .+�57      �� -   5         ��Name$ brd_roundDataGrid��57   E   $��0q�6   $��0��6&   $ 10��63   $��0q�6`   
�������58      &   System.Windows.Controls.DataGrid  59      -   5          Name$ 	dg_orders��59      $��5q�6.   $��0��69   $��2��6F   
�������-  'MaterialDesignThemes.Wpf.DataGridAssist   CornerRadius$ 5��5:      $y�Visible=�6F      
IsReadOnly .    CanUserAddRows .     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 5=       J� OpacityMask 5>      !� 5?       !� Visual 6&   ��  �� ElementName$ brd_roundDataGrid�� � 	Resources 5A      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn �� 
TargetType �� Property �� Value��%  (��       5B   *   * SystemColors.HighlightBrushKey+)'       7 7   �� 6   6^   �� 5C      8   "  6Y   .�5D      ��  � �� �#F5D7C46;   �� 5E       � ��
 �����6<   �� 5F       � ��$ 0 0 1 0q�6@   �� 5G       � ��8  6;   5H      5I         Columns 5J      0  *System.Windows.Controls.DataGridTextColumn  5K      ,  &System.Windows.Controls.DataGridColumn   Width$ * 6-   -   5        
col_orders��    Header$  orders��5K   K   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6[   �� " �� Path$" orderNum�  5L      $ * 6-   -	   5        
col_waiter��$  waiter��5L   L   ! 6b   �� $" waiter�  5M      $ * 6-   -
   5        col_invoices��$  invoices��5M   N   ! 6f   �� $" 	invNumber�  5N      $ * 6-   -   5        
col_tables��$  table��5N   L   ! 6a   �� $" tables�  5O      $ * 6-   -   5        col_shiping��$  Shiping Company\Customer��5O   L   ! 5P      `� 5Q   "    ' #shippingCompanyAndCustomerConverter# `� 	Converter## [ 6/   .1�5R   &   �� $" shippingCompanyName�6.   �� 5S   &   $" 	agentName�6.   5T   #   5V        5W      $ * 6-   -   5        
col_status��$  status��5W   L   ! 6]   �� $" status�4	  .System.Windows.Controls.DataGridCheckBoxColumn	  5X      -   5        col_chk��$   
IsReadOnly$ .  % 	  IsThreeState% .  ! 5X   B   �� *	IsChecked+& �� NotifyOnTargetUpdated& . '   ElementStyle' 5Y      �� 5Z   "   " ��5[   )   .�5]   &   ��  � ��$ LeftToRight=�6F   �� 5^   &    � ��$ Center=�6L   -   5        5b   #   (   HeaderTemplate( 5d      �� 5e   "   .R�5f   &   �� $��2q�62   5g   #   5i      5k      5m      5n      5o      5p      