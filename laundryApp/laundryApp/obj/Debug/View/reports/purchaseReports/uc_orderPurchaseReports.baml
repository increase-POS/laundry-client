   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:laundryApp.View.reports.purchaseReports'laundryApp.View.reports.purchaseReports  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5      E    ?laundryApp.View.reports.purchaseReports.uc_orderPurchaseReports   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Glocal5clr-namespace:laundryApp.View.reports.purchaseReports     5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
      -   5        
  � Tag$  orderPurchaseReports��5       .��5      � -   5         �Name$ 	grid_main��5      $��20 10q�6    � ColumnDefinitions 5   
   �� 5      $��2* �6   6,   �� 5      $
��auto �6   6/   �� 5      $��1* �6   6-   .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $
��auto �6    60   �� 5      $��* �6    6-   .+�5      �� $��	10,10,5,5q�6    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5          Opacity$	 0.3Q�5!      	   
BlurRadius$	 10Q�5"      ._�5$      �  5%      �� 5&      $��* �6$   �� 5'      $
��auto �6$   .+�5)      �� 
 �� CornerRadius$
 7,7,0,0��6   $��52��61   $��0��5*         SecondColor#��[  5,      ��5+      Z� ._�5-      � -   5        $ grid_tabControl��5-       5.      �� 5/      $
��auto �6/   �� 50      $
��auto �6/   �� 51      $
��auto �6/   �� 52      $
��auto �6/   �� 53      $��* �6/   �� 54      $
��auto �6/   .+�59      �� -   5         ��Name$ 
bdr_branch��59   !   $��0��65   
�������$��0q�6X   $��40��6m   $��60��6y   $��Bottom=�6�   $
 7,7,0,0��6�   
������ $��	10,5,0,-1q�6�   ._�5:      �� -   5         ��Name$ 
btn_branch��5:   %   
������ $  branch��6T   +  %MaterialDesignThemes.Wpf.RippleAssist   Feedback
 ���� 
������ $
��auto��5;   r    � Triggers 5<   "   :� 5=   &   -   5        trig�� :� RoutedEvent$ Button.Click��5=   A   .[�5>   *   �� .��5?   .   �� .��5@   2   W�  �� 
TargetName$ bdrMain��5A       �� TargetProperty$ Opacity�5B       W� From$ 0X�5C      	 W� To$ 1X�6"    g� Duration$ 0:0:1D�5D      5E   /   5F   +   5G   '   .��5I   "   F� -   5        $��path_branch��5I   '   #��[  6:    �� Stretch$
 Fill=�6^   $	��Top=�6m   $��Center=�6�   $��0,-1,0,0q�6�   $��25��5J   (   $��25��63   $��LeftToRight=�6@   �	����(L d�`�� dӀ�� d�ʵ@��ʵ`|$��ʵ`����ʵ��������
`����
Ѡ(L��
ӀR8��
@)���@)`���@)`|$�@)@��R8 d�(L dӠ(L d�(L d�(L d�@aN`˅� �F`˅ ?`˅�}7�D���}7@zA��}7@zAQ�}743333�Y@S�}7    �Y@�}7������Y@�}7@�)�`�y@�)S`�ygffff�X@`�y`I`�y�������̌A@��
�����̌A@@�)рh@�)Ӏh �&�h��#�h43333�Y@рh� Cрh� Cрh`˅�`z``˅@�X R� 8Q R��43333�3@ R�4333331@�     .@gffff�L@����  3�Ѡ�� 3�Ѡ���np��%��@ s`˅@aN`˅���� 9�Q :������1@Q`_� �����1@�@w  9��@w  9���  9�@w  9�Ӏ�[ ������ �)���� �i��������+@@@�� ������I@������.@������I@� �8�.D�����;@�����K@�����L?@�i�Ӏ������:������F@����̌D@ 9��23333sF@ 9������LH@������F@�����J@���efffffK@�i��23333�M@ �E� �E�����LR@�i��������R@���������S@������F@`� 9��������U@ 9�S@rf������F@�����W@��������,X@�i�������9Y@ �9������Z@ �E�#�������I@�����Y\@������I@����̌\@�����LI@������\@�i�Ӏ��)���E������ 9�� �� 9���� 9���� 9�	 � ToolTip 5R   &   _� 5S   *   -   5         _�Name$ 	tt_branch��5S   2   $��Branch��6E    _� 	Placement$
 left=�5T   /    _� HorizontalOffset$ 35��5U   /    _� PlacementRectangle$ 50,0,0,0�5V   /    _� VerticalOffset$	 -10��5W   /   5Y   #   5Z      5[      �� 5\      -	   5        $ bdr_pos��5\   !   $��1��62   #��[  6B   $��0q�6l   $��40��6�   $��60��6�   $��Bottom=�6�   $
 7,7,0,0��6�   
������ $��0,5,0,-1q�6�   ._�5]      �� -
   5        $ btn_pos��5]   %   
������ $	  pos��6Q   
 ���� 
