   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   +    %laundryApp.View.windows.wd_taxSetting   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   -   5          � WindowStyle$
  None=�5	   
    � WindowStartupLocation$ CenterScreen=�6    � Title$ Increase POS��5
   	    � 
ResizeMode$ NoResize=�5   	   $��0q�6   $	��200��5   
   $	��300��6   
������  � AllowsTransparency . .��5      � -   5         �Name$ 	grid_main��5   +   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $��Center=�6    � ColumnDefinitions 5      �� 5      $��* �6/   �� 5       $
��1.2* �6/   .+�5"      F� 
������� �� Stretch$
 Fill=�6,   $	��5,0q�6;   $��LeftToRight=�6I   $��Right=�5#      $��0��6;   $��Center=�5$      $��20��66   $��20��6A     tax#��[ 5%      �� 5&      -   5         ��Name$ 	txt_title��5&   /   $	��Tax��6$   
�������$
��Left=�5'   $   $��Center=�6?   $��1��5(      $	��5,0q�64   5)      5*      5+      5,      �� 5-   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��5.      ���#999999
�������._�5/      � $��5q�6    50      �� 51      $
��auto �6$   �� 52      $
��auto �6$   �� 53      $��* �6$   �� 54      $
��auto �6$    56      �� 57      $
��auto �6'   �� 58      $
��auto �6'   �� 59      $��* �6'   .+�5;      �� -   5        $ txt_invoiceTax��5<      $��0��5;      $��0��6,   $��Invoice��5<   6   $��5,0,5,0q�6F   ���#6e6e6e$
��Left=�5=      $��Center=�68   d� 5>      -   5         d�Name$ tgl_invoiceTax��5?       $��0��5>       $��1��6/   $��0,0,5,0q�6@   $��Center=�5@       $��	10,15,5,5q�6;   $��13��5A       $��28��6,    d� 	IsChecked 5B       �� *invoiceTax_bool+ �� Mode$ OneWay=�5C   6   �� 5D      $��0��6   $��2��6&   $��40��66   $ 10��6B   $��	10,5,10,5q�6T   $��1q�6i     	LightGrey#��[ 6}   ._�5E      � .+�5F      �� -   5         ��Name$ tb_invoiceTax��5F   &   $��	10,10,5,5q�6=   $��0��5G      $��2��6,   ���#4E4E4E���#737373$��0q�5I   5   $	��4,0q�6I   {�.  ��5K      �� *invoiceTax_decimal+$ OneWay=�.��5O   C   5P      F� 5Q      -   5         F�Name$ p_error_invoiceTax��5Q   T   ���#E81123$
 Fill=�62   $��5,5,15,5q�6A   $��0��5R   #   $��2��62   $��Right=�5S   #   $y�	Collapsed=�6B   $��Center=�5T   #   $��15��6>   $��15��6J   ����� � �� Ӏ�b �� �� ��b ��  � Ӏ� �U��b ��O � ��OӀU��O��O�U��O � Ӏ�O��b �U��  � �� � � @]� Ӑ�� @]� �ا p� �ا  � ��ا  z ��ا ��q ��� ��j  � ��j �p� ��j @]� ��q @]�  z �@]�  � �@]� p� p� @]�  � @]� �@]� @f��ا @f��ا ��� �@]� ��� �@]� @f	
 5V   "   _� 5W   &   -	   5         _�Name$ tt_errorInvoiceTax��5W   /     ToolTipError#��[ 6L   5Y      5c      5d      �� 5f      -
   5        $ txt_itemsTax��5g      $��1��5f      $��0��6,   $��Items��5g   4   $��5,0,5,0q�6B   ���#6e6e6e$
��Left=�5h      $��Center=�68   d� 5i      -   5        $ tgl_itemsTax��5j       $��1��5i       $��1��6/   $��0,0,5,0q�6A   $��Center=�5k       $��	10,15,5,5q�6;   $��13��5l       $��28��6,    5m       �� *itemsTax_bool+$ OneWay=�5n   7   � 5o      $��1��6   $��2��6'   $y�Hidden=�67   .+�5q      �� -   5        $ tb_itemsTax��5q   !   $��	10,10,5,5q�66   $��0,0,0,2q�5r   !   ���#4E4E4E���#f8f8f8
�������$��5q�5u   !   $��2��6,   {�.  ��5v   !   �� *itemsTax_decimal+$ OneWay=�.��5z   G   5|      F� 5}      -   5        $ p_error_itemsTax��5}   M   ���#E81123$
 Fill=�6*   $��
0,15,15,15q�69   $��Right=�5~   #   $��0��6@   $y�	Collapsed=�6N   $��Center=�5   #   $��15��6>   $��15��6J   ����� � �� Ӏ�b �� �� ��b ��  � Ӏ� �U��b ��O � ��OӀU��O��O�U��O � Ӏ�O��b �U��  � �� � � @]� Ӑ�� @]� �ا p� �ا  � ��ا  z ��ا ��q ��� ��j  � ��j �p� ��j @]� ��q @]�  z �@]�  � �@]� p� p� @]�  � @]� �@]� @f��ا @f��ا ��� �@]� ��� �@]� @f	
 5�      _� 5�      -   5        $ tt_errorItemsTax��5�   '   #��[ 6B   5�      5�      �� 5�      -   5        $ btn_save��5�   &   $��5��5�      $��3��6(   $
��save��6=   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6L   $
��10,5q�5�      
�������#��[  5�      ��6E   Z� 5�      5�      5�      5�      