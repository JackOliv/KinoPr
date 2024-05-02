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
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        public MainWindow mainWindow;
        private User selectedUser;
        public EditUser(User selectedUser, MainWindow main)
        {
            InitializeComponent();
            this.selectedUser = selectedUser;
            mainWindow = main;
            surname.Text = selectedUser.surname;
            name.Text = selectedUser.name;
            patronymic.Text = selectedUser.patronymic;
            login.Text = selectedUser.login;
            password.Text = selectedUser.password;
            phone_number.Text = selectedUser.phone_number;
            role_id.Text = selectedUser.role_id.ToString();
            email.Text = selectedUser.email;
            birht.Text = selectedUser.birth.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Создаем объект User для обновления данных
                User updatedUser = new User
                {
                    id = Data.currentUser.id,
                    surname = surname.Text,
                    name = name.Text,
                    patronymic = patronymic.Text,
                    login = login.Text,
                    password = password.Text,
                    phone_number = phone_number.Text,
                    email = email.Text,
                    role_id = Convert.ToInt32(role_id.Text),
                    birth = DateTime.Parse(birht.Text),

                };

                using (HttpClient client = new HttpClient())
                {
                    // Устанавливаем токен авторизации в заголовке запроса
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);

                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(updatedUser.name), "name");
                    multiContent.Add(new StringContent(updatedUser.surname), "surname");
                    multiContent.Add(new StringContent(updatedUser.patronymic), "patronymic");
                    multiContent.Add(new StringContent(updatedUser.phone_number), "phone_number");
                    multiContent.Add(new StringContent(updatedUser.birth.ToString("yyyy-M-d H:m:s")), "birth");
                    multiContent.Add(new StringContent(updatedUser.login), "login");
                    multiContent.Add(new StringContent(updatedUser.password), "password");
                    multiContent.Add(new StringContent(updatedUser.email), "email");
                    multiContent.Add(new StringContent(updatedUser.role_id.ToString()), "role_id");

                    HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/user/update", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Данные пользователя успешно обновлены!");
                        // Переходим на страницу администратора после успешного обновления
                        FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при обновлении данных пользователя: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных пользователя: " + ex.Message);
            }
        }
    }
}
