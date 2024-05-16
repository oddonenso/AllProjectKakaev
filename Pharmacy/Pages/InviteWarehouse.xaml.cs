using Pharmacy.Data;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Pharmacy.Pages
{
    /// <summary>
    /// Interaction logic for Invite.xaml
    /// </summary>
    public partial class InviteWarehouse : Page
    {
        private Warehouse newWarehouse;

        public InviteWarehouse()
        {
            InitializeComponent();
            newWarehouse = new Warehouse();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Create a new warehouse object with data from UI
            Warehouse newWarehouse = new Warehouse
            {
                WarehouseName = tbWarehouseName.Text,
                Address = tbAddress.Text,
                QuantityOfGoods = int.Parse(tbQuantityOfGoods.Text),
                Photo = LoadImageBytesFromUI()
            };

            // Save the warehouse to the database
            using (var dbContext = new Connection())
            {
                dbContext.Warehouse.Add(newWarehouse);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Склад добавлен в базу данных.");
        }

        private byte[] LoadImageBytesFromUI()
        {
            byte[] imageData = null;

            if (ImageWarehouse.Source != null && ImageWarehouse.Source is BitmapImage)
            {
                BitmapImage bitmapImage = (BitmapImage)ImageWarehouse.Source;

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

        private void AddWarehouseImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ImageWarehouse.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }
    }
}
