   M S B A M L   `   `   `  ����C  ?Pharmacy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Pharmacy.PagesPharmacy.Pages  5          Pharmacy.Pages.Invite   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:Pharmacy.Pages     5        N� Title$  Invite��5	      .��5      �  � ColumnDefinitions 5      �� 5   
   $	��260 �6   �� 5   
   �� 5   
   $	��260 �6   .+�5      �� $��1��6   $��Center=�6!   $��Center=�6>   $	��260��6Y   .+�5   
   �� $��Login:��6   $��0,0,0,5q�6"   �� 5   
   -   5         ��Name$ tbxLogin��5      $��0,0,0,10q�6$   �� 5   
   $��	Password:��6   $��0,0,0,5q�6%   G� 5   
   -   5         G�Name$ tbxPassword��5      $��0,0,0,10q�6+   �� 5   
   $��Surname:��6   $��0,0,0,5q�6$   �� 5   
   -   5        $ 
tbxSurname��5      $��0,0,0,10q�6&   �� 5   
   $��Name:��6   $��0,0,0,5q�6!   �� 5   
   -   5        $ tbxName��5      $��0,0,0,10q�6#   �� 5   
   $��Patronymic:��6   $��0,0,0,5q�6'   �� 5   
   -   5        $ tbxOtchestvo��5      $��0,0,0,10q�6(   �� 5   
   $��Phone:��6   $��0,0,0,5q�6"   �� 5   
   -   5        $ tbxPhone��5      $��0,0,0,10q�6$   �� 5   
   $��Email:��6   $��0,0,0,5q�6"   �� 5   
   -   5        $ tbxEmail��5      $��0,0,0,10q�6$   �� 5!   
   $��Role:��6   $��0,0,0,5q�6!   �� 5"   
   -   5         ��Name$ cbxRole��5"      $��30��6$   $��0,0,0,10q�60    �� DisplayMemberPath$ RoleName��6B   �� 5#   
   $��Gender:��6   $��0,0,0,5q�6#   �� 5$   
   -	   5        $ 	cbxGender��5$      $��30��6&   $��0,0,0,10q�62   $ 
GenderName��6D   �� 5%   
    �� Orientation$ 
Horizontal=�6   $��80��6.   $��Center=�6:   $	��130��6W   .+�5&      �� -
   5         ��Name$ btnSign��5&      $��Create account��6&   6@   5'      5(      �� 5)      -   5        $ btnSelectPhoto��5)      $��Select photo��6%   $��2��6Y   $��47,357,93,53q�6i   �� 5*      -   5         ��Name$ imgPhoto��5*      $��2��6   $
��Left=�6.   $	��300��6I   $��0,89,0,0q�6V   $	��Top=�6h   $	��200��6�   5+      5,      