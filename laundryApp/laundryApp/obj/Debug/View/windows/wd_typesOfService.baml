   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   /    )laundryApp.View.windows.wd_typesOfService   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   -   5          � Title$  Increase laundryApp��5	   	    � WindowStyle$
 None=�5
   
    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5   	   $��0q�6   $	��227��5   
   $	��300��6   
������  � AllowsTransparency . .��5      � -   5         �Name$ 	grid_main��5   +   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $��Center=�6    � ColumnDefinitions 5      �� 5       $��* �6/   �� 5!      $
��1.2* �6/   .+�5#      F� 
������� �� Stretch$
 Fill=�6,   $	��5,0q�6;   $��LeftToRight=�6H   $��Right=�5$      $��0��6;   $��Center=�5%      $��20��66   $��20��6A     
diningHall#��[ 5&      �� 5'      -   5         ��Name$ 	txt_title��5'   :   $��typesOfService��6$   
�������$
��Left=�5(   $   $��Center=�6?   $��1��5)      $	��5,0q�64   5*      5+      5,      5-      �� 5.   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��5/      ���#999999
�������._�50      � $��5q�6    51      �� 52      $
��auto �6$   �� 53      $
��auto �6$   �� 54      $
��auto �6$   �� 55      $��* �6$   �� 56      $
��auto �6$    58      �� 59      �� 5:      $
��auto �6'   .+�5<      �� -   5        $ txt_diningHall��5<      $��0��64   $��0��6A   $��10q�5=   !   $��
diningHall��6-   ���#6e6e6e$
��Left=�5>      $��Center=�68   $��12(�5?   #   d� 5@      -   5         d�Name$ tgl_diningHall��5@      $��0��67   $��1��6D    d� 	IsChecked$
 TrueY�6W   $��Center=�5A   "   $��10,0,5,0q�6=   $��10q�5B   /   �� 5C      -   5        $ txt_takeAway��5C      $��1��62   $��0��6?   $��10q�5D      $��takeAway��6+   ���#6e6e6e$
��Left=�5E      $��Center=�68   d� 5F      -   5        $ tgl_takeAway��5F      $��1��65   $��1��6B   $
 TrueY�6U   $��Center=�5G   "   $��10,0,5,0q�6=   $��10q�5H   /   �� 5I      -	   5        $ txt_selfService��5I      $��2��65   $��0��6B   $��10q�5J   !   $��selfService��6-   ���#6e6e6e$
��Left=�5K      $��Center=�68   $��12(�5L   #   d� 5M      -
   5        $ tgl_selfService��5M      $��2��68   $��1��6E   $
 TrueY�6X   $��Center=�5N   "   $��10,0,5,0q�6=   $��10q�5O   .   �� 5Q      -   5        $ btn_save��5R   '   $��6��5Q      $��2��6(   $
��save��6>   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6M   $��10,10q�5R      
�������#��[  5S      ��6E   Z� 5T      5V      5Y      5[      