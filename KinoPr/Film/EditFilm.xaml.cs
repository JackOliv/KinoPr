using Microsoft.Win32;
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
using static KinoPr.Genre;
using System.IO;

namespace KinoPr
{
    /// <summary>
    /// Логика взаимодействия для EditFilm.xaml
    /// </summary>
    public partial class EditFilm : Page
    {
        public MainWindow mainWindow;
        private Movie selectedMovie;
        public EditFilm(Movie selectedMovie, MainWindow main)
        {
            InitializeComponent();
            this.selectedMovie = selectedMovie;
            mainWindow = main; 
            LoadGenre();
            genre.DisplayMemberPath = "Name";
            genre.SelectedValuePath = "Id";
            title.Text = selectedMovie.Name;
            genre.SelectedValue = selectedMovie.GenreId;
            duration.Text = selectedMovie.Duration;
            year.Text = selectedMovie.Year.ToString();
            description.Text = selectedMovie.Description;
            director.Text = selectedMovie.Director;
            country.Text = selectedMovie.Country;
            filmImage.Source = new BitmapImage(new Uri(selectedMovie.Photo, UriKind.RelativeOrAbsolute));
            List genreList = new List();
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
                        genre.ItemsSource = movieResponse.Data;
                        
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
        }
        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                // Отобразить выбранное изображение в элементе Image
                filmImage.Source = new BitmapImage(new Uri(imagePath));

            }
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем объект Movie для обновления данных
                Movie updatedMovie = new Movie
                {
                    Id = selectedMovie.Id,
                    Name = title.Text,
                    GenreId = (int)genre.SelectedValue,
                    Duration = duration.Text,
                    Year = int.Parse(year.Text),
                    Description = description.Text,
                    Director = director.Text,
                    Country = country.Text,
                    Photo = selectedMovie.Photo // Пока не обновляем изображение фильма
                };

                // Преобразуем объект updatedMovie в JSON
                string json = JsonConvert.SerializeObject(updatedMovie);

                // Отправляем PATCH запрос для обновления данных фильма
                using (HttpClient client = new HttpClient())
                {
                    // Создаем мультипарт контент для отправки изображения и других данных
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    // Добавляем JSON данные
                    multiContent.Add(new StringContent(json, Encoding.UTF8, "application/json"), "movie");

                    // Добавляем изображение
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == true)
                    {
                        string imagePath = openFileDialog.FileName;
                        byte[] imageData = File.ReadAllBytes(imagePath);
                        ByteArrayContent imageContent = new ByteArrayContent(imageData);
                       // multiContent.Add(imageContent, "image", Path.GetFileName(imagePath));
                    }

                    HttpResponseMessage response = await client.PostAsync($"http://motov-ae.tepk-it.ru/api/film/{selectedMovie.Id}", multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Фильм успешно обновлен!");
                        // Переходим на страницу администратора после успешного обновления
                        FrameManager.MainFrame.Navigate(new AdminPage(mainWindow));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Ошибка при обновлении фильма: " + response.StatusCode + ", " + responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении фильма: " + ex.Message);
            }
        }

    }
}
