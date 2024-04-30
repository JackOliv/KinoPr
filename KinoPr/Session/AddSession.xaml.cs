using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using static KinoPr.Hall;
using static KinoPr.Session_status;
using static KinoPr.Type_hall;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для AddSession.xaml
    /// </summary>
    public partial class AddSession : Page
    {
        private MainWindow mainWindow;
        public AddSession(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            LoadMovies();
            LoadStatus();
            LoadHall();

        }
        private async Task LoadMovies()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/film");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(responseBody);
                        MovieResponse movieResponse = new MovieResponse { Data = movies };
                        film.ItemsSource = movieResponse.Data;
                        film.DisplayMemberPath = "Name";
                        film.SelectedValuePath = "Id";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке фильмов: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке фильмов: " + ex.Message);
            }
        }

        private async Task LoadStatus()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/sessionstatuses");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Session_status> statuss = JsonConvert.DeserializeObject<List<Session_status>>(responseBody);
                        SessionStatusResponse sessionstatusResponse = new SessionStatusResponse { Data = statuss };
                        status.ItemsSource = sessionstatusResponse.Data;
                        status.DisplayMemberPath = "Name";
                        status.SelectedValuePath = "Id";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке статусов: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке статусов: " + ex.Message);
            }
        }

        private async Task LoadHall()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/hall");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Hall> halls = JsonConvert.DeserializeObject<List<Hall>>(responseBody);
                        HallResponse hallResponse = new HallResponse { Data = halls };
                        hall.ItemsSource = hallResponse.Data;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке зала: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке зала: " + ex.Message);
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime startTime, endTime;
                if (!DateTime.TryParseExact(time_start.Text, "yyyy-M-d H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                {
                    MessageBox.Show("Неправильный формат времени начала");
                    return;
                }

                if (!DateTime.TryParseExact(time_end.Text, "yyyy-M-d H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
                {
                    MessageBox.Show("Неправильный формат времени окончания");
                    return;
                }

                // Проверяем выбранные элементы в ComboBox
                if (status.SelectedItem == null || film.SelectedItem == null || hall.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите все необходимые параметры");
                    return;
                }

                // Создаем новую сессию для добавления
                Session newSession = new Session
                {
                    time_start = startTime,
                    time_end = endTime,
                    session_status_id = ((Session_status)status.SelectedItem).Id,
                    film_id = ((Movie)film.SelectedItem).Id,
                    hall_id = ((Hall)hall.SelectedItem).Id
                };

                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(newSession.time_start.ToString("yyyy-M-d H:m:s")), "time_start");
                    multiContent.Add(new StringContent(newSession.time_end.ToString("yyyy-MM-dd HH:mm:ss")), "time_end");
                    multiContent.Add(new StringContent(newSession.session_status_id.ToString()), "session_status_id");
                    multiContent.Add(new StringContent(newSession.film_id.ToString()), "film_id");
                    multiContent.Add(new StringContent(newSession.hall_id.ToString()), "hall_id");

                    HttpResponseMessage response = await client.PostAsync("http://motov-ae.tepk-it.ru/api/session", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Сеанс успешно добавлен!");
                        // Переходим на страницу администратора после успешного добавления
                        FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при добавлении сеанса: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении сеанса: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
    }
}
