   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   .    (laundryApp.View.windows.wd_itemsUnitList   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5          � WindowStyle$
  None=�5	   
    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5
   	   $��0q�6    � Title$ Increase POS��5   	   $	��400��5   	   $	��550��6   
������  � AllowsTransparency . .��5      � -   5         �Name$ grid_offerList��5   )   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   1   $��Center=�6M   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   h   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $��30��6   $��Center=�6'    � ColumnDefinitions 5       �� 5!      $��* �6/   �� 5"      $
��1.2* �6/   .+�5$      F� 
������� �� Stretch$
 Fill=�6,   $	��5,0q�6;   $��LeftToRight=�6H   $��Right=�5%      $��0��6;   $��Center=�5&      $��20��65   $��20��6@     rawMaterials#��[ 5'      �� 5(      -   5         ��Name$ 	txt_title��5(   1   $��Items��6$   
�������$
��Left=�5)   $   $��Center=�6?   $��1��5*      $	��5,0q�64   5+      5,      5.      50      �� 51   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��52      ���#999999
�������._�53      � -   5        $ grid_mainGrid��53       54      �� 55      $��* �6'   �� 56      $
��auto �6'   �� 57      $��* �6'    59      �� 5:      $
��auto �6$   �� 5;      $
��auto �6$   �� 5<      $��30 �6$   �� 5=      $��* �6$   �� 5>      $��45 �6$   .+�5@      �� $��0��6   $��40��6*   $ 10��66   $��	10,5,10,5q�6H   $��1q�6]     	LightGrey#��[ 6q   ._�5A      � .+�5B      �� -   5         ��Name$ txb_searchitems��5B   5   $	��225��6(   $��10,0,5,0q�5C      ���#737373$��0q�5D   8   $	��4,0q�6L   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$% Enter Here To Start Search ....��5E      ���#4e4e4e$��14(�5F   '   $
��Left=�5G      $��Center=�61    % !MaterialDesignFloatingHintTextBox#��[ 5H   ,   $��40��5I      .��6    
 5J   "   _� 5K   &   -   5         _�Name$ 	tt_search��5K   .   $��Search��6A    _� 	Placement$
 left=�5L   %    _� HorizontalOffset$ 15��5M   %    _� PlacementRectangle$ 50,0,0,0�5N   %    _� VerticalOffset$ 30��5O   %   .��5P   3   5Q      � 5R      $��0,0,5,0q�6#   $��Right=�65   $��Center=�6Q   
�������$��30��6�   $��30��6�   .+�5S   "   F� ���#4e4e4e$
 Fill=�66   $��5,2.5,7.5,0q�6H   $��0��5T      $
��Left=�60   $��Center=�5U      $
��17.5��65   $
��17.5��6B   ����p��ffffff0@������lW@���fffff&P@��������LG@ffffff0@������`��`,a 0v�hx�`r��ō�������}��q����f�z��@) ����Z @ sӀ�  ���� `�Z  GS@�� �����y_@ $� ����. G��. G�. G�. G� B� �{�������F@��D�������F@��D� ���$7Ѡ'�@��S���43333U@`�������J@�A � �V `� ظ23333]@@�~�`���k�~�fffff&@@��ffffff0@Q��ffffff0@� K@zA�����LV@43333�T@@�`6 �����LL@@zA�����F@43333sM@�����F@gfffffC@�����LL@hfffff:@S�a     ,@�����LV@������+@ Khfffff:@S��i433333@@���    @D@���43333sH@� 3� Tj��i�����YP@ K@zA�`}�`F�Ӏ`F�gffff�S@@2Ygffff�S@`���gffff�S@�d�43333�S@�����=@  � ��� ����`�y �,�{s@y��`am �������'�Э �-��W`��3 �� ]����������H@S ��������G@@��������F@ M�������E@� B� �� ��`F�`}�`F�	5V      5\      5]      5^      �� 5_      $��1��6   $��0��6$   $��3��65   $
 Fill=�6I   ���#DFDFDF	 �� X2$ 1��6j   $��1,1,1,1q�6r   $��Bottom=�5`      �� 5a      -   5        $ 	txt_items��5a   )   $��Items��6   $��2��6<   $��Center=�6I   $��Center=�6f   ���#999999$��SemiBold"�5b   1   $��16(�6H   �� 5c      $��3��6   $��10q�6'   ���#999999$��1q�6K   ._�5f      &   System.Windows.Controls.DataGrid  -	   5          Name$ dg_allItems��5f      
�������$y�Visible=�5g         HeadersVisibility$
 None=�6*      
