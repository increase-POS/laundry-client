   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   .    (laundryApp.View.windows.wd_notifications   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   -   5          � WindowStyle$
  None=�5	   	    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5
   	   $��0q�6   $	��600��5   	   $	��800��6   
������  � Title$ Increase laundryApp��5   	    � AllowsTransparency . .��5      � -   5         �Name$ 	grid_main��5   *   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $	��360��6   $��30��6'   $��Center=�63    � ColumnDefinitions 5      �� 5       $��* �6/   �� 5!      $
��1.2* �6/   .+�5#      F� ���#DFDFDF �� Stretch$
 Fill=�61   $��LeftToRight=�6@   $��Right=�5$      $��0��6;   $��Center=�5%      $��20��65   $��20��6@   �����5� �
Ӑ>��O
K)p�
��b�X�	� ����r	 �=� �=� �]���(ll@P�s ���upN��[vӰy�P�0gffff^m@�z�GQQ@`����K�p\P����Q���P@�Q���P@SP�lr=
ף0=@@^�43333�5@(\���Lr@���(\O5@�23333�r@��?Q���s@�z�G�;@(\����s@*\���hE@�G�zBu@ ��Q��I@ ���Q��{v@43333c[@��(\�vv@��Q�^@effffzv@����Q��{v@�u�Q��{v@gffff�b@23333_v@�/P		ףp=�v@�a�	��z�G�v@Г 
23333w@ $H
23333gw@>
ףpf@�������w@0?�
z�G�w@P�����̤w@���Q�g@Q@u���Q�g@��|`P��
��q ��
�5� �
p�����(\�
i@Q��e��(\�
i@S��Zz�G�Bj@0���(\��Uk@@{�����\k@������ ��	5&   #   �� 5'      -   5         ��Name$ 	txt_title��5'   9   $��Notifications��6$   
�������$
��Left=�5(   $   $��Center=�6?   $��1��5)      $	��5,0q�64   5*      5+      5-      5/      �� 50   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��51      ���#999999
�������._�52      � -   5        $ grid_mainGrid��52       53      �� 54      $��* �6$   �� 55      $��10 �6$   .+�58      � $��0��6   $��2��6%   .+�59      �� -   5         ��Name$ brd_roundDataGridOrders��59   V   $��
5,5,5,17.5q�6!   $��1��67   $ 10��6D   $��0q�6w   
�������5:      &   System.Windows.Controls.DataGrid  5;      -   5          Name$ dg_notifications��5;      $��5q�69   $��0��6D   $��2��6Q   
�������-  'MaterialDesignThemes.Wpf.DataGridAssist   CornerRadius$ 5��5<      $y�Visible=�6F      
IsReadOnly .    CanUserAddRows .     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 5?       J� OpacityMask 5@      !� 5A       !� Visual 6*   ��  �� ElementName$ brd_roundDataGridOrders�� � 	Resources 5C      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn �� 
TargetType �� Property �� Value��%  (��       5D   .   * SystemColors.HighlightBrushKey+)'       7 7   �� 6   6b   �� 5E      8   "  6]   .�5F   "   ��  � �� �#F5D7C46?   �� 5G   "    � ��
 �����6@   �� 5H   "    � ��$ 0 0 1 0q�6D   �� 5I   "    � ��8  6?   5J      5K         Columns 5L      0  *System.Windows.Controls.DataGridTextColumn  5M      ,  &System.Windows.Controls.DataGridColumn   Width$ * 61       Header$  Title��6=   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6L   �� " �� Path$" title�  notificationsTitleConverter# �� 	Converter## [   5N      $ 2* 61   $  Content��6=   ! 6N   �� $" ncontent� ! notificationsContentConverter## [   5O      $ * 61   $
  Date��6<   ! 6J   �� $" 
createDate�  dateFrameConverter## [ 5Q      5R      5T      5U      5V      5W      