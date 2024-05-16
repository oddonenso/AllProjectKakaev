using Pharmacy.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Pharmacy.Pages
{
    public partial class Employee : Page
    {
        private Connection context = new Connection();
        private User currentUser;
        private List<Product> productList;
        private List<Sale> saleList;
        private List<QualityControl> qualityControlList;
        private List<Delivery> deliveryList;
        private List<Warehouse> warehouseList;
        private List<Supplier> supplierList;
        private readonly IDeliveryService _deliveryService;

        public Employee(User currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            _deliveryService = new DeliveryService(context);
            LoadData();
        }

        private void btnRequestPickup_Click(object sender, RoutedEventArgs e)
        {
            // Получить выбранный продукт из списка
            var selectedProduct = ListViewData.SelectedItem as Product;
            if (selectedProduct != null)
            {
                // Создать заявку на доставку
                _deliveryService.CreatePickupRequest(selectedProduct.ProductID, selectedProduct.Quantity, selectedProduct.WarehouseID, currentUser.UserID); // Передаем currentUser.UserID как requesterId
                MessageBox.Show("Заявка на доставку создана");
            }
            else
            {
                MessageBox.Show("Выберите продукт для заявки на доставку");
            }
        }

        private void ListViewData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewData.SelectedItem != null && cmbTables.SelectedIndex == 3) // Delivery is the 4th item in the ComboBox
            {
                var delivery = ListViewData.SelectedItem as Delivery;
                if (delivery != null)
                {
                    RedactDelivery redact = new RedactDelivery(delivery);
                    NavigationService.Navigate(redact);
                }
            }
        }

        private void LoadPendingPickupRequests()
        {
            var pendingRequests = _deliveryService.GetPendingPickupRequests();
            ListViewData.ItemsSource = pendingRequests;
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = ListViewData.SelectedItem as PickupRequest;
            if (selectedRequest != null)
            {
                // Одобрить заявку на доставку
                LoadPendingPickupRequests();
            }
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = ListViewData.SelectedItem as PickupRequest;
            if (selectedRequest != null)
            {
                // Отклонить заявку на доставку
                _deliveryService.RejectPickupRequest(selectedRequest.RequestID);
                LoadPendingPickupRequests();
            }
        }

        private void LoadData()
        {
            try
            {
                productList = context.Product.ToList() ?? new List<Product>();

                // Загружаем данные из таблицы product по умолчанию
                LoadProductColumns();
                ListViewData.ItemsSource = productList;

                // Загружаем остальные данные, если они необходимы
                saleList = context.Sale.ToList() ?? new List<Sale>();
                qualityControlList = context.QualityControl.ToList() ?? new List<QualityControl>();
                deliveryList = context.Delivery.ToList() ?? new List<Delivery>();
                warehouseList = context.Warehouse.ToList() ?? new List<Warehouse>();
                supplierList = context.Supplier.ToList() ?? new List<Supplier>();
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
                        case "Product":
                            LoadProductColumns();
                            ListViewData.ItemsSource = productList;
                            ShowAddButton(true);
                            break;
                        case "Sale":
                            LoadSaleColumns();
                            ListViewData.ItemsSource = saleList;
                            ShowAddButton(true);
                            break;
                        case "QualityControl":
                            LoadQualityControlColumns();
                            ListViewData.ItemsSource = qualityControlList;
                            ShowAddButton(true);
                            break;
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
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void LoadProductColumns()
        {
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Фото", CellTemplate = Resources["ProductImageTemplate"] as DataTemplate });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new System.Windows.Data.Binding("ProductID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Наименование", DisplayMemberBinding = new System.Windows.Data.Binding("Name") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Производитель", DisplayMemberBinding = new System.Windows.Data.Binding("Manufacturer") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Цена", DisplayMemberBinding = new System.Windows.Data.Binding("Price") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Количество", DisplayMemberBinding = new System.Windows.Data.Binding("Quantity") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Срок годности", DisplayMemberBinding = new System.Windows.Data.Binding("ShelfLife") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Сертификат", DisplayMemberBinding = new System.Windows.Data.Binding("Certificate") });
        }

        private void LoadSaleColumns()
        {
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new System.Windows.Data.Binding("SaleID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Дата продажи", DisplayMemberBinding = new System.Windows.Data.Binding("SaleDate") });
            // Добавить другие столбцы для Sale
        }

        private void LoadDeliveryColumns()
        {
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new System.Windows.Data.Binding("DeliveryID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Дата доставки", DisplayMemberBinding = new System.Windows.Data.Binding("DeliveryDate") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID Продукта", DisplayMemberBinding = new System.Windows.Data.Binding("ProductID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Количество", DisplayMemberBinding = new System.Windows.Data.Binding("Quantity") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Цена", DisplayMemberBinding = new System.Windows.Data.Binding("Price") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID Поставщика", DisplayMemberBinding = new System.Windows.Data.Binding("SupplierID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Фото", CellTemplate = Resources["ProductImageTemplate"] as DataTemplate });
        }

        private void LoadQualityControlColumns()
        {
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID контроля", DisplayMemberBinding = new System.Windows.Data.Binding("IdControl") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Дата контроля", DisplayMemberBinding = new System.Windows.Data.Binding("DataControl") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID Продукта", DisplayMemberBinding = new System.Windows.Data.Binding("ProductID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Результат контроля", DisplayMemberBinding = new System.Windows.Data.Binding("ResultControl") });
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

        private void LoadWarehouseColumns()
        {
            GridViewColumns.Columns.Clear();
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Фото", CellTemplate = Resources["ProductImageTemplate"] as DataTemplate });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new System.Windows.Data.Binding("WarehouseID") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Название склада", DisplayMemberBinding = new System.Windows.Data.Binding("WarehouseName") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Адрес", DisplayMemberBinding = new System.Windows.Data.Binding("Address") });
            GridViewColumns.Columns.Add(new GridViewColumn { Header = "Количество товаров", DisplayMemberBinding = new System.Windows.Data.Binding("QuantityOfGoods") });
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
     


        private void btnOrderDelivery_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderDelivery(currentUser, currentUser.UserID));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTables.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cmbTables.SelectedItem;
                string selectedTable = selectedItem.Content.ToString();

                switch (selectedTable)
                {
                    case "Product":
                        // Переход на страницу для добавления продукта
                        NavigationService.Navigate(new InviteProduct());
                        break;
                    case "Warehouse":
                        // Переход на страницу для добавления склада
                        NavigationService.Navigate(new InviteWarehouse());
                        break;
                    // Добавьте другие случаи для других таблиц
                    case "Supplier":
                        NavigationService.Navigate(new InviteSupplier());
                        break;

                    case "Sale":
                        NavigationService.Navigate(new InviteSale());
                        break;
                    case "Delivery":
                        NavigationService.Navigate(new InviteDelivery());
                        break;
                }
            }
        }
    }
}
