using System;
using System.Windows;
using System.Windows.Controls;
using autoservise.db;

namespace autoservise.pages
{
    public partial class AddClientPage : Page
    {
        private autoserviseEntities1 _context; // Ваш контекст базы данных

        public AddClientPage(autoserviseEntities1 context)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null"); // Проверка на null
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            // Считываем данные с текстовых полей
            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string patronymic = PatronymicTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string genderCode = GenderCodeTextBox.Text.Trim(); 

            // Проверка на заполненность обязательных полей
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                StatusTextBlock.Text = "First Name and Last Name are required.";
                StatusTextBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                return;
            }

            // Дополнительная проверка email и телефона на формат
            if (!IsValidEmail(email))
            {
                StatusTextBlock.Text = "Invalid email format.";
                StatusTextBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                return;
            }

            if (!IsValidPhone(phone))
            {
                StatusTextBlock.Text = "Invalid phone format.";
                StatusTextBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                return;
            }

            // Создаем нового клиента
            Client newClient = new Client
            {
                FirstName = firstName,
                LastName = lastName,
                Patronymic = patronymic,
                Email = email,
                Phone = phone,
                RegistrationDate = DateTime.Now,
                GenderCode = genderCode
            };

            // Добавляем клиента в базу данных
            _context.Client.Add(newClient);

                _context.SaveChanges(); // Сохраняем изменения в базе данных
                StatusTextBlock.Text = "Client added successfully!";
                StatusTextBlock.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);

                // Очищаем поля после успешного добавления
                ClearFields();
            
        }

        // Метод для очистки полей
        private void ClearFields()
        {
            FirstNameTextBox.Text = string.Empty;
            LastNameTextBox.Text = string.Empty;
            PatronymicTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            PhoneTextBox.Text = string.Empty;
        }

        // Проверка корректности email
        private bool IsValidEmail(string email)
        {
            // Простая проверка корректности email (можно заменить на более сложную)
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }

        // Проверка корректности телефона
        private bool IsValidPhone(string phone)
        {
            // Простая проверка телефона на длину (можно заменить на проверку регулярным выражением)
            return !string.IsNullOrWhiteSpace(phone) && phone.Length >= 10;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientPage());
        }
    }
}
