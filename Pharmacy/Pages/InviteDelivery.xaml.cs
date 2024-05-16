using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Pharmacy.Data;

namespace Pharmacy.Pages
{
    public partial class InviteDelivery : Page
    {
        public InviteDelivery()
        {
            InitializeComponent();
            LoadSuppliers();
            LoadPickupRequests();
        }

        private void LoadPickupRequests()
        {
            using (var dbContext = new Connection())
            {
                var pickupRequests = dbContext.PickupRequest.ToList();
                cbPickupRequests.ItemsSource = pickupRequests;
            }
        }

        private void LoadSuppliers()
        {
            using (var dbContext = new Connection())
            {
                var suppliers = dbContext.Supplier.ToList();
                cbSuppliers.ItemsSource = suppliers;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the selected supplier from the ComboBox
            Supplier selectedSupplier = cbSuppliers.SelectedItem as Supplier;
            if (selectedSupplier == null)
            {
                MessageBox.Show("Пожалуйста, выберите поставщика");
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

            // Retrieve the selected pickup request from the ComboBox
            PickupRequest selectedPickupRequest = cbPickupRequests.SelectedItem as PickupRequest;

            // Create a new delivery object
            Delivery newDelivery = new Delivery
            {
                DeliveryDate = DateTime.Now,
                SupplierID = selectedSupplier.SupplierID,
                Quantity = quantity,
                Price = price,
                PickupRequestID = selectedPickupRequest?.RequestID ?? 0, // Assign 0 if no pickup request selected
                Photo = LoadImageBytesFromUI()
            };

            // Save the delivery to the database
            using (var dbContext = new Connection())
            {
                dbContext.Delivery.Add(newDelivery);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Доставка добавлена в базу данных");
        }

        private byte[] LoadImageBytesFromUI()
        {
            byte[] imageData = null;

            if (ImageDelivery.Source != null && ImageDelivery.Source is BitmapImage)
            {
                BitmapImage bitmapImage = (BitmapImage)ImageDelivery.Source;

                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
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

        private void AddDeliveryImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ImageDelivery.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }
    }
}