������ $
��auto��5_   ?    5`   "   :� 5a   &   $ Button.Click��63   .[�5b   *   �� .��5c   .   �� .��5d   2   W� $ bdrMain��5e      $ Opacity�5f      $ 0X�5g      $ 1X�6"   $ 0:0:1D�5h      5i   /   5j   +   5k   '   .��5m   "   F� -   5         F�Name$ path_pos��5n   ^   
�������$
 Fill=�5m   4   $	��Top=�6D   $��0,-1,0,0q�6\   $��Center=�6n   $��25��5n   (   $��25��63   $��LeftToRight=�6@   �����@�� � �@��@� ����ا ���ا ��S�ا Ӏ?\�ا  Zb@�  Zb � � Zb�B� �?\@]� �S@]� р�@]� ����@]� @���B� @�� � ���@f��S@fӀ?\@f Zb������0@ Zb $� � Zb@�� �?\��� �S��� р���� ������� @��@�� @�� $� S@��������0@���@f��@f��u���рu�u�    �u�    ���рu���� -1��7� -1��.�+@�(��!@�(�@KL @�(Ӏ#C @�( 	= ��. 	= ��7� 	= @!A�#C �;G@KL �;G���!�;GӀ+�;G -1@!A -1��7��;G��7��;G��.@!A@�(��7@�(�@�(@�(Ӏ�@�( u��. u��7� u@!A���;G@�(�;Gр�7�;G�@!A�;G�;G@!A�;G��7� 	= ���р�7���р�7��� 	= ��� 	= ������!@��р�@�����	@��@f���@f����@f@����	���������!��Ӏ+�� -1@�� -1���� -1����+@����!@���`3�  �� &� `"���� �/���� ���Ӏ�� `W� &� �d�`3� �q�Ӡ@�  �gfffff-@����� ���������.@�����  � �� �q��������/@�d� $� `W� $� ���� $� �/�`�� `"� ��  �Ӡu� ����M� ���`3�  ����� ������!���Ӏ+��� -1 �� -1@x}� -1�Pt�+ 6n��! 6nр��  6n��w�  6n@]� �Pt@]� @x}�@]�  ���w� ������ ����`<``� /�m���z���Ӏ��� /�`< � S�I43333�A@�V Q%��! Q%Ӡ~& Q%��)`�# �, � S`�/    �A@ -1  -1�� -1�`�/�m �,``Ӡ~&�E�V�E`<``� ~V@1� � ~V ��� ~V`F���N���.D��� �0 ��Ӡ%& ���� `F���  ��р� @1� Ӏ� ��� �%& ���  �0 `h� � � `h� � � ��� � $� ��� � $� `h� р�`h� ����`h�  ?��@�  ?��� � ?� �m � ?��.c  ���[ �����[ р%��[ �����[  u�.c  u �m � u@�� � u�@� ��"��� ��+��� �gfffffK@��� ���N���  ~V���  ~V@1� ���O@o@� z @o@� z �Sр�O�Sр�O@o@����C�����C����ހ���ހ����C���������������������� 6n��� 6n��� ?��� ?��� 6n��ހ��Cр�C��Cр�C�ހрހ�ހрހ��C��ހ��р�C��р�C�рހ�рހ����ހ 6nр�C 6nр�C ?�рހ ?�рހ 6n�@����C�@����C�@���ހ�@���ހ�@����C�@�����@�����@����@����@�����@�� 6n�@�� 6n�@�� ?��@�� ?��@�� 6n�@��@T� ���@T� ��� -1�@�� -1�@��@T� ��ا ���Ӡl� ����y� �3� �� �&��������'@`� �  � � @x}� � `�x`��  �u �� ��r������&@gffff�7@�D� gffff�7@������$@��r� �  �u��� `�x��� @x}Ӏ��  � � `�`*� �&�Ӡ7� �3��D� ����ا �����ا  Q%� $�  Q%�������0@ Q%@f�6@f��@f �������0@�� $� ����ا ��� �� �����  ���� �Ӏ�� �6 ��  Q%�ا  Q%��ا ��� � ����B� ��@]� @��@]� ����@]� ����B� @�� � @����ا @��� �� @����� ������ ���Ӏ�� @�� �� ���ا ��� � ��� �஻ ���  �� �� `�� �� Ѡ� `�� S &� ������ @ &� ������@�� 233333@������*@������@`�� ������@������(@233333@������&@������ @�������%@`�� ������%@������"@������&@�� ��y� ��  �� ���  � ��� � $� ��[ � $� @T� � $�  |� ffffff0@��� @f��� � ���� �� |� ��@T� р� 	= Ӏ�`�; �� u8 �!��6 �������1@@�3 ������1@ �0 ��	`M/ Ӏs��- ����- ������0@`M/ ������0@ �0 ffffff0@ �0 �����L0@gfffff
@������.@433333@������-@������@������-@433333@������.@������@���� �Z  �� ��[  $� ��[ � -1��[ � -1@T� � -1 |� �G7��� @o@��� � �I��� ��O |� ��O@T� р�O 	= Ӏ�O`�; ��O u8 �*N��6 �������5@@�3 ������5@ �0 ��F`M/ Ӏ|C��- ��>��- ������4@`M/ ������4@ �0 �G7 �0 �����L4@gfffff
@�����L3@433333@S d ������@ d 433333@�����L3@������@���)�Z  �,��[  -1��[ ���j ��[ ���j @T� ���j  |� @�p ���  z ��� ��9� ��� @T�  |� @T� @T� �@T�  	= �@T� `�; @T�  u8 �͇ ��6 � G� @�3 �9�  �0 �,� `M/ Ӡ�{ ��- `�x ��- ��s `M/ ��pr `M/ ������@ �0 �co gfffff
@Q`!` 433333@������@������@������@433333@`!` ������@Ӡ.c �Z �;f ��[ ��j ��[ �@x}    �@x}@]� ���- @]� ���-     �@x}    ���^�� �@KL �� �@KL �ا ���^�ا ���^�� 	 5�   &   _� 5�   *   -   5        $ tt_pos��5�   2   $	��Pos��6B   $
 left=�5�   /   $ 35��5�   /   $ 50,0,0,0�5�   /   $	 -10��5�   /   5�   #   5�      5�      �� 5�      -   5        $ bdr_vendors��5�   !   $��2��66   #��[  6F   $��0q�6p   $��40��6�   $��60��6�   $��Bottom=�6�   $
 7,7,0,0��6�   
