using HashPasswords;
using iText.IO.Image;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Pharmacy.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pharmacy.Pages
{
    public partial class Invite : Page
    {
        private byte[] selectedImageBytes;

        public Invite()
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

            if (IsValidInput(phone, surname, name, login, password))
            {
                if (!CheckUserLoginExists(login))
                {
                    byte[] photo = selectedImageBytes;
                    SaveUser(login, password, name, surname, phone, otchestvo, role, email, photo, gender);
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                }
            }
        }

        private bool IsValidInput(string phone, string surname, string name, string login, string password)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите данные");
                return false;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен иметь длину не менее 6 символов");
                return false;
            }
            if (phone.Length != 18)
            {
                MessageBox.Show("Номер телефона должен иметь формат +9 (999) 999-99-99");
                return false;
            }
            return true;
        }

        private void SaveUser(string login, string password, string name, string surname, string phone, string otchestvo, int role, string email, byte[] photo, int gender)
        {
            try
            {
                using (var dbContext = new Connection())
                {
                    var user = new User
                    {
                        Login = login,
                        Password = password,
                        Name = name,
                        Surname = surname,
                        Phone = phone,
                        Patronymic = otchestvo,
                        RoleID = role,
                        Email = email,
                        Photo = photo,
                        GenderId = gender
                    };

                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();

                    // Reload the user with the related Role data
                    dbContext.Entry(user).Reference(u => u.Role).Load();

                    GenerateEmploymentContract(user);

                    MessageBox.Show("Пользователь успешно зарегистрирован");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации пользователя: " + ex.Message + "\n" + ex.InnerException?.Message);
            }
        }

        private bool CheckUserLoginExists(string login)
        {
            using (var dbContext = new Connection())
            {
                return dbContext.Users.Any(p => p.Login == login);
            }
        }

        private void GenerateEmploymentContract(User employee)
        {
            if (employee.Role == null)
            {
                MessageBox.Show("Oshibka: Rol' sotrudnika ne zagruzhena.");
                return;
            }

            var companyDetails = new Dictionary<string, string>
    {
        { "CompanyName", "SuperApteka" },
        { "CompanyAddress", "Dostoevskogo 20" },
        { "CompanyPhone", "+8 (800) 555-35-35" },
        { "CompanyEmail", "oddoneoddonnov@gmail.com" }
    };

            // Poluchaem put' k papke "Zagruzki" dlya tekuschego pol'zovatelya
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";

            // Ubedites', chto papka sushchestvuet
            if (!Directory.Exists(downloadsPath))
            {
                Directory.CreateDirectory(downloadsPath);
            }

            // Formiruem polnyy put' k failu
            string outputPath = Path.Combine(downloadsPath, $"Trudovoy_dogovor_{employee.Surname}_{employee.Name}_{employee.Patronymic}.pdf");

            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                document.Add(new Paragraph($"FIO: {employee.Surname} {employee.Name} {employee.Patronymic}"));
                document.Add(new Paragraph($"Dolzhnost': {employee.Role.RoleName}"));
                document.Add(new Paragraph($"Nazvanie kompanii: {companyDetails["CompanyName"]}"));
                document.Add(new Paragraph($"Adress kompanii: {companyDetails["CompanyAddress"]}"));
                document.Add(new Paragraph($"Telefon kompanii: {companyDetails["CompanyPhone"]}"));
                document.Add(new Paragraph($"Email kompanii: {companyDetails["CompanyEmail"]}"));

                document.Add(new Paragraph($"Gorod: Novosibirsk                                                                                    Data: \"{DateTime.Now.ToString("dd MM yyyy", System.Globalization.CultureInfo.GetCultureInfo("ru-RU"))}\""));
                document.Add(new Paragraph($"{companyDetails["CompanyName"]} «{companyDetails["CompanyAddress"]}», v dal'neyshem imenuemoe «Rabotodatel'», v litse general'nogo direktora Kompot Bidrosovich, deystvuyushchego na osnove Ustava, s odnoy storony,"));
                document.Add(new Paragraph($"i grazhdanin {employee.Surname} {employee.Name} {employee.Patronymic}, v dal'neyshem imenuemyy «Rabotnik», deystvuyushchiy ot svoego imeni, s drugoy storony, v dal'neyshem sovmestno imenuemyye «Storony», a po otdel'nosti – «Storona», zaklyuchili nastoyashchiy Trudovoy dogovor o nizhesleduyushchem:"));
                document.Add(new Paragraph(""));

                document.Add(new Paragraph("1. Predmet Trudovogo dogovora"));
                document.Add(new Paragraph($"1.1. Rabotnik prinimayetsya na rabotu v {companyDetails["CompanyName"]} na dolzhnost':  {employee.Role.RoleName}")); ;
                document.Add(new Paragraph($"1.2. Mesto raboty: {companyDetails["CompanyName"]}"));
                document.Add(new Paragraph("1.3. Nastoyashchiy Trudovoy dogovor yavlyaetsya dogovorom po osnovnoy rabote."));
                document.Add(new Paragraph("1.4. Nastoyashchiy Trudovoy dogovor zaklyuchen na neopredelennyy srok."));
                document.Add(new Paragraph($"1.5. Data nachala raboty \"{DateTime.Now.ToString("dd MM yyyy", System.Globalization.CultureInfo.GetCultureInfo("ru-RU"))}\" года."));
                document.Add(new Paragraph("1.6. Prodolzhitelnost' ispytaniya pri prieme na rabotu 3 mes."));


                document.Close();
            }

            MessageBox.Show($"Trudovoy dogovor dlya {employee.Surname} {employee.Name} {employee.Patronymic} uspeshno sgenerirovan.");
        }


        private void btnSelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.EndInit();
                    imgPhoto.Source = bitmap;
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
