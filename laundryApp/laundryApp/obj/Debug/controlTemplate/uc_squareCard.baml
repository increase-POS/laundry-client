   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:laundryApp.controlTemplatelaundryApp.controlTemplate  5      .    (laundryApp.controlTemplate.uc_squareCard   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:laundryApp.controlTemplate     5      -   5          � 	Resources  5	         Storyboard1 �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value��%�   &	        �� 5
   
   .��5      U� $ (UIElement.Opacity)�6,   $ 	rectangle��6\   .\�5        $ 0��6'   $ 0Q�63     5      $ 0:0:0.1��6'   $	 0.3Q�69   5      5      5       � Triggers 5      :� 5   
    :� RoutedEvent$ UIElement.MouseEnter��6    :� 
SourceName$ 	rectangle��6:   .[�5      �� -   5         ��Name$! Storyboard1_BeginStoryboard��5      #��[  6C   5      :� 5   
   $ UIElement.MouseLeave��6   $ 	rectangle��6:   .[�5      �� 	 �� BeginStoryboardName$!	 Storyboard1_BeginStoryboard��6   5      .��5      � -   5        
 �Name$
 	grid_main��5       � ForceCursor .  � Cursor$
 Hand��63   $��5q�6A    � RowDefinitions 5   
   �� 5      -   5         ��Name$ grid_mainRow��5      $��* �62   �� 5      $��22 �6   �� 5      $��18 �6   .+�5    
   �� -   5         ��Name$ brd_main��5!   $    �� CornerRadius$ 10��5       $	��1.2q�5!       �� Padding$ 0q�66   
���������5    $   �� *squareCardBorderBrush+5"      �� 5#   
   -   5         ��Name$ 	rectangle��5#      ���	#99F0F8FF J� Opacity$ 0Q�5$   )    �� RadiusX$ 10��5%       �� RadiusY$ 10��6%   5'      5(      