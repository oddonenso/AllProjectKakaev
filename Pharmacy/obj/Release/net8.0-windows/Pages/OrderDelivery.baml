   M S B A M L   `   `   `  ����   Pharmacy"    Pharmacy.Pages.OrderDelivery   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       N� Title$  OrderDelivery��.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $"��Выберите склад:��$��0���� -    ��Name$ cmbWarehouses��$��1�� �� DisplayMemberPath$ WarehouseName��$	��200��$��5q��� $&�� Выберите продукт:��$��2���� -   $ cmbProducts��$��3��$
 Name��$	��200��$��5q��� $��Количество:��$��4���� -    ��Name$ txtQuantity��$��5��$	��200��$��5q��� -    ��Name$ 	btnSubmit��$#��Отправить заказ��$��6��$	��150��$
��Left=�$��5q��� -   $ 	btnCancel��$��Отмена��$��6��$	��150��$��Right=�$��5q