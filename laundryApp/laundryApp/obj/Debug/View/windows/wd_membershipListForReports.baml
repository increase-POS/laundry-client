   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   9    3laundryApp.View.windows.wd_membershipListForReports   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   -   5          � WindowStyle$
  None=�5	   	    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5
   	   $��0q�6   $	��400��5   	   $	��700��6   
������  � Title$ 
Membership��5   	    � AllowsTransparency . .��5      � -   5         �Name$ 	grid_main��5   *   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $	��360��6   $��30��6'   $��Center=�63    � ColumnDefinitions 5      �� 5       -   5         ��Name$ 
col_coupon��5    /   $��* �6D   �� 5!      -   5        $ 	col_offer��5!   /   $��* �6D   �� 5"      -   5        $ col_invoice��5"   /   $��* �6D   �� 5#      $
��1.2* �6/   .+�5%      F� ���#DFDFDF �� Stretch$
 Fill=�61   $��LeftToRight=�6@   $��Right=�5&      $��0��6;   $��Center=�5'      $��20��6:   $��20��6E    
 coupon#��[ 5(   #   F� 5)      ���#DFDFDF$
 Fill=�61   $��LeftToRight=�6@   $��Right=�5*      $��1��6;   $��Center=�5+      $��20��6:   $��20��6E    	 offer#��[ 5,   #   F� 5-      ���#DFDFDF$
 Fill=�61   $��LeftToRight=�6@   $��Right=�5.      $��2��6;   $��Center=�5/      $��20��6:   $��20��6E     invoice#��[ 50   #   �� 51      -   5         ��Name$ 	txt_title��51   6   $��
membership��6$   
�������$
��Left=�52   $   $��Center=�6?   $��3��53   #   $	��5,0q�68   54      55      57      59      �� 5:   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��5;      ���#999999
�������._�5<      � -   5        $ grid_mainGrid��5<       5=      �� 5>      $��* �6$   �� 5?      $��10 �6$   .+�5A      �� -	   5         ��Name$ brd_roundDataGrid��5A   E   $��0q�6   $��0��6&   $ 10��63   $��0q�6`   
�������5B      &   System.Windows.Controls.DataGrid  5C      -
   5          Name$ dg_memberships��5C      $��5q�63   $��0��6>   $��2��6K   
�������-  'MaterialDesignThemes.Wpf.DataGridAssist   CornerRadius$ 5��5D      $y�Visible=�6F      
IsReadOnly .    CanUserAddRows .     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 5G       J� OpacityMask 5H      !� 5I       !� Visual 6&   ��  �� ElementName$ brd_roundDataGrid�� � 	Resources 5K      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn �� 
TargetType �� Property �� Value��%  (��       5L   *   * SystemColors.HighlightBrushKey+)'       7 7   �� 6   6^   �� 5M      8   "  6Y   .�5N      ��  � �� �#F5D7C46;   �� 5O       � ��
 �����6<   �� 5P       � ��$ 0 0 1 0q�6@   �� 5Q       � ��8  6;   5R      5S         Columns 5T      0  *System.Windows.Controls.DataGridTextColumn  5V      -   5        	col_cCode��,  &System.Windows.Controls.DataGridColumn    Width$  * 5V   B   !   Header$
! Code��6M   1  +System.Windows.Controls.DataGridBoundColumn"   Binding" 6e   �� # �� Path$# 
couponCode�  5W      -   5        	col_cName��$  * 5W   B   $
! Name��6M   " 6e   �� $
# name�  5X      -   5        col_cTypeValue��$  * 5X   E   $! Value��6O   $   
Visibility$$ Hidden=�6^   " 5Y      `� 5Z   "     accuracyDiscountConverter% `� 	Converter#% [ 6/   .1�5[   &   �� $# discountType�6.   �� 5\   &   $# discountValue�6.   5]   #   5_        5a      -   5        	col_oName��$  * 5a   B   $
! Name��6M   " 6e   �� $# 	offerName�  5b      -   5        col_oTypeValue��$  * 5b   E   $! Value��6O   $$ Hidden=�6^   " 5c      `� 5d   "   #% [ 6/   .1�5e   &   �� $# 	offerType�6.   �� 5f   &   $# 
offerValue�6.   5g   #   5i        5j      -   5        col_oQuantity��$  * 5j   F   $
! Code��6Q   " 6i   �� $# quantity�  5l      -   5        	col_iName��$  * 5l   B   $
! Name��6M   " 6e   �� $
# name�  5m      -   5        col_iTypeValue��$  * 5m   E   $! Value��6O   $$ Hidden=�6^   " 5n      `� 5o   "   #% [ 6/   .1�5p   &   �� $# discountType�6.   �� 5q   &   $# discountValue�6.   5r   #   5t        5v      -   5        col_discount��$  * 5v   E   $! Discount��6P   " 6f   �� $# finalDiscount�  accuracyConverter& �� 	Converter#& [ 5x      5z      5{      5|      5}      