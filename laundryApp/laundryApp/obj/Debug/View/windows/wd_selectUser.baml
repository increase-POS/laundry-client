   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   +    %laundryApp.View.windows.wd_selectUser   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � WindowStyle$
  None=�5	   
    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5
   	   $��0q�6   $	��140��5   
   $	��300��6   
������  � AllowsTransparency . .��5      �� $��1,0,1,1q�6   $��1��6(    �� CornerRadius$ 7,7,7,7��5      ���#F1F5F8._�5   
   � -   5         �Name$ 	grid_main��5       � RowDefinitions 5      �� 5      $��30 �6    �� 5      $��* �6    �� 5      $
��auto �6    .+�5      � $��2��6   .+�5      �� $��0q�6   $��0��6.   $ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� $��Center=�6%    �� Orientation$ 
Horizontal=�6B   .+�5      F� -   5         F�Name$ 
path_title��5   #   
�������	 �� Stretch$
	 Fill=�6D   $��Center=�5      $��LeftToRight=�68   $��20��5      $��20��6$     user#��[ 5      �� 5       -   5        
 ��Name$
 	txt_title��5    D   $��1��6(   $
��User��68   $��10,0,0,0q�6W   
�������$��14(�5!      $��Center=�6(   $
��Bold"�5"      5#      �� 5&      -   5         ��Name$ 	btn_colse��5'   Y    �� IsCancel . $��Right=�5&   4   $��Center=�6P   
�������$��7.5,0q�5'   I   $��SemiBold"�5(      $��24��64   $��0q�6?    � ToolTip$ Close��6K   $��14(�6\   ��5&   k   Z� ��5'      Z� .��5)      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6I   $��20��5*   *   $��20��65   5+      5,      5.      50      �� 51      $��1,0,1,1q�6   $��1��60   $ 0,0,7,7��52      ���#999999
�������._�53      � -   5        $ grid_mainGrid��53       54      �� 55      $
��auto �6(   �� 56      $
��auto �6(   �� 57      $��* �6(   �� 58      $
��auto �6(   .+�5:      �� $��0��6   $ 10��6*   $��
10,10,10,5q�6<   $��1q�6R     	LightGrey#��[ 6f   ._�5;      � .+�5<      �� -   5         ��Name$ 	cb_userId��5<   '   $��10,0,5,0q�5=   #   ���#4E4E4E)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ user...��5>   #    & "MaterialDesignFloatingHintComboBox#��[ 5?   $   $��0��5@   $   $��40��64   $	��4,0q�6A   ���#737373$��0q�5A   9   F� 5B      -   5        $ p_error_userId��5B   2   ���#E81123$
	 Fill=�6J   $��5,5,20,5q�6Y   $��Right=�5C      $��8��6;   $y�	Collapsed=�6I   $��Center=�5D      $��15��65   $��15��6A     
infoCircle#��[ 5E      5F      5G      �� 5I      -	   5        $ 
btn_select��5J   $   $��3��5I      $��Select��6.   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6?   $��10q�5J      
�������#��[  5K   .   ��6X   Z� 5L      5M      5P      5Q      5R      