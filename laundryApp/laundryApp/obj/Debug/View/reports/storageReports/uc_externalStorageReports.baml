   M S B A M L   `   `   `  ����E  AlaundryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:laundryApp.View.reports.storageReports&laundryApp.View.reports.storageReports  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5      F    @laundryApp.View.reports.storageReports.uc_externalStorageReports   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Flocal4clr-namespace:laundryApp.View.reports.storageReports     5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
      -   5        
  � Tag$  externalStorageReports��5      .��5      � -   5         �Name$ 	grid_main��5      $��20 10q�6    � ColumnDefinitions 5   
   �� 5      $��2* �6   6-   �� 5      $
��auto �6   6/   �� 5      $��1* �6   6-   .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $
��auto �6    60   �� 5      $��* �6    6-   .+�5      �� $��	10,10,5,5q�6    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         ShadowDepth$ 1Q�5         	Direction$	 360Q�5         Color$
 Gray��5         Opacity$	 0.3Q�5      	   
BlurRadius$	 10Q�5       ._�5"      �  5#      �� 5$      $��* �6$   �� 5%      $	��140 �6$   .+�5'      �� 
 �� CornerRadius$
 7,7,0,0��6   $��52��61   $��0��5(         SecondColor#��[  5*      ��5)      Z� ._�5+      � -   5        $ grid_tabControl��5+       5,      �� 5-      $
��auto �6/   �� 5.      $
��auto �6/   �� 5/      $
��auto �6/   �� 50      $��* �6/   �� 51      $
��auto �6/   .+�54      �� -   5         ��Name$ bdr_item��54   !   $��0��63   
�������$��0q�6V   $��40��6k   $��60��6w   $��Bottom=�6�   $
 7,7,0,0��6�   
������ $��	20,5,0,-1q�6�   ._�55      �� -   5         ��Name$ btn_item��55   %   
������ $  items��6R   +  %MaterialDesignThemes.Wpf.RippleAssist   Feedback
 ���� 
������ $
��auto��57   ?    � Triggers 58   "   :� 59   &    :� RoutedEvent$ Button.Click��63   .[�5:   *   �� .��5;   .   �� .��5<   2   W�  �� 
TargetName$ bdrMain��5=       �� TargetProperty$ Opacity�5>       W� From$ 0X�5?      	 W� To$ 1X�6"    g� Duration$ 0:0:1D�5@      5A   /   5B   +   5C   '   .��5E   "   F� -   5         F�Name$ 	path_item��5F   ^   #��[  5E   '    �� Stretch$
 Fill=�6K   $	��Top=�6[   $��0,-1,0,0q�6s   $��Center=�6�   $��25��5F   (   $��25��63   $��LeftToRight=�6@     rawMaterials#��[ 5G   -    � ToolTip 5H   &   _� 5I   *   -   5         _�Name$ tt_item��5I   2   $��Items��6C    _� 	Placement$
 left=�5J   /    _� HorizontalOffset$ 35��5K   /    _� PlacementRectangle$ 50,0,0,0�5L   /    _� VerticalOffset$	 -10��5M   /   5O   #   5P      5Q      �� 5S      -   5        $ 	bdr_agent��5S   !   $��1��64   #��[  6D   $��0q�6n   $��40��6�   $��60��6�   $��Bottom=�6�   $
 7,7,0,0��6�   
������ $��	10,5,0,-1q�6�   ._�5T      �� -	   5        $ 	btn_agent��5T   %   
������ $  agent��6S   
 ���� 
������ $
��auto��5V   ?    5W   "   :� 5X   &   $ Button.Click��63   .[�5Y   *   �� .��5Z   .   �� .��5[   2   W� $ bdrMain��5\      $ Opacity�5]      $ 0X�5^      $ 1X�6"   $ 0:0:1D�5_      5`   /   5a   +   5b   '   .��5d   "   F� -
   5        $ 
