   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   .    (laundryApp.View.windows.wd_returnInvoice   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   
   -   5        
������   � AllowsTransparency  .  � WindowStyle$
 None=�5
   
    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5   	   $��0q�6   $	��140��5   
   $	��300��6   .��5      � -   5         �Name$ 	grid_main��5   +   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y    �� IsCancel . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   	 � ToolTip$	 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������
   Kind$
 Close=�6E   $��20��5   *   $��20��65   5      � 5      $��Center=�6    � ColumnDefinitions 5      �� 5       $��* �6/   �� 5!      $
��1.2* �6/   .+�5#      F� 
������� �� Stretch$
 Fill=�6,   $	��5,0q�6;   $��LeftToRight=�6H   $��Right=�5$      $��0��6;   $��Center=�5%      $��15��65   $��15��6@   ����� �     ��eR     �eR  � �    @��eR  6n �  6n�@� 6n 6n@� 6n � S 6n������@@� � � ��  d S��>������)@@�� 233333 @���  � � � @]� р�[ @]� р�[ ��j ��w  G� � �� ��*  6n ��  ��  d 	5&      �� 5(      -   5         ��Name$ 	txt_title��5(   2   $��Return��6$   
�������$
��Left=�5)   $   $��Center=�6?   $��1��5*      $	��5,0q�64   5+      5,      5-      5.      �� 5/   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��50      ���#999999
�������._�51      � -   5        $ grid_mainGrid��51       52      �� 53      $
��auto �6$   �� 54      $
��auto �6$   �� 55      $��* �6$   �� 56      $
��auto �6$   .+�58      �� $��0��6   $ 10��6&   $��10,12.5,10,5q�68   $��1q�6P     	LightGrey#��[ 6d   ._�59      � .+�5;      �� -   5         ��Name$ tb_invoiceNum��5;   "   $��0��6:   $��	10,10,5,0q�5<   #   ���#4E4E4E)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ invoice Num...��5=   #   $��0��5>   #   $��40��63   ���#737373$��0q�5?   :   $	��4,0q�6N   5A   #   F� 5B      -   5         F�Name$ p_errorInvoiceNum��5B   .   ���#E81123$
 Fill=�6I   $��5,5,15,5q�6X   $��Right=�5C      $��0��6;   $y�	Collapsed=�6H   $��Center=�5D      $��15��65   $��15��6A     
infoCircle#��[ 5E      	 5F      _� 5G   "   -   5         _�Name$ tt_errorSerialNum��5G   *   $��Name Not Validate��6E     ToolTipError#��[ 6b   5I      5L      5M      �� 5O      -	   5        $ btn_save��5P   '   $��3��5O      $��Select��6*   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6;   $��10,10q�5P      
�������#��[  5Q   .   ��6X   Z� 5R      5S      5T      5U      