������ $��0,5,0,-1q�6�   ._�5�      �� -   5        $ btn_vendors��5�   %   
������ $  vendors��6T   
 ���� 
������ $
��auto��5�   ?   $��60��6M    5�   "   :� 5�   &   $ Button.Click��63   .[�5�   *   �� .��5�   .   �� .��5�   2   W� $ bdrMain��5�      $ Opacity�5�      $ 0X�5�      $ 1X�6"   $ 0:0:1D�5�      5�   /   5�   +   5�   '   .��5�   "   F� -   5        $ path_vendors��5�   ^   
�������$
 Fill=�5�   4   $	��Top=�6D   $��0,-1,0,0q�6\   $��Center=�6n   $��25��5�   (   $��25��63   $��LeftToRight=�6@   ��������KQ���������7@� H��(k H�`�l`�� 6n�������?@��o ����o����oѠ����o�@x}��o�@x}@�Q@x}43333�:@S@x}gffff�:@ kz`�� �u`��рG7`���233333'@`����pr `��� �m `����j gffff�:@��j @����j ��o�`M/ ��o� � ��o�@ ��ogffff�7@�hQ    43333�5@�     �Igfffff5@�����L5@S    �����L5@�����L5@�����L5@S    4333335@��      5@�� hffff�4@S�� hffff�4@�� hffff�4@�� ������4@S�� 43333�4@@ ������4@@ ������4@S � hffff�3@�� hfffff3@�� �����3@� 	=  �Q 	= ������1@�`M/ @f�%&  $� �%& ��� Q�%& 433333,@р�  P� ��� ஻ �� �� `# @� Ӡ%& `_� ��* �ا ������@ R� �������	@�7� 233333@gfffff$@������@�� �ffffff@`�� ������@�n� ������@`�� � �� ��� �@T�  � �n�  �� �� �ˤ ������#@gfffff&@ � gfffff'@��� gfffff(@�`�� `3� Q`�� gfffff-@�`�� ��� �`��  $� �͇ @f z  �Q z gfffff2@� � `q#S �� 4333333@@�� �~& R� ��)�@]� ���@]� ��@]� ��@]� ���@]� ��S �� 4333330@ R� gfffff-@ R�      *@Q R� gfffff(@S R� gfffff(@�ˤ 433333(@�ˤ      (@р��  G� S�� 433333 @��� @�p  �� 433333@S�ˤ ������@@� 433333@��� ������@Q��������@Ӏ� >I ��@KL @(`�S �`�/�Z @:4�.c �����L4@`Vl ������4@������@2333335@�pr ������5@������@Ӏ�O�,� ��R�ڊ ��O@�� Q Egfffff(@S Egfffff(@�|C஻ ��A�5� ���A@]� ���A�M� ��5@1� ��!�����!�����!����!gffff�2@��!gffff�2@Q@o@    �3@S�|C�����L3@`H�����3@@�L     3@Q �igfffff2@S��fgfffff2@ b`<��^`<р�[`<���R���K ��K@fѠK��� ӠK� � ������7@�� ����� �gfffff<@�� `W�� � `W���� �`W� �� �`W� $� �`W�@f�`W� �gffff�=@�����`<�@��`<S`��gfffff2@�Y��I�Ŭ�I� J� �$�@��@(`�� �,gfffff?@�����4@    �?@�����4@�-�@:443333�?@�����L4@� H�`�;����|C����K	 5�   &   _� 5�   *   -   5        $ 
tt_vendors��5�   2   $��	Customers��6F   $
 left=�5�   /   $ 35��5�   /   $ 50,0,0,0�5�   /   $	 -10��5�   /   5�   #   5�      5�      �� 5�      -   5        $��	bdr_users��5�   !   $��3��62   #��[  6B   $��0q�6l   $��40��6�   $��60��6�   $��Bottom=�6�   $
 7,7,0,0��6�   
������ $��0,5,0,-1q�6�   ._�5�      �� -   5        $ 	btn_users��5�   %   
������ $  users��6S   
 ���� 