path_agent��5e   ^   
�������$
 Fill=�5d   4   $	��Top=�6D   $��0,-1,0,0q�6\   $��Center=�6n   $��25��5e   (   $��25��63   $��LeftToRight=�6B     customer#��[ 5f   !    5g   &   _� 5h   *   -   5        $ tt_agent��5h   2   $��Agents��6D   $
 left=�5i   /   $ 35��5j   /   $ 50,0,0,0�5k   /   $	 -10��5l   /   5n   #   5o      5p      �� 5r      -   5        $ bdr_invoice��5r   !   $��2��66   #��[  6F   $��0q�6p   $��40��6�   $��60��6�   $��Bottom=�6�   $
 7,7,0,0��6�   
������ $��	10,5,0,-1q�6�   ._�5s      �� -   5        $ btn_invoice��5s   %   
������ $  invoices��6U   
 ���� 
������ $
��auto��5u   ?    5v   "   :� 5w   &   $ Button.Click��63   .[�5x   *   �� .��5y   .   �� .��5z   2   W� $ bdrMain��5{      $ Opacity�5|      $ 0X�5}      $ 1X�6"   $ 0:0:1D�5~      5   /   5�   +   5�   '   .��5�   "   F� -   5        $ path_invoice��5�   ^   
�������$
 Fill=�5�   4   $	��Top=�6D   $��0,-1,0,0q�6\   $��Center=�6n   $��25��5�   (   $��25��63   $��LeftToRight=�6@     invoice#��[ 5�   '    5�   &   _� 5�   *   -   5        $ 
tt_invoice��5�   2   $��Invoices��6F   $
 left=�5�   /   $ 35��5�   /   $ 50,0,0,0�5�   /   $	 -10��5�   /   5�   #   5�      5�      �� 5�      $��10��6!   $
 7��63   ._�5�      �� -   5         ��Name$ txt_tabTitle��5�   )   
�������$��10��6U   $��Center=�5�      $��16(�6?   $��5 5 10 5q�6Q   5�      5�      5�      �� 5�      $
 0,0,7,7��6   $��1��5�      
�������$
��auto��5�   ,   ��5�      Z� ._�5�      �� -   5        $ bdrMain��5�   9   $��	15,0,15,0q�6   $
��auto��6J   
������ ._�5�      �� $��Disabled=�6'   $
��Auto=�6N   .��5�      � .+�5�   "   � -   5        $ grid_external��5�   '   $y�Visible=�5�   '    5�   &   �� 5�   *   $
��auto �68   6H   �� 5�   *   $
��auto �68   6H   .+�5�   &   �� -   5        $ bdrMainExternal��5�   V   $��1��6-   $��	15,0,15,0q�6:   $
��auto��6o   
������ ._�5�   *   � .+�5�   .   � -   5        $ grid_externalItems��5�   3   $��
10,0,10,10q�5�   3   $y�Visible=�5�   3    5�   2   �� 5�   6   $
��auto �6D   6T   �� 5�   6   $
��auto �6D   6T   �� 5�   6   $
��auto �6D   6T    5�   2   �� 5�   6   $��* �6G   6S   �� 5�   :   $
��auto �6K   6Z   �� 5�   6   $��* �6G   6S   �� 5�   :   $
��auto �6K   6Z   �� 5�   6   $��* �6G   6S   �� 5�   6   $��* �6G   6S   .+�5�   6   �� $��1��6=   $��40��6J   $
 10��6V   $��
10,10,10,5q�6h   $��1q�6~     	LightGrey#��[ 6�   ._�5�   :   � .+�5�   >   �� -   5        $��cb_externalItemsBranches��5�   G    & "MaterialDesignFloatingHintComboBox#��[ 5�   ;   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Branch/Store ....��5�   ;   ���#4E4E4E���#737373$��0q�5�   S   $	��4,0q�6g   $��40��5�   ;   $��10,0,5,0q�5�   ;   $��1��5�   ;   5�   ;   5�   ;   5�   7   �� 5�   2   -   5         ��Name$" chk_externalItemsAllBranches��5�   ;     	MainColor#��[ 5�   ;   $��1��5�   ;   $��1��5�   ;   $	��All��5�   ;    2 .MaterialDesignFilterChipPrimaryOutlineCheckBox#��[ 5�   ;   5�   ;   �� 5�   2   $��1��6=   $��2��5�   =     �� Orientation$  
Horizontal=�5�   =   .+�5�   6   �� -   5        $ chk_externalItemsIn��5�   ?   $��In��5�   ?   $��Center=�6L   $��5,0,10,0q�5�   ?   5�   ?   �� 5�   6   -   5        $ chk_externalItemsOut��5�   ?   $	��Out��5�   ?   $��Center=�6N   5�   ?   5�   3   ]� 5�   2   $��3��6<   $��1��5�   <   $��4��5�   <   ��.  .+�5�   :   �� $��0��6A   $��Right=�6N   $��40��6e   $
 10��6q   $��
10,10,10,5q�6�   $��1q�6�   #��[ 6�   ._�5�   >   � .+�5�   B   (  "System.Windows.Controls.DatePicker  -   5        $��dp_externalItemsEndDate��5�   M   !   SelectedDateFormat$! Short=�5�   A   $	��100��5�   A   $��10,0,5,0q�5�   A   $��40��5�   A   ���#737373$��0q�5�   Y   $	��4,0q�6m   $��Right=�5�   A    ( $MaterialDesignFloatingHintDatePicker#��[ 5�   A   $ End Date...��5�   A   .  (MaterialDesignThemes.Wpf.TextFieldAssist"   CharacterCounterStyle" 5�   A   �� 5�   A   5�   ?   5�   ;   �� 5�   :   $��0��6A   $��Right=�6N   $��40��6e   $
 10��6q   $��
10,10,10,5q�6�   $��1q�6�   #��[ 6�   ._�5�   >   � .+�5   B     -   5        $��dp_externalItemsStartDate��5   M   $! Short=�5  A   $	��100��5  A   $��10,0,5,0q�5  A   $��40��5  A   ���#737373$��0q�5  Y   $	��4,0q�6m   $��Right=�5  A   #��[ 5  A   $ Start date...��5	  A   " 5  A   �� 5
  A   5  ?   5  ;   5  3   �� 5  6   $��2��6=   $��40��6J   $
 10��6V   $��	10,5,10,5q�6h   $��1q�6}   #��[ 6�   ._�5  :   � .+�5  >   �� -   5        $��cb_externalItemsItems��5  G   $��2��5  ;   {�.  #��[ 5  ;   $ 
