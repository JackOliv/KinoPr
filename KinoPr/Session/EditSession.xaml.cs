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
            film.DisplayMemberPath = "Name";
            film.SelectedValuePath = "Name";
            film.SelectedValue = selectedSession.film_id;
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
