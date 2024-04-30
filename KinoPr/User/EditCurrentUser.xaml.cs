using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public partial class EditCurrentUser : Page
    {
        public MainWindow mainWindow;
        public EditCurrentUser( MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            surname.Text = Data.currentUser.surname;
            name.Text = Data.currentUser.name;
            patronymic.Text = Data.currentUser.patronymic;
            login.Text = Data.currentUser.login;
            password.Text = Data.currentUser.password;
            phone_number.Text = Data.currentUser.phone_number;
            email.Text = Data.currentUser.email;
            birht.Text = Data.currentUser.birth.ToString();
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

                    HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/user/update", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Данные пользователя успешно обновлены!");
                        // Переходим на страницу администратора после успешного обновления
                        Data.currentUser.surname = surname.Text;
                        Data.currentUser.name = name.Text;
                        Data.currentUser.patronymic = patronymic.Text;
                        Data.currentUser.login = login.Text;
                        Data.currentUser.password = password.Text;
                        Data.currentUser.birth = DateTime.Parse(birht.Text);
                        Data.currentUser.email = email.Text;
                        Data.currentUser.phone_number = phone_number.Text;
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
