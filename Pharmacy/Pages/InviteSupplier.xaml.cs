using Pharmacy.Data;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Pharmacy.Pages
{
    public partial class InviteSupplier : Page
    {
        public InviteSupplier()
        {
            InitializeComponent();
        }

        private void BtnSaveSupplier_Click(object sender, RoutedEventArgs e)
        {
            // Create a new supplier object with data from UI
            Supplier newSupplier = new Supplier
            {
                Name = tbSupplierName.Text,
                Address = tbSupplierAddress.Text,
                Phone = tbSupplierPhone.Text,
                Email = tbSupplierEmail.Text,
                Photo = LoadImageBytesFromUI()
            };

            // Save the supplier to the database
            using (var dbContext = new Connection())
            {
                dbContext.Supplier.Add(newSupplier);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Поставщик добавлен в базу данных");
        }

        private byte[] LoadImageBytesFromUI()
        {
            byte[] imageData = null;

            if (ImageSupplier.Source != null && ImageSupplier.Source is BitmapImage)
            {
                BitmapImage bitmapImage = (BitmapImage)ImageSupplier.Source;

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

        private void AddSupplierImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ImageSupplier.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }
    }
}
