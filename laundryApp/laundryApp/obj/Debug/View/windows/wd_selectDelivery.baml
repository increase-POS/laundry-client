   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   /    )laundryApp.View.windows.wd_selectDelivery   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � WindowStyle$
  None=�5	   
    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5
   	   $��0q�6   $	��225��5   
   $	��300��6   
������  � AllowsTransparency . .��5      �� $��1,0,1,1q�6   $��1��6(    �� CornerRadius$ 7,7,7,7��5      ���#F1F5F8._�5   
   � -   5         �Name$ 	grid_main��5       � RowDefinitions 5      �� 5      $��30 �6    �� 5      $��* �6    �� 5      $
��auto �6    .+�5      � $��2��6   .+�5      �� $��0q�6   $��0��6.   $ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� $��Center=�6%    �� Orientation$ 
Horizontal=�6B   .+�5      F� -   5         F�Name$ 
path_title��5   #   
�������	 �� Stretch$
	 Fill=�6D   $��Center=�5      $��LeftToRight=�68   $��20��5      $��20��6$     delivery#��[ 5      �� 5       -   5        
 ��Name$
 	txt_title��5    H   $��1��6(   $��delivery��68   $��10,0,0,0q�6[   
�������$��14(�5!      $��Center=�6(   $
��Bold"�5"      5#      �� 5&      -   5         ��Name$ 	btn_colse��5'   Y    �� IsCancel . $��Right=�5&   4   $��Center=�6P   
�������$��7.5,0q�5'   I   $��SemiBold"�5(      $��24��64   $��0q�6?    � ToolTip$ Close��6K   $��14(�6\   ��5&   k   Z� ��5'      Z� .��5)      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6I   $��20��5*   *   $��20��65   5+      5,      5.      50      �� 51      $��1,0,1,1q�6   $��1��60   $ 0,0,7,7��52      ���#999999
�������._�53      � -   5        $ grid_mainGrid��53       54      �� 55      $
��auto �6(   �� 56      $
��auto �6(   �� 57      $
��auto �6(   �� 58      $��* �6(   �� 59      $
��auto �6(   .+�5;      �� $��0��6   $��40��6*   $ 10��66   $��
10,10,10,5q�6H   $��1q�6^     	LightGrey#��[ 6r   ._�5<      � .+�5=      �� -   5         ��Name$ 
cb_company��5=   '   $��0��6<    & "MaterialDesignFloatingHintComboBox#��[ 5>   '   $��10,0,5,0q�5?   #   ���#4E4E4E)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Company...��5@   #   $��0��5A   #   $��40��63   ���#737373$��0q�5B   9   $	��4,0q�6M   $	��Top=�5D   #   F� 5E      -   5        $ p_error_company��5E   2   ���#E81123$
	 Fill=�6K   $��5,5,20,5q�6Z   $��Right=�5F      $��0��6;   $y�	Collapsed=�6I   $��Center=�5G      $��15��65   $��15��6A     
infoCircle#��[ 5H      5K      5L      �� 5M      -	   5         ��Name$ brd_user��5M      $��1��60   $y�	Collapsed=�6=   $��40��6T   $ 10��6`   $��	10,5,10,5q�6r   $��1q�6�   #��[ 6�   ._�5N      � .+�5O      �� -
   5        $ cb_user��5O   '   $��1��69   #��[ 5P   '   $��10,0,5,0q�5Q   #   ���#4E4E4E$ User...��5R   #   $y�Visible=�5S   "   $��0��5T   #   $��40��63   ���#737373$��0q�5U   ?   $	��4,0q�6S   F� 5V      -   5        $ p_error_user��5V   2   ���#E81123$
	 Fill=�6H   $��5,5,20,5q�6W   $��Right=�5W      $��1��6;   $y�	Collapsed=�6H   $��Center=�5X      $��15��65   $��15��6A   #��[ 5Y      5Z      5[      �� 5]      $��2��6!   $ 
Horizontal=�6.   $
��Left=�6G   $��Center=�6b   $��	10,0,10,0q�5^   %   .+�5_      �� -   5        $
 txt_deliveryCostTitle��5_   M   $��0��6(   $��Delivery cost: ��66   ���#4E4E4E$��Center=�5a   #   $
��Left=�5b   (   $��10,5,5,5q�6C   �� 5d      -   5        $
 tb_deliveryCost��5d   7   $��1��6(   $��5q�6Q   ���#4E4E4E$��0��5e   -   $
��Left=�5f      $��Center=�64   $
��Bold"�5g   #   $��12(�66   �� 5i      -   5        $
 tb_moneyIconDeliveryCost��5i   7   $��1��6(   $��1��6Y   $��0q�6i   ���#4E4E4E$
��Left=�5k      $��Center=�64   $
��Bold"�5l   #   $��12(�66   ��5j   -   ��  �� Path$ MainWindow.Currency� �� Mode$ OneWay=�5l   6   5m      �� 5o      -   5        $ 
btn_select��5p   '   $��10��5o      $��Select��6/   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6@   $��10,10q�5p      
�������#��[  5q   .   ��6X   Z� 5r      5s      5v      5w      5x      