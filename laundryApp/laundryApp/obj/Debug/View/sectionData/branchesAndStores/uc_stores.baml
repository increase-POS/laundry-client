   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:laundryApp.View.sectionData.branchesAndStores-laundryApp.View.sectionData.branchesAndStores  5      =    7laundryApp.View.sectionData.branchesAndStores.uc_stores   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Mlocal;clr-namespace:laundryApp.View.sectionData.branchesAndStores     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5        
  � Tag$  stores��5   !   .��5
      � -   5         �Name$ 	grid_main��5
      $��20 10q�6    � ColumnDefinitions 5   
   �� 5      $
��2.5* �6   �� 5      -   5         ��Name$ 	gridWidth��5      $��* �62   $	��225��6<   .+�5   
   �  � RowDefinitions 5      �� 5      $��60 �6    �� 5      $��* �6    .+�5      � $��0 0 10 0q�6    5      �� 5      $
��auto �6'   �� 5      $��2* �6'   �� 5      $��* �6'   �� 5      $
��auto �6(   �� 5      $
��auto �6(   �� 5      $
��auto �6(   .+�5      �� $��40��6   $��50��6%    �� CornerRadius$ 7��60      SecondColor#��[  5        J� Effect 5!      3  -System.Windows.Media.Effects.DropShadowEffect  5"         ShadowDepth$ 1Q�5#         	Direction$	 360Q�5$      	   Color$
	 Gray��5%      
   Opacity$	
 0.3Q�5&         
BlurRadius$ 10Q�5'      ._�5)      F� 
������� �� Stretch$
 Fill=�6(   $��Center=�5*      $��0��6<   $��Center=�5+      $
��17.5��65   $
��17.5��6B   ����p��ffffff0@������lW@���fffff&P@��������LG@ffffff0@������`��`,a 0v�hx�`r��ō�������}��q����f�z��@) ����Z @ sӀ�  ���� `�Z  GS@�� �����y_@ $� ����. G��. G�. G�. G� B� �{�������F@��D�������F@��D� ���$7Ѡ'�@��S���43333U@`�������J@�A � �V `� ظ23333]@@�~�`���k�~�fffff&@@��ffffff0@Q��ffffff0@� K@zA�����LV@43333�T@@�`6 �����LL@@zA�����F@43333sM@�����F@gfffffC@�����LL@hfffff:@S�a     ,@�����LV@������+@ Khfffff:@S��i433333@@���    @D@���43333sH@� 3� Tj��i�����YP@ K@zA�`}�`F�Ӏ`F�gffff�S@@2Ygffff�S@`���gffff�S@�d�43333�S@�����=@  � ��� ����`�y �,�{s@y��`am �������'�Э �-��W`��3 �� ]����������H@S ��������G@@��������F@ M�������E@� B� �� ��`F�`}�`F�	5,      52      �� 53      
�������$��1��6,   $ 10��6<   $��15,10q�6N   $��1q�6_     	LightGrey#��[ 6s    54        55      $ 1Q�56      $	 360Q�57      $
	 Gray��58      $	
 0.3Q�59      $ 10Q�5:      ._�5<      � .+�5=      �� -   5         ��Name$ 	tb_search��5=   "   $��1��65   $��10,0,5,0q�6H    % !MaterialDesignFloatingHintTextBox#��[ 5>   '   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$% Enter Here To Start Search ....��5?   '   ���#4E4E4E �� 	MaxLength$ 50��5A   '   ���#737373$��0q�5C   A   $	��4,0q�6U   .  (MaterialDesignThemes.Wpf.TextFieldAssist   CharacterCounterStyle 5B   &   �� 5C   U   5D      5E      �� 5F      -   5         ��Name$ 
txt_active��5F      $��3��63   $��5,0,5,0q�5G   $   $��Active��65   #��[  6C   $
��Left=�5H   $   $��Center=�6?   $��12(�5I   $   d� 5J      -   5         d�Name$ tgl_isActive��5J   Y   $��4��6     	MainColor#��[ 6/   $��Center=�5K      $��	10,15,5,5q�69    d� 	IsChecked$
 TrueY�6N   $��0q�5L   +   $��13��66   $��28��6B   �� 5M      -   5         ��Name$ btn_refresh��5M   O   $��LeftToRight=�6   $��40��68   $��40��6C   $��0��5O      $��5��6(   $��0q�68   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 15��5P       � Cursor$
 Hand��6I     SmallButtonCircleRotate#��[ 5Q   J    J� RenderTransformOrigin$ 0.5,0.5&�5R       � DataContext$�
 �
M896,0L896,0C401.2,0,0,401.2,0,896v0c0,494.8,401.2,896,896,896h0c494.8,0,896-401.2,896-896v0C1792,401.2,1390.8,0,896,0z   M1451.3,1021c-31.9,133.7-98.8,242.1-200.6,325.2c-101.8,83.1-221,124.6-357.7,124.6c-72.8,0-143.3-13.7-211.4-41.2  s-128.8-66.6-182.2-117.5l-96.5,96.5c-9.5,9.5-20.7,14.2-33.7,14.2s-24.2-4.7-33.7-14.2c-9.5-9.5-14.2-20.7-14.2-33.7v-335.3  c0-13,4.7-24.2,14.2-33.7s20.7-14.2,33.7-14.2h335.3c13,0,24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7c0,13-4.7,24.2-14.2,33.7  l-102.5,102.5c35.4,32.9,75.6,58.4,120.5,76.3c44.9,18,91.5,26.9,139.9,26.9c66.9,0,129.2-16.2,187.1-48.6  c57.9-32.4,104.3-77.1,139.2-134c5.5-8.5,18.7-37.7,39.7-87.6c4-11.5,11.5-17.2,22.4-17.2h143.7c6.5,0,12.1,2.4,16.8,7.1  c4.7,4.7,7.1,10.4,7.1,16.8C1452,1018.2,1451.8,1020,1451.3,1021z M1470.7,752.3c0,13-4.7,24.2-14.2,33.7  c-9.5,9.5-20.7,14.2-33.7,14.2h-335.3c-13,0-24.2-4.7-33.7-14.2s-14.2-20.7-14.2-33.7c0-13,4.7-24.2,14.2-33.7l103.3-103.3  C1083.3,547,996.3,512.9,896,512.9c-66.9,0-129.2,16.2-187.1,48.6s-104.3,77.1-139.2,134c-5.5,8.5-18.7,37.7-39.7,87.6  c-4,11.5-11.5,17.2-22.5,17.2H358.7c-6.5,0-12.1-2.4-16.8-7.1c-4.7-4.7-7.1-10.4-7.1-16.8V771c32.4-133.7,99.8-242.1,202-325.2  c102.3-83.1,222-124.6,359.2-124.6c72.8,0,143.7,13.8,212.5,41.5s130,66.7,183.3,117.1l97.3-96.5c9.5-9.5,20.7-14.2,33.7-14.2  s24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7V752.3z��5S      $��Right=�5^   :   ��5Q      Z� ��61   Z� z�5_      V� 5`      .~�5a      �� �� 5b      �� 5c       �� Angle$ 0Q�6.   T� 5d      5e       � ToolTip 5g      _� 5h      -   5         _�Name$ 
tt_refresh��5h   "   $��Refresh��66    _� 	Placement$
 left=�5i   /    _� HorizontalOffset$ 15��5j   /    _� PlacementRectangle$ 50,0,0,0�5k   /    _� VerticalOffset$ 25��5l   /   5n      5p      �� 5q      $��0,0,10,5q�6   $��1��6'   
�������$ 10��6H    5r        5s      $ 1Q�5t      $	 360Q�5u      $
	 Gray��5v      $	
 0.3Q�5w      $ 10Q�5x      ._�5z      �  5{      �� 5|      $
��auto �6(   �� 5}      .+�5�      � $��1��6   .+�5�      �� -	   5          ��Name$  brd_roundDataGrid��5�   V   $��
5,5,5,17.5q�6!   $��1��67   $ 10��6D   $��0q�6q   
�������5�      &   System.Windows.Controls.DataGrid  5�      -
   5        !  Name$! 	dg_branch��5�      $��5,5,5,45q�5�   $   $��1��66   "   
IsReadOnly" . #   HeadersVisibility$# Column=�6X   $   AutoGenerateColumns$ .  
�������%   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$% SizeToHeader 5�      &   AlternatingRowBackground& �#f3f3f3' �� AlternationCount$' 2��6Z   ( J� OpacityMask( 5�      !� 5�   "   ) !� Visual) 6.   �� * �� ElementName$* brd_roundDataGrid��+ � 	Resources+ 5�      =  7System.Windows.Controls.Primitives.DataGridColumnHeader, �� BasedOn- �� 
TargetType. �� Property/ �� Value��%  (��       5�   2   * SystemColors.HighlightBrushKey+)'       7 7   �� 6"   6f   �� 5�   "   8,   "-  6a   .�5�   &   �� . � ��/ �#F5D7C46C   �� 5�   &   . � ��
/ �����6D   �� 5�   &   . � ��$/ 0 0 1 0q�6H   �� 5�   &   . � ��8/  6C   5�   #   5�      0   Columns0 5�      0  *System.Windows.Controls.DataGridTextColumn  5�   "   ,	  &System.Windows.Controls.DataGridColumn1 	  Width$1 * 65   2 	  Header$
2 code��6@   1
  +System.Windows.Controls.DataGridBoundColumn3 
  Binding3 6N   �� 4 �� Path$
4 code�5 
  ElementStyle5 5�   &   �� 5�   *   "- ��60   6 �� Triggers6 5�   .   �� 5�   2   7 �� Binding7 6>   �� *
isActive+8 �� Value8 06[   .a�5�   6   �� . � ��$/ Strikethrough{�6X   5�   3   5�   +   5�   #     5�   "   $1 * 65   $
2 name��6@   3 6N   �� $
4 name�5 5�   &   �� 5�   *   "- ��60   6 5�   .   �� 5�   2   7 6>   �� *
isActive+8 06[   .a�5�   6   �� . � ��$/ Strikethrough{�6X   5�   3   5�   +   5�   #     5�   "   $1 * 65   $2 mobile��6@   3 6P   �� $4 mobile�5 5�   &   �� 5�   *   "- ��60   6 5�   .   �� 5�   2   7 6>   �� *
isActive+8 06[   .a�5�   6   �� . � ��$/ Strikethrough{�6X   5�   3   5�   +   5�   #     5�   "   $1 * 65   $2 notes��6@   3 6O   �� $4 notes�5 5�   &   �� 5�   *   "- ��60   6 5�   .   �� 5�   2   7 6>   �� *
isActive+8 06[   .a�5�   6   �� . � ��$/ Strikethrough{�6X   5�   3   5�   +   5�   #   5�      5�      �� 5�      $��0,0,0,1q�6   #��[ 67   $��
10,0,10,10q�6`   $
��auto��6t   $��2��5�      $
��Left=�6)   $��Bottom=�6D   ._�5�      �� 9 �� Orientation$9 
Horizontal=�6&   .+�5�      �� -   5        $ btn_pdf��5�   %   $��10,0,5,0q�6P   $��0q�5�      $��35��6'   ��5�   8   Z� ��5�      Z� .��5�   "   '  !MaterialDesignThemes.Wpf.PackIcon  ���#D20A0A:   Kind$: FilePdf=�6O   $��22��5�      $��22��6&    5�   &   _� 5�   *   -   5        $ 	tt_report��5�   2   $��Report��6E   $
 left=�5�   /   $ 5��5�   /   $ 50,0,0,0�5�   /   $	 -35��5�   /   5�   #   5�      �� 5�      -   5        $ 	btn_print��5�   4   $��5,0,5,0q�5�   @   $��0q�5�      $��35��6'   ��5�   '   Z� ��5�      Z� .��5�   "     ���#313131$: Printer=�6O   $��Center=�5�      $��22��5�      $��22��6"    5�   &   _� 5�   *   -   5        $ tt_print��5�   2   $��Print��6D   $
 left=�5�   /   $ 5��5�   /   $ 50,0,0,0�5�   /   $	 -35��5�   /   5�   #   5�      �� 5�      -   5        $ btn_pieChart��5�   B   $��LeftToRight=�5�   %   $��2��6\   $��2.5,0,2.5,0q�6l   $��0q�5�      $��20��6)   $��25��65   ��5�   C   Z� ��5�   (   Z� .��5�   "   � $��20��6'   $��20��62   $��Center=�6>   $��Center=�6[   .+�5�   &   F� $	��8.8��6+   $	��9.6��67   $
 Fill=�6D   ����p �������� @Q`�����0@Q@�4333339@43333�<@43333�6@���    �3@���     0@Ӏ����� ���7�  �������� @	���#DD191B$	 5,5&�5�      z�5�   *   V� 5�   .   .~�5�   2   �� �� 5�   2   �� 5�   2   $ 0Q�6B   T� 5�   2   ; T� X$	; 6.5Q�6F   < T� Y$	< 0.2Q�6N   5�   /   5�   '   F� 5�   &   $	��7.2��6+   $	��7.2��67   $
 Fill=�6D   D����@f��� � Ւ�;f S 6nffffff@`�;`# @f�� �@f��� 	���#F74110$	 0,0&�6�   z�5�   *   V� 5�   .   .~�5�   2   �� �� 5�   2   �� 5�   2   T� 5�   2   $	; 6.2Q�6F   $< -6Q�6N   5�   /   5�   '   F� 5   &   ���#377EB8$��16��6:   $��17��6E   $
 Fill=�6Q   �����u� `�� Ӡu� `�� �u� `�� �u� `�� S �� ������0@ �� ������0@�u� `�� gfffff.@������0@gfffff.@ �� gfffff.@ �� gfffff.@ffffff0@433333.@ffffff0@433333.@�� 433333.@4333330@433333.@@1� 433333.@�����0@433333.@�����0@433333.@ $� 433333.@ $� �433333.@   � ~u @�' �� �9� ��  $� Ӏ�  �i������ @��� $� ������)����_Z`�����@�Ѡu� `�� 	$	 0,0&�5  m   z�5  *   V� 5  .   .~�5  2   �� = �� ScaleX$= 1Q�6A   �� 5  2   �� 5  2   T� 5  2   5  /    5
  *   _� 5  .   -   5        $ tt_pieChart��5  6   $��	Pie Chart��6K   $
 left=�5  /   $ 35��5  /   $ 50,0,0,0�5  /   $	 -35��5  /   5  '   5  #   5     �� 5     -   5        $ btn_exportToExcel��5  7   $��3��5  @   $��5,0,5,0q�6P   $��0q�5     $��35��6)   ��5  '   Z� ��5  (   Z� .��5  "   F� ���#378500$
 Fill=�66   $��20��5     $��20��6&   $��LeftToRight=�5     ��������- � $� ��- � $�  O[ ��c" O[ Q�c"H�z� @Q $� H�z� @� $� ��� ���!��� ���! � � $�  � � $� ��� ���!��� ���! $� � $�  $� � $� �����!�����! -1� $�  -1� $� ��^����^������^���h�X���P�Qр��0; Ӏ���3 �����- ���- � 6n -1� -1 -1� -1��� 6n��� 6n -1� 6n $� � -1 $� � -1��� � 6n��� � 6n $� � 6n � � -1 � � -1��� � 6n��� � 6n � � 6n z � -1 z � -1��[ � 6n��[ � 6n z �    (, �    @�`���� �������     �    (, ��Ɛ  �E�����@Zd;ߏ-@�H$u Щ� U-��o@v��/�,@���x�&@��~j�4,@Q؛r ��~j�4,@��r P�� @�p 0)� �	o ��� ��QT  ����*  �Q8\ �x�&1*@�/ �Ry ��bY �Ry ��MbX9@੧ �x4q `S�  �r ��� p=
ףp@�v���'@Yd;�O�@�v���'@G�z��@|?5^�I'@MbX94@23333�&@k�����@�8� n���!#@������@���K7(@������@�0�Z$"@��� ��A`��b(@�рƐ  �рƐ  �	 5  &   _� 5  *   -   5        $ tt_excel��5  2   $��Export to excel��6D   $
 left=�5  /   $ 35��5  /   $ 50,0,0,0�5   /   $	 -35��5!  /   5#  #   5$     �� 5%     -   5        $ btn_preview��5&  9   $��3��5%  @   $��0,1,5,-2q�6P   $��0q�5&     $
��Auto��6)   ��5%  '   Z� ��5'  (   Z� .��5(  "   � $��LeftToRight=�6'   .+�5)  &   F� ���#4E4E4E$
 Fill=�6:   $��15��5*     $��18��6&   $��LeftToRight=�5+     ������T1 E� 1���A�������dx@�pi�Y:������$q@@��������n@ s� �M�����iu@������Ik@�� �����)k@������t@ ������Tt@S�>/�����'t@@�P������s@`�p������s@S@ 23333+s@�H�����̔r@�9������r@S�	�23333kq@������r@������p@@G.effff�p@S �ceffff�p@gffffft@effff�p@gffffft@����̴p@43333ct@�����om@`�r03333si@`�rcffffve@S`�rcffff�d@`�rcffff�d@��#cffff�d@gffffFo@cffff�d@gffff�n@cffff�d@    `n@������d@    `n@03333�c@�    `n@���     n@ ��    �m@`�����̼m@�m��@���m�Ӏ���m�����m� �{������T@S��������T@�/���Gਤ���ਤ����43333sS@ jM43333sS@�S�ਤ =Q�ਤ�bw�Э �  ����gffff�T@@� 2:�}.gfffffW@�}.��Q��}.�gfffft@�}.�3����gffff^t@�����ix@�`�Z��F�*E��F�T1 E��/��aS� f��T`#Y��T�����T�`�b��T���T��T��H oV43333S^@��T�gffff�]@@�Q     ]@�~<`~�@��43333�\@����� ���j<�����yp@S@��gffffvp@���ఴ@Oఴ� ��ఴ�������q@ �������	r@��������	r@ �� )2�I0 ��aS�/��aS�_ z"S� $gffff�]@�����Y^@�_�3M�_� ���_�43333�i@�_��V�]943333�i@@$gӠ%[�q�����̜i@ �������i@ ��hffffvg@fffff6`@������e@ ��hffff&d@ ���63333s^@ ���63333�]@ ��������\@fffff`@     ]@ �wӠ��@�Z��j<�_ z"��+���	�������^@ E�	 �|���	fffff`@���	������lj@���	�23333�j@���	�ͼ���	������j@@R�	Ӡ���l 
 � �
�����ik@ $H
������ik@@dt
 P��T�
������j@ ��
������j@�����9f@������j@�|�
�����yj@�|�
� E�	�|�
�@�	�|�
�dD�����9f@�����_@�|�
�������^@�|�
�����l^@�����9f@�+ ��
�`� �ۏ
�����u
�����I
�����j
`� �l 
�+���	���Q�D��@���D���s��D���@���d��D��@+��� )2`��gffffr@�&��/�VR    �q@�To    �q@�����9l@gffff�q@�����?l@43333�q@�����<l@������q@�����<l@�����9i@�����<l@63333f@�����<l@������b@�����<l@kffff�_@�����?l@����̬^@�����?l@kffff�]@�����<l@������\@�����	l@73333�\@������k@� q�`��`�D���Q�D��`�K�0�ӀOG@�gffff�i@`���{�����w@gffffFi@�����$w@�}������!w@`;������!w@43333�^@�����!w@43333s]@�����!w@gffff]@���43333�\@�#��    �\@����a�`�s43333s^@`�s43333�`@�����|v@��������|v@433333d@�����|v@gffff�e@�����|v@�W"23333{v@������h@�����|v@43333#i@�����|v@�����li@������v@43333�i@������v@Ӡ�B 5�@wP�M�`�K�0�� ,��/S�Ӵ�����!t@`�c�T1���T1�43333�^@�T1��D�T1�����^@�a443333�]@`�)����gffff]@@s�gffff]@���gffff]@gffff�s@gfffff]@gffff�s@    �]@43333�s@������]@gffffvs@43333s^@���43333�^@����gffff&d@����gffff�e@���gffffFg@���gffff�h@���gffff�h@��gffffi@��gffff6i@���gffff�i@gffff~s@    �i@`��gffff�i@���� U��  5@G. ,��/�`�YCS ������V@`�Q`�����yV@S`����̬[@ ������l`@��@c	����D	`O(����̌c@�����p@����̌c@S ׋������c@ �������c@ 1�������c@Q��=������c@� n����� x�%�`�YC	 5,  *   _� 5-  .   -   5        $ 
tt_preview��5-  6   $��Preview��6J   $
 left=�5.  /   $ 35��5/  /   $ 50,0,0,0�50  /   $	 -10��51  /   53  '   F� 54  &     Blue#��[ 6+   $
 Fill=�6H   $��13��55     $��6��6&   $��	17,10,0,0q�61   $��LeftToRight=�56     ������O|��}�`g� � e`���� �� Թ �V@� ���%����������s@@W��g,@��@ܳ S�V3�����Qr@`{��T�����iS �������t@ ���������#� %��^�fffffp@�����lv@`s ��S ��������w@���P����@#S������̬v@����k�@w�@����0�  ��#��Ҧ�O|��}� @�����'@����\�� \[��������1r@gffff�s@ M&��������t@��������u@������r@������u@ ��������u@�I��@���)�@k"@�� @���@?S �����T`g�~��i��%�i�������is@�i�������r@��e������r@�<��������r@�������ds@ *������t@�����������Q��@?S ��	58  '   59  #   5:     �� 5;     -   5        $ 	txt_count��5;  @   $��4��6U   > �� Padding$> 10q�5<     $��0��6,   ���#378500$��12(�6J   ��5;  *   Z�  5=  "   _� 5>  &   -   5        $ tt_count��5>  .   $��Customers Count��6@   $
 left=�5?  /   $ 35��5@  /   $ 	50,25,0,0�5A  /   $	 -45��5B  /   5D     5F     5G     5I     5J     5K     �� 5L  
   $��	10 10 0 5q�6   $��1��6$    5M       5N     $ 1Q�5O     $	 360Q�5P     $
	 Gray��5Q     $	
 0.3Q�5R     $ 10Q�5S     ._�5V     �  5W     �� 5X     $
��auto �6    60   �� 5Y     $��* �6    6-   �� 5Z     $
��auto �6    60   �� 5[     $
��auto �6    60   .+�5]     �� #��[ 6   $ 7 7 0 0��6@   ._�5_     � $��40��6   $
��10 0q�6$    5`     �� 5a     $
��auto �6+   �� 5b     $
��auto �6+   �� 5c     $��* �6+   �� 5d     $
��auto �6+   .+�5f     F� 
�������$
 Fill=�6(   $��Center=�5g     $��20��5h     $��20��6$   $��LeftToRight=�60   ������K�. ��Ӡ>�.@� ��. �� $�. �a� $�.`�3 ��.�w �.�D��������@�� ���,�!�	����̼�@@H[��3*&���s������g�@�W��43333�w@�@��gffff�u@����gfffft@�����gffff�s@ ��    �s@����    �s@����̌C�    Pq@����̌C�43333�p@�����?C�    Xp@23333�B�hffff�o@��������g�`�
 ��^�;�� Ǡ�`P � �3���`��	`z+�43333#i@�����̌g��.��	��43333�k@����̼g�    @m@S &��������n@������g������Yp@������g�gffff6q@������g�gffff^q@����	�������g��/��������g� �4�������?� �4�������?���=� �n%��=� �n%�w�4� ��.�w�4� ��.�Pp����.�|?�K�.�/�K�. ���@��@+рDF	@+рDF	 ��@�� ��@��@+��5�@+�@��@+�@�� �р5� �р5�@+� F�#�� A�� A@+� F�#@+� F�#��@
| ���'ӀL� ��X(��� ���(��� �ى)�@t� @6�, ӌ ������@ ӌ �hk2� ӌ ��W3 � ���3�cL�����i�@S 3�����x�@�F������{�@��e�����}�@S���������@�j��������@@_���y4�@����y4�� �4����w�4Ѡ�l�w�4S���������@ F��y443333�z@�������@43333y@�����}�@43333#w@�i443333;u@ �b4�43333�t@�la443333#t@��04��������%�@S�D�������@ �{ �N3�9z�������@S`�r������@ �g�����M�@��d�������@43333Cs@�����S�@�;] �* �[�����ф@� �[@��'�}l�`['@a��������@S 1������8�@�ǆ23333�@43333Ku@������@    �v@�������@gffffvx@`$&43333z@��"&�43333z@��"& ������ �@@T��3*&�@T�`W,�43333C{@`W,�43333C{@�&'&� �`$&    x{@ !&gffff�{@ !&�43333�|@ !&@�� !&gffffv@43333��@S��P43333��@������@43333�@gffff6�@    �@��������@@LM&��f �
�&@
| ���'����B_%� zW�Z%43333�p@�5\%`Md�5\%� #��5\%@v�
���$�]�
 �$$���
`��#�����If@�C#�����If@�����A�@S ��
�������@��
23333�}@��
����̔z@S��
������y@@��
�����Wy@�-������x@�@� �X��`��2^`�� �`�`��`�`)`�Q`)����̴~@Q !W����̴~@Q !W�����$x@������u@ ���������1x@ �L�����Dx@S@e�����Lx@`!������,y@�c�������y@S`�������lz@��������z@�������W{@S��������t~@�����z@�����Ȁ@�c� b�"S�c������}�@�����z@ ލ#������y@ �#� ߺ`;T$23333�y@�����=�@�����?y@ �$�`^�@�@% �L�B_%���B_%��8��O(Ӡ� ��+�z'�����̈@@�-�������@��/��|3��k�nD4ffffffp@�����m�@�23333n@ �b4�����Yk@�����}�@������h@`5r4�����d@�������@�����,_@`5r4�����V@�����}�@������T@�����}�@dffff�S@@�j4�����LR@�����o�@������M@�����O�@cfffffI@23333��@�����I@23333��@������H@23333�@������H@23333��@����̌H@23333�@/3333sH@�������@cffff�H@�����`�@cfffffH@23333�@�����LH@����̔�@������O@23333�@�����?S@effff�@dffffV@effff�@13333�X@23333��@������[@effff��@������_@�������@�m�effff��@������c@effff��@�������c@ �X,������)f@ �X,������)f@ !&������_h@ !&�����yj@��&23333�l@gffff��@������m@43333��@�����Io@43333��@�����Op@    �@23333q@������@effff�q@�����h�@������q@43333��@� �0�'�8��
(�8��O(	5i     �� 5z     -   5        $ 	txt_title��5z  >   $��1��6    $��Stores��60   $��10,0,0,0q�6Q   
�������$��14(�5{     $��Center=�6(   $
��Bold"�5|     �� 5}     -   5        $ 	btn_clear��5}     $��3��6J   $��0q�6Z   $��Center=�5~      ��6;   Z� ��6Q   Z� .��5       
�������$: RefreshCircle=�6I   $��30��6`   $��30��6k   5�     5�     5�     �� 5�     
�������$��1��6/   $
��Auto=�6>   .��5�     �� .+�5�     ]� $
��10 0q�6    ��. .+�5�     �� -   5        $ txt_baseInformation��5�  $   $��Base Infomation��6A   #��[  6X   $��12(�6�   $
��Bold"�6�   $��Center=�6�   $��5,5,5,5q�6�   �� 5�     #��[ 6%   $��Center=�6M   $
��auto��6h   5�     �� 5�     $��2��6   $ 10��63   $��5,5,10,5q�6E   $��1q�6Y   #��[ 6m   ._�5�     � .+�5�     �� -   5        $ tb_code��5�  &   $��10,0,5,0q�6[   #��[ 5�  '   $ code...��5�  '   ���#4E4E4E$ 50��5�  '   $��0q�66   $��40��5�  &   $	��4,0q�64   ��5�  8   �� *code+? �� Mode$? OneWay=� 5�  &   �� 5�  F   F� 5�     -   5        @ F�Name$@ p_error_code��5�  U   ���#E81123$
 Fill=�64   $��5,5,15,5q�6C   $��Right=�5�     $y�	Collapsed=�6;   $��Center=�5�     $��15��65   $��15��6A     
infoCircle#��[ 5�     5�     5�     5�     �� 5�     $��1��6   $ 10��6*   $��5,5,10,5q�6<   $��1q�6P   #��[ 6d   ._�5�     � .+�5�     �� -   5        $ tb_name��5�  &   $��10,0,5,0q�6[   #��[ 5�  '   $ name...��5�  '   ���#4E4E4E$ 50��5�  '   $��0q�66   $��40��5�  &   $	��4,0q�64   ��5�  8   �� *name+$? OneWay=� 5�  &   �� 5�  F   F� 5�     -   5        $@ p_error_name��5�  U   ���#E81123$
 Fill=�64   $��5,5,15,5q�6C   $��Right=�5�     $y�	Collapsed=�6;   $��Center=�5�     $��15��65   $��15��6A   #��[ 5�     5�     5�     5�     �� 5�     $��3��6   $ 10��6*   $��5,5,10,5q�6<   $��1q�6P   #��[ 6d   ._�5�     � .+�5�     �� -   5        A ��Name$A cb_parentId��5�  '   ���#4E4E4E$	��4,0q�5�  *   $ Parent Branch...��5�  '    & "MaterialDesignFloatingHintComboBox#��[ 5�  $   $��10,0,5,0q�5�  %   $��40��5�  '   $��0q�5�  '     Font-cairo-light#��[ 5�  '   $��3��5�  '   B �� SelectedValueB 5�  <   �� *
parentId+$? OneWay=�5�  5   F� 5�     -   5        $@ p_error_parentId��5�  U   ���#E81123$
 Fill=�64   $��5,5,20,5q�6C   $��Right=�5�     $y�	Collapsed=�6;   $��Center=�5�     $��15��65   $��15��6A   #��[ 5�     5�     5�     5�     ]� 5�     $
��10 0q�6    ��. $��4��6C   .+�5�     �� -    5        $ txt_contactInformation��5�  B   $��Contact Information��6%   #��[  5�  #   $��12(�6M   $
��Bold"�6[   $��Center=�6m   $��5,5,5,5q�6�   �� 5�     #��[ 6$   $��5,0,5,0q�6O   $��1��5�  '   $��Center=�67   5�     � 5�     $��5��6    5�     �� 5�     $
��auto �6/   6>   �� 5�     $��* �6/   6;   .+�5�     �� $��1��6!   $��0��6.   $ 10��6>   $��10,5,5,5q�6P   $��1q�6d   #��[ 6x   ._�5�     � .+�5�  "   �� -!   5        $A cb_areaMobile��5�  -   $��10,0,5,0q�6D   #��[ 5�  -   ���#4E4E4E$	��4,0q�5�  L   $��Center=�5�  +   $��0q�6F   $��40��5�  +   $��60��67   5�     5�     �� 5�     $��1��6!   $��1��6.   $ 10��6>   $��5,5,10,5q�6P   $��1q�6d   #��[ 6x   ._�5�     � .+�5�  "   �� -"   5        $ 	tb_mobile��5�  7   $	��4,0q�6*   #��[ 5�  *   $��10,0,5,0q�5�  O   $��0q�6d   $ 	Mobile...��5�  *   ���#4E4E4E$ 9��5�  *   $��40��5�  k   $	  int��5�  (   ��5�  *   �� *mobile+$? OneWay=� 5�  *   �� 5�  O   F� 5�  "   -#   5        $@ p_error_mobile��5�  H   ���#E81123$
 Fill=�66   $��Right=�5�  '   $y�	Collapsed=�6C   $��Center=�6\   $��15��5�  '   $��15��63   $��5,5,15,5q�6?   #��[ 5�  '   5�  #   5�     5�     5     � 5     $��1��6   $��6��6-    5     �� 5     $
��auto �6/   �� 5     $
��auto �6/   �� 5     $��* �6/   .+�5     �� $��1��6!   $ 10��6.   $��10,5,5,5q�6@   $��1q�6T   #��[ 6h   ._�5	     � .+�5
  "   �� -$   5        $A cb_areaPhone��5
  +   $��10,0,5,0q�6A   ���#4E4E4E#��[ 5  -   $��40��5     $	��4,0q�6$   $��60��63   $��0q�6@   5  '   5     5     �� 5     -%   5        $  brd_areaPhoneLocal��5  8   $y�	Collapsed=�6!   $��1��6T   $��1��6a   $ 10��5  !   $��5,5,5,5q�63   $��1q�6F   #��[ 6Z   ._�5     � .+�5  "   �� -&   5        $A cb_areaPhoneLocal��5  ,   $��10,0,5,0q�6G   #��[ 5  -   ���#4E4E4E$	��4,0q�5  8   $��40��5     $��60��6&   $��0q�64   5     5     �� 5     $��1��6!   $��2��6.   $ 10��6>   $��5,5,10,5q�6P   $��1q�6d   #��[ 6x   ._�5     � .+�5  "   �� -'   5        $ tb_phone��5  *   $��10,0,5,0q�6c   #��[ 5   '   $��0q�5!  &   $ Phone...��6:   ���#4E4E4E$ 9��5#     $��40��6'   $	��4,0q�65   $	  int��5$  (   ��5  =   �� *phone+$? OneWay=� 5"  3   �� 5$  _   F� 5%  "   ���#E81123$
 Fill=�66   $��5,5,15,5q�6E   $��Right=�5&     $y�	Collapsed=�6>   $��Center=�5'     $��15��65   $��15��6A   #��[ 5(     5)  #   5*     5+     5-     �� 5/     $��8��6   $ 10��6*   $��	10,5,10,5q�6<   $��1q�6Q   #��[ 6e   ._�50     � .+�51     �� -(   5        $ tb_email��51  &   $��10,0,5,0q�6]   #��[ 52  '   $ email...��53  '   ���#4E4E4E$ 50��55  '   $��0q�66   $��40��57  &   $	��4,0q�63   ��51  9   �� *email+$? OneWay=� 56  &   �� 58  J   F� 59     -)   5        $@ p_error_email��59  U   ���#E81123$
 Fill=�64   $��5,5,15,5q�6C   $��Right=�5:     $y�	Collapsed=�6;   $��Center=�5;     $��15��65   $��15��6A   #��[ 5<     5=     5>     5?     �� 5@     $��9��6   $ 10��6*   $��	10,5,10,5q�6<   $��1q�6Q   #��[ 6e   ._�5A     � .+�5B     �� -*   5        $ 
tb_address��5B  '   #��[ 5C  #   $��10,0,5,0q�5D  #   $ 
address...��5F  #   ���#4E4E4E$	��4,0q�5H  "   $��0q�60   $��1��5I     $��70��6.   C �� TextWrapping$
C Wrap=�6:   ��5B  ;   �� *	address+$? OneWay=� 5E  "   �� 5I  :   F� 5J     ���#E81123$��8��62   $
 Fill=�6?   $��5,5,15,5q�6N   $��Right=�5K  #   $��2��6?   $y�	Collapsed=�6P   $��Center=�5L     $��15��69   $��15��6E   #��[ 5M  "   5N     5P     5Q     ]� 5R     $
��10 0q�6    ��. $��2��6C   .+�5S     �� -+   5        $ txt_contentInformatin��5S  ?   $��More Information��6%   #��[  6^   $��12(�6�   $
��Bold"�6�   $��Center=�6�   $��5,5,5,5q�6�   �� 5T     #��[ 6$   $��5q�6N   $��Center=�6Z   5U     �� 5X     $��10��6   $ 10��6+   $��	10,5,10,5q�6=   $��1q�6R   #��[ 6f   ._�5Y     � .+�5Z     �� -,   5        $ tb_notes��5Z  '   #��[ 5[  #   $��10,0,5,0q�5\  #   $ Note...��5^  #   ���#4E4E4E$	��4,0q�5`  "   $��0q�60   $��1��5a     $��70��6.   $
C Wrap=�6:   ��5Z  9   �� *notes+$? OneWay=� 5]  "   �� 5a  :   5b     5c     5g     5h     �� 5j     $��3��6   
�������$ 0 0 7 7��65   ._�5k     � $��3��6   $��5,5,5,10q�6%    5l     �� 5m     $
��auto �6(   �� 5n     $��* �6(   �� 5o     $��* �6(    5q     �� 5r     $��* �6+   �� 5s     $��* �6+   �� 5t     $��* �6+   .+�5v     �� 
�������$��3��60   $��1��6E   #��[ 6Q   $��1q�5w     D J� Opacity$D 1Q�6)   $��5,5,5,5q�65   �� 5x     $��1��6   $��3��6*   ._�5z     �� --   5        $ 
btn_stores��5z  !   $��Stores/Branch��67   $ 7��6O   #��[  5{     $	��1,1q�5|     
���������5{  E   Z� 5|  ;   5}     �� 5~     $��2��6   ._�5�     �� -.   5        $ btn_add��5�  !   $ 7��63   #��[  5�     $	��1,1q�5�     
���������5�  E   Z� .��5�     �� $9 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�  "     -/   5        $��txt_add_Icon��5�  :   $: 	PlusThick=�6N   
�������$��Center=�6r   $��15��6�   $��15��6�   $��0,1,2,0q�6�   5�  #   �� 5�  "   -0   5        $��txt_addButton��5�  C   $y�	Collapsed=�6,   $	��Add��6X   
�������$��Center=�6v   5�  #   5�     5�      5�     _� 5�     -1   5        $ tt_add_Button��5�  &   $	��Add��6=   $
 left=�5�  %   $ 15��5�  %   $ 50,0,0,0�5�  %   $ 30��5�  %   5�     �� 5�     $��2��6   $��1��6*   ._�5�     �� -2   5        $ 
btn_update��5�  !   $ 7��67   #��[  5�     $	��1,1q�5�     ��5�  C   Z� .��5�     �� $9 
Horizontal=�6)   $��Center=�6B   $��Center=�6_   .+�5�  "     -3   5        $��txt_update_Icon��5�  :   
�������$
: Edit=�6d   $��Center=�6p   $��15��6�   $��15��6�   $��0,1,2,0q�6�   5�  #   �� 5�  "   -4   5        $��txt_updateButton��5�  V   
�������$y�	Collapsed=�6?   $��Update��6n   $��Center=�6|   5�  #   5�     5�      5�     _� 5�     -5   5        $ tt_update_Button��5�  &   $��Update��6@   $
 left=�5�  %   $ 15��5�  %   $ 50,0,0,0�5�  %   $ 30��5�  %   5�     �� 5�     $��2��6   $��2��6*   ._�5�     �� -6   5        $ 
btn_delete��5�  !   $ 7��66   #��[  5�     
�������$	��1,1q�5�     ��5�  C   Z� .��5�     �� $��Center=�6)   $9 
Horizontal=�6F   $��Center=�6_   .+�5�  "     -7   5        $��txt_delete_Icon��5�  :   $: Delete=�6Q   
�������$��Center=�6r   $��15��6�   $��15��6�   $��0,1,2,0q�6�   5�  #   �� 5�  "   -8   5        $��txt_deleteButton��5�  C   $y�	Collapsed=�6,   
�������$
��auto��6n   $��Delete��6{   $��Center=�6�   5�  #   5�     5�      5�     _� 5�     -9   5        $ tt_delete_Button��5�  &   $��delete��6@   $
 left=�5�  %   $ 15��5�  %   $ 50,0,0,0�5�  %   $ 30��5�  %   5�     5�     5�     5�     5�     5�     5�     