������ $
��auto��5�   ?    5�   "   :� 5�   &   $ Button.Click��63   .[�5�   *   �� .��5�   .   �� .��5�   2   W� $ bdrMain��5�      $ Opacity�5�      $ 0X�5�      $ 1X�6"   $ 0:0:1D�5�      5�   /   5�   +   5�   '   .��5�   "   F� -   5        $ 
path_users��5�   ^   
�������$
 Fill=�5�   4   $	��Top=�6D   $��0,-1,0,0q�6\   $��Center=�6n   $��25��5�   (   $��25��63   $��LeftToRight=�6@   ����������Ip@��k� �� �igffff�|@ �� 7:��!�`�8�Α��������{@�����Qp@���� �	�(��������#� �`>��G�@c	��k�����Ip@��k� �����
� �� ��Ҧ@`��43333cg@S���43333�g@gffff�t@������g@gffff�t@43333h@43333�t@gffffi@ ��gffffj@gffffNt@     k@S��=43333sk@     t@gffff�k@    �s@�����	l@gffff�s@hffffl@������s@�����)l@gffff�s@�����Ll@gffff�s@    �l@    �s@433333m@43333�s@hffff�m@S���    Pn@gffff>s@������n@gffffs@    Po@S��hffff�o@ �53333�o@ �gffffp@Ӏ'  ~�`������ap@�0=gfffffp@S@�R�����ip@`cf43333kp@ �{43333kp@S�D������lp@ (������yp@gffff�s@    �p@gffff�s@43333�p@�������q@ :+    @q@S��I������q@43333ct@������q@��    �q@S��gffff�q@`�43333�q@��     r@S��Tgffffr@��������9r@gffff�u@�����Yr@gffffnv@hffff�r@gffff�v@53333�r@gffffnw@������r@gffff�w@hffff6s@ �i    xs@�L�hffff�s@S�     �s@��F�����t@gffff�y@�����9t@gffff�{@������p@gffff�{@63333Se@��j     [@43333�s@�����YI@43333�j@����̌G@    0b@93333�Z@    �S@63333�d@�����R@    0p@������[@�����9t@    �\@�����t@�����y]@53333�s@�����l^@������s@     `@������s@    a@53333Cs@43333b@�����s@433333c@    �r@gfffffd@43333�r@����̜e@43333Kr@�����Lf@gffff&r@43333g@    r@43333�g@43333�q@S ��43333�q@����̌h@����̤q@`��@�S���43333;q@ �� �B gffff�p@S �<������p@�gj�����ap@�a�43333kp@S�������lp@`*�43333kp@��������ip@S�w43333cp@`3�����Qp@`3�eh� �`�4`�� � R�@��Ӡͼ e��43333�n@�����9j@@^]�������i@`�'������i@�f�������i@`��������i@���������i@@:i�����i@ dU������h@������k@����̬h@�����<k@�����ih@�a�Ӏ�k��V������g@���������g@��k�effff�g@��=23333�g@�������Ih@�$�
� ��z�
�����h@`�o
@�t�,
Ӡ�Z��u	`�K�v�������g@�N������g@fffff`@�����h@ NS�����\h@`� �������h@ *_23333j@@��23333Ck@����������l@�v,23333sn@�������p@gffff�T@�����Oq@gffff�T@�����qr@ �-�����qs@gffff�X@S�a443333�Z@���43333s]@��43333s`@S�~�������a@������t@43333�c@@Y��
��	���U
�ԉ@8�
 �����
	 5�   &   _� 5�   *   -   5        $ tt_users��5�   2   $��Users��6D   $
 left=�5�   /   $ 35��5�   /   $ 50,0,0,0�5�   /   $	 -10��5�   /   5�   #   5�      5�      �� 5�      $��10��6!   $
 7��62   ._�5�      �� -   5         ��Name$ txt_tabTitle��5�   )   
�������$��10��6U   $��Center=�5�   )   $��16(�6J   $��5 5 10 5q�6\   5�      5�      5�      �� 5�      $
 0,0,7,7��6   $��1��5�      
�������$
��auto��5�   ,   ��5�      Z� ._�5�      �� -   5        $ bdrMain��5�   9   $��	15,0,15,0q�6   $
��auto��6J   
������ ._�5�      �� $��Disabled=�6'   $
��Auto=�6N   .��5�      � .+�5�   "   � -   5        $ grid_branch��5�   4   $��1��6'   $��
10,0,10,10q�6I   $y�Visible=�6^    5�   &   �� 5�   *   $
��auto �68   6H   �� 5�   *   $
��auto �68   6H    5�   &   �� 5�   *   $��* �6;   6G   �� 5�   *   $��* �6;   6G   �� 5�   *   $
��1.5* �6;   $	��200��6H   6Y   .+�5�   &   ]� $��3��60   ��.  .+�5�   .   �� $��0��65   $��Right=�6B   $��40��6Y   $
 10��6e   $��
10,10,10,5q�6w   $��1q�6�     	LightGrey#��[ 6�   ._�5�   2   � .+�5�   6   (  "System.Windows.Controls.DatePicker  -   5        $��
dp_endDate��5�   $      SelectedDateFormat$ Short=�5�   A   $	��100��5�   6   ���#737373$��0q�5�   <   $	��4,0q�6P   $��10,0,5,0q�5�   #   $��40��66   $��Right=�5   $    ( $MaterialDesignFloatingHintDatePicker#��[ 5  (   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ End Date...��5  !   .  (MaterialDesignThemes.Wpf.TextFieldAssist    CharacterCounterStyle  5  '   �� 5  !   5  3   5  /   �� 5	  .   $��0��65   $��Right=�6C   $��40��6]   $
 10��6i   $��
10,10,10,5q�6{   $��1q�6�   #��[ 6�   ._�5
  2   � .+�5  6     -   5        $��dp_startDate��5  *   $ Short=�5  A   $	��100��5  >   ���#737373$��0q�5  C   $	��4,0q�6W   $��10,0,5,0q�5  '   $��40��6:   $��Right=�5  *   #��[ 5  (   $ Start date...��5  !     5  (   �� 5  !   5  3   5  /   5  '   �� 5  *   $��0��61   $��40��6>   $
 10��6J   $��
10,10,10,5q�6\   $��1q�6r   #��[ 6�   ._�5  .   � .+�5  2   �� -   5        $��cb_branches��5  k    & "MaterialDesignFloatingHintComboBox#��[ 5  &   $ Branch ....��5  '   ���#737373$��0q�5  ?   $	��4,0q�6S   $��0��5   &   ���#4E4E4E$��40��5"  %   $��10,0,5,0q�61   5$  /   5%  +   �� 5&  &   -   5        ! ��Name$! chk_allBranches��5&  X     	MainColor#��[ 60   $��1��6�   $	��All��6�    2 .MaterialDesignFilterChipPrimaryOutlineCheckBox#��[ 6�   6�   ]� 5'  &   $��1��61   $��2��6>   ��.  .+�5(  .   �� $��0��65   $��Right=�6B   $��40��6Z   $
 10��6f   $��	10,5,10,5q�6x   $��1q�6�   #��[ 6�   ._�5)  2   � .+�5*  6   )  #MaterialDesignThemes.Wpf.TimePicker  -   5        $��
dt_endTime��5+  3   ���#737373$��0q�5,  K   $	��4,0q�6_   $��10,0,5,0q�5-  3   $��40��6F   $	��100��5.  3   $��Right=�6?    ( $MaterialDesignFloatingHintTimePicker#��[ 5/  3   $ End time...��51  !     50  3   �� 51  !   52  3   53  /   �� 55  .   $��0��65   $��Right=�6B   $��40��6Z   $
 10��6f   $��	10,5,10,5q�6x   $��1q�6�   #��[ 6�   ._�56  2   � .+�58  6     -   5        $��dt_startTime��58  }   $��10,0,5,0q�59  )   $��40��6<   $	��100��5:  )   $	��100��65   $��Right=�5;  )   ���#737373$��0q�5<  @   $	��4,0q�6T   #��[ 5=  (   $ Start time...��5?  !     5>  (   �� 5?  !   5A  3   5B  /   5E  '   5H  #   5I     5K     5L     5M     5N     5O     �� 5P     $��1��6   $��	10,5,5,10q�6#    5Q       5R     $ 1Q�5S     $	 360Q�5T     $
 Gray��5U     $	 0.3Q�5V     $	 10Q�5W     ._�5Z     � $��1��6    5[     �� 5\     $
��auto �6(   68   �� 5]     $��45 �6(   66   �� 5^     $��* �6(   65   �� 5_     $��45 �6(   .+�5a     �� $��1��6   
�������$
 7,7,0,0��6=   ._�5b     �� $��0,0,0,1q�6   ���	#FFC8C8C8._�5c     ]� ��.  .+�5e  "   �� $��0��6)   $	��200��66   $
 10��6C   $��	10,5,10,5q�6U   $��1q�6j   #��[ 6~   ._�5f  &   � .+�5g  *   �� -   5        " ��Name$" 
txt_search��5g  2   $��10,0,5,0q�5h     ���#737373$��0q�5i  J   $	��4,0q�6^    % !MaterialDesignFloatingHintTextBox#��[ 5j  ,   $% Enter Here To Start Search ....��5k     ���#4e4e4e5l     5n  '   5o  #   �� 5p  "   $��0,5,0,0q�6)   
�������$��30��6N   $��30��6Y   ._�5q     F� ���#4e4e4e$
 Fill=�62   $��0��5r     $
��17.5��5s     $
��17.5��6&   ����p��ffffff0@������lW@���fffff&P@��������LG@ffffff0@������`��`,a 0v�hx�`r��ō�������}��q����f�z��@) ����Z @ sӀ�  ���� `�Z  GS@�� �����y_@ $� ����. G��. G�. G�. G� B� �{�������F@��D�������F@��D� ���$7Ѡ'�@��S���43333U@`�������J@�A � �V `� ظ23333]@@�~�`���k�~�fffff&@@��ffffff0@Q��ffffff0@� K@zA�����LV@43333�T@@�`6 �����LL@@zA�����F@43333sM@�����F@gfffffC@�����LL@hfffff:@S�a     ,@�����LV@������+@ Khfffff:@S��i433333@@���    @D@���43333sH@� 3� Tj��i�����YP@ K@zA�`}�`F�Ӏ`F�gffff�S@@2Ygffff�S@`���gffff�S@�d�43333�S@�����=@  � ��� ����`�y �,�{s@y��`am �������'�Э �-��W`��3 �� ]����������H@S ��������G@@��������F@ M�������E@� B� �� ��`F�`}�`F�	5t     5z     �� 5|     -   5        $ btn_refresh��5|  k   $��LeftToRight=�6!   $��Right=�6=   $��40��6T   $��40��6_   +  %MaterialDesignThemes.Wpf.ButtonAssist#   CornerRadius$# 15��5}     $ � Cursor$
$ Hand��6K     SmallButtonCircleRotate#��[ 5~  c   $	��5,1q�5     $��
إضافة��6,   % J� RenderTransformOrigin$% 0.5,0.5&�6=   & � DataContext$�
& �
M896,0L896,0C401.2,0,0,401.2,0,896v0c0,494.8,401.2,896,896,896h0c494.8,0,896-401.2,896-896v0C1792,401.2,1390.8,0,896,0z   M1451.3,1021c-31.9,133.7-98.8,242.1-200.6,325.2c-101.8,83.1-221,124.6-357.7,124.6c-72.8,0-143.3-13.7-211.4-41.2  s-128.8-66.6-182.2-117.5l-96.5,96.5c-9.5,9.5-20.7,14.2-33.7,14.2s-24.2-4.7-33.7-14.2c-9.5-9.5-14.2-20.7-14.2-33.7v-335.3  c0-13,4.7-24.2,14.2-33.7s20.7-14.2,33.7-14.2h335.3c13,0,24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7c0,13-4.7,24.2-14.2,33.7  l-102.5,102.5c35.4,32.9,75.6,58.4,120.5,76.3c44.9,18,91.5,26.9,139.9,26.9c66.9,0,129.2-16.2,187.1-48.6  c57.9-32.4,104.3-77.1,139.2-134c5.5-8.5,18.7-37.7,39.7-87.6c4-11.5,11.5-17.2,22.4-17.2h143.7c6.5,0,12.1,2.4,16.8,7.1  c4.7,4.7,7.1,10.4,7.1,16.8C1452,1018.2,1451.8,1020,1451.3,1021z M1470.7,752.3c0,13-4.7,24.2-14.2,33.7  c-9.5,9.5-20.7,14.2-33.7,14.2h-335.3c-13,0-24.2-4.7-33.7-14.2s-14.2-20.7-14.2-33.7c0-13,4.7-24.2,14.2-33.7l103.3-103.3  C1083.3,547,996.3,512.9,896,512.9c-66.9,0-129.2,16.2-187.1,48.6s-104.3,77.1-139.2,134c-5.5,8.5-18.7,37.7-39.7,87.6  c-4,11.5-11.5,17.2-22.5,17.2H358.7c-6.5,0-12.1-2.4-16.8-7.1c-4.7-4.7-7.1-10.4-7.1-16.8V771c32.4-133.7,99.8-242.1,202-325.2  c102.3-83.1,222-124.6,359.2-124.6c72.8,0,143.7,13.8,212.5,41.5s130,66.7,183.3,117.1l97.3-96.5c9.5-9.5,20.7-14.2,33.7-14.2  s24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7V752.3z��5�     ��5~     Z� ��61   Z� z�5�     V� 5�  "   .~�5�  &   �� �� 5�  &   �� 5�  &   ' �� Angle$' 0Q�66   T� 5�  &   5�  #    5�     _� 5�  "   -    5        $ 
tt_refresh��5�  *   $��Refresh��6>   $
 left=�5�  /   $ 15��5�  /   $ 50,0,0,0�5�  /   $ 25��5�  /   5�     5�     5�     5�     �� 5�     $��2��6   
�������._�5�     � $��1��6   .+�5�     �� -!   5        $ brd_roundDataGrid��5�  R   $��
5,5,5,17.5q�6   $��1��63   $
 10��6@   $��0q�6m   
�������5�     &   System.Windows.Controls.DataGrid  5�     -"   5        $��	dgInvoice��5�     $��5,4,5,18q�5�      (   
IsReadOnly( . 
�������)   AutoGenerateColumns) .  *   ColumnWidth5	  /System.Windows.Controls.DataGridLengthConverter$* SizeToHeader	 5�     +   AlternatingRowBackground+ �#f3f3f3, �� AlternationCount$, 2��5�  @   - J� OpacityMask- 5�     !� 5�     . !� Visual. 6*   �� / �� ElementName$/ brd_roundDataGrid��0 � 	Resources0 5�     =
  7System.Windows.Controls.Primitives.DataGridColumnHeader1 �� BasedOn2 �� 
TargetType3 �� Property4 �� Value��%  (��       5�  .   * SystemColors.HighlightBrushKey+)'
       7
 7   �� 6   6b   �� 5�     81   "2 
 6]   .�5�  "   �� 3 � ��4 �#F5D7C46?   �� 5�  "   3 � ��
4 �����6@   �� 5�  "   3 � ��$4 0 0 1 0q�6D   �� 5�  "   3 � ��84  6?   5�     5�     5   Columns5 5�     0  *System.Windows.Controls.DataGridTextColumn  5�     -#   5        
col_No��,  &System.Windows.Controls.DataGridColumn6   Width$6 *	 5�  A   7   Header$	7 No.��6K   1  +System.Windows.Controls.DataGridBoundColumn8   Binding8 6[   �� *	invNumber+  5�     -$   5        col_date��$6 *	 5�  A   $
7 Date��6K   8 6[   �� *	invDate+ 	 dateFrameConverter9 �� 	Converter#9 [	   5�     -%   5        
col_branch��$6 *	 5�  A   $7 Branch��6K   :   
Visibility$: Visible=�6�   8 6[   �� *branchCreatorName+6�     5�     -&   5        col_pos��$6 *	 5�  >   $	7 Pos��6K   $: Hidden=�6t   8 6X   �� *	posName+6t     5�     -'   5        
col_vendor��$6 *	 5�  A   $7 Vendor��6K   $: Hidden=�6y   8 6[   �� *	agentName+6y     5�     -(   5        col_agentCompany��$6 *	 5�  G   $7 Company��6Q   $: Hidden=�6�   8 6b   �� *agentCompany+6�     5�     -)   5        col_user��$6 *	 5�  ?   $
7 User��6K   $: Hidden=�6z   8 6Y   �� *uUserAccName+6z     5�     -*   5        col_item��$6 *	 5�  ?   $
7 Item��6K   $: Hidden=�6}   8 6Y   �� *ITitemUnitName1+6}     5�     -+   5        	col_count��$6 *	 5�  @   $	7 QTY��6K   $: Visible=�6r   8 6X   �� *count+6r     5�     -,   5        col_itQuantity��$6 *	 5�  E   $	7 QTY��6O   $: Hidden=�6{   8 6\   �� *
ITquantity+6{     5�     --   5        	col_price��$6 *	 5�  @   $7 Price��6K   $: Hidden=�6�   8 6Z   �� *	ITprice+ 
 accuracyConverter#9 [
 6�   5�     5�     5�     �� 5�     $��3��6   
�������$
 0,0,7,7��65   ._�5�     ]� ��.  .+�5�     �� $��0,0,0,1q�6   #��[ 67   $��
10,0,10,10q�6`   $
��auto��6t   $
��Left=�6�   ._�5�     �� ; �� Orientation$; 
Horizontal=�6&   .+�5�     �� -.   5        $ btn_pdf��5�  %   $��	10,0,5,-1q�6P   $��0q�5�     $��35��6'   $	��Top=�5�  H   ��5�  8   Z� ��5�     Z� .��5�  "   '  !MaterialDesignThemes.Wpf.PackIcon  ���#D20A0A<   Kind$< FilePdf=�6O   $��22��5�     $��22��6&    5�  &   _� 5�  *   -/   5        $ 	tt_report��5�  2   $��Report��6E   $
 left=�5�  /   $ 5��5�  /   $ 50,0,0,0�5�  /   $	 -10��5�  /   5�  #   5�     �� 5�     -0   5        $ 	btn_print��5�  4   $��5,0,5,0q�5�  @   $��0q�5�     $��35��6'   ��5�  '   Z� ��5�     Z� .��5�  "     ���#313131$< Printer=�6O   $��Center=�5�     $��22��5�     $��22��6"    5�  &   _� 5�  *   -1   5        $ tt_print��5�  2   $��Print��6D   $
 left=�5�  /   $ 5��5�  /   $ 50,0,0,0�5�  /   $	 -10��5�  /   5�  #   5�     �� 5�     -2   5        $ btn_exportToExcel��5�  7   $��3��5�  @   $��5,0,5,0q�6P   $��0q�5�     $��35��6)   ��5�  '   Z� ��5�  (   Z� .��5�  "   F� ���#378500$
 Fill=�66   $��20��5�     $��20��6&   $��LeftToRight=�5�     ��������- � $� ��- � $�  O[ ��c" O[ Q�c"H�z� @Q $� H�z� @� $� ��� ���!��� ���! � � $�  � � $� ��� ���!��� ���! $� � $�  $� � $� �����!�����! -1� $�  -1� $� ��^����^������^���h�X���P�Qр��0; Ӏ���3 �����- ���- � 6n -1� -1 -1� -1��� 6n��� 6n -1� 6n $� � -1 $� � -1��� � 6n��� � 6n $� � 6n � � -1 � � -1��� � 6n��� � 6n � � 6n z � -1 z � -1��[ � 6n��[ � 6n z �    (, �    @�`���� �������     �    (, ��Ɛ  �E�����@Zd;ߏ-@�H$u Щ� U-��o@v��/�,@���x�&@��~j�4,@Q؛r ��~j�4,@��r P�� @�p 0)� �	o ��� ��QT  ����*  �Q8\ �x�&1*@�/ �Ry ��bY �Ry ��MbX9@੧ �x4q `S�  �r ��� p=
ףp@�v���'@Yd;�O�@�v���'@G�z��@|?5^�I'@MbX94@23333�&@k�����@�8� n���!#@������@���K7(@������@�0�Z$"@��� ��A`��b(@�рƐ  �рƐ  �	 5�  &   _� 5�  *   -3   5        $ tt_excel��5�  2   $��Export to excel��6D   $
 left=�5�  /   $ 35��5�  /   $ 50,0,0,0�5�  /   $	 -10��5�  /   5�  #   5�     �� 5�     -4   5        $ btn_preview��5�  7   $��3��5�  @   $��0,1,5,0q�6P   $��0q�5�     $��35��6)   ��5�  '   Z� ��5�  (   Z� .��5�  "   � $��LeftToRight=�6'   .+�5�  &   F� ���#4E4E4E$
 Fill=�6:   $��15��5      $��18��6&   $��LeftToRight=�5     ������T1 E� 1���A�������dx@�pi�Y:������$q@@��������n@ s� �M�����iu@������Ik@�� �����)k@������t@ ������Tt@S�>/�����'t@@�P������s@`�p������s@S@ 23333+s@�H�����̔r@�9������r@S�	�23333kq@������r@������p@@G.effff�p@S �ceffff�p@gffffft@effff�p@gffffft@����̴p@43333ct@�����om@`�r03333si@`�rcffffve@S`�rcffff�d@`�rcffff�d@��#cffff�d@gffffFo@cffff�d@gffff�n@cffff�d@    `n@������d@    `n@03333�c@�    `n@���     n@ ��    �m@`�����̼m@�m��@���m�Ӏ���m�����m� �{������T@S��������T@�/���Gਤ���ਤ����43333sS@ jM43333sS@�S�ਤ =Q�ਤ�bw�Э �  ����gffff�T@@� 2:�}.gfffffW@�}.��Q��}.�gfffft@�}.�3����gffff^t@�����ix@�`�Z��F�*E��F�T1 E��/��aS� f��T`#Y��T�����T�`�b��T���T��T��H oV43333S^@��T�gffff�]@@�Q     ]@�~<`~�@��43333�\@����� ���j<�����yp@S@��gffffvp@���ఴ@Oఴ� ��ఴ�������q@ �������	r@��������	r@ �� )2�I0 ��aS�/��aS�_ z"S� $gffff�]@�����Y^@�_�3M�_� ���_�43333�i@�_��V�]943333�i@@$gӠ%[�q�����̜i@ �������i@ ��hffffvg@fffff6`@������e@ ��hffff&d@ ���63333s^@ ���63333�]@ ��������\@fffff`@     ]@ �wӠ��@�Z��j<�_ z"��+���	�������^@ E�	 �|���	fffff`@���	������lj@���	�23333�j@���	�ͼ���	������j@@R�	Ӡ���l 
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
�+���	���Q�D��@���D���s��D���@���d��D��@+��� )2`��gffffr@�&��/�VR    �q@�To    �q@�����9l@gffff�q@�����?l@43333�q@�����<l@������q@�����<l@�����9i@�����<l@63333f@�����<l@������b@�����<l@kffff�_@�����?l@����̬^@�����?l@kffff�]@�����<l@������\@�����	l@73333�\@������k@� q�`��`�D���Q�D��`�K�0�ӀOG@�gffff�i@`���{�����w@gffffFi@�����$w@�}������!w@`;������!w@43333�^@�����!w@43333s]@�����!w@gffff]@���43333�\@�#��    �\@����a�`�s43333s^@`�s43333�`@�����|v@��������|v@433333d@�����|v@gffff�e@�����|v@�W"23333{v@������h@�����|v@43333#i@�����|v@�����li@������v@43333�i@������v@Ӡ�B 5�@wP�M�`�K�0�� ,��/S�Ӵ�����!t@`�c�T1���T1�43333�^@�T1��D�T1�����^@�a443333�]@`�)����gffff]@@s�gffff]@���gffff]@gffff�s@gfffff]@gffff�s@    �]@43333�s@������]@gffffvs@43333s^@���43333�^@����gffff&d@����gffff�e@���gffffFg@���gffff�h@���gffff�h@��gffffi@��gffff6i@���gffff�i@gffff~s@    �i@`��gffff�i@���� U��  5@G. ,��/�`�YCS ������V@`�Q`�����yV@S`����̬[@ ������l`@��@c	����D	`O(����̌c@�����p@����̌c@S ׋������c@ �������c@ 1�������c@Q��=������c@� n����� x�%�`�YC	 5  *   _� 5  .   -5   5        $ 
tt_preview��5  6   $��Preview��6J   $
 left=�5  /   $ 35��5  /   $ 50,0,0,0�5  /   $	 -10��5  /   5	  '   F� 5
  &     Blue#��[ 6+   $
 Fill=�6H   $��13��5     $��6��6&   $��	17,10,0,0q�61   $��LeftToRight=�5     ������O|��}�`g� � e`���� �� Թ �V@� ���%����������s@@W��g,@��@ܳ S�V3�����Qr@`{��T�����iS �������t@ ���������#� %��^�fffffp@�����lv@`s ��S ��������w@���P����@#S������̬v@����k�@w�@����0�  ��#��Ҧ�O|��}� @�����'@����\�� \[��������1r@gffff�s@ M&��������t@��������u@������r@������u@ ��������u@�I��@���)�@k"@�� @���@?S �����T`g�~��i��%�i�������is@�i�������r@��e������r@�<��������r@�������ds@ *������t@�����������Q��@?S ��	5  '   5  #   5     �� 5     -6   5        $ btn_settings��5  9   $��3��5  @   $��
