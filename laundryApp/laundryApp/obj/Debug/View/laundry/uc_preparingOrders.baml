   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.kitchenlaundryApp.View.kitchen  5      0    *laundryApp.View.kitchen.uc_preparingOrders   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:laundryApp.View.kitchen     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      -   5        
  � Tag$  preparingOrders��5       .��5      � -   5         �Name$ 	grid_main��5      $��20 10q�6    � ColumnDefinitions 5   
   �� 5      $
��2.5* �6   �� 5      -   5         ��Name$ 	gridWidth��5      $��* �62   $	��225��6<   .+�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $��* �6    .+�5      �  5      �� 5      $��* �6$   �� 5      $��* �6$   .+�5      �  5      �� 5       $
��auto �6+   �� 5!      $��* �6+   �� 5"      $��* �6,   �� 5$      $��* �6,   �� 5%      $
��auto �6,   .+�5'      �� $��40��6   $��50��6)    �� CornerRadius$ 7��64      SecondColor#��[  5(       J� Effect 5)      3  -System.Windows.Media.Effects.DropShadowEffect  5*         ShadowDepth$ 1Q�5+         	Direction$	 360Q�5,      	   Color$
	 Gray��5-      
   Opacity$	
 0.3Q�5.         
BlurRadius$ 10Q�5/      ._�51      F� 
������� �� Stretch$
 Fill=�6,   $��Center=�52      $��0��6<   $��Center=�53      $
