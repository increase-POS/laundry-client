   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:laundryApp.View.purchaselaundryApp.View.purchase  5      *    $laundryApp.View.purchase.uc_purchase   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:laundryApp.View.purchase     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        
  � Tag$  purchase��5      .��5      � -   5         �Name$ 	grid_main��5      .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ txt_mainTitle��5      $��Purchase��6+   $��40 15q�6<   $��24(�6K   $
��Bold"�6Y      	LightGrey#��[  6k   �� 5   
   $��1��6   $��5,0,-5,0q�6$   .��5      � $��20 15q�6   $
��Left=�6'   .+�5      �� $  
payInvoice��6   
������� �� CornerRadius$	 7.5��6=   $	��145��6P   $	��260��6^   $��15 10q�6l    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5      	   Opacity$		 0.3Q�5      
   
BlurRadius$
 10Q�5      ._�5      �  � ColumnDefinitions 5       �� 5!      $
��auto �6/   6>   �� 5"      $
��auto �6/   6>   �� 5#      $��3* �6/   6<   .+�5&      �� $ 7 0 0 7��6!     	MainColor#��[ 6:   $	��145��6b   $��10��6o   F� 5'      -   5         F�Name$ path_payInvoiceIcon��5'   #   $  
payInvoice��6@     LightMainColor#��[ 6Q    �� Stretch$
 Fill=�6x   $��10,10,10,10q�6�   $��Center=�5(      $��1��6<   $��Center=�5)      $��50��65   $��50��6A   $��LeftToRight=�5*        invoice#��[ 5+      �� 5,      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5-   "   �� -   5        $ txt_payInvoiceInfo��5-   ,   $  
payInvoice��6H   $��Pay Invoice��6Y   $	��165��6l   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5.   "   -   5        $ txt_payInvoiceHint��5.   ,   $  
payInvoice��6H   $��Pay Invoice��6Y   $	��165��6l   $��0,2,0,0q�6x   ���#9494946�   5/      �� 50      -   5         ��Name$ btn_payInvoice��52   3   $  
payInvoice��50   %   $��30��66   $��90��6B   $��2��6N   $��Bottom=�6_   $��	0,0,10,10q�6z   #��[ 51   !   $��Right=�6I   $��0q�52      $��0q�53      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6+   $��Center=�54      $��Center=�6>   .��55      ��  �� Orientation$ 
Horizontal=�6,   .+�56   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 58   &    	 White#��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�59      $��Center=�5:      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	 J� RenderTransformOrigin$ 0.5,0.5&�5;   b   F� 5<   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5=      $��Center=�5>      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5?   b   5@   #   5A      5B      5C      �� 5F      $  purchaseOrder��6   
�������$	 7.5��6@   $	��145��6S   $	��260��6a   $��15 10q�6o    5G        5H      $ 1Q�5I      $	 360Q�5J      $
 Gray��5K      $		 0.3Q�5L      $
 10Q�5M      ._�5O      �  5P      �� 5Q      $
��auto �6/   6>   �� 5R      $
��auto �6/   6>   �� 5S      $��3* �6/   6<   .+�5V      �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5W      -   5        $ path_purchaseOrderIcon��5W   #   $  purchaseOrder��6C   #��[ 6W   $
 Fill=�6~   $��10,10,10,10q�6�   $��Center=�5X      $��1��6<   $��Center=�5Y      $��50��65   $��50��6A   $��LeftToRight=�5Z        
dolarsList#��[ 5[      �� 5\      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5]   "   �� -	   5        $ txt_purchaseOrderInfo��5]   ,   $  purchaseOrder��6K   $��Purchase Order��6_   $	��165��6v   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5^   "   -
   5        $ txt_purchaseOrderHint��5^   ,   $  purchaseOrder��6K   $��Purchase Order��6_   $	��165��6u   $��0,2,0,0q�6�   ���#9494946�   5_      �� 5`      -   5        $ btn_purchaseOrder��5b   3   $  purchaseOrder��5`   %   $��30��6:   $��90��6F   $��2��6R   $��Bottom=�6c   $��	0,0,10,10q�6~   #��[ 5a   !   $��Right=�6I   $��0q�5b      $��0q�5c      $ 7��6+   $��Center=�5d      $��Center=�6>   .��5e      �� $ 
Horizontal=�6,   .+�5f   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 5h   "   #��[ 6(   $
 Fill=�6F   $��	0 0 -3 00q�5i      $��Center=�5j      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5k   b   F� 5l   "   #��[ 6(   $
 Fill=�6F   $��0 0 0 00q�5m      $��Center=�5n      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5o   b   5p      5q      5r      5s      5�      5�      5�      5�      5�      