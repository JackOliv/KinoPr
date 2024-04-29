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
using Newtonsoft.Json;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        
        public MainWindow mainWindow;
        private const string BaseUrl = "http://motov-ae.tepk-it.ru/api/login";
        public User currentUser = new User();
        public AutorizationPage(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            polelogin.Focus();
        }
        private async void btnlog_Click(object sender, RoutedEventArgs e)
        {
            string login = polelogin.Text;
            string password = polepassword.Password;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Формируем параметры запроса
                    var parameters = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "login", login },
                        { "password", password }
                    };

                    // Формируем строку запроса с параметрами
                    string queryString = string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));

                    // Отправляем POST-запрос на сервер
                    HttpResponseMessage response = await client.PostAsync($"{BaseUrl}?{queryString}", null);

                    // Проверяем, был ли ответ успешным
                    if (response.IsSuccessStatusCode)
                    {
                        // Если успешный, получаем данные пользователя из ответа
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseContent);

                        // Заполняем данные текущего пользователя
                        currentUser.name = responseData.data.name;
                        currentUser.surname = responseData.data.surname;
                        currentUser.patronymic = responseData.data.patronymic;
                        currentUser.phone_number = responseData.data.phone_number;
                        currentUser.birth = responseData.data.birth;
                        currentUser.login = responseData.data.login;
                        currentUser.password = responseData.data.password;
                        currentUser.email = responseData.data.email;
                        currentUser.api_token = responseData.data.api_token;
                        currentUser.role_id = responseData.data.role_id;
                        Data.currentUser = currentUser;
                        // В зависимости от роли пользователя выполняем различные действия
                        switch (currentUser.role_id)
                        {
                            case 2:
                                FrameManager.MainFrame.Navigate(new ManagerPage(mainWindow));
                                break;
                            case 3:
                                FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                                break;
                            default:
                                MessageBox.Show("Доступ запрещен");
                                break;
                        }
                    }
                    else
                    {
                        // Если нет, выводим сообщение об ошибке
                        MessageBox.Show("Ошибка аутентификации. Пожалуйста, проверьте логин и пароль.");
                    }
                }
            }
            catch (Exception ex)
            {
                // В случае возникновения ошибки при отправке запроса, выводим сообщение об ошибке
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    
    }
}
