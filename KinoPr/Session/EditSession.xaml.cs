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
            LoadHall();
            film.DisplayMemberPath = "Name";
            film.SelectedValuePath = "Id";
            film.SelectedValue = selectedSession.FilmId;
            status.DisplayMemberPath = "Name";
            status.SelectedValuePath = "Name";
            status.SelectedValue = selectedSession.sessions;
            hall.DisplayMemberPath = "Id";
            hall.SelectedValuePath = "Id";
            hall.SelectedValue = selectedSession.hall;
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
                        List <Hall> halls = JsonConvert.DeserializeObject<List<Hall>>(responseBody);
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new ManagerPage(mainWindow));
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime startTime, endTime;
                if (!DateTime.TryParseExact(time_start.Text, "d.M.yyyy H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                {
                    MessageBox.Show("Неправильный формат времени начала");
                    return;
                }

                if (!DateTime.TryParseExact(time_end.Text, "d.M.yyyy H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
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
                    id = selectedSession.id,
                    time_start = startTime,
                    time_end = endTime,
                    session_status_id = ((Session_status)status.SelectedItem).Id,
                    FilmId = ((Movie)film.SelectedItem).Id,
                    hall = ((Hall)hall.SelectedItem).Id
                };

                using (HttpClient client = new HttpClient())
                {
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем данные формы в мультипарт контент
                    multiContent.Add(new StringContent(updatedSession.time_start.ToString("yyyy-M-d H:m:s")), "time_start");
                    multiContent.Add(new StringContent(updatedSession.time_end.ToString("yyyy-M-d H:m:s")), "time_end");
                    multiContent.Add(new StringContent(updatedSession.session_status_id.ToString()), "session_status_id");
                    multiContent.Add(new StringContent(updatedSession.FilmId.ToString()), "film_id");
                    multiContent.Add(new StringContent(updatedSession.hall.ToString()), "hall_id");

                    HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/session/{selectedSession.id}", multiContent);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Сеанс успешно обновлен!");
                        // Переходим на страницу администратора после успешного обновления
                        FrameManager.MainFrame.Navigate(new ManagerPage(mainWindow));
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