IsReadOnly .     CanUserAddRows .     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 5j         Columns 5k      0  *System.Windows.Controls.DataGridTextColumn  5l      ,  &System.Windows.Controls.DataGridColumn   Width$ * 62      
IsReadOnly .     Header$
  Item��6O   1  +System.Windows.Controls.DataGridBoundColumn!   Binding! 6]   �� " �� Path$" itemName�5n      5o      � 5q      $��3��6   $��1��6$    5r      �� 5s      $��* �6(   �� 5t      $
��auto �6(   �� 5u      $
��auto �6(   �� 5v      $
��auto �6(   �� 5w      $
��auto �6(   �� 5x      $��* �6(   .+�5|      �� -
   5        $ btn_selectedAll��5}   I   $��2.5,7.5q�5|      $��4��6.   $��1��6>   $��Center=�6K   $��25��5}   %   $��25��61   $��0q�6=   ���#686868���#DFDFDF$��0q�5~   V   .��5      F� ���#686868$
 Fill=�6.   $��Center=�5�      $��0��6<   $��Center=�5�      $��12��65   $��12��6@   �������s ��* р�s  � Ӏ�s �y� ������@ �� `Vl  �� Ӏ�g  �� @�d �y� @�d  � �@�d ��* �@�d �%& ��g `# `Vl `# �@�p `# ��s �%& ��s ��* � �a  �a 433333@     @S �0 ������@gfffff@     @gfffff@������@�gfffff@�ا �gfffff@� � `M/  � 433333@� � Q �a ������@�@�d �co @�d �;f  �a  �a 	5�      
 5�      _� 5�      -   5        $ tt_selectAllItem��5�   &   $
 left=�5�   %   $ 15��5�   %   $ 50,0,0,0�5�   %   $ 30��5�   %   5�      �� 5�      -   5        $ btn_selectedItem��5�   I   $��2.5,7.5q�5�      $��4��6/   $��2��6?   $��Center=�6L   $��25��5�   %   $��25��61   $��0q�6=   ���#686868���#DFDFDF$��0q�5�   V   .��5�      F� ���#686868$
 Fill=�6.   $��Center=�5�      $��0��6<   $��Center=�5�      $��12��65   $��12��6@   ����� u8 �%& ��g5 ��! `M/ �%& `M/ `M/ �`M/ �ا �`M/ � � @�3  �  u8 � � Q�;f ������@S Ii 233333@ Ii ������@�;f ������@� u8 �%& 	5�      
 5�      _� 5�      -   5        $ tt_selectItem��5�   &   $
 left=�5�   %   $ 15��5�   %   $ 50,0,0,0�5�   %   $ 30��5�   %   5�      �� 5�      -   5        $ btn_unSelectedItem��5�   I   $��2.5,7.5q�5�      $��4��6.   $��3��6>   $��Center=�6K   $��25��5�   %   $��25��61   $��0q�6=   ���#686868���#DFDFDF$��0q�5�   V   .��5�      F� ���#686868$
 Fill=�6.   $��Center=�5�      $��0��6<   $��Center=�5�      $��12��65   $��12��6@   �����`!` � � Ӡ.c `�� ������@� � ������@�ا р�g `M/ Ӏ�g �%& �.c    ��^ �%& � �0  �a S��- 433333@��- @�p  �0  ~u �`!` � � 	5�      
 5�      _� 5�      -   5        $ tt_unselectItem��5�   &   $
 left=�5�   %   $ 15��5�   %   $ 50,0,0,0�5�   %   $ 30��5�   %   5�      �� 5�      -   5        $ btn_unSelectedAll��5�   I   $��2.5,7.5q�5�      $��4��6.   $��4��6>   $��Center=�6K   $��25��5�   %   $��25��61   $��0q�6=   ���#686868���#DFDFDF$��0q�5�   V   .��5�      F� ���#686868$
 Fill=�6.   $��Center=�5�      $��0��6<   $��Center=�5�      $��12��65   $��12��6@   ����� @, �l� Q @, hfffff@S @,      @`M/ hfffff@�Z2 hfffff@������@hfffff@��9      @��9 hfffff@���9 �l� S��9 433333'@������@�� �Z2 �� �`M/  ��  @, �y�  @, �l� � 	=  ~u ���j � � � �m `�� ��s � � ��s �ا Q��s ������@S��s ������@�co ������ @��j ������@� 	= �.c ���9 �;f ��9 @�p  	=  ~u 	5�      
 5�      _� 5�      -   5        $ tt_unselectAllItem��5�   &   $
 left=�5�   %   $ 15��5�   %   $ 50,0,0,0�5�   %   $ 30��5�   %   5�      5�      �� 5�      -   5        $ txt_selectedItems��5�      $��Selcted items��67   $��2��6L   $��2��6Y   $��Center=�6i   $��Center=�6�   ���#999999$��SemiBold"�5�   1   $��16(�6H   �� 5�      -   5        # ��Name$# bdr_general��5�      $��10q�6/   $��3��6;   $��4��6I   ���#999999$��1q�6p   ._�5�        -   5        $ dg_selectedItems��5�      
�������$y�Visible=�5�      $
 None=�6*    .   .   .  $ SizeToHeader 5�       5�        5�      -   5        item��$ 3* 5�   ?    . $
  Item��6]   ! 6k   �� $" itemName�  5�      -   5        note��$ * 5�   ?    . $
  Item��6]   ! 6k   �� $" notes�  5�      $ * 61   -   5        quantity��$  Quantity��5�   M   ! 6_   �� $" quantity�5�      5�      �� 5�      -   5        $ btn_save��5�   )   $
��Save��5�      $��5��6(   $��5��65   
�������$��10,2.5q�5�      +  %MaterialDesignThemes.Wpf.ButtonAssist$   CornerRadius$$ 7��6;   #��[  5�      ��6C   Z� 5�      5�      5�      5�      5�      