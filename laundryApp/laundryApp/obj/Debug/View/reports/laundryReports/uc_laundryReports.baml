   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:laundryApp.View.reports.accountsReports'laundryApp.View.reports.accountsReports  5      >    8laundryApp.View.reports.kitchenReports.uc_laundryReports   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Glocal5clr-namespace:laundryApp.View.reports.accountsReports     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        
  � Tag$  kitchenReports��5      .��5      � -   5         �Name$ 	grid_main��5      .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $��* �6    .+�5      �� -   5         ��Name$ txt_mainTitle��5      $��kitchenReports��6/   $��40 15q�6F   $��24(�6U   $
��Bold"�6c      	LightGrey#��[  6u   �� 5      $��1��6   $��5,0,-5,0q�6(   .��5      � $��20 15q�6   $
��Left=�6+   .+�5      �� $#  preparingOrdersKitchenReports��6   
������� �� CornerRadius$	 7.5��6T   $	��145��6g   $	��260��6u   $��15 10q�6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5      	   Opacity$		 0.3Q�5      
   
BlurRadius$
 10Q�5      ._�5      �  � ColumnDefinitions 5       �� 5!   "   $
��auto �63   6B   �� 5"   "   $
��auto �63   6B   �� 5#   "   $��3* �63   6@   .+�5&      �� $ 7 0 0 7��6%     	MainColor#��[ 6>   $	��145��6f   $��10��6s   F� 5'      -   5         F�Name$, &path_preparingOrdersKitchenReportsIcon��5'   #   $#  preparingOrdersKitchenReports��6S     LightMainColor#��[ 6w    �� Stretch$
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5(   #   $��1��6@   $��Center=�5)   #   $��50��6>   $��50��6J   $��LeftToRight=�6U     cooking#��[ 5*   #   �� 5+      $��2��6)   $	��Top=�69   $��2��6R   $��5,20,0,0q�6f   $
��Left=�6x   .+�5,   "   �� -   5        $+ %txt_preparingOrdersKitchenReportsInfo��5,   ,   $#  preparingOrdersKitchenReports��6[   $#��preparingOrdersKitchenReports��6   $	��165��6�   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5-   "   -   5        $+ %txt_preparingOrdersKitchenReportsHint��5-   ,   $#  preparingOrdersKitchenReports��6[   $��Branch, Category, ...��6   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5.      �� 5/      -   5         ��Name$' !btn_preparingOrdersKitchenReports��51   3   $#  preparingOrdersKitchenReports��5/   %   $��30��6I   $��90��6U   $��2��6a   $��Bottom=�6r   $��	0,0,10,10q�6�   #��[ 50   !   $��Right=�6I   $��0q�51      $��0q�52      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6+   $��Center=�53      $��Center=�6>   .��54   "   ��  �� Orientation$ 
Horizontal=�60   .+�55   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 57   &    	 White#��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�58      $��Center=�59      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	 J� RenderTransformOrigin$ 0.5,0.5&�5:   b   F� 5;   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5<      $��Center=�5=      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5>   b   5?   #   5@      5A      5B      �� 5E      $$  spendingRequestsKitchenReports��6   
�������$	 7.5��6U   $	��145��6h   $	��260��6v   $��15 10q�6�    5F        5G      $ 1Q�5H      $	 360Q�5I      $
 Gray��5J      $		 0.3Q�5K      $
 10Q�5L      ._�5N      �  5O      �� 5P   "   $
��auto �63   6B   �� 5Q   "   $
��auto �63   6B   �� 5R   "   $��3* �63   6@   .+�5U      �� $ 7 0 0 7��6%   #��[ 6>   $	��145��6f   $��10��6s   F� 5V      -   5        $- 'path_spendingRequestsKitchenReportsIcon��5V   #   $$  spendingRequestsKitchenReports��6T   #��[ 6y   $
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5W   #   $��1��6@   $��Center=�5X   #   $��50��6>   $��50��6J   $��LeftToRight=�6U     freezer#��[ 5Y   #   �� 5Z      $��2��6)   $	��Top=�69   $��2��6R   $��5,20,0,0q�6f   $
��Left=�6x   .+�5[   "   �� -	   5        $, &txt_spendingRequestsKitchenReportsInfo��5[   ,   $$  spendingRequestsKitchenReports��6\   $$��spendingRequestsKitchenReports��6�   $	��165��6�   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5\   "   -
   5        $, &txt_spendingRequestsKitchenReportsHint��5\   ,   $$  spendingRequestsKitchenReports��6\   $��Invoice, Item...��6�   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5]      �� 5^      -   5        $( "btn_spendingRequestsKitchenReports��5`   9   $$  spendingRequestsKitchenReports��5^   %   $��30��6J   $��90��6V   $��2��6b   $��Bottom=�6s   $��	0,0,10,10q�6�   #��[ 5_   %   $��Right=�6M   $��0q�5`   %   $��0q�5a   %   $ 7��62   $��Center=�5b   %   $��Center=�6I   .��5c   "   �� $ 
Horizontal=�60   .+�5d   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 5f   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�6Y   $��Center=�5g   ,   $
��12.5��6G   $��10��6U   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5h   o   F� 5i   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5j   ,   $��Center=�6>   $
��12.5��6Y   $��10��6g   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5k   o   5l   #   5m      5n      5o      �� 5r      $  consumptionKitchenReports��6   
�������$	 7.5��6P   $	��145��6c   $	��260��6q   $��15 10q�6    5s        5t      $ 1Q�5u      $	 360Q�5v      $
 Gray��5w      $		 0.3Q�5x      $
 10Q�5y      ._�5{      �  5|      �� 5}   "   $
��auto �63   6B   �� 5~   "   $
��auto �63   6B   �� 5   "   $��3* �63   6@   .+�5�      �� $ 7 0 0 7��6%   #��[ 6>   $	��145��6f   $��10��6s   F� 5�      -   5        $( "path_consumptionKitchenReportsIcon��5�   #   $  consumptionKitchenReports��6O   #��[ 6o   $
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5�   #   $��1��6@   $��Center=�5�   #   $��50��6>   $��50��6J   $��LeftToRight=�6U     rawMaterials#��[ 5�   #   �� 5�      $��2��6)   $	��Top=�69   $��2��6R   $��5,20,0,0q�6f   $
��Left=�6x   .+�5�   "   �� -   5        $' !txt_consumptionKitchenReportsInfo��5�   ,   $  consumptionKitchenReports��6W   $��consumptionKitchenReports��6w   $	��165��6�   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5�   "   -   5        $( "txt_consumptionsKitchenReportsHint��5�   ,   $  consumptionKitchenReports��6X   $��Invoice, Item...��6x   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5�      �� 5�      -   5        $# btn_consumptionKitchenReports��5�   9   $  consumptionKitchenReports��5�   %   $��30��6E   $��90��6Q   $��2��6]   $��Bottom=�6n   $��	0,0,10,10q�6�   #��[ 5�   %   $��Right=�6M   $��0q�5�   %   $��0q�5�   %   $ 7��62   $��Center=�5�   %   $��Center=�6I   .��5�   "   �� $ 
Horizontal=�60   .+�5�   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�6Y   $��Center=�5�   ,   $
��12.5��6G   $��10��6U   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   o   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5�   ,   $��Center=�6>   $
��12.5��6Y   $��10��6g   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   o   5�   #   5�      5�      5�      5�      5�      5�      5�      5�      