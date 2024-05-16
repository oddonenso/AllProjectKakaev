using Pharmacy.Data;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для NewPass.xaml
    /// </summary>
    public partial class NewPass : Page
    {
        private User currentUser;
        public NewPass(User currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string pass = HashPasswords.HashPassword.Hash(tbxPass.Text.Replace("\"", ""));
            if (!String.IsNullOrEmpty(pass))
                SaveUser(pass);
            else
                MessageBox.Show("Введите данные");
        }
        private void SaveUser(string pass)
        {
            var dbContext = new Connection();
            currentUser.Password = pass;

            var existingUser = dbContext.Users.FirstOrDefault(u => u.UserID == currentUser.UserID);
            if (existingUser != null)
            {
                dbContext.Entry(existingUser).CurrentValues.SetValues(currentUser);
            }
            else
            {
                dbContext.Users.Add(currentUser);
            }

            dbContext.SaveChanges();
            MessageBox.Show("Пароль изменен");
            NavigationService.Navigate(new Autho());
        }



    }
}