Items ....��5  ;   ���#4E4E4E���#737373$��0q�5  S   $	��4,0q�6g   $��40��5  ;   $��10,0,5,0q�5  ;   ��5  ;   �� 5  ;   5!  ;   5"  7   �� 5%  2   -   5        $ chk_externalItemsAllItems��5%  ;   #��[ 5&  ;   $��2��5'  ;   $��1��5(  ;   {�.  $	��All��5*  ;   #��[ 5+  ;   5-  ;   �� 5.  6   $��2��6=   $��2��6J   $��40��6Z   $
 10��6f   $��	10,5,10,5q�6x   $��1q�6�   #��[ 6�   ._�5/  :   � .+�50  >   �� -   5        $��cb_externalItemsUnits��50  G   {�.  $��2��52  ;   #��[ 53  ;   $ 
Units ....��54  ;   $��2��55  ;   ���#737373$��0q�56  S   $	��4,0q�6g   ���#4E4E4E$��40��58  ;   $��10,0,5,0q�59  ;   5:  ;   5=  ;   5>  7   �� 5B  2   -   5        $ chk_externalItemsAllUnits��5B  ;   #��[ 5C  ;   {�.  $��2��5E  ;   $��3��5F  ;   $	��All��5G  ;   #��[ 5H  ;   5J  ;   5L  /   � 5N  .   -   5        $ grid_externalAgents��5N  3   $��
10,0,10,10q�5O  3   $��1��5P  3   $y�	Collapsed=�5Q  3    5S  2   �� 5T  6   $
��auto �6D   6T   �� 5U  6   $
��auto �6D   6T   �� 5V  6   $
��auto �6D   6T    5X  2   �� 5Y  6   $��* �6G   6S   �� 5Z  :   $
��auto �6K   6Z   �� 5[  6   $��* �6G   6S   �� 5\  :   $
��auto �6K   6Z   �� 5]  6   $��* �6G   6S   �� 5^  6   $��* �6G   6S   .+�5`  6   �� $��1��6=   $��40��6J   $
 10��6V   $��
10,10,10,5q�6h   $��1q�6~   #��[ 6�   ._�5a  :   � .+�5b  >   �� -    5        $��cb_externalAgentsBranches��5b  H   #��[ 5c  <   $ Branch/Store ....��5d  <   ���#4E4E4E���#737373$��0q�5f  T   $	��4,0q�6h   $��40��5g  <   $��10,0,5,0q�5h  <   $��1��5i  <   5j  <   5k  ;   5l  7   �� 5p  2   -!   5        $# chk_externalAgentsAllBranches��5p  ;   #��[ 5q  ;   $��1��5r  ;   $��1��5s  ;   $	��All��5t  ;   #��[ 5u  ;   5w  ;   �� 5y  2   $��1��6=   $��2��5z  =   $  
Horizontal=�5{  =   .+�5}  6   �� -"   5        $ chk_externalAgentsIn��5}  ?   $��In��5~  ?   $��5,0,10,0q�5  ?   5�  ?   �� 5�  6   -#   5        $ chk_externalAgentsOut��5�  ?   $	��Out��5�  ?   5�  ?   5�  3   ]� 5�  2   $��3��6<   $��1��5�  <   $��4��5�  <   ��.  .+�5�  :   �� $��0��6A   $��Right=�6N   $��40��6e   $
 10��6q   $��
10,10,10,5q�6�   $��1q�6�   #��[ 6�   ._�5�  >   � .+�5�  B     -$   5        $��dp_externalAgentsEndDate��5�  M   $! Short=�5�  A   $	��100��5�  A   $��10,0,5,0q�5�  A   $��40��5�  A   $��Right=�5�  A   ���#737373$��0q�5�  Z   $	��4,0q�6n   #��[ 5�  A   $ End Date...��5�  A   " 5�  A   �� 5�  A   5�  ?   5�  ;   �� 5�  :   $��0��6A   $��Right=�6N   $��40��6e   $
 10��6q   $��
10,10,10,5q�6�   $��1q�6�   #��[ 6�   ._�5�  >   � .+�5�  B     -%   5        $ ��dp_externalAgentsStartDate��5�  M   $! Short=�5�  A   $	��100��5�  A   $��10,0,5,0q�5�  A   $��40��5�  A   ���#737373$��0q�5�  Y   $	��4,0q�6m   #��[ 5�  A   $ Start date...��5�  A   " 5�  A   �� 5�  A   5�  ?   5�  ;   5�  3   �� 5�  6   $��2��6=   $��40��6J   $
 10��6V   $��	10,5,10,5q�6h   $��1q�6}   #��[ 6�   ._�5�  :   � .+�5�  >   �� -&   5        $!��cb_externalAgentsAgentsType��5�  G   $��2��5�  ;   {�.  #��[ 5�  ;   $ Agent Type ....��5�  ;   ���#4E4E4E���#737373$��0q�5�  S   $	��4,0q�6g   $��40��5�  ;   $��10,0,5,0q�5�  ;   5�  ;   5�  ;   5�  7   �� 5�  2   -'   5        $% chk_externalAgentsAllAgentsType��5�  ;   #��[ 5�  ;   $��2��5�  ;   $��1��5�  ;   {�.  $	��All��5�  ;   #��[ 5�  ;   5�  ;   �� 5�  6   $��2��6=   $��2��6J   $��40��6Z   $
 10��6f   $��	10,5,10,5q�6x   $��1q�6�   #��[ 6�   ._�5�  :   � .+�5�  >   �� -(   5        $��cb_externalAgentsCustomer��5�  G   $��2��5�  ;   {�.  #��[ 5�  ;   $ Customer/Vendor ....��5�  ;   $��2��5�  ;   ���#4E4E4E���#737373$��0q�5�  S   $	��4,0q�6g   $��40��5�  ;   $��10,0,5,0q�5�  ;   5�  ;   5�  ;   5�  7   �� 5�  2   -)   5        $$ chk_externalAgentsAllCustomers��5�  ;   #��[ 5�  ;   {�.  $��2��5�  ;   $��3��5�  ;   $	��All��5�  ;   #��[ 5�  ;   5�  ;   5�  /   � 5�  .   -*   5        $ grid_externalInvoices��5�  3   $��1��5�  3   $��
10,0,10,10q�5�  3   $y�	Collapsed=�5�  3    5�  2   �� 5�  6   $
��auto �6D   6T   �� 5�  6   $
��auto �6D   6T   �� 5�  6   $
��auto �6D   6T    5�  2   �� 5�  6   $��* �6G   6S   �� 5�  :   $
��auto �6K   6Z   �� 5�  6   $��* �6G   6S   �� 5�  :   $
��auto �6K   6Z   �� 5�  6   $��* �6G   6S   �� 5�  6   $��* �6G   6S   .+�5�  6   �� $��1��6=   $��40��6J   $
 10��6V   $��
10,10,10,5q�6h   $��1q�6~   #��[ 6�   ._�5�  :   � .+�5   >   �� -+   5        $!��cb_externalInvoicesBranches��5   H   #��[ 5  <   $ Branch/Store ....��5  <   ���#4E4E4E���#737373$��0q�5  T   $	��4,0q�6h   $��40��5  <   $��10,0,5,0q�5  <   $��1��5  <   5	  ?   5
  ;   5  7   �� 5  2   -,   5        $% chk_externalInvoicesAllBranches��5  <   #��[ 5  <   $��1��5  <   $��1��5  <   $	��All��5  <   #��[ 5  <   5  <   ]� 5  2   $��3��6<   $��1��5  <   $��4��5  <   ��.  .+�5  :   �� $��0��6A   $��Right=�6N   $��40��6e   $
 10��6q   $��
10,10,10,5q�6�   $��1q�6�   #��[ 6�   ._�5  >   � .+�5  B     --   5        $ ��dp_externalInvoicesEndDate��5  M   $! Short=�5   A   $	��100��5!  A   $��40��5"  A   ���#737373$��0q�5#  Y   $	��4,0q�6m   $��Right=�5$  A   $��10,0,5,0q�5%  A   #��[ 5&  A   $ End Date...��5'  A   5(  A   5*  ?   5+  ;   �� 5.  :   $��0��6A   $��Right=�6N   $��40��6e   $
 10��6q   $��
10,10,10,5q�6�   $��1q�6�   #��[ 6�   ._�5/  >   � .+�51  B     -.   5        $"��dp_externalInvoicesStartDate��51  M   $! Short=�52  A   $	��100��53  A   $��10,0,5,0q�54  A   $��40��55  A   ���#737373$��0q�56  Y   $	��4,0q�6m   $��Right=�57  A   #��[ 58  A   $ Start date...��59  A   5:  A   5<  ?   5=  ;   5?  3   �� 5A  6   $��2��6>   $��40��6K   $
 10��6W   $��	10,5,10,5q�6i   $��1q�6~   #��[ 6�   ._�5B  :   � .+�5D  >   �� -/   5        $$��cb_externalInvoicesInvoiceType��5D  G   $��2��5E  ;   {�.  #��[ 5G  ;   $ Invoice Type ....��5H  ;   ���#4E4E4E$��40��5J  ;   ���#737373$��0q�5K  S   $	��4,0q�6g   $��10,0,5,0q�5L  ;   5M  ;   5O  ;   5P  7   �� 5S  2   -0   5        $) #chk_externalInvoicesAllInvoicesType��5S  ;   #��[ 5T  ;   $��2��5U  ;   $��1��5V  ;   {�.  $	��All��5X  ;   #��[ 5Y  ;   5[  ;   �� 5]  6   $��2��6=   $��2��6J   $��40��6Z   $
 10��6f   $��	10,5,10,5q�6x   $��1q�6�   #��[ 6�   ._�5^  :   � .+�5`  <   �� -1   5        $ ��cb_externalInvoicesInvoice��5`  E   $��2��5a  ;   {�.  #��[ 5c  ;   $ Invoice ....��5d  ;   $��2��5e  ;   ���#737373$��0q�5f  S   $	��4,0q�6g   ���#4E4E4E$��40��5h  ;   $��10,0,5,0q�5i  ;   5j  ;   5l  ;   5m  7   �� 5p  2   -2   5        $$ chk_externalInvoicesALlInvoice��5p  ;   #��[ 5q  ;   {�.  $��2��5s  ;   $��3��5t  ;   $	��All��5u  ;   #��[ 5v  ;   5x  ;   5y  /   5{  +   5|  '   5}  #   5�     5�     5�     5�     5�     5�     �� 5�     $��1��6   $��	10,5,5,10q�6#    5�       5�     $ 1Q�5�     $	 360Q�5�     $
 Gray��5�     $	 0.3Q�5�     $	 10Q�5�     ._�5�     � $��1��6    5�     �� 5�     $
��auto �6(   68   �� 5�     $��45 �6(   66   �� 5�     $��* �6(   65   �� 5�     $��45 �6(   .+�5�     �� $��1��6   
�������$
 7,7,0,0��6=   ._�5�     �� $��0,0,0,1q�6   ���	#FFC8C8C8._�5�     ]� ��.  .+�5�  "   �� $��0��6)   $	��200��66   $
 10��6C   $��	10,5,10,5q�6U   $��1q�6j   #��[ 6~   ._�5�  &   � .+�5�  *   �� -3   5        # ��Name$# 
txt_search��5�  2   $��10,0,5,0q�5�     ���#737373$��0q�5�  J   $	��4,0q�6^    %	 !MaterialDesignFloatingHintTextBox#��[	 5�  ,   $% Enter Here To Start Search ....��5�     ���#4e4e4e5�     5�  '   5�  #   �� 5�  "   $��0,5,0,0q�6)   
�������$��30��6N   $��30��6Y   ._�5�     F� ���#4e4e4e$
 Fill=�62   $��0��5�     $
��17.5��5�     $
��17.5��6&   ����p��ffffff0@������lW@���fffff&P@��������LG@ffffff0@������`��`,a 0v�hx�`r��ō�������}��q����f�z��@) ����Z @ sӀ�  ���� `�Z  GS@�� �����y_@ $� ����. G��. G�. G�. G� B� �{�������F@��D�������F@��D� ���$7Ѡ'�@��S���43333U@`�������J@�A � �V `� ظ23333]@@�~�`���k�~�fffff&@@��ffffff0@Q��ffffff0@� K@zA�����LV@43333�T@@�`6 �����LL@@zA�����F@43333sM@�����F@gfffffC@�����LL@hfffff:@S�a     ,@�����LV@������+@ Khfffff:@S��i433333@@���    @D@���43333sH@� 3� Tj��i�����YP@ K@zA�`}�`F�Ӏ`F�gffff�S@@2Ygffff�S@`���gffff�S@�d�43333�S@�����=@  � ��� ����`�y �,�{s@y��`am �������'�Э �-��W`��3 �� ]����������H@S ��������G@@��������F@ M�������E@� B� �� ��`F�`}�`F�	5�     5�     �� 5�     -4   5        $ btn_refresh��5�  k   $��LeftToRight=�6!   $��Right=�6=   $��40��6T   $��40��6_   +  %MaterialDesignThemes.Wpf.ButtonAssist$   CornerRadius$$ 15��5�     % � Cursor$
% Hand��6K    
 SmallButtonCircleRotate#��[
 5�  J   $	��5,1q�5�     $��
إضافة��6,   & J� RenderTransformOrigin$& 0.5,0.5&�6=   ' � DataContext$�
' �
M896,0L896,0C401.2,0,0,401.2,0,896v0c0,494.8,401.2,896,896,896h0c494.8,0,896-401.2,896-896v0C1792,401.2,1390.8,0,896,0z   M1451.3,1021c-31.9,133.7-98.8,242.1-200.6,325.2c-101.8,83.1-221,124.6-357.7,124.6c-72.8,0-143.3-13.7-211.4-41.2  s-128.8-66.6-182.2-117.5l-96.5,96.5c-9.5,9.5-20.7,14.2-33.7,14.2s-24.2-4.7-33.7-14.2c-9.5-9.5-14.2-20.7-14.2-33.7v-335.3  c0-13,4.7-24.2,14.2-33.7s20.7-14.2,33.7-14.2h335.3c13,0,24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7c0,13-4.7,24.2-14.2,33.7  l-102.5,102.5c35.4,32.9,75.6,58.4,120.5,76.3c44.9,18,91.5,26.9,139.9,26.9c66.9,0,129.2-16.2,187.1-48.6  c57.9-32.4,104.3-77.1,139.2-134c5.5-8.5,18.7-37.7,39.7-87.6c4-11.5,11.5-17.2,22.4-17.2h143.7c6.5,0,12.1,2.4,16.8,7.1  c4.7,4.7,7.1,10.4,7.1,16.8C1452,1018.2,1451.8,1020,1451.3,1021z M1470.7,752.3c0,13-4.7,24.2-14.2,33.7  c-9.5,9.5-20.7,14.2-33.7,14.2h-335.3c-13,0-24.2-4.7-33.7-14.2s-14.2-20.7-14.2-33.7c0-13,4.7-24.2,14.2-33.7l103.3-103.3  C1083.3,547,996.3,512.9,896,512.9c-66.9,0-129.2,16.2-187.1,48.6s-104.3,77.1-139.2,134c-5.5,8.5-18.7,37.7-39.7,87.6  c-4,11.5-11.5,17.2-22.5,17.2H358.7c-6.5,0-12.1-2.4-16.8-7.1c-4.7-4.7-7.1-10.4-7.1-16.8V771c32.4-133.7,99.8-242.1,202-325.2  c102.3-83.1,222-124.6,359.2-124.6c72.8,0,143.7,13.8,212.5,41.5s130,66.7,183.3,117.1l97.3-96.5c9.5-9.5,20.7-14.2,33.7-14.2  s24.2,4.7,33.7,14.2c9.5,9.5,14.2,20.7,14.2,33.7V752.3z��5�     ��5�     Z� ��61   Z� z�5�     V� 5�  "   .~�5�  &   �� �� 5�  &   �� 5�  &   ( �� Angle$( 0Q�66   T� 5�  &   5�  #    5�     _� 5�  "   -5   5        $ 
tt_refresh��5�  *   $��Refresh��6>   $
 left=�5�  /   $ 15��5�  /   $ 50,0,0,0�5�  /   $ 25��5�  /   5�     5�     5�     5�     �� 5�     $��2��6   
�������._�5�     � $��1��6    .+�5�     �� -6   5        $ brd_roundDataGrid��5�  Z   $��
5,5,5,17.5q�6%   $��1��6;   $
 10��6H   $��0q�6u   
�������5�     &   System.Windows.Controls.DataGrid  5�     -7   5        $��dgStock��5�     $��5q�5�  (   )   
IsReadOnly) . 
�������*   AutoGenerateColumns* .  +   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$+ SizeToHeader 5�  !   ,   AlternatingRowBackground, �#f3f3f3- �� AlternationCount$- 2��5�  H   . J� OpacityMask. 5�  "   !� 5�  &   / !� Visual/ 62   �� 0 �� ElementName$0 brd_roundDataGrid��1 � 	Resources1 5�  "   =	  7System.Windows.Controls.Primitives.DataGridColumnHeader2 �� BasedOn3 �� 
TargetType4 �� Property5 �� Value��%  (��       5�  6   * SystemColors.HighlightBrushKey+)'	       7	 7   �� 6&   6j   �� 5�  &   82   "3 	 6e   .�5�  *   �� 4 � ��5 �#F5D7C46G   �� 5�  *   4 � ��
5 �����6H   �� 5�  *   4 � ��$5 0 0 1 0q�6L   �� 5�  *   4 � ��85  6G   5�  '   5�  #   6   Columns6 5�  "   0
  *System.Windows.Controls.DataGridTextColumn
  5�  "   -8   5        col_invNumber��,  &System.Windows.Controls.DataGridColumn7   
Visibility$7 Hidden=�5�  L   8   Width$8 * 6`   9   Header$	9 No.��6j   1  +System.Windows.Controls.DataGridBoundColumn:   Binding: 6w   �� *	invNumber+
  5�  "   -9   5        col_invTypeNumber��$7 Hidden=�5�  P   $8 * 6d   $9 Invoice Type-Number��6n   : 6�   �� *	invType+  invoiceTypeConverter; �� 	Converter#; [ 
  5�  "   -:   5        col_invDate��$7 Hidden=�5�  J   $8 * 6^   $9 Invoice Date��6h   : 6~   �� *	invDate+  dateFrameConverter#; [ 
  5�  "   -;   5        
