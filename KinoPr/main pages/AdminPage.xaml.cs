using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
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
using static KinoPr.Genre;

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
            LoadGenre();
        }


        //Список фильмов
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
                        MoviesDataGrid.ItemsSource = movieResponse.Data;
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
        private async Task LoadGenre()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/genre");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Genre> movies = JsonConvert.DeserializeObject<List<Genre>>(responseBody);
                        GenreResponse movieResponse = new GenreResponse { Data = movies };
                        GenreDataGrid.ItemsSource = movieResponse.Data;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке Жанров: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке Жанров: " + ex.Message);
            }
        }
        private void AddMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddFilm(mainWindow));
        }
        private void EditMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенный элемент
            Movie selectedFilm = (Movie)MoviesDataGrid.SelectedItem;

            // Проверяем, что элемент выбран
            if (selectedFilm != null)
            {
                // Переходим на страницу редактирования, передавая выбранный элемент как параметр
                FrameManager.MainFrame.Navigate(new EditFilm(selectedFilm, mainWindow));

            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для редактирования.");
            }
           
        }
        private async void DeleteMoviesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выделенный элемент
                Movie selectedFilm = (Movie)MoviesDataGrid.SelectedItem;

                // Проверяем, что элемент выбран
                if (selectedFilm != null)
                {
                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот фильм?", "Подтверждение удаления", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/film/{selectedFilm.Id}");

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Фильм успешно удален!");
                                // Обновляем список фильмов после удаления
                                await LoadMovies();
                            }
                            else
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                MessageBox.Show("Ошибка при удалении фильма: " + response.StatusCode + ", " + responseBody);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении фильма: " + ex.Message);
            }
        }

        //Список жанров
        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddGenre(mainWindow));
        }
        private void EditGenreButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенный элемент
            Genre selectedGenre = (Genre)GenreDataGrid.SelectedItem;

            // Проверяем, что элемент выбран
            if (selectedGenre != null)
            {
                // Переходим на страницу редактирования, передавая выбранный элемент как параметр
                FrameManager.MainFrame.Navigate(new EditGenre(selectedGenre, mainWindow));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для редактирования.");
            }
        }
        private async void DeleteGenreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выделенный элемент
                Genre selectedGenre = (Genre)GenreDataGrid.SelectedItem;

                // Проверяем, что элемент выбран
                if (selectedGenre != null)
                {
                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот жанр?", "Подтверждение удаления", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/genre/{selectedGenre.Id}");

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Жанр успешно удален!");
                                // Обновляем список жанров после удаления
                                await LoadGenre();
                            }
                            else
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                MessageBox.Show("Ошибка при удалении жанра: " + response.StatusCode + ", " + responseBody);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении жанра: " + ex.Message);
            }
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
