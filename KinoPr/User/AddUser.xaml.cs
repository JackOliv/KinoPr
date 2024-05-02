using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Page
    {
        public MainWindow mainWindow;
        public AddUser(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем объект User для добавления нового пользователя
                User newUser = new User
                {
                    surname = surname.Text,
                    name = name.Text,
                    patronymic = patronymic.Text,
                    login = login.Text,
                    password = password.Text,
                    phone_number = phone_number.Text,
                    email = email.Text,
                    role_id = Convert.ToInt32(role_id.Text),
                    birth = DateTime.Parse(birht.Text)
                };

                using (HttpClient client = new HttpClient())
                {
                    // Устанавливаем токен авторизации в заголовке запроса
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);

                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(newUser.name), "name");
                    multiContent.Add(new StringContent(newUser.surname), "surname");
                    multiContent.Add(new StringContent(newUser.patronymic), "patronymic");
                    multiContent.Add(new StringContent(newUser.phone_number), "phone_number");
                    multiContent.Add(new StringContent(newUser.birth.ToString("yyyy-M-d H:m:s")), "birth");
                    multiContent.Add(new StringContent(newUser.login), "login");
                    multiContent.Add(new StringContent(newUser.password), "password");
                    multiContent.Add(new StringContent(newUser.email), "email");
                    multiContent.Add(new StringContent(newUser.role_id.ToString()), "role_id");

                    HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/user/create", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Пользователь успешно добавлен!");
                        // Переходим на страницу администратора после успешного добавления
                        FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при добавлении пользователя: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении пользователя: " + ex.Message);
            }
        }
    }
}
