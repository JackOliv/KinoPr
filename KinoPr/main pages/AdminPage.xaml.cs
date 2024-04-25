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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public MainWindow mainWindow;
        public AdminPage(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            profSurname.Content = Data.currentUser.Surname;
            profName.Content = Data.currentUser.Name;
            profPatronymic.Content = Data.currentUser.Patronymic;
            profTelephone.Content = Data.currentUser.PhoneNumber;
            profEmail.Content = Data.currentUser.Email;
            profLogin.Content = Data.currentUser.Login;
            profPassword.Content = Data.currentUser.Password;
            profBiht.Content = Data.currentUser.Birth;
            LoadMovies();
        }


        //Список фильмов
        private async void LoadMovies()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://cinema/api/film");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(responseBody);
                    MoviesDataGrid.ItemsSource = movieResponse.Data;
                }
                else
                {
                    MessageBox.Show("Ошибка при загрузке фильмов: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке фильмов: " + ex.Message);
            }
        }
        private void AddMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddFilm(mainWindow));
        }
        private void EditMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditFilm(mainWindow));
        }
        private void DeleteMoviesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        //Список жанров
        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddGenre(mainWindow));
        }
        private void EditGenreButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditGenre(mainWindow));
        }



        //Список сеансов
        private void AddSessionButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddSession(mainWindow));
        }
        private void EditSessionButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditSession(mainWindow));
        }
        private void DeleteSessionButton_Click(object sender, RoutedEventArgs e)
        {

        }


        //Список пользователей
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddUser(mainWindow));
        }
        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new EditUser(mainWindow));
        }
        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        
        //Список пользователей
        private void AddFoodButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddFood(mainWindow));
        }
        private void EditFoodButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddFood(mainWindow));
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

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Data.currentUser = null;
            FrameManager.MainFrame.Navigate(new AutorizationPage(mainWindow));
        }

    }
}