5,0,5,-0.6q�6P   $��0q�5     $
��Auto��6)   $y�	Collapsed=�6P   ��5  '   Z� ��5  (   Z� .��5  "     $��20��5     $��20��6&   $��LeftToRight=�5     $< Settings=�68   
�������5  #   5     �� 5     -7   5        $ 	txt_count��5  @   $��4��6U   = �� Padding$= 10q�5     $��0��6,   ���#378500$��12(�6J   ��5  *   Z�  5  "   _� 5  &   -8   5        $ tt_count��5  .   $��Customers Count��6@   $
 left=�5  /   $ 35��5  /   $ 	50,25,0,0�5   /   $	 -20��5!  /   5#     5%     5&     �� 5'     -9   5        > ��Name$> stk_tagsBranches��5'  9   $��Right=�6!   $; 
Horizontal=�6S   �� 5(     -:   5        $> stk_tagsPos��5(  9   $��Right=�6!   $; 
Horizontal=�6N   �� 5)     -;   5        $> stk_tagsVendors��5)  9   $��Right=�6!   $; 
Horizontal=�6R   �� 5*     -<   5        $> stk_tagsUsers��5*  9   $��Right=�6!   $; 
Horizontal=�6P   �� 5+     -=   5        $> stk_tagsItems��5+  9   $��Right=�6!   $; 
Horizontal=�6P   �� 5,     ->   5        $> stk_tagsCoupons��5,  9   $��Right=�6!   $; 
Horizontal=�6R   �� 5-     -?   5        $> stk_tagsOffers��5-  9   $��Right=�6!   $; 
Horizontal=�6Q   5/     50     51     52     53     �� 55  
   $��1��6   
