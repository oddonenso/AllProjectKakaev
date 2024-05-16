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
using ClosedXML.Excel;


namespace Pharmacy.Pages
{
    public partial class Admin : Page
    {
        private User currentUser;
        private Connection context = new Connection();

        public Admin(User currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            var ppl = context.Users.ToList();
            LViewPpl.ItemsSource = ppl;
        }

        private void Selector_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedUser = (User)LViewPpl.SelectedItem;

            if (selectedUser != null)
            {
                // NavigationService.Navigate(new Redact(selectedUser));
            }
            else
            {
                MessageBox.Show("Please select a user to edit.");
            }
        }
        private void btnSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем диалоговое окно для сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Workbook|*.xlsx",
                    Title = "Save an Excel File"
                };

                // Проверяем, нажата ли кнопка "Сохранить" в диалоговом окне
                if (saveFileDialog.ShowDialog() == true)
                {
                    // Получаем список пользователей из базы данных
                    var users = context.Users.ToList();

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Users");

                        // Добавляем заголовки
                        worksheet.Cell(1, 1).Value = "Name";
                        worksheet.Cell(1, 2).Value = "Surname";
                        worksheet.Cell(1, 3).Value = "Patronymic";
                        worksheet.Cell(1, 4).Value = "Photo";

                        // Добавляем пользователей в таблицу
                        int row = 2;
                        foreach (var user in users)
                        {
                            worksheet.Cell(row, 1).Value = user.Name;
                            worksheet.Cell(row, 2).Value = user.Surname;
                            worksheet.Cell(row, 3).Value = user.Patronymic;

                            // Преобразуем массив байтов в изображение и добавляем его в ячейку
                            if (user.Photo != null)
                            {
                                using (var ms = new MemoryStream(user.Photo))
                                {
                                    var image = worksheet.AddPicture(ms).MoveTo(worksheet.Cell(row, 4)).Scale(0.1); // Adjust scale as needed
                                }
                            }
                            row++;
                        }

                        // Сохраняем файл
                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show("Excel file saved successfully!");
                }
            }
            catch (Exception ex)
            {
                string message = $"An unexpected error occurred: {ex.Message}\n\n";
                message += $"Stack trace:\n{ex.StackTrace}";
                MessageBox.Show(message);
            }
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text;

            if (searchText.Length == 0)
            {
                var ppl = context.Users.ToList();
                LViewPpl.ItemsSource = ppl;
            }
            else
            {
                IQueryable<User> query = context.Users;

                switch (cmbFilter.SelectedIndex)
                {
                    case 0: // Должность
                        query = query.Where(u => u.Role.RoleName.Contains(searchText));
                        break;
                    case 1: // Фамилия
                        query = query.Where(u => u.Surname.Contains(searchText));
                        break;
                    case 2: // Имя
                        query = query.Where(u => u.Name.Contains(searchText));
                        break;
                    case 3: // Отчество
                        query = query.Where(u => u.Patronymic.Contains(searchText));
                        break;
                }

                if (cmbSorting.SelectedIndex == 0) // по возрастанию
                {
                    query = query.OrderBy(u => u.Role.RoleName);
                }
                else if (cmbSorting.SelectedIndex == 1) // по убыванию
                {
                    query = query.OrderByDescending(u => u.Role.RoleName);
                }

                LViewPpl.ItemsSource = query.ToList();
            }
        }



        private void btnSaveToPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем диалоговое окно для сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Document|*.pdf",
                    Title = "Save a PDF File"
                };

                // Проверяем, нажата ли кнопка "Сохранить" в диалоговом окне
                if (saveFileDialog.ShowDialog() == true)
                {
                    // Получаем список пользователей из базы данных
                    var users = context.Users.ToList();

                    // Создаем новый PDF-документ
                    using (PdfWriter writer = new PdfWriter(saveFileDialog.FileName))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            using (Document document = new Document(pdf))
                            {
                                // Добавляем заголовок
                                document.Add(new Paragraph("Список пользователей"));

                                // Создаем таблицу
                                Table table = new Table(4);
                                table.AddHeaderCell("Name");
                                table.AddHeaderCell("Surname");
                                table.AddHeaderCell("Patronymic");
                                table.AddHeaderCell("Photo");

                                // Добавляем пользователей в таблицу
                                foreach (var user in users)
                                {
                                    // Добавляем текстовые ячейки
                                    table.AddCell(user.Name);
                                    table.AddCell(user.Surname);
                                    table.AddCell(user.Patronymic);

                                    // Преобразуем массив байтов в изображение
                                    iText.Layout.Element.Image image = null;
                                    if (user.Photo != null)
                                    {
                                        using (var ms = new MemoryStream(user.Photo))
                                        {
                                            image = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(ms.ToArray()));
                                            // Устанавливаем фиксированную ширину и высоту изображения
                                            float width = 25; // Ширина изображения в пикселях
                                            float height = width * image.GetImageScaledHeight() / image.GetImageScaledWidth(); // Вычисляем высоту пропорционально ширине
                                            image.SetWidth(width);
                                            image.SetHeight(height);
                                        }
                                    }

                                    // Добавляем изображение в ячейку
                                    Cell photoCell = new Cell();
                                    if (image != null)
                                    {
                                        photoCell.Add(image);
                                    }
                                    table.AddCell(photoCell);
                                }

                                // Добавляем таблицу в документ
                                document.Add(table);
                            }
                        }

                    }

                    MessageBox.Show("PDF file saved successfully!");
                }
            }

            catch (Exception ex)
            {
                string message = $"An unexpected error occurred: {ex.Message}\n\n";
                message += $"Stack trace:\n{ex.StackTrace}";
                MessageBox.Show(message);
            }

        }

        private void GenerateEmploymentContract(User employee)
        {
            string companyName = "Your Company Name";
            string companyAddress = "Your Company Address";
            string companyPhone = "Your Company Phone";
            string companyEmail = "Your Company Email";

            // Путь к шаблону договора
            string templatePath = "path/to/your/employment_contract_template.pdf";

            // Путь для сохранения сгенерированного договора
            string outputPath = $"path/to/save/employment_contract_{employee.Surname}_{employee.Name}_{employee.Patronymic}.pdf";

            // Открываем шаблон договора
            PdfReader reader = new PdfReader(templatePath);
            PdfWriter writer = new PdfWriter(outputPath);
            PdfDocument pdf = new PdfDocument(reader, writer);
            Document document = new Document(pdf);

            // Заполняем пропуски в договоре
            document.Add(new Paragraph($"Full name: {employee.Surname} {employee.Name} {employee.Patronymic}"));
            document.Add(new Paragraph($"Position: {employee.Role.RoleName}"));
            document.Add(new Paragraph($"Company name: {companyName}"));
            document.Add(new Paragraph($"Company address: {companyAddress}"));
            document.Add(new Paragraph($"Company phone: {companyPhone}"));
            document.Add(new Paragraph($"Company email: {companyEmail}"));

            // Закрываем документ
            document.Close();

            MessageBox.Show($"Employment contract for {employee.Surname} {employee.Name} {employee.Patronymic} has been generated successfully.");
        }
        private void LViewPpl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ваш код обработки события
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ваш код обработки события
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            Invite invite = new Invite();
            NavigationService.Navigate(invite);

        }
    }
}
