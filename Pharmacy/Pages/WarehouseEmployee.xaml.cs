using Pharmacy.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pharmacy.Pages
{
    /// <summary>
    /// Логика взаимодействия для WarehouseEmployee.xaml
    /// </summary>
    public partial class WarehouseEmployee : Page
    {
        private Connection context = new Connection();
        private User currentUser;
        private List<Delivery> deliveryList;
        private List<Warehouse> warehouseList;
        private List<Supplier> supplierList;
        private List<PickupRequest> pickupList;

        public WarehouseEmployee(User currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                warehouseList = context.Warehouse.ToList() ?? new List<Warehouse>();

                // Загружаем данные из таблицы product по умолчанию
                LoadWarehouseColumns();
                ListViewData.ItemsSource = warehouseList;

                // Загружаем остальные данные, если они необходимы
                deliveryList = context.Delivery.ToList() ?? new List<Delivery>();
                supplierList = context.Supplier.ToList() ?? new List<Supplier>();
                pickupList = context.PickupRequest.ToList() ?? new List<PickupRequest>();

            }
            catch (Exception ex)
            {
                // Очистим список отображаемых данных
                ListViewData.ItemsSource = null;
            }
        }
        private void cmbTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTables.SelectedItem != null)
            {
                try
                {
                    ComboBoxItem selectedItem = (ComboBoxItem)cmbTables.SelectedItem;
                    string selectedTable = selectedItem.Content.ToString();

                    switch (selectedTable)
                    {
                      
                       
                        case "Delivery":
                            LoadDeliveryColumns();
                            ListViewData.ItemsSource = deliveryList;
                            ShowAddButton(true);
                            break;
                        case "Warehouse":
                            LoadWarehouseColumns();
                            ListViewData.ItemsSource = warehouseList;
                            ShowAddButton(true);
                            break;
                        case "Supplier":
                            LoadSupplierColumns();
                            ListViewData.ItemsSource = supplierList;
                            ShowAddButton(true);
                            break;

                        case "PickupRequst":
                            LoadPickupRequest();
                            ListViewData.ItemsSource = pickupList;
                            ShowAddButton(false);
                            break;

                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        private void LoadSupplierColumns()
        {
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Фото", CellTemplate = Resources["ProductImageTemplate"] as DataTemplate });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID Поставщика", DisplayMemberBinding = new System.Windows.Data.Binding("SupplierID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Имя", DisplayMemberBinding = new System.Windows.Data.Binding("Name") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Адрес", DisplayMemberBinding = new System.Windows.Data.Binding("Address") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Телефон", DisplayMemberBinding = new System.Windows.Data.Binding("Phone") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Email", DisplayMemberBinding = new System.Windows.Data.Binding("Email") });

        }

        private void LoadPickupRequest()
        {
            List<PickupRequest> pickupRequests = context.PickupRequest.ToList();

            // Отображение данных о заявках на забор в ListView
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID заявки", DisplayMemberBinding = new System.Windows.Data.Binding("RequestID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Продукт", DisplayMemberBinding = new System.Windows.Data.Binding("ProductID") }); // Теперь должно отображаться название продукта
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Склад", DisplayMemberBinding = new System.Windows.Data.Binding("Warehouse.WarehouseName") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Количество", DisplayMemberBinding = new System.Windows.Data.Binding("Quantity") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Дата заявки", DisplayMemberBinding = new System.Windows.Data.Binding("RequestDate") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Заявитель", DisplayMemberBinding = new System.Windows.Data.Binding("RequesterID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Статус", DisplayMemberBinding = new System.Windows.Data.Binding("Status") });
        }


        private void LoadWarehouseColumns()
        {
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Фото", CellTemplate = Resources["ProductImageTemplate"] as DataTemplate });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new System.Windows.Data.Binding("WarehouseID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Название склада", DisplayMemberBinding = new System.Windows.Data.Binding("WarehouseName") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Адрес", DisplayMemberBinding = new System.Windows.Data.Binding("Address") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Количество товаров", DisplayMemberBinding = new System.Windows.Data.Binding("QuantityOfGoods") });
        }
        private void LoadDeliveryColumns()
        {
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new System.Windows.Data.Binding("DeliveryID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Дата доставки", DisplayMemberBinding = new System.Windows.Data.Binding("DeliveryDate") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID Заказа", DisplayMemberBinding = new System.Windows.Data.Binding("PickupRequestID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Количество", DisplayMemberBinding = new System.Windows.Data.Binding("Quantity") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Цена", DisplayMemberBinding = new System.Windows.Data.Binding("Price") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID Поставщика", DisplayMemberBinding = new System.Windows.Data.Binding("SupplierID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Фото", CellTemplate = Resources["ProductImageTemplate"] as DataTemplate });
        }
        private void ShowAddButton(bool show)
        {
            // Показать или скрыть кнопку добавления в зависимости от значения параметра
            // В данном примере предполагается, что кнопка для добавления имеет название "btnAdd"
            if (show)
            {
                btnAdd.Visibility = Visibility.Visible;
            }
            else
            {
                btnAdd.Visibility = Visibility.Collapsed;
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                // Получаем контекст данных
                var dataContext = image.DataContext;

                // Проверяем тип данных и устанавливаем изображение
                if (dataContext is Product product && product.Photo != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    using (MemoryStream stream = new MemoryStream(product.Photo))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = stream;
                        bitmapImage.EndInit();
                    }
                    image.Source = bitmapImage;
                }
                // Добавьте другие проверки для других типов данных, если необходимо
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTables.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cmbTables.SelectedItem;
                string selectedTable = selectedItem.Content.ToString();

                switch (selectedTable)
                {
                   
                    case "Warehouse":
                        // Переход на страницу для добавления склада
                        NavigationService.Navigate(new InviteWarehouse());
                        break;
                    // Добавьте другие случаи для других таблиц
                    case "Supplier":
                        NavigationService.Navigate(new InviteSupplier());
                        break;

                   
                    case "Delivery":
                        NavigationService.Navigate(new InviteDelivery());
                        break;

                   
                }
            }
        }
    }
}
