   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:laundryApp.View.reports.purchaseReports'laundryApp.View.reports.purchaseReports  5      @    :laundryApp.View.reports.purchaseReports.uc_purchaseReports   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Glocal5clr-namespace:laundryApp.View.reports.purchaseReports     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        
  � Tag$  purchaseReports��5      .��5      � -   5         �Name$ 	grid_main��5      .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $��* �6    .+�5      �� -   5         ��Name$ txt_mainTitle��5      $��purchaseReports��6/   $��40 15q�6G   $��24(�6V   $
��Bold"�6d      	LightGrey#��[  6v   �� 5      $��1��6   $��5,0,-5,0q�6(   .��5      � $��20 15q�6   $
��Left=�6+   .+�5      �� $  invoicePurchaseReports��6   
������� �� CornerRadius$	 7.5��6M   $	��145��6`   $	��260��6n   $��15 10q�6|    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5      	   Opacity$		 0.3Q�5      
   
BlurRadius$
 10Q�5      ._�5      �  � ColumnDefinitions 5       �� 5!   "   $
��auto �63   6B   �� 5"   "   $
��auto �63   6B   �� 5#   "   $��3* �63   6@   .+�5&      �� $ 7 0 0 7��6%     	MainColor#��[ 6>   $	��145��6f   $��10��6s   F� 5'      -   5         F�Name$% path_invoicePurchaseReportsIcon��5'   #   $  invoicePurchaseReports��6L     LightMainColor#��[ 6i    �� Stretch$
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5(      $��1��6<   $��Center=�5)      $��50��65   $��50��6A   $��LeftToRight=�5*        invoice#��[ 5+      �� 5,      $��2��6)   $	��Top=�69   $��2��6R   $��5,20,0,0q�6f   $
��Left=�6x   .+�5-   "   �� -   5        $$ txt_invoicePurchaseReportsInfo��5-   ,   $  invoicePurchaseReports��6T   $��invoicePurchaseReports��6q   $	��165��6�   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5.   "   -   5        $$ txt_invoicePurchaseReportsHint��5.   ,   $  invoicePurchaseReports��6T   $��invoicePurchaseReports��6q   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5/      �� 50      -   5         ��Name$  btn_invoicePurchaseReports��52   3   $  invoicePurchaseReports��50   %   $��30��6B   $��90��6N   $��2��6Z   $��Bottom=�6k   $��	0,0,10,10q�6�   #��[ 51   !   $��Right=�6I   $��0q�52      $��0q�53      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6+   $��Center=�54      $��Center=�6>   .��55   "   ��  �� Orientation$ 
Horizontal=�60   .+�56   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 58   &    	 White#��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�59      $��Center=�5:      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	 J� RenderTransformOrigin$ 0.5,0.5&�5;   b   F� 5<   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5=      $��Center=�5>      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5?   b   5@   $   5A      5B      5C      �� 5F      $  itemPurchaseReports��6   
�������$	 7.5��6J   $	��145��6]   $	��260��6k   $��15 10q�6y    5G        5H      $ 1Q�5I      $	 360Q�5J      $
 Gray��5K      $		 0.3Q�5L      $
 10Q�5M      ._�5O      �  5P      �� 5Q   "   $
��auto �63   6B   �� 5R   "   $
��auto �63   6B   �� 5S   "   $��3* �63   6@   .+�5V      �� $ 7 0 0 7��6%   #��[ 6>   $	��145��6f   $��10��6s   F� 5W      -   5        $" path_itemPurchaseReportsIcon��5W   #   $  itemPurchaseReports��6I   #��[ 6c   $
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5X      $��1��6<   $��Center=�5Y      $��50��65   $��50��6A   $��LeftToRight=�5Z        rawMaterials#��[ 5[      �� 5\      $��2��6)   $	��Top=�69   $��2��6R   $��5,20,0,0q�6f   $
��Left=�6x   .+�5]   "   �� -	   5        $! txt_itemPurchaseReportsInfo��5]   ,   $  itemPurchaseReports��6Q   $��itemPurchaseReports��6k   $	��165��6�   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5^   "   -
   5        $! txt_itemPurchaseReportsHint��5^   ,   $  itemPurchaseReports��6Q   $��itemPurchaseReports��6k   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5_      �� 5`      -   5        $ btn_itemPurchaseReports��5b   3   $  itemPurchaseReports��5`   %   $��30��6?   $��90��6K   $��2��6W   $��Bottom=�6h   $��	0,0,10,10q�6�   #��[ 5a   !   $��Right=�6I   $��0q�5b      $��0q�5c      $ 7��6+   $��Center=�5d      $��Center=�6>   .��5e   "   �� $ 
Horizontal=�60   .+�5f   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 5h   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5i      $��Center=�5j      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5k   b   F� 5l   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5m      $��Center=�5n      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5o   b   5p   #   5q      5r      5s      �� 5v      $  orderPurchaseReports��6   
�������$	 7.5��6K   $	��145��6^   $	��260��6l   $��15 10q�6z    5w        5x      $ 1Q�5y      $	 360Q�5z      $
 Gray��5{      $		 0.3Q�5|      $
 10Q�5}      ._�5      �  5�      �� 5�   "   $
��auto �63   6B   �� 5�   "   $
��auto �63   6B   �� 5�   "   $��3* �63   6@   .+�5�      �� $ 7 0 0 7��6%   #��[ 6>   $	��145��6f   $��10��6s   F� 5�      -   5        $# path_orderPurchaseReportsIcon��5�   #   $  orderPurchaseReports��6J   #��[ 6e   $
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5�      $��1��6<   $��Center=�5�      $��50��65   $��50��6A   $��LeftToRight=�5�        delivery#��[ 5�      �� 5�      $��2��6)   $	��Top=�69   $��2��6R   $��5,20,0,0q�6f   $
��Left=�6x   .+�5�   "   �� -   5        $" txt_orderPurchaseReportsInfo��5�   ,   $  orderPurchaseReports��6R   $��orderPurchaseReports��6m   $	��165��6�   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5�   "   -   5        $" txt_orderPurchaseReportsHint��5�   ,   $  orderPurchaseReports��6R   $��orderPurchaseReports��6m   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5�      �� 5�      -   5        $ btn_orderPurchaseReports��5�   3   $  orderPurchaseReports��5�   %   $��30��6@   $��90��6L   $��2��6X   $��Bottom=�6i   $��	0,0,10,10q�6�   #��[ 5�   !   $��Right=�6I   $��0q�5�      $��0q�5�      $ 7��6+   $��Center=�5�      $��Center=�6>   .��5�   "   �� $ 
Horizontal=�60   .+�5�   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   5�   %   5�      5�      5�      5�      5�      5�      5�      5�      