   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:laundryApp.View.sectionData.branchesAndStores-laundryApp.View.sectionData.branchesAndStores  5      H    BlaundryApp.View.sectionData.branchesAndStores.uc_branchesAndStores   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mlocal;clr-namespace:laundryApp.View.sectionData.branchesAndStores     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        
  � Tag$  branchesAndStores��5      .��5      � -   5         �Name$ 	grid_main��5      .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ txt_mainTitle��5      $��Branches And Stores��6+   $��40 15q�5      $��24(�6#   $
��Bold"�61      	LightGrey#��[  6C   �� 5   
   $��1��6   $��5,0,-5,0q�6$   .��5      � $��20 15q�6   $
��Left=�6'   .+�5      �� $  branches��6   
������� �� CornerRadius$	 7.5��6;   $	��145��6N   $	��260��6\   $��15 10q�6j    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5      	   Opacity$		 0.3Q�5      
   
BlurRadius$
 10Q�5      ._�5       �  � ColumnDefinitions 5!      �� 5"      $
��auto �6/   6>   �� 5#      $
��auto �6/   6>   �� 5$      $��3* �6/   6<   .+�5'      �� $ 7 0 0 7��6!     	MainColor#��[ 6:   $	��145��6b   $��10��6o   F� 5(      -   5         F�Name$ path_branchesIcon��5(   #   $  branches��6>     LightMainColor#��[ 6M    �� Stretch$
 Fill=�6t   $��10,10,10,10q�6�   $��Center=�5)      $��1��6<   $��Center=�5*      $��50��65   $��50��6A   $��LeftToRight=�5+        branches#��[ 5,      �� 5-      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5.   "   �� -   5        $ txt_branchesInfo��5.   ,   $  branches��6F   $��Branches��6U   $	��165��6e   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5/   "   -   5        $ txt_branchesHint��5/   ,   $  branches��6F   $��add, update, delete, ...��6U   $	��165��6u   $��0,2,0,0q�6�   ���#9494946�   50      �� 51      -   5         ��Name$ btn_branches��53   3   $  branches��51   &   $��30��65   $��90��6A   $��2��6M   $��Bottom=�6^   $��	0,0,10,10q�6y   #��[ 52   !   $��Right=�6I   $��0q�53      $��0q�54      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6+   $��Center=�55      $��Center=�6>   .��56      ��  �� Orientation$ 
Horizontal=�6,   .+�57   &   �� $��Enter��60   $  enterButton��6?   
�������$��	0 0 7.5 0q�6d   6z   F� 59   &    	 White#��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5:      $��Center=�5;      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	 J� RenderTransformOrigin$ 0.5,0.5&�5<   b   F� 5=   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5>      $��Center=�5?      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5@   b   5A   #   5B      5C      5D      �� 5G      $  stores��6   
�������$	 7.5��69   $	��145��6L   $	��260��6Z   $��15 10q�6h    5H        5I      $ 1Q�5J      $	 360Q�5K      $
 Gray��5L      $		 0.3Q�5M      $
 10Q�5N      ._�5P      �  5Q      �� 5R      $
��auto �6/   6>   �� 5S      $
��auto �6/   6>   �� 5T      $��3* �6/   6<   .+�5W      �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5X      -   5        $ path_storesIcon��5X   #   $  stores��6<   #��[ 6I   $
 Fill=�6p   $��10,10,10,10q�6   $��Center=�5Y      $��1��6<   $��Center=�5Z      $��50��65   $��50��6A   $��LeftToRight=�5[        storage#��[ 5\      �� 5]      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5^   "   �� -	   5        $ txt_storesInfo��5^   ,   $  stores��6D   $��Stores��6Q   $	��165��6`   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5_   "   -
   5        $ txt_storesHint��5_   ,   $  stores��6D   $��add, update, delete, ...��6Q   $	��165��6q   $��0,2,0,0q�6}   ���#9494946�   5`      �� 5a      -   5        $ 
btn_stores��5c   3   $  stores��5a   %   $��30��63   $��90��6?   $��2��6K   $��Bottom=�6\   $��	0,0,10,10q�6w   #��[ 5b   !   $��Right=�6I   $��0q�5c      $��0q�5d      $ 7��6+   $��Center=�5e      $��Center=�6>   .��5f      �� $ 
Horizontal=�6,   .+�5g   &   �� $��Enter��60   $  enterButton��6=   
�������$��	0 0 7.5 0q�6d   6z   F� 5i   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5j      $��Center=�5k      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5l   b   F� 5m   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5n      $��Center=�5o      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5p   b   5q   #   5r      5s      5t      �� 5w      $	  pos��6   
�������$	 7.5��66   $	��145��6I   $	��260��6W   $��15 10q�6e    5x        5y      $ 1Q�5z      $	 360Q�5{      $
 Gray��5|      $		 0.3Q�5}      $
 10Q�5~      ._�5�      �  5�      �� 5�      $
��auto �6/   6>   �� 5�      $
��auto �6/   6>   �� 5�      $��3* �6/   6<   .+�5�      �� $ 7 0 0 7��6!   #��[ 6:   $	��145��6b   $��10��6o   F� 5�      -   5        $ path_posIcon��5�   #   $	  pos��69   #��[ 6C   $
 Fill=�6j   $��10,10,10,10q�6y   $��Center=�5�      $��1��6<   $��Center=�5�      $��50��65   $��50��6A   $��LeftToRight=�5�        POS#��[ 5�      �� 5�      $��2��6%   $	��Top=�65   $��2��6N   $��5,20,0,0q�6b   $
��Left=�6t   .+�5�   "   �� -   5        $ txt_posInfo��5�   ,   $	  pos��6A   $	��POS��6K   $	��165��6W   ���#6e6e6e$��SemiBold"�6x   $��16(�6�   6�   �� 5�   "   -   5        $ txt_posHint��5�   ,   $	  pos��6A   $��add, update, delete, ...��6K   $	��165��6k   $��0,2,0,0q�6w   ���#9494946�   5�      �� 5�      -   5        $ btn_pos��5�   3   $	  pos��5�   %   $��30��6/   $��90��6;   $��2��6G   $��Bottom=�6X   $��	0,0,10,10q�6s   #��[ 5�   !   $��Right=�6I   $��0q�5�      $��0q�5�      $ 7��6+   $��Center=�5�      $��Center=�6>   .��5�      �� $ 
Horizontal=�6,   .+�5�   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   F� 5�   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5�      $��Center=�5�      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5�   b   5�   #   5�      5�      5�      5�      5�      5�      5�      5�      