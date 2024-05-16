using Pharmacy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Pharmacy.Pages
{
    /// <summary>
    /// Логика взаимодействия для ForgotPass.xaml
    /// </summary>
    public partial class ForgotPass : Page
    {
        Connection dbContext = new Connection();
        User user = new User();
        private string code;
        public ForgotPass()
        {
            InitializeComponent();

        }

        private void btncheck_Click(object sender, RoutedEventArgs e) //проверка почты существет ли она
        {
            string Email = tbxEmail.Text.Trim();
            try
            {
                user = dbContext.Users.Where(p => p.Email == Email).FirstOrDefault();
                if (user.Email == Email)
                {
                    string email = Email;
                    code = GenerateCode();
                    EmailSend(email, code);
                    codeemail.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("такой почты не существует");
            }

        }
        private string GenerateCode()
        {
            char[] letters = "0123456789".ToCharArray();
            Random rand = new Random();

            string word = "";
            for (int j = 1; j <= 4; j++)
            {
                int letter_num = rand.Next(0, letters.Length - 1);
                word += letters[letter_num];
            }

            return word;
        }
        private void EmailSend(string Email, string generatedCode)
        {
            MailMessage message = new MailMessage("oddoneoddonov@gmail.com", Email, "Код подтверждения", "Ваш код подтверждения: " + generatedCode);
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("oddoneoddonov@gmail.com", "siuj xxkp hquy uhcp");

            if (Email.Contains("@yandex.ru"))
            {
                client.Host = "smtp.yandex.ru";
                client.Port = 587;
            }
            else if (Email.Contains("@mail.ru"))
            {
                client.Host = "smtp.mail.ru";
                client.Port = 587;
            }
            else if (Email.Contains("@gmail.com"))
            {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
            }
            client.EnableSsl = true; // Включаем SSL для безопасной передачи
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email. Error message: " + ex.Message);
            }
        }
        private void btncheck1_Click(object sender, RoutedEventArgs e) //проверка введенного кода с генерированным 
        {
            if (code == tbxCode.Text.Trim())
                NavigationService.Navigate(new NewPass(user));
            else
                MessageBox.Show("неверный код");
        }



    }
}
