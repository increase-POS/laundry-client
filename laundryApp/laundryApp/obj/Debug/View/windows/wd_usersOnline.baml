   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   ,    &laundryApp.View.windows.wd_usersOnline   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   
   -   5          � Title$  Increase laundryApp��5   	    � WindowStyle$
 None=�5
   	    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5   	   $��0q�6   $	��600��5   	   $	��800��6   
������  � AllowsTransparency . .��5      � -   5         �Name$ 	grid_main��5   *   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $	��360��6   $��30��6'   $��Center=�63    � ColumnDefinitions 5      �� 5       $��* �6/   �� 5!      $
��1.2* �6/   .+�5#      F� ���#DFDFDF �� Stretch$
 Fill=�61   $��LeftToRight=�6@   $��Right=�5$      $��0��6;   $��Center=�5%      $��20��65   $��20��6@     user#��[ 5&   #   �� 5'      -   5         ��Name$ 	txt_title��5'   8   $��Users Online��6$   
�������$
��Left=�5(   $   $��Center=�6?   $��1��5)      $	��5,0q�64   5*      5+      5-      5/      �� 50   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��51      ���#999999
�������._�52      � -   5        $ grid_mainGrid��52       53      �� 54      $��* �6$   �� 55      $��10 �6$   .+�57      � $��0��6   $��2��6%   .+�58      �� -   5         ��Name$ brd_roundDataGrid��58   F   $��
5,5,5,17.5q�6   $ 10��64   $��0q�6a   
�������59   !   &   System.Windows.Controls.DataGrid  5:      -   5          Name$ dg_usersOnline��5:      $��5q�67   $��0��6B   $��2��6O   
�������-  'MaterialDesignThemes.Wpf.DataGridAssist   CornerRadius$ 5��5;      $y�Visible=�6F      
IsReadOnly .    CanUserAddRows .     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 5>       J� OpacityMask 5?      !� 5@       !� Visual 6*   ��  �� ElementName$ brd_roundDataGrid�� � 	Resources 5B      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn �� 
TargetType �� Property �� Value��%  (��       5C   .   * SystemColors.HighlightBrushKey+)'       7 7   �� 6   6b   �� 5D      8   "  6]   .�5E   "   ��  � �� �#F5D7C46?   �� 5F   "    � ��
 �����6@   �� 5G   "    � ��$ 0 0 1 0q�6D   �� 5H   "    � ��8  6?   5I      5J         Columns 5K      0  *System.Windows.Controls.DataGridTextColumn  5L      ,  &System.Windows.Controls.DataGridColumn   Width$ 2* 6-       Header$  Account Name��6:   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6P   �� " �� Path$" usernameAccount�  5M      $ 2* 6-   $	  Pos��6:   ! 6G   �� $" posName�  5N      $ 2* 6-   $  Branch��6:   ! 6J   �� $" 
branchName�4	  .System.Windows.Controls.DataGridTemplateColumn	  5O      $ * 61   $  Online��6;   # 	  CellTemplate# 5P      �� 5Q   "   .R�5R   &   �� $��LeftToRight=�6/   $ d� 	IsChecked$ 5S   /   �� *
isOnline+% �� Mode$% OneWay=�5T   #   5V      5X      5Y      5[      5\      5]      5^      