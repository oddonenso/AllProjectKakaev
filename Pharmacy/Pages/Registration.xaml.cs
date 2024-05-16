using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Pharmacy.Data;
using HashPasswords;

namespace Pharmacy.Pages
{
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
            LoadRoles();
            LoadGenders();
        }

        private void LoadRoles()
        {
            using (var dbContext = new Connection())
            {
                var roles = dbContext.Role.ToList();
                cbxRole.ItemsSource = roles;
            }
        }

        private void LoadGenders()
        {
            using (var dbContext = new Connection())
            {
                var genders = dbContext.Genders.ToList();
                cbxGender.ItemsSource = genders;
            }
        }

        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            string login = tbxLogin.Text;
            string password = HashPassword.Hash(tbxPassword.Password);
            string name = tbxName.Text;
            string surname = tbxSurname.Text;
            string phone = tbxPhone.Text;
            string otchestvo = tbxOtchestvo.Text;
            int role = (cbxRole.SelectedItem as Role)?.RoleID ?? -1;
            string email = tbxEmail.Text;
            int gender = (cbxGender.SelectedItem as Gender)?.GenderID ?? -1;

            if (!String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(surname) && !String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(password))
            {
                if (tbxPassword.Password.Length >= 6)
                {
                    if (phone.Length == 18)
                    {
                        if (!CheckUserLoginExists(login))
                        {
                            byte[] photo = null;
                            if (selectedImageBytes != null)
                            {
                                photo = selectedImageBytes;
                            }
                            SaveUser(login, password, name, surname, phone, otchestvo, role, email, photo, gender);
                        }
                        else
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Номер телефона должен иметь формат +9 (999) 999-99-99");
                    }
                }
                else
                {
                    MessageBox.Show("Пароль должен иметь длину не менее 6 символов");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите данные");
            }
        }

        private void SaveUser(string login, string password, string name, string surname, string phone, string otchestvo, int role, string email, byte[] photo, int gender)
        {
            try
            {
                using (var dbContext = new Connection())
                {
                    var user = new User();
                    user.Login = login;
                    user.Password = password;
                    user.Name = name;
                    user.Surname = surname;
                    user.Phone = phone;
                    user.Patronymic = otchestvo;
                    user.RoleID = role;
                    user.Email = email;
                    user.Photo = photo;
                    user.GenderId = gender;

                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    MessageBox.Show("Пользователь успешно зарегистрирован");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации пользователя: " + ex.Message + "\n" + ex.InnerException);
            }
        }

        private bool CheckUserLoginExists(string login)
        {
            using (var dbContext = new Connection())
            {
                return dbContext.Users.Any(p => p.Login == login);
            }
        }

        private byte[] selectedImageBytes;

        private void btnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.EndInit();
                    imgPhoto.Source = bitmap;

                    // Установка режима масштабирования
                    imgPhoto.Stretch = Stretch.UniformToFill;

                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        selectedImageBytes = new byte[fs.Length];
                        fs.Read(selectedImageBytes, 0, selectedImageBytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }
    }
}
