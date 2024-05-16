using Pharmacy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Pharmacy.Pages
{
    public partial class OrderDelivery : Page
    {
        private Connection context = new Connection();
        private User currentUser;

        public OrderDelivery(User currentUser, int userId)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            LoadWarehouses();
        }

        private void LoadProducts(int warehouseId)
        {
            try
            {
                using (var dbContext = new Connection())
                {
                    var productList = dbContext.Product.Where(p => p.WarehouseID == warehouseId).ToList();
                    cmbProducts.ItemsSource = productList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Продукты не загружены: {ex.Message}");
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product selectedProduct = cmbProducts.SelectedItem as Product;
                if (selectedProduct == null)
                {
                    MessageBox.Show("Пожалуйста, выберите продукт из списка");
                    return;
                }

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Пожалуйста, выберите количество продуктов.");
                    return;
                }

                Warehouse selectedWarehouse = cmbWarehouses.SelectedItem as Warehouse; // Получаем выбранный склад
                if (selectedWarehouse == null)
                {
                    MessageBox.Show("Пожалуйста, выберите склад.");
                    return;
                }

                // Create a pickup request с передачей warehouseId
                var deliveryService = new DeliveryService(context);
                deliveryService.CreatePickupRequest(selectedProduct.ProductID, quantity, selectedWarehouse.WarehouseID, currentUser.UserID);
                MessageBox.Show("Ваш заказ был отправлен и ожидает одобрения на складе.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке заказа: {ex.Message}");
            }
        }

        private void cmbWarehouses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Warehouse selectedWarehouse = cmbWarehouses.SelectedItem as Warehouse;
            if (selectedWarehouse != null)
            {
                LoadProducts(selectedWarehouse.WarehouseID);
            }
        }

        private void LoadWarehouses()
        {
            try
            {
                using (var dbContext = new Connection())
                {
                    var warehouseList = dbContext.Warehouse.ToList();
                    cmbWarehouses.ItemsSource = warehouseList;
                    cmbWarehouses.DisplayMemberPath = "WarehouseName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке складов: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
