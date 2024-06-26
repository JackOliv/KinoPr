﻿using System;
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
                        Data.currentUser.name = responseData.data.name;
                        Data.currentUser.surname = responseData.data.surname;
                        Data.currentUser.patronymic = responseData.data.patronymic;
                        Data.currentUser.phone_number = responseData.data.phone_number;
                        Data.currentUser.birth = responseData.data.birth;
                        Data.currentUser.login = responseData.data.login;
                        Data.currentUser.password = responseData.data.password;
                        Data.currentUser.email = responseData.data.email;
                        Data.currentUser.api_token = responseData.data.api_token;
                        Data.currentUser.role_id = responseData.data.role_id;
                        // В зависимости от роли пользователя выполняем различные действия
                        switch (Data.currentUser.role_id)
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