col_branch��$8 * 5�  I   $9 Brnach��6S   : 6c   �� *
branchName+
  5�  "   -<   5        col_agentType��$7 Hidden=�5�  L   $8 * 6`   $9 
Agent Type��6j   : 6~   �� *	agentType+  agentTypeConverter#; [ 
  5�  "   -=   5        	col_agent��$7 Hidden=�5�  H   $8 * 6\   $9 
Agent Name��6f   : 6z   �� *	agentName+
  5�  "   ->   5        col_itemUnits��$7 Hidden=�5�  L   $8 * 6`   $9 Items - Units��6j   : 6�   �� *	ItemUnits+
  5�  "   -?   5        col_item��$8 * 5�  G   $
9 Item��6Q   : 6_   �� *
itemName+
  5�  "   -@   5        col_unit��$8 * 5�  G   $
9 Unit��6Q   : 6_   �� *
unitName+
  5�  "   -A   5        col_agentTypeAgent��$7 Hidden=�5�  Q   $8 * 6e   $9 Agent Type-Name��6o   : 5�  &   `� 5�  *   < �� StringFormat$< {0}{1}��67   .1�5�  .   �� = �� Path$= AgentTypeAgent�66   �� 5   .   $= AgentNameAgent�66   5  +   5  #   
  5  "   -B   5        col_quantity��$8 * 5  K   $	9 QTY��6U   : 6b   �� *
