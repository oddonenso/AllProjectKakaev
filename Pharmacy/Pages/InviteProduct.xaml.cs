using Pharmacy.Data;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Pharmacy.Pages
{
    public partial class InviteProduct : Page
    {
        private Product newProduct;

        public InviteProduct()
        {
            InitializeComponent();
            newProduct = new Product();
            LoadWarehouses();

        }
        private void LoadWarehouses()
        {
            using (var dbContext = new Connection())
            {
                var warehouses = dbContext.Warehouse.ToList();
                cbWarehouses.ItemsSource = warehouses;
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Create a new product object with data from UI
            Product newProduct = new Product
            {
                Name = tbProductName.Text,
                Manufacturer = tbManufacturer.Text,
                Price = decimal.Parse(tbPrice.Text),
                Quantity = int.Parse(tbQuantity.Text),
                ShelfLife = dpShelfLife.SelectedDate ?? DateTime.Now,
                Certificate = tbCertificate.Text,
                Photo = LoadImageBytesFromUI()
            };

            // Retrieve the selected warehouse from the ComboBox
            Warehouse selectedWarehouse = cbWarehouses.SelectedItem as Warehouse;
            if (selectedWarehouse != null)
            {
                // Set the WarehouseID of the new product
                newProduct.WarehouseID = selectedWarehouse.WarehouseID;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите складское помещение");
                return;
            }

            // Save the product to the database
            using (var dbContext = new Connection())
            {
                dbContext.Product.Add(newProduct);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Продукт добавлен в базу данных");

        }
   
        private byte[] LoadImageBytesFromUI()
        {
            byte[] imageData = null;

            if (ImageProduct.Source != null && ImageProduct.Source is BitmapImage)
            {
                BitmapImage bitmapImage = (BitmapImage)ImageProduct.Source;

                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder(); // Assuming the image format is PNG
                        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                        encoder.Save(stream);
                        imageData = stream.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error converting image to bytes: " + ex.Message);
                }
            }

            return imageData;
        }

        private void AddProductImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ImageProduct.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }
    }
}
