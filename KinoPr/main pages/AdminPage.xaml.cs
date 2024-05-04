using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using static KinoPr.Session;
using static KinoPr.User;

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
            profSurname.Content = Data.currentUser.surname;
            profName.Content = Data.currentUser.name;
            profPatronymic.Content = Data.currentUser.patronymic;
            profTelephone.Content = Data.currentUser.phone_number;
            profEmail.Content = Data.currentUser.email;
            profLogin.Content = Data.currentUser.login;
            profPassword.Content = Data.currentUser.password;
            profBiht.Content = Data.currentUser.birth;
            LoadMovies();
            LoadGenre();
            LoadUsers();
        }


        //Список фильмов
        private async Task LoadMovies()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
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
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
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
        
        private async Task LoadUsers()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
                    HttpResponseMessage response = await client.GetAsync("http://motov-ae.tepk-it.ru/api/users");

                    if (response.IsSuccessStatusCode)
                    {

                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<User> users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                        UserResponse userResponse = new UserResponse { Data = users };
                        UsersDataGrid.ItemsSource = userResponse.Data;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при загрузке пользователей: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке пользователей: " + ex.Message);
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
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
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
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
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


        


        //Список пользователей
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AddUser(mainWindow));
        }
        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенный элемент
            User selectedUser = (User)UsersDataGrid.SelectedItem;

            // Проверяем, что элемент выбран
            if (selectedUser != null)
            {
                // Переходим на страницу редактирования, передавая выбранный элемент как параметр
                FrameManager.MainFrame.Navigate(new EditUser(selectedUser, mainWindow));

            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для редактирования.");
            }
        }
        private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выделенный элемент
                User selectedUser = (User)UsersDataGrid.SelectedItem;

                // Проверяем, что элемент выбран
                if (selectedUser != null)
                {
                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение удаления", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.currentUser.api_token);
                            HttpResponseMessage response = await client.DeleteAsync($"http://motov-ae.tepk-it.ru/api/user/delete/{selectedUser.id}");

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Пользователь успешно удален!");
                                // Обновляем список жанров после удаления
                                await LoadUsers();
                            }
                            else
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                MessageBox.Show("Ошибка при удалении пользователя: " + response.StatusCode + ", " + responseBody);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите элемент для пользователя.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении сессии: " + ex.Message);
            }
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
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите изменить данные профиля?", "Подтверждение действия", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                FrameManager.MainFrame.Navigate(new EditCurrentUser(mainWindow));
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из профиля?", "Подтверждение действия", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Data.currentUser = new User();
                FrameManager.MainFrame.Navigate(new AutorizationPage(mainWindow));
            }
           
        }

    }
}