quantity+5     5	     5
     �� 5     $��3��6   
�������$
 0,0,7,7��6>   ._�5     ]� ��.  .+�5     �� $��0,0,0,1q�6   #��[ 67   $��
10,0,10,10q�6`   $
��auto��6t   $
��Left=�6�   ._�5     �� $  
Horizontal=�6&   .+�5     �� -C   5        $ btn_pdf��5  %   $��10,0,5,0q�6P   $��0q�5     $��35��6'   ��5  8   Z� ��5     Z� .��5  "   '  !MaterialDesignThemes.Wpf.PackIcon  ���#D20A0A>   Kind$> FilePdf=�6O   $��22��5     $��22��6&    5  &   _� 5  *   -D   5        $ 	tt_report��5  2   $��Report��6E   $
 left=�5  /   $ 5��5  /   $ 50,0,0,0�5  /   $	 -10��5  /   5  #   5     �� 5     -E   5        $ 	btn_print��5   4   $��5,0,5,0q�5  @   $��0q�5      $��35��6'   ��5  '   Z� ��5!     Z� .��5"  "     ���#313131$> Printer=�6O   $��Center=�5#     $��22��5$     $��22��6"    5%  &   _� 5&  *   -F   5        $ tt_print��5&  2   $��Print��6D   $
 left=�5'  /   $ 5��5(  /   $ 50,0,0,0�5)  /   $	 -10��5*  /   5,  #   5-     �� 5/     -G   5        $ btn_exportToExcel��50  7   $��3��5/  @   $��5,0,5,0q�6P   $��0q�50     $��35��6)   ��5/  '   Z� ��51  (   Z� .��52  "   F� ���#378500$
 Fill=�66   $��20��53     $��20��6&   $��LeftToRight=�54     ��������- � $� ��- � $�  O[ ��c" O[ Q�c"H�z� @Q $� H�z� @� $� ��� ���!��� ���! � � $�  � � $� ��� ���!��� ���! $� � $�  $� � $� �����!�����! -1� $�  -1� $� ��^����^������^���h�X���P�Qр��0; Ӏ���3 �����- ���- � 6n -1� -1 -1� -1��� 6n��� 6n -1� 6n $� � -1 $� � -1��� � 6n��� � 6n $� � 6n � � -1 � � -1��� � 6n��� � 6n � � 6n z � -1 z � -1��[ � 6n��[ � 6n z �    (, �    @�`���� �������     �    (, ��Ɛ  �E�����@Zd;ߏ-@�H$u Щ� U-��o@v��/�,@���x�&@��~j�4,@Q؛r ��~j�4,@��r P�� @�p 0)� �	o ��� ��QT  ����*  �Q8\ �x�&1*@�/ �Ry ��bY �Ry ��MbX9@੧ �x4q `S�  �r ��� p=
ףp@�v���'@Yd;�O�@�v���'@G�z��@|?5^�I'@MbX94@23333�&@k�����@�8� n���!#@������@���K7(@������@�0�Z$"@��� ��A`��b(@�рƐ  �рƐ  �	 57  &   _� 58  *   -H   5        $ tt_excel��58  2   $��Export to excel��6D   $
 left=�59  /   $ 35��5:  /   $ 50,0,0,0�5;  /   $	 -10��5<  /   5>  #   5?     �� 5@     -I   5        $ btn_settings��5A  9   $��3��5@  @   $��
5,0,5,-0.6q�6P   $��0q�5A     $
��Auto��6)   $y�	Collapsed=�6O   ��5@  '   Z� ��5B  (   Z� .��5C  "     $��20��5D     $��20��6&   $��LeftToRight=�5E     $> Settings=�68   
�������5G  #   5H     �� 5I     -J   5        $ btn_preview��5J  7   $��3��5I  @   $��0,1,5,0q�6P   $��0q�5J     $��35��6)   ��5I  '   Z� ��5K  (   Z� .��5L  "   � $��LeftToRight=�6'   .+�5M  &   F� ���#4E4E4E$
 Fill=�6:   $��15��5N     $��18��6&   $��LeftToRight=�5O     ������T1 E� 1���A�������dx@�pi�Y:������$q@@��������n@ s� �M�����iu@������Ik@�� �����)k@������t@ ������Tt@S�>/�����'t@@�P������s@`�p������s@S@ 23333+s@�H�����̔r@�9������r@S�	�23333kq@������r@������p@@G.effff�p@S �ceffff�p@gffffft@effff�p@gffffft@����̴p@43333ct@�����om@`�r03333si@`�rcffffve@S`�rcffff�d@`�rcffff�d@��#cffff�d@gffffFo@cffff�d@gffff�n@cffff�d@    `n@������d@    `n@03333�c@�    `n@���     n@ ��    �m@`�����̼m@�m��@���m�Ӏ���m�����m� �{������T@S��������T@�/���Gਤ���ਤ����43333sS@ jM43333sS@�S�ਤ =Q�ਤ�bw�Э �  ����gffff�T@@� 2:�}.gfffffW@�}.��Q��}.�gfffft@�}.�3����gffff^t@�����ix@�`�Z��F�*E��F�T1 E��/��aS� f��T`#Y��T�����T�`�b��T���T��T��H oV43333S^@��T�gffff�]@@�Q     ]@�~<`~�@��43333�\@����� ���j<�����yp@S@��gffffvp@���ఴ@Oఴ� ��ఴ�������q@ �������	r@��������	r@ �� )2�I0 ��aS�/��aS�_ z"S� $gffff�]@�����Y^@�_�3M�_� ���_�43333�i@�_��V�]943333�i@@$gӠ%[�q�����̜i@ �������i@ ��hffffvg@fffff6`@������e@ ��hffff&d@ ���63333s^@ ���63333�]@ ��������\@fffff`@     ]@ �wӠ��@�Z��j<�_ z"��+���	�������^@ E�	 �|���	fffff`@���	������lj@���	�23333�j@���	�ͼ���	������j@@R�	Ӡ���l 
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
�+���	���Q�D��@���D���s��D���@���d��D��@+��� )2`��gffffr@�&��/�VR    �q@�To    �q@�����9l@gffff�q@�����?l@43333�q@�����<l@������q@�����<l@�����9i@�����<l@63333f@�����<l@������b@�����<l@kffff�_@�����?l@����̬^@�����?l@kffff�]@�����<l@������\@�����	l@73333�\@������k@� q�`��`�D���Q�D��`�K�0�ӀOG@�gffff�i@`���{�����w@gffffFi@�����$w@�}������!w@`;������!w@43333�^@�����!w@43333s]@�����!w@gffff]@���43333�\@�#��    �\@����a�`�s43333s^@`�s43333�`@�����|v@��������|v@433333d@�����|v@gffff�e@�����|v@�W"23333{v@������h@�����|v@43333#i@�����|v@�����li@������v@43333�i@������v@Ӡ�B 5�@wP�M�`�K�0�� ,��/S�Ӵ�����!t@`�c�T1���T1�43333�^@�T1��D�T1�����^@�a443333�]@`�)����gffff]@@s�gffff]@���gffff]@gffff�s@gfffff]@gffff�s@    �]@43333�s@������]@gffffvs@43333s^@���43333�^@����gffff&d@����gffff�e@���gffffFg@���gffff�h@���gffff�h@��gffffi@��gffff6i@���gffff�i@gffff~s@    �i@`��gffff�i@���� U��  5@G. ,��/�`�YCS ������V@`�Q`�����yV@S`����̬[@ ������l`@��@c	����D	`O(����̌c@�����p@����̌c@S ׋������c@ �������c@ 1�������c@Q��=������c@� n����� x�%�`�YC	 5P  *   _� 5Q  .   -K   5        $ 
tt_preview��5Q  6   $��Preview��6J   $
 left=�5R  /   $ 35��5S  /   $ 50,0,0,0�5T  /   $	 -10��5U  /   5W  '   F� 5X  &     Blue#��[ 6+   $
 Fill=�6H   $��13��5Y     $��6��6&   $��	17,10,0,0q�61   $��LeftToRight=�5Z     ������O|��}�`g� � e`���� �� Թ �V@� ���%����������s@@W��g,@��@ܳ S�V3�����Qr@`{��T�����iS �������t@ ���������#� %��^�fffffp@�����lv@`s ��S ��������w@���P����@#S������̬v@����k�@w�@����0�  ��#��Ҧ�O|��}� @�����'@����\�� \[��������1r@gffff�s@ M&��������t@��������u@������r@������u@ ��������u@�I��@���)�@k"@�� @���@?S �����T`g�~��i��%�i�������is@�i�������r@��e������r@�<��������r@�������ds@ *������t@�����������Q��@?S ��	5\  '   5]  #   5^     �� 5_     -L   5        $ 	txt_count��5_  @   $��4��6U   ? �� Padding$? 10q�5`     $��0��6,   ���#378500$��12(�6J   ��5_  *   Z�  5a  "   _� 5b  &   -M   5        $ tt_count��5b  .   $��Customers Count��6@   $
 left=�5c  /   $ 35��5d  /   $ 	50,25,0,0�5e  /   $	 -20��5f  /   5h     5j     5k     �� 5l     -N   5        @ ��Name$@ stk_tagsBranches��5l  9   $��Right=�6!   $  
