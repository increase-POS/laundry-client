   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:laundryApp.View.setuplaundryApp.View.setup  5      +    %laundryApp.View.setup.uc_serverConfig   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:laundryApp.View.setup     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      -   5        .��5      �   � RowDefinitions  5   
   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   6,   �� 5      $
��auto �6   6,   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ 	txt_title��5   )   $��Server Config��6      	MainColor#��[  6<   $��2��5   
   $
��Bold"�6   $��18(�60   $��Center=�6A   $
��Left=�5      $��
10,10,10,5q�64   �� 5   
   $��1��6   $��3��6   $��5 0 5 5q�64   $��2��6J   $��0q�5      #��[  6&   �� 5   
   -   5        $ txt_serverUriTitle��5      $��Server Uri:��60   $��14(�6C   $
��Left=�5      $��5q�6/   #��[  5       $��2��5!      $��0��6$   $��Center=�5"      �� 5#   
   $��2��6   $��1��6     �� CornerRadius$ 10��61   $��	10,5,10,5q�6C   $��1q�6X     	LightGrey#��[ 6l   ._�5$      � .+�5%      �� -   5         ��Name$ tb_serverUri��5%      $�� ��60   $��10,0,5,0q�68   $��2��5&      $��1��6(   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Server Uri��5'       % !MaterialDesignFloatingHintTextBox#��[ 5(   '     	ThickGrey#��[ 5)   &   ���#737373$��0q�5*   5   $	��4,0q�6I   $��40��5,   &   .  (MaterialDesignThemes.Wpf.TextFieldAssist   CharacterCounterStyle 5+   &   �� .��5.   L   5/      F� 50      -   5         F�Name$ p_errorServerUri��50   G   ���#E81123 �� Stretch$
 Fill=�6&   $��5,5,15,5q�65   $��Right=�51   #   $��1��6?   $��2��6R   $y�	Collapsed=�6`   $��Center=�52   #   $��15��6>   $��15��6J   ����� � �� Ӏ�b �� �� ��b ��  � Ӏ� �U��b ��O � ��OӀU��O��O�U��O � Ӏ�O��b �U��  � �� � � @]� Ӑ�� @]� �ا p� �ا  � ��ا  z ��ا ��q ��� ��j  � ��j �p� ��j @]� ��q @]�  z �@]�  � �@]� p� p� @]�  � @]� �@]� @f��ا @f��ا ��� �@]� ��� �@]� @f		 � ToolTip	 54      _� 55      -   5        
 _�Name$
 tt_errorServerUri��55   #   $�� ��6>     ToolTipError#��[ 6J   57      5;      5<      �� 5A   
   -   5        $ txt_activationkeyTitle��5A      $��Activation key:��64   $��14(�6K   $
��Left=�5B      $��5q�6/   #��[  5C      $��3��5D      $��0��6$   $��Center=�5E      �� 5F   
   $��3��6   $��1��6   $ 10��6/   $��	10,5,10,5q�6A   $��1q�6V   #��[ 6j   ._�5G      � .+�5H      �� -   5        $ tb_activationkey��5H      $�� ��64   $��10,0,5,0q�6<   $ Activation key��5I      #��[ 5J   '   #��[ 5K   &   ���#737373$��0q�5L   2   $	��4,0q�6F    �� 	MaxLength$ 20��5M   &   $��40��5N   &    5M   5   �� .��5P   L   5Q      F� 5R      -	   5        $ p_errorActivationkey��5R   G   ���#E81123$
 Fill=�6&   $��5,5,15,5q�65   $��Right=�5S   #   $��1��6?   $��3��6Q   $y�	Collapsed=�6_   $��Center=�5T   #   $��15��6>   $��15��6J   ����� � �� Ӏ�b �� �� ��b ��  � Ӏ� �U��b ��O � ��OӀU��O��O�U��O � Ӏ�O��b �U��  � �� � � @]� Ӑ�� @]� �ا p� �ا  � ��ا  z ��ا ��q ��� ��j  � ��j �p� ��j @]� ��q @]�  z �@]�  � �@]� p� p� @]�  � @]� �@]� @f��ا @f��ا ��� �@]� ��� �@]� @f		 5V      _� 5W      -
   5        $
 tt_errorActivationkey��5W   #   #��[ 6C   5Y      5[      5\      5`      5a      