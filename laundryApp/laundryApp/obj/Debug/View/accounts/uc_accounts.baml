   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:laundryApp.View.accountslaundryApp.View.accounts  5      *    $laundryApp.View.accounts.uc_accounts   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:laundryApp.View.accounts     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        
  � Tag$  accounts��5       .��5      � -   5         �Name$ 	grid_main��5      .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ txt_mainTitle��5      $��Accounts��6+   $��40 15q�6<   $��24(�6K   $
��Bold"�6Y      	LightGrey#��[  6k   �� 5   
   $��1��6   $��5,0,-5,0q�6$   .��5      � $��20 15q�6   $
��Left=�6'   .+�5      �� $  posTransfers��6   
������� �� CornerRadius$	 7.5��6?   $	��145��6R   $	��260��6`   $��15 10q�6n    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5      	   Opacity$		 0.3Q�5      
   
BlurRadius$
 10Q�5      ._�5      �  � ColumnDefinitions 5       �� 5!      $
��auto �6/   6>   �� 5"      $
��auto �6/   6>   �� 5#      $��3* �6/   6<   .+�5&      �� $ 7 0 0 7��6!     	MainColor#��[ 6:   $	��145��6b   $��10��6o   F� 5'      -   5         F�Name$ path_posTransfersIcon��5'   #   $  posTransfers��6B     LightMainColor#��[ 6U    �� Stretch$
 Fill=�6|   $��10,10,10,10q�6�   $��Center=�5(      $��1��6<   $��Center=�5)      $��50��65   $��50��6A   $��LeftToRight=�5*        POS#��[ 5+      �� 5,      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5-   "   �� -   5        $ txt_posTransfersInfo��5-   ,   $  posTransfers��6J   $��	Transfers��6]   $	��165��6n   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5.   "   -   5        $ txt_posTransfersHint��5.   ,   $  posTransfers��6J   $��add, update, delete, ...��6]   $	��165��6}   $��0,2,0,0q�6�   ���#9494946�   5/      �� 50      -   5         ��Name$ btn_posTransfers��52   3   $  posTransfers��50   %   $��30��68   $��90��6D   $��2��6P   $��Bottom=�6a   $��	0,0,10,10q�6|   #��[ 51   !   $��Right=�6I   $��0q�52      $��0q�53      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6+   $��Center=�54      $��Center=�6>   .��55      ��  �� Orientation$ 
Horizontal=�6,   .+�56   &   �� $��Enter��60   $  enterButton��6=   
�������$��	0 0 7.5 0q�6d   6z   F� 58   &    	 White#��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�59      $��Center=�5:      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	 J� RenderTransformOrigin$ 0.5,0.5&�5;   b   F� 5<   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5=      $��Center=�5>      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5?   b   5@   #   5A      5B      5C      �� 5F      $  payments��6   
�������$	 7.5��6;   $	��145��6N   $	��260��6\   $��15 10q�6j    5G        5H      $ 1Q�5I      $	 360Q�5J      $
 Gray��5K      $		 0.3Q�5L      $
 10Q�5M      ._�5O      �  5P      �� 5Q      $
��auto �6/   6>   �� 5R      $
��auto �6/   6>   �� 5S      $��3* �6/   6<   .+�5V      �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5W      -   5        $ path_paymentsIcon��5W   #   $  payments��6>   #��[ 6M   $
 Fill=�6t   $��10,10,10,10q�6�   $��Center=�5X      $��1��6<   $��Center=�5Y      $��50��65   $��50��6A   $��LeftToRight=�5Z        payments#��[ 5[      �� 5\      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5]   "   �� -	   5        $ txt_paymentsInfo��5]   ,   $  payments��6F   $��Payments��6U   $	��165��6f   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5^   "   -
   5        $ txt_paymentsHint��5^   ,   $  payments��6F   $��add, update, delete, ...��6U   $	��165��6u   $��0,2,0,0q�6�   ���#9494946�   5_      �� 5`      -   5        $ btn_payments��5b   3   $  payments��5`   %   $��30��64   $��90��6@   $��2��6L   $��Bottom=�6]   $��	0,0,10,10q�6x   #��[ 5a   !   $��Right=�6I   $��0q�5b      $��0q�5c      $ 7��6+   $��Center=�5d      $��Center=�6>   .��5e      �� $ 
Horizontal=�6,   .+�5f   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6e   6{   F� 5h   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5i      $��Center=�5j      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5k   b   F� 5l   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5m      $��Center=�5n      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5o   b   5p   #   5q      5r      5s      �� 5v      $  received��6   
�������$	 7.5��6;   $	��145��6N   $	��260��6\   $��15 10q�6j    5w        5x      $ 1Q�5y      $	 360Q�5z      $
 Gray��5{      $		 0.3Q�5|      $
 10Q�5}      ._�5      �  5�      �� 5�      $
��auto �6/   6>   �� 5�      $
��auto �6/   6>   �� 5�      $��3* �6/   6<   .+�5�      �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5�      -   5        $ path_receivedIcon��5�   #   $  received��6>   #��[ 6M   $
 Fill=�6t   $��10,10,10,10q�6�   $��Center=�5�      $��1��6<   $��Center=�5�      $��50��65   $��50��6A   $��LeftToRight=�5�        received#��[ 5�      �� 5�      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5�   "   �� -   5        $ txt_receivedInfo��5�   ,   $  received��6F   $��Received��6U   $	��165��6f   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5�   "   -   5        $ txt_receivedHint��5�   ,   $  received��6F   $��add, update, delete, ...��6U   $	��165��6u   $��0,2,0,0q�6�   ���#9494946�   5�      �� 5�      -   5        $ btn_received��5�   3   $  received��5�   %   $��30��65   $��90��6A   $��2��6M   $��Bottom=�6^   $��	0,0,10,10q�6y   #��[ 5�   !   $��Right=�6I   $��0q�5�      $��0q�5�      $ 7��6+   $��Center=�5�      $��Center=�6>   .��5�      �� $ 
Horizontal=�6,   .+�5�   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6e   6{   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   5�   #   5�      5�      5�      �� 5�      $  banksAccounting��6   
�������$	 7.5��6B   $	��145��6U   $	��260��6c   $��15 10q�6q    5�        5�      $ 1Q�5�      $	 360Q�5�      $
 Gray��5�      $		 0.3Q�5�      $
 10Q�5�      ._�5�      �  5�      �� 5�      $
��auto �6/   6>   �� 5�      $
��auto �6/   6>   �� 5�      $��3* �6/   6<   .+�5�      �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5�      -   5        $ path_banksAccountingIcon��5�   #   $  banksAccounting��6E   #��[ 6[   $
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5�      $��1��6<   $��Center=�5�      $��50��65   $��50��6A   $��LeftToRight=�5�        bank#��[ 5�      �� 5�      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5�   "   �� -   5        $ txt_banksAccountingInfo��5�   ,   $  banksAccounting��6M   $��banksAccounting��6c   $	��165��6{   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5�   "   -   5        $ txt_banksAccountingHint��5�   ,   $  banksAccounting��6M   $��add, update, delete, ...��6c   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5�      �� 5�      -   5        $ btn_banksAccounting��5�   3   $  banksAccounting��5�   %   $��30��6;   $��90��6G   $��2��6S   $��Bottom=�6d   $��	0,0,10,10q�6   #��[ 5�   !   $��Right=�6I   $��0q�5�      $��0q�5�      $ 7��6+   $��Center=�5�      $��Center=�6>   .��5�      �� $ 
Horizontal=�6,   .+�5�   &   �� $��Enter��60   $  enterButton��6?   
�������$��	0 0 7.5 0q�6e   6{   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   5�   #   5�      5�      5�      �� 5�      $  subscriptions��6   
�������$	 7.5��6@   $	��145��6S   $	��260��6a   $��15 10q�6o    5�        5�      $ 1Q�5�      $	 360Q�5�      $
 Gray��5�      $		 0.3Q�5�      $
 10Q�5�      ._�5�      �  5�      �� 5�      $
��auto �6/   6>   �� 5�      $
��auto �6/   6>   �� 5�      $��3* �6/   6<   .+�5�      �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5�      -   5        $ path_subscriptionsIcon��5�   #   $  subscriptions��6C   #��[ 6W   $
 Fill=�6~   $��10,10,10,10q�6�   $��Center=�5�      $��1��6<   $��Center=�5�      $��50��65   $��50��6A   $��LeftToRight=�5�        subscriptions#��[ 5�      �� 5�      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5�   "   �� -   5        $ txt_subscriptionsInfo��5�   ,   $  subscriptions��6K   $��subscriptions��6_   $	��165��6u   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5�   "   -   5        $ txt_subscriptionsHint��5�   ,   $  subscriptions��6K   $��add, update, delete, ...��6_   $	��165��6   $��0,2,0,0q�6�   ���#9494946�   5�      �� 5�      -   5        $ btn_subscriptions��5�   3   $  subscriptions��5�   %   $��30��69   $��90��6E   $��2��6Q   $��Bottom=�6b   $��	0,0,10,10q�6}   #��[ 5�   !   $��Right=�6I   $��0q�5�      $��0q�5�      $ 7��6+   $��Center=�5�      $��Center=�6>   .��5�      �� $ 
Horizontal=�6,   .+�5�   &   �� $��Enter��60   $  enterButton��6?   
�������$��	0 0 7.5 0q�6e   6{   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   5   #   5     5     5     �� 5     $  ordersAccounting��6   
�������$	 7.5��6C   $	��145��6V   $	��260��6d   $��15 10q�6r    5       5     $ 1Q�5	     $	 360Q�5
     $
 Gray��5     $		 0.3Q�5     $
 10Q�5     ._�5     �  5     �� 5     $
��auto �6/   6>   �� 5     $
��auto �6/   6>   �� 5     $��3* �6/   6<   .+�5     �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5     -   5        $ path_ordersAccountingIcon��5  #   $  ordersAccounting��6F   #��[ 6]   $
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5     $��1��6<   $��Center=�5     $��50��65   $��50��6A   $��LeftToRight=�5      	 deliveryMan#��[	 5     �� 5     $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5  "   �� -   5        $ txt_ordersAccountingInfo��5  ,   $  ordersAccounting��6N   $��orders��6e   $	��165��6t   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5  "   -   5        $ txt_ordersAccountingHint��5  ,   $  ordersAccounting��6N   $��add, update, delete, ...��6e   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5     �� 5      -   5        $ btn_ordersAccounting��5"  3   $  ordersAccounting��5   %   $��30��6<   $��90��6H   $��2��6T   $��Bottom=�6e   $��	0,0,10,10q�6�   #��[ 5!  !   $��Right=�6I   $��0q�5"     $��0q�5#     $ 7��6+   $��Center=�5$     $��Center=�6>   .��5%     �� $ 
Horizontal=�6,   .+�5&  &   �� $��Enter��60   $  enterButton��6?   
�������$��	0 0 7.5 0q�6e   6{   F� 5(  &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5)     $��Center=�5*     $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5+  b   F� 5,  &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5-     $��Center=�5.     $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5/  b   50  #   51     52     53     55     56     57     58     59     