using autoservise.db;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace autoservise.pages
{
    public partial class ClientPage : Page
    {
        private autoserviseEntities1 _context = new autoserviseEntities1(); // Ваш DbContext
        private Client _selectedClient;
        private int _currentPage = 1;
        private int _pageSize = 10; // Количество клиентов на странице

        public ClientPage()
        {
            InitializeComponent();
            LoadClients();
        }

        // Load clients into the DataGrid with pagination
        private void LoadClients()
        {
            int totalClients = _context.Client.Count();
            int totalPages = (int)Math.Ceiling((double)totalClients / _pageSize);

            // Показ только текущей страницы
            var clients = _context.Client
                .OrderBy(c => c.ID) // Сортировка по ID
                .Skip((_currentPage - 1) * _pageSize) // Пропустить предыдущие страницы
                .Take(_pageSize) // Показать только элементы на текущей странице
                .ToList();

            ClientDataGrid.ItemsSource = clients;
            PageNumberTextBlock.Text = $"Page {_currentPage} of {totalPages}";
        }

        // Search clients by Last Name
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            var filteredClients = _context.Client
                .Where(c => c.LastName.ToLower().Contains(searchText))
                .OrderBy(c => c.ID) // Сортировка по ID
                .Skip((_currentPage - 1) * _pageSize) // Пропустить предыдущие страницы
                .Take(_pageSize) // Показать только элементы на текущей странице
                .ToList();

            ClientDataGrid.ItemsSource = filteredClients;
        }

        // Add new client
        private void AddNewClient_Click(object sender, RoutedEventArgs e)
        {
            // Открываем страницу для добавления клиента в Frame
            AddClientPage addClientPage = new AddClientPage(_context);
            NavigationService.Navigate(addClientPage); // Навигация на страницу добавления клиента
        }

        // Delete selected client
        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClient != null)
            {
                _context.Client.Remove(_selectedClient);
                _context.SaveChanges();
                LoadClients();
            }
            else
            {
                MessageBox.Show("Please select a client to delete.");
            }
        }

        // Update selected client
        private void UpdateClient_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClient != null)
            {
                _selectedClient.FirstName = "UpdatedFirstName"; // Пример изменений
                _selectedClient.LastName = "UpdatedLastName";   // Пример изменений
                _context.SaveChanges();
                LoadClients();
            }
            else
            {
                MessageBox.Show("Please select a client to update.");
            }
        }

        // Select client in DataGrid
        private void ClientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedClient = (Client)ClientDataGrid.SelectedItem;
        }

        // Go to the previous page
        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadClients();
            }
        }

        // Go to the next page
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int totalClients = _context.Client.Count();
            int totalPages = (int)Math.Ceiling((double)totalClients / _pageSize);

            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadClients();
            }
        }
    }
}