������ $��5��6@   $��Stretch=�6J   � 56  
   $��2��6    57     �� 59     $��* �6    6-   �� 5:     $
��auto �6    60   �� 5;     $��* �6    6-   �� 5<     $
��auto �6    60   .+�5?     �� $��0��6   
�������$��	5,10,10,5q�66   $
 7��6I    5@       5A     $ 1Q�5B     $	 360Q�5C     $
 Gray��5D     $	 0.3Q�5E     $	 10Q�5F     ._�5H     � $��	5,10,5,10q�6   .+�5I     #  LiveCharts.Wpf.CartesianChart  -@   5        $��cartesianChart��5I  )   &   LiveCharts.Wpf.Charts.Base.Chart?   LegendLocation$? Bottom=�6?   @   AxisX@ 5K       LiveCharts.Wpf.Axis  -A   5        A  Name$A axcolumn��5K  (   B   	SeparatorB 5L  "     LiveCharts.Wpf.Separator  5M  &   C   Step$C 1Q�64   5O     5Q     5R     5S     �� 5T     $��1��6   
������ $��Stretch=�6A   $��5��6_   �� 5U     $��2��6   
�������$��	5,5,10,10q�65   $
 7��6H    5V       5W     $ 1Q�5X     $	 360Q�5Y     $
 Gray��5Z     $	 0.3Q�5[     $	 10Q�5\     ._�5^     � -B   5        $ grid1��5^     $��	5,10,5,10q�6&    5_     �� 5`     $��* �6+   67   �� 5a     $
��auto �6+   6:   .+�5c       LiveCharts.Wpf.PieChart  -C   5        $��chart1��5c  #   $? Right=�61   5e     5g     5h     5j     5k     5m     