��17.5��65   $
��17.5��6B   ����p��ffffff0@������lW@���fffff&P@��������LG@ffffff0@������`��`,a 0v�hx�`r��ō�������}��q����f�z��@) ����Z @ sӀ�  ���� `�Z  GS@�� �����y_@ $� ����. G��. G�. G�. G� B� �{�������F@��D�������F@��D� ���$7Ѡ'�@��S���43333U@`�������J@�A � �V `� ظ23333]@@�~�`���k�~�fffff&@@��ffffff0@Q��ffffff0@� K@zA�����LV@43333�T@@�`6 �����LL@@zA�����F@43333sM@�����F@gfffffC@�����LL@hfffff:@S�a     ,@�����LV@������+@ Khfffff:@S��i433333@@���    @D@���43333sH@� 3� Tj��i�����YP@ K@zA�`}�`F�Ӏ`F�gffff�S@@2Ygffff�S@`���gffff�S@�d�43333�S@�����=@  � ��� ����`�y �,�{s@y��`am �������'�Э �-��W`��3 �� ]����������H@S ��������G@@��������F@ M�������E@� B� �� ��`F�`}�`F�	54      5:      �� 5;      
�������$��1��60   $ 10��6@   $��	10,5,10,5q�6U   $��1q�6i     	LightGrey#��[ 6}    5<        5=      $ 1Q�5>      $	 360Q�5?      $
	 Gray��5@      $	
 0.3Q�5A      $ 10Q�5B      ._�5D      � .+�5E      �� -   5         ��Name$ 	tb_search��5E   &   $��1��69   $��10,0,5,0q�6K    % !MaterialDesignFloatingHintTextBox#��[ 5F   '   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$% Enter Here To Start Search ....��5G   '   ���#4E4E4E �� 	MaxLength$ 50��5I   '   ���#737373$��0q�5K   =   $	��4,0q�6Q   .  (MaterialDesignThemes.Wpf.TextFieldAssist   CharacterCounterStyle 5J   &   �� 5K   Q   5L      5M      �� 5N      $��2��6   $ 10��6-   $��	10,5,10,5q�6?   $��1q�6T   #��[ 6h   
������� 5O        5P      $ 1Q�5Q      $	 360Q�5R      $
	 Gray��5S      $	
 0.3Q�5T      $ 10Q�5U      ._�5W      � .+�5X      �� -   5         ��Name$ cb_searchInvType��5X   '   $��1��6B   $��10,0,5,0q�5Y   $   ���#4E4E4E & "MaterialDesignFloatingHintComboBox#��[ 5Z   #   $ invoice type...��5[   #   $��1��5\   #   $��40��63   ���#737373$��0q�5]   :   $	��4,0q�6N   ��5^   )   �� 5_      5a      5b      �� 5c      $��3��6   $ 10��6-   $��	10,5,10,5q�6?   $��1q�6T   #��[ 6h   
������� 5d        5e      $ 1Q�5f      $	 360Q�5g      $
	 Gray��5h      $	
 0.3Q�5i      $ 10Q�5j      ._�5l      � .+�5m      �� -   5        $ cb_searchCatalog��5m   '   $��1��6B   $��10,0,5,0q�5n   $   ���#4E4E4E#��[ 5o   #   $ 
catalog...��5p   #   $��1��5q   #   $��40��63   ���#737373$��0q�5r   :   $	��4,0q�6N   ��5s   )   �� ��5t   "   �� 5u   &   .R�5v   *   �� ��64   ��  " translateCategoryNameConverter �� 	Converter# [ 5w   '   5y      5{      5|      �� 5�      -   5         ��Name$ btn_refresh��5�      $��LeftToRight=�5�      $��40��6<   $��40��6G   $��0��5�      $��5��6,   $��	10,5,10,5q�6<   $��0q�5�      +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 15��5�       � Cursor$
 Hand��6K     SmallButtonCircleRotate#��[ 5�   J    J� RenderTransformOrigin$ 0.5,0.5&�5�        � DataContext$�
 �
M896,0L896,0C401.2,0,0,401.2,0,896v0c0,494.8,401.2,896,896,896h0c494.8,0,896-401.2,896-896v0C1792,401.2,1390.8,0,896,0z   M1451.3,1021c-31.9,133.7-98.8,242.1-200.6,325.2c-101.8,83.1-221,124.6-357.7,124.6c-72.8,0-143.3-13.7-211.4-41.2  s-128.8-66.6-182.2-117.5l-96.5,96.5c-9.5,9.5-20.7,14.2-33.7,14.2s-24.2-4.7-33.7-14.2c-9.5-9.5-14.2-20.7-14.2-33.7v-335.3  c0-13,4.7-24.2,14.2-33.7s20.7-14.2,33.7-14.2h335.3c13,0,24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7c0,13-4.7,24.2-14.2,33.7  l-102.5,102.5c35.4,32.9,75.6,58.4,120.5,76.3c44.9,18,91.5,26.9,139.9,26.9c66.9,0,129.2-16.2,187.1-48.6  c57.9-32.4,104.3-77.1,139.2-134c5.5-8.5,18.7-37.7,39.7-87.6c4-11.5,11.5-17.2,22.4-17.2h143.7c6.5,0,12.1,2.4,16.8,7.1  c4.7,4.7,7.1,10.4,7.1,16.8C1452,1018.2,1451.8,1020,1451.3,1021z M1470.7,752.3c0,13-4.7,24.2-14.2,33.7  c-9.5,9.5-20.7,14.2-33.7,14.2h-335.3c-13,0-24.2-4.7-33.7-14.2s-14.2-20.7-14.2-33.7c0-13,4.7-24.2,14.2-33.7l103.3-103.3  C1083.3,547,996.3,512.9,896,512.9c-66.9,0-129.2,16.2-187.1,48.6s-104.3,77.1-139.2,134c-5.5,8.5-18.7,37.7-39.7,87.6  c-4,11.5-11.5,17.2-22.5,17.2H358.7c-6.5,0-12.1-2.4-16.8-7.1c-4.7-4.7-7.1-10.4-7.1-16.8V771c32.4-133.7,99.8-242.1,202-325.2  c102.3-83.1,222-124.6,359.2-124.6c72.8,0,143.7,13.8,212.5,41.5s130,66.7,183.3,117.1l97.3-96.5c9.5-9.5,20.7-14.2,33.7-14.2  s24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7V752.3z��5�      $��Right=�5�   :   ��5�      Z� ��61   Z� z�5�      V� 5�      .~�5�   "   �� �� 5�   "   �� 5�   "    �� Angle$ 0Q�62   T� 5�   "   5�       � ToolTip 5�      _� 5�      -   5         _�Name$ 
tt_refresh��5�   &   $��Refresh��6:    _� 	Placement$
 left=�5�   /    _� HorizontalOffset$ 15��5�   /    _� PlacementRectangle$ 50,0,0,0�5�   /    _� VerticalOffset$ 25��5�   /   5�      5�      �� 5�      $��1��6   $��2��6*   $��0 0 10 0q�6;    �� Orientation$ 
Horizontal=�6O   .+�5�      �� $��5q�6   
�������$ 16��6<   $��40��6N   ._�5�      �� -	   5          ��Name$  chk_allForDelivery��5�   #     	MainColor#��[ 5�   &   $	��all��5�   &   ! d� 	IsChecked$
! TrueY�64    2 .MaterialDesignFilterChipPrimaryOutlineCheckBox#��[ 5�   &   $��0q�5�   A   $��40��6L   5�   &   5�      �� 5�      $��5q�6   
�������$ 16��6<   $��40��6N   ._�5�      �� -
   5        $  
chk_listed��5�   #   #��[ 5�   ;   $��listed��5�   ;   #��[ 5�   ;   $��0q�5�   V   $��40��6a   5�   ;   5�      �� 5�      $��5q�6   
�������$ 16��6<   $��40��6N   ._�5�      �� -   5        $  chk_preparing��5�   #   #��[ 5�   ;   $��	preparing��5�   ;   #��[ 5�   ;   $��0q�5�   V   $��40��6a   5�   ;   5�      �� 5�      $��5q�6   
�������$ 16��6<   $��40��6N   ._�5�      �� -   5        $  	chk_ready��5�   #   #��[ 5�   ;   $��ready��5�   ;   #��[ 5�   ;   $��0q�5�   V   $��40��6a   5�   ;   5�      5�      5�      �� 5�      $��	0,10,10,5q�6   $��1��6(   
�������$ 10��6I    5�        5�      $ 1Q�5�      $	 360Q�5�      $
	 Gray��5�      $	
 0.3Q�5�      $ 10Q�5�      ._�5�      �  5�      �� 5�      $
��auto �6(   �� 5�      .+�5�      � $��1��6   .+�5�      �� -   5        " ��Name$" brd_roundDataGrid��5�   V   $��
5,5,5,17.5q�6!   $��1��67   $ 10��6D   $��0q�6q   
�������5�      &   System.Windows.Controls.DataGrid  5�      -   5        #  Name$# 	dg_orders��5�      $��5,5,5,45q�5�   $   $��1��66   $   
IsReadOnly$ . %   HeadersVisibility$% Column=�6X   &   AutoGenerateColumns& .  
�������'   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$' SizeToHeader 5�      (   AlternatingRowBackground( �#f3f3f3) �� AlternationCount$) 2��6Z   * J� OpacityMask* 5�      !� 5�   "   + !� Visual+ 6.   �� , �� ElementName$, brd_roundDataGrid��- � 	Resources- 5�      =  7System.Windows.Controls.Primitives.DataGridColumnHeader. �� BasedOn/ �� 
TargetType0 �� Property1 �� Value��%  (��       5�   2   * SystemColors.HighlightBrushKey+)'       7 7   �� 6"   6f   �� 5�   "   8.   "/  6a   .�5�   &   �� 0 � ��1 �#F5D7C46C   �� 5�   &   0 � ��
1 �����6D   �� 5�   &   0 � ��$1 0 0 1 0q�6H   �� 5�   &   0 � ��81  6C   5   #   5     2   Columns2 5     4  .System.Windows.Controls.DataGridCheckBoxColumn  5  "   -   5        col_chk��,	  &System.Windows.Controls.DataGridColumn3 	  
Visibility$3 	Collapsed=�5  6   4 	  
IsReadOnly4 .  5   IsThreeState5 .  1
  +System.Windows.Controls.DataGridBoundColumn6 
  Binding6 5  J   �� *	IsChecked+7 �� NotifyOnTargetUpdated7 . 8 
  ElementStyle8 5  &   �� 5  *   "/ ��5  )   .�5  .   �� 0 � ��$1 LeftToRight=�6N   �� 5  .   0 � ��$1 Center=�6T   -   5        5$  +   9 	  HeaderTemplate9 5&  &   �� 5'  *   .R�5(  .   �� -   5        $��2q�5(  �   5)  +   5+  #   0  *System.Windows.Controls.DataGridTextColumn  5,  "   : 	  Width$: * 65   -   5        col_orderNum��; 	  Header$; #��5,  U   6 6`   �� < �� Path$< orderNum�  5-  "   $; invNum��65   $: * 6E   6 6O   �� $< invNum�54  #     55  "   $: * 65   $; remaining time��6@   -   5        col_remainingTime��6 55  s   �� $< .� ) %OrderPreparing_remainingTimeConverter# [   56  "   $: * 65   $; status��6@   6 6P   �� $< status� !	 preparingOrderStatusConverter# [	   57  "   $: * 65   $; table��6@   -   5        col_orderType��6 57  f   �� $< invType� 
 preparingOrderTypeConverter# [
   58  "   $: * 65   $; table��6@   -   5        	col_table��6 58  b   �� $< tables�5:     5<     5�     5�     5�     �� 5�  
   $��	10 10 0 5q�6   $��1��6$    5�       5�     $ 1Q�5�     $	 360Q�5�     $
	 Gray��5�     $	
 0.3Q�5�     $ 10Q�5�     ._�5�  
   �  5�     �� 5�     $
��auto �6    60   �� 5�     $��* �6    6-   �� 5�     $
��auto �6    60   �� 5�     $
��auto �6    60   .+�5�     �� #��[ 6   $ 7 7 0 0��6@   ._�5�     � $��40��6   $
��10 0q�6$    5�     �� 5�     $
��auto �6+   �� 5�     $
��auto �6+   �� 5�     $��* �6+   �� 5�     $
��auto �6+   .+�5�     F� 
�������$
 Fill=�6(   $��Center=�5�     $��20��5�     $��20��6$   $��LeftToRight=�60     cooking#��[ 5�     �� 5�     -   5        = ��Name$= 	txt_title��5�  =   $��1��6    $��order��60   $��10,0,0,0q�6P   
�������$��14(�5�     $��Center=�6(   $
��Bold"�5�     �� 5�     -   5        $ 	btn_clear��5�     $��3��6J   $��0q�6Z   $��Center=�5�      ��6;   Z� ��6Q   Z� .��5�     '  !MaterialDesignThemes.Wpf.PackIcon  
�������>   Kind$> RefreshCircle=�6I   $��30��6`   $��30��6k   5�     5�     5�     � 5�     
�������$��1��6(    5�     �� 5�     $
��auto �6,   �� 5�     $
��auto �6,   �� 5�     $
��auto �6,   �� 5�     $
��auto �6,   �� 5�     $��* �6,   .+�5�     ]� $
��10 0q�6    ��. .+�5�     �� -   5        $= txt_details��5�  $   $��details��69   #��[  6H   $��12(�6r   $
��Bold"�6�   $��Center=�6�   $��5,5,5,5q�6�   �� 5�     #��[ 6%   $��Center=�6M   $
��auto��6h   5�     � 5�     -   5        $��gd_preparingTime��5�  $   $��1��6    5�     �� 5�     $��* �6+   �� 5      $
��auto �6+   �� 5     $
��auto �6+   .+�5     �� $��1��6   $ 10��6+   $��	10,5,10,5q�6=   $��1q�6R   #��[ 5     ._�5     � .+�5     �� -   5        $ tb_preparingTime��5  3   $	��4,0q�6&   #��[ 5  *   $��10,0,5,0q�5  V   $��0q�6k   $ preparing time...��5	  *   ���#4E4E4E$��40��5
  k   $  decimal��5  )   ��5  *   �� *preparingTime+? �� Mode$? OneWay=� 5
  *   �� 5  O   F� 5  "   -   5        @ F�Name$@ p_error_preparingTime��5  H   ���#E81123$
 Fill=�66   $��Right=�5  '   $y�	Collapsed=�6C   $��Center=�6\   $��15��5  '   $��15��63   $��5,5,15,5q�6?     
infoCircle#��[ 5  '   5     5     5     �� 5     -   5        $��
txt_minute��5  <   $��minutes��6   $��1��6,     	ThickGrey#��[ 5     $��14(�6E   $��Center=�5  %   $��Right=�6@   $��5,5,15,5q�5     �� 5     -   5        $ btn_updatePreparingTime��5  %   $��2��5  !   $��0q�62   $��Right=�5  R   $��5 5 10 5q�5  %   {�. ��5  %   Z� ��6;   Z� .��5     F� ���#4E4E4E$
 Fill=�62   $
��25.5��5  '   $
��27.5��64   $��LeftToRight=�6B     save#��[ 5  '   5     5      �� 5!     $��2��6   $ 10��6"   $��	10,5,10,5q�64   $��1q�6I   #��[ 6]   ._�5"     � .+�5#     �� -   5        $ tb_notes��5#  '   #��[ 5$  #   $��10,0,5,0q�5%  #   $ Note...��5'  #   ���#4E4E4E$	��4,0q�5)  "   $��0q�60   $��1��5*     $��70��6.   A �� TextWrapping$
A Wrap=�6:   ��5#  9   �� *notes+$? OneWay=� 5&  "   �� 5*  :   5+     5,     � 5-     $��3��6   $
��10 0q�6(    5.     �� 5/     $��30 �6,   �� 50     $��30 �6,   .+�52     �� -   5        $��txt_tablesTitle��53  %   $��tables��52  %   #��[  54  %   $��14(�6O   $��Center=�55  %   $��SemiBold"�6@   $��5,5,5,5q�56  %   �� 57     -    5        $��
txt_tables��58  %   $��1��57  K   #��[  59  %   $��14(�6O   $��Center=�5:  %   $��5,5,5,5q�5;  %   ��57  %   �� *tables+$? OneWay=�5;  %   5<     � 5=     $��4��6   $
��10 0q�6-    5>     �� 5?     $
��auto �6,   �� 5@     $��* �6,   �� 5A     $
��auto �6,   .+�5D     �� -!   5        $��	txt_items��5E  %   $��items��5D  %   $��2��62   #��[  5F  %   $��14(�6O   $��Center=�5G  %   $��SemiBold"�6@   $��5,5,5,5q�5H  %   �� 5I     
�������$��1��67   $��0 0 -5 0q�6D   $
��Auto=�6V   .��5J     �� -"   5        B ��Name$B sp_items��5J  )   $��1��6;   $�� 5 0 q�6H   5K     5L     5M     �� 5O     $��3��6   
�������$ 0 0 7 7��65   ._�5P     � $��3��6   $��5,5,5,10q�6%    5Q     �� 5R     $
��auto �6(   �� 5S     $��* �6(   �� 5T     $��* �6(   .+�5Z     �� 
�������$��6��60   $��1��6E   #��[ 6Q   $��1q�5[     C J� Opacity$C 1Q�6)   $��5,5,5,5q�65   �� 5^     $��1��6!   $��1��6.   ._�5_     �� -#   5        $ btn_save��5_  !   $
��Save��6L   $ 7��5`     #��[  5a     $	��1,1q�5b     
�������{�.  ��5a  E   Z� 5b  7   5c     5f     5h     5j     5k     5m     5n     