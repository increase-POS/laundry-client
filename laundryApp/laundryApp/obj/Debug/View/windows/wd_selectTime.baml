   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   +    %laundryApp.View.windows.wd_selectTime   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � WindowStyle$
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
	 Fill=�6D   $��Center=�5      $��LeftToRight=�68   $��20��5      $��20��6$     time#��[ 5      �� 5       -   5        
 ��Name$
 	txt_title��5    D   $��1��6(   $
��time��68   $��10,0,0,0q�6W   
�������$��14(�5!      $��Center=�6(   $
��Bold"�5"      5#      �� 5&      -   5         ��Name$ 	btn_colse��5'   Y    �� IsCancel . $��Right=�5&   4   $��Center=�6P   
�������$��7.5,0q�5'   I   $��SemiBold"�5(      $��24��64   $��0q�6?    � ToolTip$ Close��6K   $��14(�6\   ��5&   k   Z� ��5'      Z� .��5)      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6I   $��20��5*   *   $��20��65   5+      5,      5.      50      �� 51      $��1,0,1,1q�6   $��1��60   $ 0,0,7,7��52      ���#999999
�������._�53      � -   5        $ grid_mainGrid��53       54      �� 55      $
��auto �6(   �� 56      $
��auto �6(   �� 57      $��* �6(   �� 58      $
��auto �6(   .+�5:      �� $��0��6   $ 10��6*   $��
10,10,10,5q�6<   $��1q�6R     	LightGrey#��[ 5;   -   ._�5<      � .+�5=      )  #MaterialDesignThemes.Wpf.TimePicker  -   5          Name$ tp_time��5=   9    ( $MaterialDesignFloatingHintTimePicker#��[ 5>      ���#737373$��0q�5?   8   $	��4,0q�6L   ���#4E4E4E$��10,0,5,0q�5B   "   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ time...��5D   !   ���#4E4E4E$��40��5F   !      SelectedTime 5A   !   �� *reservationTime+ �� Mode$ OneWay=�.  (MaterialDesignThemes.Wpf.TextFieldAssist   CharacterCounterStyle 5C   !   �� 5F   !   F� 5G      -   5        $ p_error_time��5G   #   ���#E81123$
	 Fill=�6H   $��5,5,30,5q�6W   $��Right=�5H      $��2��6;   $y�	Collapsed=�6L   $��Center=�5I      $��15��65   $��15��6A   ����� � �� Ӏ�b �� �� ��b ��  � Ӏ� �U��b ��O � ��OӀU��O��O�U��O � Ӏ�O��b �U��  � �� � � @]� Ӑ�� @]� �ا p� �ا  � ��ا  z ��ا ��q ��� ��j  � ��j �p� ��j @]� ��q @]�  z �@]�  � �@]� p� p� @]�  � @]� �@]� @f��ا @f��ا ��� �@]� ��� �@]� @f	5J      5N      5O      �� 5Q      -	   5        $ 
btn_select��5R   '   $��3��5Q      $��Select��6.   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6?   $��10,10q�5R      
�������#��[  5S   .   ��6X   Z� 5T      5U      5X      5Y      5Z      