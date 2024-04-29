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
using static KinoPr.Session_status;
using static KinoPr.Type_hall;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для EditSession.xaml
    /// </summary>
    public partial class EditSession : Page
    {
        public MainWindow mainWindow;
        private Session selectedSession;
        public EditSession(Session selectedSession, MainWindow main)
        {
            InitializeComponent();
            this.selectedSession = selectedSession;
            mainWindow = main;
            LoadMovies();
            LoadStatus();
            LoadType();
            film.DisplayMemberPath = "Name";
            film.SelectedValuePath = "Name";
            film.SelectedValue = selectedSession.film;
            status.DisplayMemberPath = "Name";
            status.SelectedValuePath = "Name";
            status.SelectedValue = selectedSession.sessions;
            hall.DisplayMemberPath = "Name";
            hall.SelectedValuePath = "Name";
            hall.SelectedValue = selectedSession.type_hall;
            time_start.Text = selectedSession.time_start.ToString();
            time_end.Text = selectedSession.time_end.ToString();
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
        private async Task LoadType()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/typehall");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List < Type_hall> typehall = JsonConvert.DeserializeObject<List<Type_hall>>(responseBody);
                        TypeHallResponse typeResponse = new TypeHallResponse { Data = typehall };
                        hall.ItemsSource = typeResponse.Data;
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime startTime, endTime;
                if (!DateTime.TryParseExact(time_start.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                {
                    MessageBox.Show("Неправильный формат времени начала");
                    return;
                }

                if (!DateTime.TryParseExact(time_end.Text, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
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

                // Создаем объект сеанса для обновления данных
                Session updatedSession = new Session
                {
                    Id = selectedSession.Id,
                    time_start = startTime,
                    time_end = endTime,
                    session_status_id = ((Session_status)status.SelectedItem).Id,
                    film_id = ((Movie)film.SelectedItem).Id,
                    type_hall_id = ((Type_hall)hall.SelectedItem).Id
                };

                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(updatedSession.time_start.ToString("yyyy-MM-dd HH:mm:ss")), "time_start");
                    multiContent.Add(new StringContent(updatedSession.time_end.ToString("yyyy-MM-dd HH:mm:ss")), "time_end");
                    multiContent.Add(new StringContent(updatedSession.session_status_id.ToString()), "session_status_id");
                    multiContent.Add(new StringContent(updatedSession.film_id.ToString()), "film_id");
                    multiContent.Add(new StringContent(updatedSession.type_hall_id.ToString()), "hall_id");

                    HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/session/{selectedSession.Id}", multiContent);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Сеанс успешно обновлен!");
                        // Переходим на страницу администратора после успешного обновления
                        FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при обновлении сеанса: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении сеанса: " + ex.Message);
            }
        }


    }
}
