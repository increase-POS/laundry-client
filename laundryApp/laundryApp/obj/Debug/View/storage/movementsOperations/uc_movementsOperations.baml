   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulli9clr-namespace:laundryApp.View.storage.movementsOperations+laundryApp.View.storage.movementsOperations  5      H    BlaundryApp.View.storage.movementsOperations.uc_movementsOperations   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Klocal9clr-namespace:laundryApp.View.storage.movementsOperations     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        
  � Tag$  movementsOperations��5      .��5      � -   5         �Name$ 	grid_main��5      .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ txt_mainTitle��5   C   $��40 15q�6   $��24(�6#   $
��Bold"�61   $��Movements operations��6Z      	LightGrey#��[  6v   �� 5   
   $��1��6   $��5,0,-5,0q�6$   .��5      � $��20 15q�6   $
��Left=�6'   .+�5      �� $  storageMovements��6   
������� �� CornerRadius$	 7.5��6C   $	��145��6V   $	��260��6d   $��15 10q�6r    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5      	   Opacity$		 0.3Q�5      
   
BlurRadius$
 10Q�5      ._�5      �  � ColumnDefinitions 5       �� 5!      $
��auto �6/   6>   �� 5"      $
��auto �6/   6>   �� 5#      $��3* �6/   6<   .+�5&      �� $ 7 0 0 7��6!     	MainColor#��[ 6:   $	��145��6b   $��10��6o   F� 5'      -   5         F�Name$ path_storageMovementsIcon��5'   #   $  storageMovements��6F     LightMainColor#��[ 6]    �� Stretch$
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5(      $��1��6<   $��Center=�5)      $��50��65   $��50��6A   $��LeftToRight=�5*        storageMovements#��[ 5+      �� 5,      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5-   "   �� -   5        $ txt_storageMovementsInfo��5-   ,   $  storageMovements��6N   $��Storage movements��6e   $	��165��6~   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5.   "   -   5        $ txt_storageMovementsHint��5.   ,   $  storageMovements��6N   $��import, export,...��6e   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5/      �� 50      -   5         ��Name$ btn_storageMovements��52   3   $  storageMovements��50   %   $��30��6<   $��90��6H   $��2��6T   $��Bottom=�6e   $��	0,0,10,10q�6�   #��[ 51   !   $��Right=�6I   $��0q�52      $��0q�53      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6+   $��Center=�54      $��Center=�6>   .��55      ��  �� Orientation$ 
Horizontal=�6,   .+�56   &   �� $��Enter��60   $  enterButton��6=   
�������$��	0 0 7.5 0q�6e   6{   F� 58   &    	 White#��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�59      $��Center=�5:      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	 J� RenderTransformOrigin$ 0.5,0.5&�5;   b   F� 5<   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5=      $��Center=�5>      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5?   b   5@   #   5A      5B      5C      �� 5F      $  spendingOrder��6   
�������$	 7.5��6D   $	��145��6W   $	��260��6e   $��15 10q�6s    5G        5H      $ 1Q�5I      $	 360Q�5J      $
 Gray��5K      $		 0.3Q�5L      $
 10Q�5M      ._�5O      �  5P      �� 5Q      $
��auto �6/   6>   �� 5R      $
��auto �6/   6>   �� 5S      $��3* �6/   6<   .+�5V      �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5W      -   5        $ path_spendingOrderIcon��5W   #   $  spendingOrder��6C   #��[ 6W   $
 Fill=�6~   $��10,10,10,10q�6�   $��Center=�5X      $��1��6<   $��Center=�5Y      $��50��65   $��50��6A   $��LeftToRight=�5Z        rawMaterials#��[ 5[      �� 5\      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5]   "   �� -	   5        $ txt_spendingOrderInfo��5]   ,   $  spendingOrder��6K   $��spending order��6_   $	��165��6v   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   �� 5^   "   -
   5        $ txt_spendingOrderHint��5^   ,   $  spendingOrder��6K   $"��spending order raw materials��6_   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5_      �� 5`      -   5        $ btn_spendingOrder��5b   3   $  spendingOrder��5`   %   $��30��69   $��90��6E   $��2��6Q   $��Bottom=�6b   $��	0,0,10,10q�6}   #��[ 5a   !   $��Right=�6I   $��0q�5b      $��0q�5c      $ 7��6+   $��Center=�5d      $��Center=�6>   .��5e      �� $ 
Horizontal=�6,   .+�5f   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 5h   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5i      $��Center=�5j      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5k   b   F� 5l   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5m      $��Center=�5n      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5o   b   5p   #   5q      5r      5s      5u      5v      5w      5x      5y      