   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   &     laundryApp.View.windows.wd_notes   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   -   5          � WindowStyle$
  None=�5	   
    � WindowStartupLocation$ CenterScreen=�6    � Title$ Increase laundryApp��5
   	    � 
ResizeMode$ NoResize=�5   	   $��0q�6   $	��250��5   	   $	��300��6   
������  � AllowsTransparency . .��5      � -   5         �Name$ 	grid_main��5   +   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $��Center=�6    � ColumnDefinitions 5      �� 5       $��* �6/   �� 5!      $
��1.2* �6/   .+�5#      F� 
������� �� Stretch$
 Fill=�6,   $	��5,0q�6;   $��LeftToRight=�6H   $��Right=�5$      $��0��6;   $��Center=�5%      $��20��66   $��20��6A    	 notes#��[ 5&      �� 5'      -   5         ��Name$ 	txt_title��5'   0   $
��note��6$   
�������$
��Left=�5(   $   $��Center=�6?   $��1��5)      $	��5,0q�64   5*      5+      5,      5-      �� 5.   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��5/      ���#999999
�������._�50      � $��5q�6    51      �� 52      $
��auto �6$   �� 53      $��* �6$   �� 54      $
��auto �6$   .+�56      �� $��0��6   $ 10��6&   $��	10,5,10,5q�68   $��1q�6M     	LightGrey#��[ 6a   ._�57      � .+�58      �� -   5         ��Name$ tb_notes��58   #    % !MaterialDesignFloatingHintTextBox#��[ 59   #   $��10,0,5,0q�5:   #   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Note...��5<   #   ���#4E4E4E$	��4,0q�5>   "   $��0q�60   $��1��5?      $	��150��6.    �� TextWrapping$
 Wrap=�6;   ��58   5   �� *notes+ �� Mode$ OneWay=�.  (MaterialDesignThemes.Wpf.TextFieldAssist   CharacterCounterStyle 5;   "   �� 5?   ;   5@      5A      �� 5D      -   5        $ btn_save��5E   (   $��5��5D      $
��save��6*   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��69   $��10,2.5q�5E      
�������#��[  5F      ��6E   Z� 5G      5H      5I      5J      