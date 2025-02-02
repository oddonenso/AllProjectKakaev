﻿using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HashPasswords;
using Pharmacy.Data;


namespace Pharmacy.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        Connection dbContext = new Connection();
        User user = new User();
        int countUnsuccessful = 0;
        private string verificationCode;

        public Autho()
        {
            InitializeComponent();
            tboxCaptcha.Visibility = Visibility.Hidden;
            tblockCaptcha.Visibility = Visibility.Hidden;
        }
        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxLogin.Text) && !String.IsNullOrEmpty(pasboxPassword.Password))
            {
                LoginUser();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль", "Предупреждение");
                countUnsuccessful++;
                GenerateCaptcha();
                if (countUnsuccessful % 3 == 0)
                {
                    TimerBLock();
                    return;
                }
            }
        }

        private void LoginUser()
        {
            if (countUnsuccessful > 0)
            {
                if (countUnsuccessful % 3 == 0)
                {
                    TimerBLock();
                    return;
                }
                if (!CheckingCaptcha())
                {
                    MessageBox.Show("Неверная капча", "Предупреждение");
                    countUnsuccessful++;
                    GenerateCaptcha();
                    return;
                }
            }

            string Login = tbxLogin.Text.Trim();
            string pass = pasboxPassword.Password.Trim();
            string Password = HashPassword.Hash(pass.Replace("\"", ""));

            // Проверяем, существует ли пользователь с указанным логином
            var userExists = dbContext.Users.Any(p => p.Login == Login);
            if (!userExists)
            {
                MessageBox.Show("Пользователя с логином '" + Login + "' не существует", "Предупреждение");
                GenerateCaptcha();
                countUnsuccessful++;
                if (countUnsuccessful % 3 == 0)
                    TimerBLock();
                return;
            }

            // Извлекаем пользователя из базы данных
            user = dbContext.Users.FirstOrDefault(p => p.Login == Login);

            if (user != null)
            {
                if (user?.Password == Password)
                {
                    // Вход выполнен успешно
                    LoadForm(user.RoleID.ToString());
                    tbxLogin.Text = "";
                    pasboxPassword.Password = "";
                    tboxCaptcha.Text = "";
                    countUnsuccessful = 0;
                    tboxCaptcha.Visibility = Visibility.Hidden;
                    tblockCaptcha.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Неверный пароль", "Предупреждение");
                    GenerateCaptcha();
                    countUnsuccessful++;
                    if (countUnsuccessful % 3 == 0)
                        TimerBLock();
                }
            }
        }


        private async void TimerBLock()
        {
            panel.IsEnabled = false;

            await Task.Factory.StartNew(() =>
            {
                for (int i = 10; i > 0; i--)
                {
                    //Каждую секунду вызывает метод для обновления текста
                    tblockTimer.Dispatcher.Invoke(() =>
                    {
                        tblockTimer.Text = $"подождите {i} сек";
                    });
                    Task.Delay(1000).Wait();//приостанавливает выполнение задачи на 1 секунду
                }
            });

            tblockTimer.Text = "";
            panel.IsEnabled = true;
        }

        private void LoadForm(string _role)
        {
            switch (_role)
            {
                //клиент -- посмотреть свои данные и обьекты 
                case "1":
                   NavigationService.Navigate(new WarehouseEmployee(user));
                    break;
                //админ -- умеет все 
                case "2":
                    NavigationService.Navigate(new Admin(user));
                    break;

                case "3":
                    NavigationService.Navigate(new Employee(user));
                    break;
                    ;
            }
        }

        private bool TimeWork()
        {
            var currentTime = DateTime.Now;
            if (currentTime.Hour < 10 || currentTime.Hour > 19) return false;
            return true;
        }
        private void GenerateCaptcha()
        {
            /// <summary>
            /// Метод используется для генерации капчи
            /// </summary>
            tboxCaptcha.Visibility = Visibility.Visible;
            tblockCaptcha.Visibility = Visibility.Visible;

            // В данном куске кода создается массив letters,
            // содержащий все буквы английского алфавита (кроме буквы "l") и цифры.
            char[] letters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            Random rand = new Random();

            string word = "";
            for (int j = 1; j <= 8; j++)
            {
                int letter_num = rand.Next(0, letters.Length - 1);
                word += letters[letter_num];
            }
            // Затем создается объект Random для генерации случайных чисел.
            // Далее в цикле формируется строка word из 8 случайно выбранных символов из массива letters.

            tblockCaptcha.Text = word;
            tblockCaptcha.TextDecorations = TextDecorations.Strikethrough;
            tboxCaptcha.Text = "";
        }
        private bool CheckingCaptcha() => tblockCaptcha.Text == tboxCaptcha.Text.Trim();

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }
        private string GenerateCode()
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void EmailSend(string email, string code)
        {
            MailMessage message = new MailMessage("oddoneoddonov@gmail.com", email, "Код подтверждения", "Ваш код подтверждения: " + code);
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("oddoneoddonov@gmail.com", "siuj xxkp hquy uhcp");
            /* 
                Настройка учётных данных для SMTP-клиента, указывающая на почтовый ящик отправителя и пароль      для авторизации перед отправкой сообщения
            */
            // Определяем сервер для отправки письма в зависимости от почтового домена получателя
            if (email.Contains("@yandex.ru"))
            {
                client.Host = "smtp.yandex.ru";
                client.Port = 587;
            }
            else if (email.Contains("@mail.ru"))
            {
                client.Host = "smtp.mail.ru";
                client.Port = 587;
            }
            else if (email.Contains("@gmail.com"))
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
        private void btnForgotPass_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new ForgotPass());
        }




    }
}
