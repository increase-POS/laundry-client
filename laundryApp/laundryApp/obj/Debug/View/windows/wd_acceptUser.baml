   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:laundryApp.View.windowslaundryApp.View.windows  5   	   +    %laundryApp.View.windows.wd_acceptUser   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:laundryApp.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   
   -   5        
������   � AllowsTransparency  .  � WindowStyle$
 None=�5
   
    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5   	   $��0q�6    � Title$ Increase laundryApp��5   
   $	��185��6&   $	��300��63   .��5      � -   5         �Name$ grid_acceptUser��5   *   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         SecondColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y   	 �� IsCancel	 . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   
 � ToolTip$
 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6E   $��20��5   *   $��20��65   5      � 5      $��Center=�6    � ColumnDefinitions 5      �� 5       $��* �6/   �� 5!      $
��1.2* �6/   .+�5#      F� 
������� �� Stretch$
 Fill=�6,   $	��5,0q�6;   $��LeftToRight=�6H   $��Right=�5$      $��0��6;   $��Center=�5%      $��15��65   $��15��6@   ����������Ip@��k� �� �igffff�|@ �� 7:��!�`�8�Α��������{@�����Qp@���� �	�(��������#� �`>��G�@c	��k�����Ip@��k� �����
� �� ��Ҧ@`��43333cg@S���43333�g@gffff�t@������g@gffff�t@43333h@43333�t@gffffi@ ��gffffj@gffffNt@     k@S��=43333sk@     t@gffff�k@    �s@�����	l@gffff�s@hffffl@������s@�����)l@gffff�s@�����Ll@gffff�s@    �l@    �s@433333m@43333�s@hffff�m@S���    Pn@gffff>s@������n@gffffs@    Po@S��hffff�o@ �53333�o@ �gffffp@Ӏ'  ~�`������ap@�0=gfffffp@S@�R�����ip@`cf43333kp@ �{43333kp@S�D������lp@ (������yp@gffff�s@    �p@gffff�s@43333�p@�������q@ :+    @q@S��I������q@43333ct@������q@��    �q@S��gffff�q@`�43333�q@��     r@S��Tgffffr@��������9r@gffff�u@�����Yr@gffffnv@hffff�r@gffff�v@53333�r@gffffnw@������r@gffff�w@hffff6s@ �i    xs@�L�hffff�s@S�     �s@��F�����t@gffff�y@�����9t@gffff�{@������p@gffff�{@63333Se@��j     [@43333�s@�����YI@43333�j@����̌G@    0b@93333�Z@    �S@63333�d@�����R@    0p@������[@�����9t@    �\@�����t@�����y]@53333�s@�����l^@������s@     `@������s@    a@53333Cs@43333b@�����s@433333c@    �r@gfffffd@43333�r@����̜e@43333Kr@�����Lf@gffff&r@43333g@    r@43333�g@43333�q@S ��43333�q@����̌h@����̤q@`��@�S���43333;q@ �� �B gffff�p@S �<������p@�gj�����ap@�a�43333kp@S�������lp@`*�43333kp@��������ip@S�w43333cp@`3�����Qp@`3�eh� �`�4`�� � R�@��Ӡͼ e��43333�n@�����9j@@^]�������i@`�'������i@�f�������i@`��������i@���������i@@:i�����i@ dU������h@������k@����̬h@�����<k@�����ih@�a�Ӏ�k��V������g@���������g@��k�effff�g@��=23333�g@�������Ih@�$�
� ��z�
�����h@`�o
@�t�,
Ӡ�Z��u	`�K�v�������g@�N������g@fffff`@�����h@ NS�����\h@`� �������h@ *_23333j@@��23333Ck@����������l@�v,23333sn@�������p@gffff�T@�����Oq@gffff�T@�����qr@ �-�����qs@gffff�X@S�a443333�Z@���43333s]@��43333s`@S�~�������a@������t@43333�c@@Y��
��	���U
�ԉ@8�
 �����
	5&      �� 52      -   5         ��Name$ txt_user��52   8   $��User Confirm��6$   
�������$
��Left=�53   $   $��Center=�6?   $��1��54      $	��5,0q�64   55      56      57      58      �� 59   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��5:      ���#999999
�������._�5;      � -   5        $ grid_mainGrid��5;       5<      �� 5=      $
��auto �6$   �� 5>      $
��auto �6$   �� 5?      $��* �6$   �� 5@      $
��auto �6$   .+�5B      �� $��0��6   $ 10��6&   $��
10,10,10,5q�68   $��1q�6N     	LightGrey#��[ 6b   ._�5C      � .+�5D      �� -   5         ��Name$ tb_userName��5D   "   $��0��67   $��10,0,5,0q�6k    % !MaterialDesignFloatingHintTextBox#��[ 5E   "   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ User Name...��5F   "   ���#4E4E4E �� 	MaxLength$ 20��5G   7   $��12��6F    �� 
IsReadOnly . ���#737373$��0q�5H   :   $	��4,0q�6N   $��40��5J   "   ��5D   D   �� *
username+ �� Mode$ OneWay=�.  (MaterialDesignThemes.Wpf.TextFieldAssist   CharacterCounterStyle 5I   "   �� 5L   "   F� 5M      -   5         F�Name$ p_error_userName��5N      ���#E81123$��0��5M   .   $
 Fill=�6;   $��5,5,15,5q�6J   $��Right=�5O      $��2��6<   $y�	Collapsed=�6M   $��Center=�5P      $��15��6:   $��15��6F     
infoCircle#��[ 5Q      5R      5S      �� 5U      $��1��6   $��2��6'   $ 10��6=   $��	10,5,10,5q�6O   $��1q�6d   #��[ 6x   ._�5V      � .+�5W      �� -   5        $ tb_password��5W   "   $��1��67   $��10,0,5,0q�6D   $ Password...��5X   "   #��[ 5Y   "   ���#4E4E4E . $ 20��5[   "   $y�	Collapsed=�61   ���#737373$��0q�5\   :   $	��4,0q�6N   $��40��5^   "    5]   "   �� 5`   "   G� 5a      -	   5         G�Name$ pb_password��5a   3   $��1��6&   $��10,0,5,0q�6H    ) %MaterialDesignFloatingHintPasswordBox#��[ 5b   &   $ Password...��5c   &   ���#4E4E4E���#737373$��0q�5e   >   $	��4,0q�6R    G� 	MaxLength$ 20��5f   &   $��40��5h   &    5g   &   �� 5j   &   F� 5k      -
   5        $ p_error_password��5n      ���#E81123$��1��5k   .   $
 Fill=�6;   $��5,5,40,5q�6J   $��Right=�5l      $��2��6<   $y�	Collapsed=�6M   $��Center=�5m      $��15��6:   $��15��6F   #��[ 5o      F� 5p      -   5        $ p_showPassword��5r   R   ���#7e7e7e$��1��5p   .   $
 Fill=�6;   $��5,5,15,5q�6J   $��Right=�5q      $��2��6<   $y�Visible=�6M   $��Center=�5r      $��20��6:   $��15��6F   ������ا  � Ӏ�[  � �� ��6  z Ӡ� �5� ��[ ��� �ا ��� � $� ��� ��5�5� ��O z ���5��6  $�  � �ا  � ��ا @]� �ffffff @@]� ��[ �ˤ ��[  z S��[ ������@ffffff @��- �ا ��- �@�� ��-  $� �XO  $�  z � $� �ˤ @�� @]� �ا @]� ��ا @KL � � @KL  z `!`  z  z � z ��  � �ا �ا �ا �`�� �ا ��� �� ���  z S��� 433333@`�� @KL �ا @KL 	5w      5x      5y      �� 5z      -   5        $ btn_confirmation��5{      $��3��5z      $��10,10q�6)   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 7��68   
�������#��[  5|      ��6E   Z� .��5}      ��  �� Orientation$ 
Horizontal=�6!   $��Center=�6:   $��Center=�6W   .+�5~      �� -   5        $��txt_confirmation��5~   :   
�������$��Confirmation��6R   $��Center=�6f     5      -   5        $��path_confirmation��5   2   
�������$ CheckCircle=�5�   2   $��Center=�6E   $��15��5�   2   $��15��6>   $��4,1,0,0q�6J   5�      5�      5�      5�      5�      5�      