   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullg8clr-namespace:laundryApp.View.sales.promotion.membership*laundryApp.View.sales.promotion.membership  5      >    8laundryApp.View.sales.promotion.membership.uc_membership   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jlocal8clr-namespace:laundryApp.View.sales.promotion.membership     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        
  � Tag$  
membership��5      .��5      � -   5         �Name$ 	grid_main��5      .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $��* �6    .+�5      �� -   5         ��Name$ txt_mainTitle��5   G   $��40 15q�6   $��24(�6'   $
��Bold"�65   $��
membership��6^      	LightGrey#��[  6p   �� 5      $��1��6   $��5,0,-5,0q�6(   .��5      � $��20 15q�6   $
��Left=�6+   .+�5      �� $  membershipCreate��6   
������� �� CornerRadius$	 7.5��6G   $	��145��6Z   $	��260��6h   $��15 10q�6v    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5      	   Opacity$		 0.3Q�5      
   
BlurRadius$
 10Q�5      ._�5      �  � ColumnDefinitions 5       �� 5!   "   $
��auto �63   6B   �� 5"   "   $
��auto �63   6B   �� 5#   "   $��3* �63   6@   .+�5&      �� $ 7 0 0 7��6%     	MainColor#��[ 6>   $	��145��6f   $��10��6s   F� 5'      -   5         F�Name$ path_membershipCreateIcon��5'   #   $  membershipCreate��6F     LightMainColor#��[ 6]    �� Stretch$
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5(      $��1��6<   $��Center=�5)      $��50��65   $��50��6A   $��LeftToRight=�5*        membershipAdd#��[ 5+      �� 5,      $��2��6)   $	��Top=�69   $��2��6R   $��5,20,0,0q�6f   $
��Left=�6x   .+�5-   "   �� -   5        $ txt_membershipCreateInfo��5-   ,   $  membershipCreate��6N   $��membershipCreate��6e   $	��165��6~   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5.   "   -   5        $ txt_membershipCreateHint��5.   ,   $  membershipCreate��6N   $��add, update, delete, ...��6e   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5/      �� 50      -   5         ��Name$ btn_membershipCreate��52   3   $  membershipCreate��50   %   $��30��6<   $��90��6H   $��2��6T   $��Bottom=�6e   $��	0,0,10,10q�6�   #��[ 51   !   $��Right=�6I   $��0q�52      $��0q�53      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��6+   $��Center=�54      $��Center=�6>   .��55   "   ��  �� Orientation$ 
Horizontal=�60   .+�56   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 58   &    	 White#��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�59      $��Center=�5:      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	 J� RenderTransformOrigin$ 0.5,0.5&�5;   b   F� 5<   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5=      $��Center=�5>      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5?   b   5@   #   5A      5B      5C      �� 5F      $  membershipUpdate��6   
�������$	 7.5��6G   $	��145��6Z   $	��260��6h   $��15 10q�6v    5G        5H      $ 1Q�5I      $	 360Q�5J      $
 Gray��5K      $		 0.3Q�5L      $
 10Q�5M      ._�5O      �  5P      �� 5Q   "   $
��auto �63   6B   �� 5R   "   $
��auto �63   6B   �� 5S   "   $��3* �63   6@   .+�5V      �� $ 7 0 0 7��6%   #��[ 6>   $	��145��6f   $��10��6s   F� 5W      -   5        $ path_membershipUpdateIcon��5W   #   $  membershipUpdate��6F   #��[ 6]   $
 Fill=�6�   $��10,10,10,10q�6�   $��Center=�5X      $��1��6<   $��Center=�5Y      $��50��65   $��50��6A   $��LeftToRight=�6L     membershipUpdate#��[ 5Z      �� 5[      $��2��6)   $	��Top=�69   $��2��6R   $��5,20,0,0q�6f   $
��Left=�6x   .+�5\   "   �� -	   5        $ txt_membershipUpdateInfo��5\   ,   $  membershipUpdate��6N   $��membershipUpdate��6e   $	��165��6~   ���#6e6e6e$��SemiBold"�6�   $��16(�6�   6�   �� 5]   "   -
   5        $ txt_membershipUpdateHint��5]   ,   $  membershipUpdate��6N   $��add, update, delete, ...��6e   $	��165��6�   $��0,2,0,0q�6�   ���#9494946�   5^      �� 5_      -   5        $ btn_membershipUpdate��5a   3   $  membershipUpdate��5_   %   $��30��6=   $��90��6I   $��2��6U   $��Bottom=�6f   $��	0,0,10,10q�6�   #��[ 5`   !   $��Right=�6I   $��0q�5a      $��0q�5b      $ 7��6+   $��Center=�5c      $��Center=�6>   .��5d   "   �� $ 
Horizontal=�60   .+�5e   &   �� $��Enter��60   $  enterButton��6>   
�������$��	0 0 7.5 0q�6d   6z   F� 5g   &   #��[ 6,   $
 Fill=�6J   $��	0 0 -3 00q�5h      $��Center=�5i      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5j   b   F� 5k   &   #��[ 6,   $
 Fill=�6J   $��0 0 0 00q�5l      $��Center=�5m      $
��12.5��65   $��10��6C   P���p�� �G�z�0@�P��  � Ѱ� Pq р�� ��[ � $�  � р�� ��Q�� �G�z�0@	$ 0.5,0.5&�5n   b   5o   %   5p      5q      5r      5t      5u      5v      5w      5x      