using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using static KinoPr.Product;
using static KinoPr.Session;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        public MainWindow mainWindow;
        public ManagerPage(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            profSurname.Content = Data.currentUser.surname;
            profName.Content = Data.currentUser.name;
            profPatronymic.Content = Data.currentUser.patronymic;
            profTelephone.Content = Data.currentUser.phone_number;
            profEmail.Content = Data.currentUser.email;
            profLogin.Content = Data.currentUser.login;
            profPassword.Content = Data.currentUser.password;
            profBiht.Content = Data.currentUser.birth;
            LoadSessions();
            LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/product");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Product> products = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                        ProductResponse productResponse = new ProductResponse { Data = products };
                        ProductDataGrid.ItemsSource = productResponse.Data;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке продуктов: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке продуктов: " + ex.Message);
            }
        }
        private async Task LoadSessions()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/session");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Session> sessions = JsonConvert.DeserializeObject<List<Session>>(responseBody);
                        SessionResponse sessionResponse = new SessionResponse { Data = sessions };
                        SessionDataGrid.ItemsSource = sessionResponse.Data;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке сеансов: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке сеансов: " + ex.Message);
            }
        }

        //Список сеансов
        private void AddSessionButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddSession(mainWindow));
        }
        private void EditSessionButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенный элемент
            Session selectedSession = (Session)SessionDataGrid.SelectedItem;

            // Проверяем, что элемент выбран
            if (selectedSession != null)
            {
                // Переходим на страницу редактирования, передавая выбранный элемент как параметр
                FrameManager.MainFrame.Navigate(new EditSession(selectedSession, mainWindow));

            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для редактирования.");
            }
        }
        private async void DeleteSessionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выделенный элемент
                Session selectedSession = (Session)SessionDataGrid.SelectedItem;

                // Проверяем, что элемент выбран
                if (selectedSession != null)
                {
                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот сеанс?", "Подтверждение удаления", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/session/{selectedSession.Id}");

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Сеанс успешно удален!");
                                // Обновляем список жанров после удаления
                                await LoadSessions();
                            }
                            else
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                MessageBox.Show("Ошибка при удалении сеанса: " + response.StatusCode + ", " + responseBody);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении сеанса: " + ex.Message);
            }
        }



        //Список пользователей
        private void AddFoodButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddFood(mainWindow));
        }
        private void EditFoodButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенный элемент
            Product selectedProduct = (Product)ProductDataGrid.SelectedItem;

            // Проверяем, что элемент выбран
            if (selectedProduct != null)
            {
                // Переходим на страницу редактирования, передавая выбранный элемент как параметр
                FrameManager.MainFrame.Navigate(new EditFood(selectedProduct, mainWindow));

            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для редактирования.");
            }
        }
        private void DeleteFoodButton_Click(object sender, RoutedEventArgs e)
        {

        }



        //Профиль
        private void SeeButton_Click(object sender, RoutedEventArgs e)
        {

            if (profPassword.Visibility == Visibility.Visible)
            {
                profPassword.Visibility = Visibility.Collapsed;
                placeholder.Visibility = Visibility.Visible;
            }
            else
            {
                profPassword.Visibility = Visibility.Visible;
                placeholder.Visibility = Visibility.Collapsed;
            }
        }
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditCurrentUser(mainWindow));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Data.currentUser = null;
            FrameManager.MainFrame.Navigate(new AutorizationPage(mainWindow));
        }
    }
}