Horizontal=�6S   �� 5m     -O   5        $@ stk_tagsPos��5m  9   $��Right=�6!   $  
Horizontal=�6N   �� 5n     -P   5        $@ stk_tagsVendors��5n  9   $��Right=�6!   $  
Horizontal=�6R   �� 5o     -Q   5        $@ stk_tagsUsers��5o  9   $��Right=�6!   $  
Horizontal=�6P   �� 5p     -R   5        $@ stk_tagsItems��5p  9   $��Right=�6!   $  
Horizontal=�6P   5r     5s     5t     5u     5v     �� 5w  
   $��1��6   
������ $��5��6@   $��Stretch=�6J   � 5y  
   $��2��6    5z     �� 5|     $��* �6    6-   �� 5}     $
��auto �6    60   �� 5~     $��* �6    6-   �� 5     $
��auto �6    60   �� 5�     $��* �6    6-   .+�5�     �� $��0��6   
�������$��	5,10,10,5q�66   $
 7��6I    5�       5�     $ 1Q�5�     $	 360Q�5�     $
 Gray��5�     $	 0.3Q�5�     $	 10Q�5�     ._�5�     � $��	5,10,5,10q�6   .+�5�     #  LiveCharts.Wpf.CartesianChart  -S   5        $��cartesianChart��5�  )   &   LiveCharts.Wpf.Charts.Base.ChartA   LegendLocation$A Bottom=�6?   B   AxisXB 5�       LiveCharts.Wpf.Axis  -T   5        C  Name$C axcolumn��5�  (   D   	SeparatorD 5�  "     LiveCharts.Wpf.Separator  5�  &   E   Step$E 1Q�64   5�     5�     5�     5�     �� 5�     $��1��6   
������ $��Stretch=�6A   $��5��6_   �� 5�     $��2��6   
�������$��5,5,10,5q�65   $
 7��6G    5�       5�     $ 1Q�5�     $	 360Q�5�     $
 Gray��5�     $	 0.3Q�5�     $	 10Q�5�     ._�5�     � -U   5        $ grid1��5�     $��	5,10,5,10q�6&    5�     �� 5�     $��* �6+   67   �� 5�     $
��auto �6+   6:   .+�5�       LiveCharts.Wpf.PieChart  -V   5        $��chart1��5�  #   $A Right=�61   5�     5�     5�     �� 5�     $��3��6   
������ $��Stretch=�6A   $��5��6_   �� 5�     $��4��6   
�������$��	5,5,10,10q�65   $
 7��6H    5�       5�     $ 1Q�5�     $	 360Q�5�     $
 Gray��5�     $	 0.3Q�5�     $	 10Q�5�     ._�5�       -W   5        $��rowChart��5�  %   $��	5,10,5,10q�65   $A Bottom=�6H   B 5�       -X   5        $C MyAxis��5�  $   D 5�       5�  "   $E 1Q�60   5�     5�     5�     5�     5�     5�     