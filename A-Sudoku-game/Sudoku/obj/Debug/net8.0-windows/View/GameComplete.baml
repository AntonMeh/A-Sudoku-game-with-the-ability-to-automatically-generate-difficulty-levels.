   M S B A M L   `   `   `  ����A  =Sudoku, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Sudoku.ViewModelSudoku.ViewModel  5   	       Sudoku.View.GameComplete   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   0srcVMclr-namespace:Sudoku.ViewModel     5   	     � Title$-  'Пазл судоку складено!��5   	   $	��250��5   	   $	��400��5   	    � ShowInTaskbar$ False��5   	    � WindowStartupLocation$ CenterOwner=�5	   	    � WindowStyle$ SingleBorderWindow=�5
   	    � 
ResizeMode$ NoResize=�5   	    � SizeToContent$ Height=�5   	    � 	Resources 5       �� 
TargetType �� Property	 �� Value
 �� 	Resources��%�  '��       '�� �     �� 5   
   " ��6   .�5      ��  � ��$	 Center=�64   �� 5       � ��$	 Center=�62   �� 5       �@��Padding$	 5q�6(   �� 5       �@��TextAlignment$	 Center=�6.   �� 5       � ��$	 16(�6)   5      �� 5   
   " ��6   
 5      ���� 5      '��       " ��6   .�5      ��  �@��CornerRadius$	 12��65   5      5      5      5      ��5      �� 5   
    �� ImageSource$ /View/Confeti.jpg��6   .��5!      �  � RowDefinitions 5"   
   �� 5#      $��65 �6   �� 5$      $	��100 �6   �� 5%      $��75 �6   .+�5'   
   �� $��Вітання!��6   $��0��5(       �� Padding$ 10q�5)      $��30(�5*      $
��Bold"�6"   $��Bodoni MT Black)�64   ��5+      �� 5,       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5-      �  � Color$ 	#FFFFF500��6#   � 5.      $	 Red��6#    � Offset$ 1Q�6/   5/      51      �� 52   
   $��Center=�6   $��Center=�62   $��1��6M   $	��120��6Z   .+�53      �� $'��!Ти завершив гру!🥳��6   $��16(�54      $��0,0,0,0q�55       �� TextAlignment$ Center=�56      
���   �6+   �� 58      -   5         ��Name$ CompletionTimeTextBlock��58      $��14(�59      $��5q�5:      $ Center=�5;      
��� d �5<      �� 5>      -   5        $ HintsUsedTextBlock��5>      $��14(�5?      $��5q�5@      $ Center=�5A      
����=H�5B      5C      �� 5D   
   $��3��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��10q�6X   .+�5E      �� -   5        $��Нова гра��5E      $��80��6(   $��30��63   $��5q�6?   
�������
���   ���5F      �� 5G      $ 0.5,1&�6*   $ 0.5,0&�6;   .��5H      � $ 	#FFFFF500��6'   � 5I      $ 	#FFC50A0A��6'   $ 1Q�69   5J      5L      �� 5N      -   5        $��OK��5N      $��80��6"   $��5q�6-   
�������$��30��6d   $��20(�6p   $
��Bold"�6~   
���   ���5O      �� 5P      $ 0.5,1&�6.   $ 0.5,0&�6?   .��5Q      � $ 	#FFFFF500��6+   � 5R      $ 	#FFC50A0A��6+   $ 1Q�6=   5S      5U      5V      5W      5X      