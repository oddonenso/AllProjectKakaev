   M S B A M L   `   `   `  ����C  ?Pharmacy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Pharmacy.PagesPharmacy.Pages  5      #    Pharmacy.Pages.RedactDelivery   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Pharmacy.Pages     5        N� Title$  RedactDelivery��5	      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��ID доставки:��6   $��0��6(   $��0��65   $��5q�6E   �� 5   
   -   5         ��Name$ DeliveryIDTextBlock��5      $��0��61   $��1��6>   $��5q�6N   �� 5   
   $ ��Дата доставки:��6   $��1��6*   $��0��67   $��5q�6G   �� 5   
   -   5        $ DeliveryDateTextBlock��5      $��1��63   $��1��6@   $��5q�6P   �� 5    
   $��ID заказа:��6   $��2��6&   $��0��63   $��5q�6C   �� 5!   
   -   5        $ PickupRequestIDTextBlock��5!      $��2��66   $��1��6C   $��5q�6S   �� 5#   
   $��Количество:��6   $��3��6'   $��0��64   $��5q�6D   �� 5$   
   -   5        $ QuantityTextBlock��5$      $��3��6/   $��1��6<   $��5q�6L   �� 5&   
   $��	Цена:��6   $��4��6!   $��0��6.   $��5q�6>   �� 5'   
   -   5        $ PriceTextBlock��5'      $��4��6,   $��1��69   $��5q�6I   � 5)   
   $��5��6   $��2��6   $��5q�60    5*      �� 5+      $
��Auto �6#   �� 5,      $��* �6#   .+�5.      �� -   5         ��Name$ AcceptButton��5.      $��Принять заказ��6+   $��0��6C   $��5q�6S   6^   �� 5/      -   5        $ RejectButton��5/      $#��Отклонить заказ��6+   $��1��6E   $��270,5,270,0q�6U   $��40��6�   $	��Top=�6�   50      51      52      