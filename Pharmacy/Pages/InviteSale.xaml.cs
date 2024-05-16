using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Pharmacy.Data;

namespace Pharmacy.Pages
{
    public partial class InviteSale : Page
    {
        public InviteSale()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (var dbContext = new Connection())
            {
                var products = dbContext.Product.ToList();
                cbProducts.ItemsSource = products;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the selected product from the ComboBox
            Product selectedProduct = cbProducts.SelectedItem as Product;
            if (selectedProduct == null)
            {
                MessageBox.Show("Пожалуйста, выберите продукт");
                return;
            }

            // Parse quantity and price
            if (!int.TryParse(tbQuantity.Text, out int quantity))
            {
                MessageBox.Show("Некорректное количество");
                return;
            }

            if (!decimal.TryParse(tbPrice.Text, out decimal price))
            {
                MessageBox.Show("Некорректная цена");
                return;
            }

            Sale newSale = new Sale
            {
                SaleDate = DateTime.Now,
                ProductID = selectedProduct.ProductID,
                Quantity = quantity,
                Price = price
            };

            using (var dbContext = new Connection())
            {
                dbContext.Sale.Add(newSale);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Сделка добавлена в базу данных");
        }
    }
}
